using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ContractEventHandler : Handler
{
    public ContractEventHandler(ErrorsHandler errorsHandler) : base(errorsHandler)
    {
    }

    protected override ParseNode? verifyTokens()
    {
        switch (CurrentToken.Type)
        {
            case Token.Types.K_AUTO_RESET:
                return parseAutoResetEvent();
            case Token.Types.K_AUTO_MOVE:
                return parseAutoMoveEvent();
            case Token.Types.K_FAIL:
                return parseFailEvent();
            default:
                throw new SyntaxErrorException(
                    ["'auto reset'", "'auto move'", "'fail'"], 
                    CurrentToken
                );
        }
    }

    private ContractAutoResetEventNode parseAutoResetEvent()
    {
        expectTokenType(Token.Types.K_AUTO_RESET);
        expectTokenType(Token.Types.K_AFTER);
        
        if (!HasMoreTokens)
        {
            throw new SyntaxErrorException("Expected state identifier after 'after' keyword");
        }
        
        var statesList = new PrintableList<StateNode>();
        assertTokenType(Token.Types.IDENT);
        statesList.Add(new StateNode(CurrentToken.Value));
        incrementIndex();
        
        while (HasMoreTokens && CurrentToken.Type == Token.Types.OP_OR)
        {
            incrementIndex();
            if (!HasMoreTokens)
            {
                throw new SyntaxErrorException("Expected state identifier after '|'");
            }
            assertTokenType(Token.Types.IDENT);
            statesList.Add(new StateNode(CurrentToken.Value));
            incrementIndex();
        }
        
        if (!HasMoreTokens)
        {
            throw new SyntaxErrorException("Expected ';' at end of auto-reset statement");
        }
        expectTokenType(Token.Types.D_SEMICOLON);
        return new ContractAutoResetEventNode(statesList);
    }

    private ContractAutoMoveEventNode parseAutoMoveEvent()
    {
        expectTokenType(Token.Types.K_AUTO_MOVE);
        expectTokenType(Token.Types.K_AFTER);
        
        if (!HasMoreTokens)
        {
            throw new SyntaxErrorException("Expected state identifier after 'after' keyword");
        }
        
        var statesList = new PrintableList<StateNode>();
        assertTokenType(Token.Types.IDENT);
        statesList.Add(new StateNode(CurrentToken.Value));
        incrementIndex();
        
        while (HasMoreTokens && CurrentToken.Type == Token.Types.OP_OR)
        {
            incrementIndex();
            if (!HasMoreTokens)
            {
                throw new SyntaxErrorException("Expected state identifier after '|'");
            }
            assertTokenType(Token.Types.IDENT);
            statesList.Add(new StateNode(CurrentToken.Value));
            incrementIndex();
        }
        
        if (!HasMoreTokens)
        {
            throw new SyntaxErrorException("Expected 'to' keyword in auto-move statement");
        }
        expectTokenType(Token.Types.K_TO);
        
        if (!HasMoreTokens)
        {
            throw new SyntaxErrorException("Expected target state identifier after 'to'");
        }
        assertTokenType(Token.Types.IDENT);
        string targetState = CurrentToken.Value;
        incrementIndex();
        
        if (!HasMoreTokens)
        {
            throw new SyntaxErrorException("Expected ';' at end of auto-move statement");
        }
        expectTokenType(Token.Types.D_SEMICOLON);
        return new ContractAutoMoveEventNode(statesList, new StateNode(targetState));
    }

    private ContractFailEventNode parseFailEvent()
    {
        expectTokenType(Token.Types.K_FAIL);
        
        if (!HasMoreTokens)
        {
            throw new SyntaxErrorException("Expected identifier after 'fail'");
        }
        assertTokenType(Token.Types.IDENT);
        string identifier = CurrentToken.Value;
        incrementIndex();
        
        if (!HasMoreTokens)
        {
            throw new SyntaxErrorException("Expected ';' or '{' after fail identifier");
        }
        
        if (CurrentToken.Type == Token.Types.D_SEMICOLON)
        {
            incrementIndex();
            return new ContractFailEventNode(identifier, new PrintableList<FuncParam>());
        }
        expectTokenType(Token.Types.D_CBRAC_OP);
        var parameters = parseParameters();
        
        if (!HasMoreTokens)
        {
            throw new SyntaxErrorException("Expected ';' at end of fail statement");
        }
        expectTokenType(Token.Types.D_SEMICOLON);
        return new ContractFailEventNode(identifier, parameters);
    }

    private PrintableList<FuncParam> parseParameters()
    {
        var parameters = new PrintableList<FuncParam>();

        while (CurrentToken.Type != Token.Types.D_CBRAC_CLO)
        {
            assertTokenType(Token.Types.IDENT);
            string paramName = CurrentToken.Value;
            incrementIndex();

            expectTokenType(Token.Types.D_COLON);

            TypeDefinitionNode? paramType = parseParameterType();
            if (paramType != null)
            {
                parameters.Add(new FuncParam(paramName, paramType));
            }

            if (CurrentToken.Type == Token.Types.COMMA)
            {
                incrementIndex();
            }
            else if (CurrentToken.Type != Token.Types.D_CBRAC_CLO)
            {
                throw new SyntaxErrorException(["','", "'}'"], CurrentToken);
            }
        }
        incrementIndex();

        return parameters;
    }

    private TypeDefinitionNode? parseParameterType()
    {
        DataTypeHandler dataTypeHandler = new(errorHandler);
        TypeNode? dataType = (TypeNode?)delegateToHandler(dataTypeHandler);

        if (dataType == null)
        {
            return null;
        }

        if (HasMoreTokens && CurrentToken.Type == Token.Types.D_BRAC_OP)
        {
            CapabilityHandler capabilityHandler = new(errorHandler);
            Capabilities? capabilities = (Capabilities?)delegateToHandler(capabilityHandler);
            if (capabilities == null)
            {
                return null;
            }
            return new TypeDefinitionNode(dataType, capabilities);
        }

        Capabilities defaultCapabilities = CapabilityHandler.GenerateDefaultCapabilities();
        return new TypeDefinitionNode(dataType, defaultCapabilities);
    }
}