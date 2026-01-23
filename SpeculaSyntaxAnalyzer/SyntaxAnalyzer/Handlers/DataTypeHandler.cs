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
                return new TypeNode(CurrentToken.Value);
            default:
                throw new SyntaxErrorException(["TYPE", "IDENTIFIER"], CurrentToken);
        }
    }
}
