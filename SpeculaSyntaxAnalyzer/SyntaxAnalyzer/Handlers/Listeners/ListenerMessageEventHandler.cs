using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ListenerMessageEventHandler : Handler
{
    private readonly BodyHandler bodyHandler;
    public ListenerMessageEventHandler(ErrorsHandler errorsHandler) : base(errorsHandler)
    {
        bodyHandler = new(errorsHandler);
    }

    protected override ParseNode? verifyTokens()
    {
        expectTokenType(Token.Types.K_ON);
        assertTokenType(Token.Types.IDENT);
        string name = CurrentToken.Value;
        incrementIndex();
        expectTokenType(Token.Types.D_CBRAC_OP);
        PrintableList<FuncParam> parameters = getParameters();
        BodyNode? body = parseFunctionBody();
        if (body == null) return null;
        return new ListenerMessageEventNode(name, parameters, body);
    }

    private PrintableList<FuncParam> getParameters()
    {
        var parameters = new PrintableList<FuncParam>();
        while (CurrentToken.Type != Token.Types.D_CBRAC_CLO)
        {
            assertTokenType(Token.Types.IDENT);
            string paramName = CurrentToken.Value;
            incrementIndex();
            if (CurrentToken.Type != Token.Types.D_COLON)
            {
                TypeDefinitionNode parameterType = new TypeDefinitionNode(new TypeNode(DataTypes.INFER), CapabilityHandler.GenerateDefaultCapabilities());
                parameters.Add(new FuncParam(paramName, parameterType));
            }
            else
            {
                expectTokenType(Token.Types.D_COLON);

                TypeDefinitionNode? paramType = parseParameterType();
                if (paramType != null)
                {
                    parameters.Add(new FuncParam(paramName, paramType));
                }
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

    private BodyNode? parseFunctionBody()
    {
        ParseNode? bodyNode = delegateToHandler(bodyHandler);
        if (bodyNode == null)
        {
            return null;
        }
        return (BodyNode)bodyNode;
    }
}