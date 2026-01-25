using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class WhileLoopHandler : Handler
{
    private readonly BodyHandler bodyHandler;
    private readonly ExpressionHandler expressionHandler;
    public WhileLoopHandler(ErrorsHandler errors) : base(errors)
    {
        bodyHandler = new(errors);
        expressionHandler = new(errors);
    }

    protected override ParseNode? verifyTokens()
    {
        if (CurrentToken.Type != Token.Types.K_WHILE)
        {
            throw new SyntaxErrorException(["while"], CurrentToken);
        }
        incrementIndex();
        Expression? condition = handleCondition();
        if (condition == null)
        {
            return null;
        }

        BodyNode? body = handleBody();
        if (body == null)
        {
            return null;
        }
        return new WhileLoop(condition, body);
    }

    private Expression? handleCondition()
    {
        if (CurrentToken.Type != Token.Types.D_PAR_OP)
        {
            throw new SyntaxErrorException(["("], CurrentToken);
        }
        incrementIndex();
        Expression? condition = (Expression?)delegateToHandler(expressionHandler);
        if (CurrentToken.Type != Token.Types.D_PAR_CLO)
        {
            throw new SyntaxErrorException([")"], CurrentToken);
        }
        incrementIndex();
        return condition;
    }

    private BodyNode? handleBody()
    {
        BodyNode? body = (BodyNode?)delegateToHandler(bodyHandler);
        return body!;
    }
}