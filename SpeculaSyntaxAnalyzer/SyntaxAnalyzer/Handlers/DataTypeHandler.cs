using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class DataTypeHandler : Handler
{
    public DataTypeHandler(ErrorsHandler errors) : base(errors) { }

    protected override ParseNode? verifyTokens()
    {
        if (CurrentToken.Type != Token.Types.D_COLON)
        {
            throw new SyntaxErrorException([":"], CurrentToken);
        }
        incrementIndex();
        switch (CurrentToken.Type)
        {
            case Token.Types.K_TYPE:
            case Token.Types.IDENT:
                return new TypeNode(tokenToDataType(CurrentToken));
            default:
                throw new SyntaxErrorException(["TYPE", "IDENTIFIER"], CurrentToken);
        }
    }

    private DataTypes tokenToDataType(Token token)
    {
        if (token.Type == Token.Types.K_TYPE)
        {
            return token.Value switch {
                "int" => DataTypes.INT,
                "float" => DataTypes.FLOAT,
                "double" => DataTypes.DOUBLE,
                "bool" => DataTypes.BOOL,
                "char" => DataTypes.CHAR,
                "void" => DataTypes.VOID,
                _ => DataTypes.UNKNOWN
            };
        }
        if (token.Type == Token.Types.L_NULL)
        {
            return DataTypes.NULL;
        }
        if (token.Type == Token.Types.IDENT)
        {
            return DataTypes.IDENTIFIER;
        }
        throw new ArgumentException("Token is not a valid data type");
    }
}
