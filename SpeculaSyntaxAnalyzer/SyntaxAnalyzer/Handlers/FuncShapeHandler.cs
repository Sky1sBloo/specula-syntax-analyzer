using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class FuncShapeHandler: Handler
{
    private readonly VarDefinitionHandler varDefinitionHandler;
    private readonly BodyHandler bodyHandler;
    public FuncShapeHandler(ErrorsHandler errors) : base(errors)
    {
        varDefinitionHandler = new(errors, true);
        bodyHandler = new(errors);
    }

    protected override ParseNode? verifyTokens()
    {


        if (CurrentToken.Type != Token.Types.D_PAR_OP)
        {
            throw new SyntaxErrorException(["("], CurrentToken);
        }
        incrementIndex();
        PrintableList<FuncParam> parameters = parseParameters();

        TypeDefinitionNode? returnType = getReturnType();
        if (returnType == null)
        {
            returnType = new TypeDefinitionNode(new TypeNode(DataTypes.VOID), CapabilityHandler.GenerateDefaultCapabilities());
        }
        BodyNode? body = parseFunctionBody();
        if (body == null)
            return null;
        return new FuncShapeNode(parameters, returnType, body);
    }

    private PrintableList<FuncParam> parseParameters()
    {
        var parameters = new PrintableList<FuncParam>();

        while (CurrentToken.Type != Token.Types.D_PAR_CLO)
        {
            if (CurrentToken.Type != Token.Types.IDENT)
            {
                throw new SyntaxErrorException(["IDENTIFIER"], CurrentToken);
            }
            string paramName = CurrentToken.Value;
            incrementIndex();

            if (CurrentToken.Type != Token.Types.D_COLON)
            {
                throw new SyntaxErrorException([":"], CurrentToken);
            }
            incrementIndex();

            TypeDefinitionNode? paramType = parseTypeDefinitionNode();
            if (paramType != null)
            {
                parameters.Add(new FuncParam(paramName, paramType));
            } 

            if (CurrentToken.Type == Token.Types.COMMA)
            {
                incrementIndex();
            }
            else if (CurrentToken.Type != Token.Types.D_PAR_CLO)
            {
                throw new SyntaxErrorException(["','", "')'"], CurrentToken);
            }
        }

        incrementIndex();

        return parameters;
    }

    private TypeDefinitionNode? getReturnType()
    {
        if (CurrentToken.Type != Token.Types.OP_RIGHT_OP)
        {
            return null;
        }
        incrementIndex();

        return parseTypeDefinitionNode();
    }

    private TypeDefinitionNode? parseTypeDefinitionNode()
    {
        ParseNode? varDefNode = delegateToHandler(varDefinitionHandler);
        if (varDefNode == null)
        {
            return null;
        }
        return (TypeDefinitionNode)varDefNode;
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