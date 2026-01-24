using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Contains conditions 
/// </summary>
public class IfStatementHandler : Handler
{
    private readonly ExpressionHandler expressionHandler;
    private readonly BodyHandler bodyHandler;

    public IfStatementHandler(ErrorsHandler errors) : base(errors)
    {
        expressionHandler = new(errors);
        bodyHandler = new(errors);
    }

    protected override ParseNode? verifyTokens()
    {
        if (CurrentToken.Type != Token.Types.K_IF)
        {
            throw new SyntaxErrorException(["if"], CurrentToken);
        }
        incrementIndex();
        if (CurrentToken.Type != Token.Types.D_PAR_OP)
        {
            SyntaxErrorException ex = new SyntaxErrorException(["("], CurrentToken);
            errorHandler.AddError(ex);
            throw ex;
        }

        ParseNode? conditionNode = delegateToHandler(expressionHandler);
        if (conditionNode is null || conditionNode is not Expression conditionExpr)
        {
            throw new SyntaxErrorException(["Expression"], CurrentToken);
        }

        ParseNode? bodyNode = delegateToHandler(bodyHandler);
        if (bodyNode is null || bodyNode is not BodyNode body)
        {
            throw new SyntaxErrorException(["Body"], CurrentToken);
        }

        return new IfStatementNode(conditionExpr, body);
    }
}