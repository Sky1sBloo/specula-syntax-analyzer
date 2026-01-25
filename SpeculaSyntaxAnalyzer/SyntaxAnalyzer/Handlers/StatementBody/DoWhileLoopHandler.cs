using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class DoWhileLoopHandler : Handler
{
    private readonly BodyHandler bodyHandler;
    private readonly ExpressionHandler expressionHandler;
    public DoWhileLoopHandler(ErrorsHandler errors) : base(errors)
    {
        bodyHandler = new(errors);
        expressionHandler = new(errors);
    }

    protected override ParseNode? verifyTokens()
    {
        if (CurrentToken.Type != Token.Types.K_DO)
        {
            throw new SyntaxErrorException(["do"], CurrentToken);
        }
        incrementIndex();
        BodyNode? body = handleBody();
        if (body == null)
        {
            return null;
        }
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
        if (CurrentToken.Type != Token.Types.D_SEMICOLON)
        {
            throw new SyntaxErrorException([";"], CurrentToken);
        }
        incrementIndex();

        return new DoWhileLoop(body, condition);
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