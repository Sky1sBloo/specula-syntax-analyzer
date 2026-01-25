using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;



public class InterfaceHandler : Handler
{
    private readonly VarDefinitionHandler varDefHandler;
    public InterfaceHandler(ErrorsHandler errors) : base(errors)
    {
        varDefHandler = new VarDefinitionHandler(errors);
    }

    protected override ParseNode? verifyTokens()
    {
        expectTokenType(Token.Types.K_INTERFACE);
        assertTokenType(Token.Types.IDENT);
        string interfaceName = CurrentToken.Value;
        incrementIndex();
        expectTokenType(Token.Types.D_CBRAC_OP);
        PrintableList<InterfaceFuncNode> methods = new();
        while (CurrentToken.Type != Token.Types.D_CBRAC_CLO)
        {
            expectTokenType(Token.Types.K_FN);
            assertTokenType(Token.Types.IDENT);
            string funcName = CurrentToken.Value;
            incrementIndex();

            expectTokenType(Token.Types.D_PAR_OP);
            PrintableList<ParamNode> parameters = new();
            while (CurrentToken.Type != Token.Types.D_PAR_CLO)
            {
                assertTokenType(Token.Types.IDENT);
                string paramName = CurrentToken.Value;
                incrementIndex();
                expectTokenType(Token.Types.D_COLON);
                
                if (CurrentToken.Type == Token.Types.K_SELF)
                {
                    parameters.Add(new InterfaceSelfParam(paramName));
                    incrementIndex();
                }
                else
                {
                    VarDefinitionHandler nocolonHandler = new VarDefinitionHandler(errorHandler, consumeColon: false);
                    TypeDefinitionNode? paramType = (TypeDefinitionNode?)delegateToHandler(nocolonHandler);
                    if (paramType == null)
                    {
                        throw new SyntaxErrorException(["TYPE"], CurrentToken);
                    }
                    parameters.Add(new FuncParam(paramName, paramType));
                }

                if (CurrentToken.Type == Token.Types.COMMA)
                {
                    incrementIndex();
                }
            }
            expectTokenType(Token.Types.D_PAR_CLO);
            if (CurrentToken.Type != Token.Types.D_COLON)
            {
                methods.Add(new InterfaceFuncReturnNode(funcName, parameters, new TypeDefinitionNode(new TypeNode(DataTypes.VOID), CapabilityHandler.GenerateDefaultCapabilities())));
            }

            if (CurrentToken.Type == Token.Types.K_SELF)
            {
                incrementIndex();
                expectTokenType(Token.Types.D_SEMICOLON);
                methods.Add(new InterfaceFuncReturnSelfNode(funcName, parameters));
            }
            else
            {
                TypeDefinitionNode returnType = (TypeDefinitionNode?)delegateToHandler(varDefHandler)
                    ?? throw new SyntaxErrorException(["TYPE"], CurrentToken);

                expectTokenType(Token.Types.D_SEMICOLON);
                methods.Add(new InterfaceFuncReturnNode(funcName, parameters, returnType));
            }

        }
        incrementIndex();
        return new InterfaceDefNode(interfaceName, methods);
    }
}