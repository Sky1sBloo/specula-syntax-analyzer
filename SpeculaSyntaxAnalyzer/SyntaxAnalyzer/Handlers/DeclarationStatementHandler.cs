using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class DeclarationStatementHandler : Handler
{
    private enum States
    {
        LET,
        IDENTIFIER,
        COLON_EQUALS,
        TYPE_CAPABILITY,
        TYPE,
        EQUALS_SEMI_COLON,
        VALUE,
        WAIT_FOR_VALUE,
        SEMI_COLON,
        INVALID
    }

    private States currentState = States.LET;
    private string identifier = "";
    private string dataType = "";
    private ValueNode? value;

    public override void handleToken(Token token, ParseNode? node)
    {
        try
        {
            switch (currentState)
            {
                case States.LET:
                    handleLetState(token);
                    break;
                case States.IDENTIFIER:
                    handleIdentState(token);
                    break;
                case States.COLON_EQUALS:
                    handleColonEqualsState(token);
                    break;
                case States.TYPE_CAPABILITY:
                    handleTypeCapabilityState(token);
                    break;
                case States.EQUALS_SEMI_COLON:
                    handleEqualsSemiColonState(token);
                    break;
                case States.VALUE:
                    handleValueState(token);
                    break;
                case States.WAIT_FOR_VALUE:
                    handleWaitForValueState(token, node);
                    break;
                case States.SEMI_COLON:
                    handleSemiColonState(token);
                    break;
                case States.INVALID:
                    handleInvalidState(token);
                    break;
            }
        }
        catch (SyntaxErrorException ex)
        {
            currentState = States.INVALID;
            throw ex;
        }
    }

    private void end()
    {
        if (value == null)
        {
            value = (ValueNode)new LiteralValue("L_NULL", "null");
        }
        ParseNode node = new DeclarationStatementNode(identifier, dataType, value);
        SetHandlerFinished(node);

        currentState = States.LET;
        identifier = "";
        dataType = "";
        value = null;
    }

    private void handleLetState(Token token)
    {
        if (token.Type != Token.Types.K_LET)
        {
            throw new SyntaxErrorException(["LET"], token);
        }
        currentState = States.IDENTIFIER;
    }

    private void handleIdentState(Token token)
    {
        if (token.Type != Token.Types.IDENT)
        {
            throw new SyntaxErrorException(["IDENTIFIER"], token);
        }
        currentState = States.COLON_EQUALS;
    }

    private void handleColonEqualsState(Token token)
    {
        switch (token.Type)
        {
            case Token.Types.D_COLON:
                currentState = States.TYPE_CAPABILITY;
                break;
            case Token.Types.OP_EQUALS:
                currentState = States.VALUE;
                break;
            default:
                throw new SyntaxErrorException(["':'", "'='"], token);
        }
    }

    private void handleTypeCapabilityState(Token token)
    {
        switch (token.Type)
        {
            case Token.Types.IDENT:
            case Token.Types.K_TYPE:
                dataType = token.Value;
                currentState = States.TYPE;
                break;

            // todo handle capabilities
            default:
                throw new SyntaxErrorException(["TYPE", "["], token);
        }
    }

    private void handleEqualsSemiColonState(Token token)
    {
        switch (token.Type)
        {
            case Token.Types.OP_EQUALS:
                currentState = States.EQUALS_SEMI_COLON;
                break;
            case Token.Types.D_SEMICOLON:
                end();
                break;
            default:
                throw new SyntaxErrorException(["'='", "'[", ";"], token);
        }
    }

    private void handleValueState(Token token)
    {
        currentState = States.WAIT_FOR_VALUE;
        SetDelegateToState(SyntaxAnalyzerRoot.States.VALUE);
        // todo handle expressions
        /*
        switch (token.Type)
        {
            case Token.Types.IDENT:
            case Token.Types.L_INT:
            case Token.Types.L_FLOAT:
            case Token.Types.L_DOUBLE:
            case Token.Types.L_CHAR:
            case Token.Types.L_STRING:
            case Token.Types.L_BOOL:
            case Token.Types.L_NULL:
                currentState = States.SEMI_COLON;
                break;
            default:
                throw new SyntaxErrorException(["VALUE", "EXPRESSION"], token);
        } */
    }

    private void handleWaitForValueState(Token token, ParseNode? node)
    {
        if (node == null)
        {
            throw new SyntaxErrorException(["Value"], token);
        }
        if (node is ValueNode valueNode)
        {
            value = valueNode;
        }
        else
        {
            throw new SyntaxErrorException(["Value"], token);
        }
    }

    private void handleSemiColonState(Token token)
    {
        if (token.Type != Token.Types.D_SEMICOLON)
        {
            throw new SyntaxErrorException([";"], token);
        }
        end();
    }

    private void handleInvalidState(Token token)
    {
        if (token.Type != Token.Types.D_SEMICOLON)
        {
            return;
        }
        end();
    }
}
