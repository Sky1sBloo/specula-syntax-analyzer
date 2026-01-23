using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Contains conditions 
/// </summary>
public class ForLoopHandler : Handler
{
    private readonly ExpressionHandler expressionHandler;
    private readonly BodyHandler bodyHandler;
    public ForLoopHandler(ErrorsHandler errors) : base(errors)
    {
        expressionHandler = new(errors);
        bodyHandler = new(errors);
    }

    protected override ParseNode? verifyTokens()
    {
        if (CurrentToken.Type != Token.Types.K_FOR)
        {
            throw new SyntaxErrorException(["for"], CurrentToken);
        }
        incrementIndex();
        if (CurrentToken.Type != Token.Types.D_PAR_OP)
        {
            throw new SyntaxErrorException(["("], CurrentToken);
        }
        incrementIndex();
        return null;
    }
}
