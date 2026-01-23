using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Used for recognizing variable assignment statements
/// </summary>
public class VarAssignHandler : Handler
{
    private readonly ExpressionHandler expressionHandler;
    public VarAssignHandler(ErrorsHandler errors) : base(errors)
    {
        expressionHandler = new(errors);
    }

    protected override ParseNode? verifyTokens()
    {
        if (CurrentToken.Type != Token.Types.IDENT)
        {
            throw new SyntaxErrorException(["IDENTIFIER"], CurrentToken);
        }
        incrementIndex();
        switch (CurrentToken.Type)
        {
            case Token.Types.OP_EQUALS:
            case Token.Types.OP_PLUS_EQ:
            case Token.Types.OP_MINUS_EQ:
            case Token.Types.OP_MULT_EQ:
            case Token.Types.OP_DIV_EQ:
            case Token.Types.OP_MOD_EQ:
                incrementIndex();
                return handleExpressionAfterOperator();
        }

        return null;
    }
    private ParseNode? handleExpressionAfterOperator()
    {
        ParseNode? value = delegateToHandler(expressionHandler);
        if (value is null || value is not Expression exprValue)
        {
            throw new SyntaxErrorException(["Expression"], CurrentToken);
        }
        return exprValue;
    }
}