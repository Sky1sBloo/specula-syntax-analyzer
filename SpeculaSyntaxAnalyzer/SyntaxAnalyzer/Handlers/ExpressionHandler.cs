using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Handles expression parsing with support for:
/// - Binary operators: +, -, *, /
/// - Comparison: ==
/// - Unary operators: ++, -- (pre and post)
/// - Bracket expressions
/// </summary>
public class ExpressionHandler : Handler
{
    private ValueHandler valueHandler;

    public ExpressionHandler(ErrorsHandler err) : base(err)
    {
        valueHandler = new ValueHandler(err);
    }

    protected override ParseNode? verifyTokens()
    {
        return ParseExpression();
    }

    /// <summary>
    /// Parses a complete expression (lowest precedence)
    /// Handles == operator
    /// </summary>
    private Expression ParseExpression()
    {
        Expression left = ParseAddSubtract();

        while (HasMoreTokens && CurrentToken.Type == Token.Types.OP_REL_EQ)
        {
            incrementIndex();
            Expression right = ParseAddSubtract();
            left = new AddExpression(left, right); // TODO: Replace with proper comparison expression
        }

        return left;
    }

    /// <summary>
    /// Parses addition and subtraction expressions
    /// </summary>
    private Expression ParseAddSubtract()
    {
        Expression left = ParseMultiplyDivide();

        while (HasMoreTokens && (CurrentToken.Type == Token.Types.OP_PLUS || CurrentToken.Type == Token.Types.OP_MINUS))
        {
            Token.Types op = CurrentToken.Type;
            incrementIndex();
            Expression right = ParseMultiplyDivide();

            left = op == Token.Types.OP_PLUS
                ? new AddExpression(left, right)
                : new SubExpression(left, right);
        }

        return left;
    }

    /// <summary>
    /// Parses multiplication and division expressions
    /// </summary>
    private Expression ParseMultiplyDivide()
    {
        Expression left = ParseUnary();

        while (HasMoreTokens && (CurrentToken.Type == Token.Types.OP_MULT || CurrentToken.Type == Token.Types.OP_DIVIDE))
        {
            Token.Types op = CurrentToken.Type;
            incrementIndex();
            Expression right = ParseUnary();

            left = op == Token.Types.OP_MULT
                ? new MultExpression(left, right)
                : new DivExpression(left, right);
        }

        return left;
    }

    /// <summary>
    /// Parses unary operators (pre-increment/decrement and unary +/-)
    /// </summary>
    private Expression ParseUnary()
    {
        switch (CurrentToken.Type)
        {
            case Token.Types.OP_INCR:
                incrementIndex();
                Expression preIncExpr = ParseUnary();
                return new PreIncExpression(preIncExpr);

            case Token.Types.OP_DECR:
                incrementIndex();
                Expression preDecExpr = ParseUnary();
                return new PreDecExpression(preDecExpr);

            case Token.Types.OP_PLUS:
                incrementIndex();
                Expression posExpr = ParseUnary();
                return new PrePosExpression(posExpr);

            case Token.Types.OP_MINUS:
                incrementIndex();
                Expression negExpr = ParseUnary();
                return new PreNegExpression(negExpr);

            default:
                return ParsePostfix();
        }
    }

    /// <summary>
    /// Parses postfix operators (post-increment/decrement) and primary expressions
    /// </summary>
    private Expression ParsePostfix()
    {
        Expression expr = ParsePrimary();

        while (HasMoreTokens && (CurrentToken.Type == Token.Types.OP_INCR || CurrentToken.Type == Token.Types.OP_DECR))
        {
            Token.Types op = CurrentToken.Type;
            incrementIndex();

            expr = op == Token.Types.OP_INCR
                ? new PostIncExpression(expr)
                : new PostDecExpression(expr);
        }

        return expr;
    }

    /// <summary>
    /// Parses primary expressions (values and bracketed expressions)
    /// </summary>
    private Expression ParsePrimary()
    {
        switch (CurrentToken.Type)
        {
            case Token.Types.D_PAR_OP:
                // Handle parenthesized expressions: (expression)
                incrementIndex();
                Expression parenExpr = ParseExpression();
                
                if (CurrentToken.Type != Token.Types.D_PAR_CLO)
                {
                    throw new SyntaxErrorException([")"], CurrentToken);
                }
                incrementIndex();
                return parenExpr;

            case Token.Types.D_BRAC_OP:
                // Handle array bracket expressions: [expression]
                incrementIndex();
                Expression bracketExpr = ParseExpression();
                
                if (CurrentToken.Type != Token.Types.D_BRAC_CLO)
                {
                    throw new SyntaxErrorException(["]"], CurrentToken);
                }
                incrementIndex();
                return bracketExpr;

            default:
                // Parse a value (literal, identifier, or function call)
                return (Expression?)delegateToHandler(valueHandler) 
                    ?? throw new SyntaxErrorException(["Value"], CurrentToken);
        }
    }
}

