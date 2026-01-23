using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class DeclarationHandler : Handler
{
    private readonly DataTypeHandler dataTypeHandler;

    public DeclarationHandler(ErrorsHandler errors) : base(errors)
    {
        dataTypeHandler = new(errors);
    }

    protected override ParseNode? verifyTokens()
    {
        string identifier = "";
        TypeNode? dataType = null;
        ValueNode? value = null;
        if (CurrentToken.Type != Token.Types.K_LET)
        {
            throw new SyntaxErrorException(["let"], CurrentToken);
        }
        incrementIndex();
        if (CurrentToken.Type != Token.Types.IDENT)
        {
            throw new SyntaxErrorException(["IDENTIFIER"], CurrentToken);
        }
        identifier = CurrentToken.Value;
        incrementIndex();
        switch (CurrentToken.Type)
        {
            case Token.Types.D_COLON:
                dataType = sawColon();
                break;
            case Token.Types.OP_EQUALS:
                break;
        }
        incrementIndex();
        switch (CurrentToken.Type)
        {
            case Token.Types.D_SEMICOLON:
                {
                    if (dataType == null) throw new SyntaxErrorException(["Definition of datatype"], CurrentToken);
                    return new DeclarationStatementNode(identifier, dataType, new LiteralValue(new TypeNode(DataTypes.NULL), "null"));
                }
            default:
                throw new SyntaxErrorException([";", "="], CurrentToken);
        }
        //return new DeclarationStatementNode(identifier, dataType, value);
    }

    private TypeNode? sawColon()
    {
        ParseNode? parseNode = delegateToHandler(dataTypeHandler);
        if (parseNode == null) return null;
        if (parseNode is TypeNode typeNode)
        {
            return typeNode;
        }
        else throw new InvalidOperationException($"Expected type node. received: {parseNode}");
    }
}
