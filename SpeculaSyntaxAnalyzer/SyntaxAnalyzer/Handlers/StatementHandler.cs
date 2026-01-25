using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Parses a single statement (not multiple statements like BodyHandler)
/// </summary>
public class StatementHandler : Handler
{
    private readonly IdentifierStartHandler identifierStartHandler;
    private readonly ExpressionHandler expressionHandler;
    public StatementHandler(ErrorsHandler err) : base(err)
    {
        expressionHandler = new(err);
        identifierStartHandler = new(err);
    }

    protected override ParseNode? verifyTokens()
    {
        switch (CurrentToken.Type)
        {
            case Token.Types.K_LET:
                return handleDeclarationStmt();
            case Token.Types.IDENT:
                int errorCountBefore = errorHandler.ErrorList.Count;
                ParseNode? identResult = tryHandleIdentifierStartWithResult();
                if (identResult != null)
                {
                    return identResult;
                }
                
                // Clear errors from failed identifier start attempt
                while (errorHandler.ErrorList.Count > errorCountBefore)
                {
                    errorHandler.ErrorList.RemoveAt(errorHandler.ErrorList.Count - 1);
                }

                int errorsBeforeExpression = errorHandler.ErrorList.Count;

                // Try expression with error recording to get actual error
                ParseNode? exprResult = tryHandleExpressionStmtRecordingErrors();
                if (exprResult != null)
                {
                    return exprResult;
                }
                
                if (errorHandler.ErrorList.Count == errorsBeforeExpression)
                {
                    throw new SyntaxErrorException(
                        ["assignment", "expression"],
                        CurrentToken);
                }
                return null;
            case Token.Types.K_IF:
                return handleIfStatement();
            case Token.Types.K_FOR:
                return handleForLoop();
            case Token.Types.K_DO:
                return handleDoWhileLoop();
            case Token.Types.K_WHILE:
                return handleWhileLoop();
            default:
                ParseNode? stmtResult = tryHandleExpressionStmt();
                if (stmtResult != null)
                {
                    return stmtResult;
                }
                throw new SyntaxErrorException(
                    ["statement", "expression", "declaration"],
                    CurrentToken);
        }
    }

    private ParseNode? handleDeclarationStmt()
    {
        return delegateToHandler(new DeclarationHandler(errorHandler));
    }

    private ParseNode? handleIfStatement()
    {
        return delegateToHandler(new ConditionalStatementHandler(errorHandler));
    }

    private ParseNode? handleForLoop()
    {
        return delegateToHandler(new ForLoopHandler(errorHandler));
    }

    private ParseNode? handleDoWhileLoop()
    {
        return delegateToHandler(new DoWhileLoopHandler(errorHandler));
    }

    private ParseNode? handleWhileLoop()
    {
        return delegateToHandler(new WhileLoopHandler(errorHandler));
    }

    private ParseNode? tryHandleIdentifierStartWithResult()
    {
        return tryDelegateToHandler(identifierStartHandler);
    }

    private ParseNode? tryHandleExpressionStmt()
    {
        try
        {
            return tryDelegateToHandler(expressionHandler);
        }
        catch (SyntaxErrorException)
        {
            return null;
        }
    }

    /// <summary>
    /// Tries to parse an expression statement, recording all errors
    /// Returns the expression if successful, null otherwise (errors will be recorded)
    /// </summary>
    private ParseNode? tryHandleExpressionStmtRecordingErrors()
    {
        try
        {
            return delegateToHandler(new ExpressionHandler(errorHandler));
        }
        catch (SyntaxErrorException)
        {
            // Expression parsing failed, errors should be recorded
            return null;
        }
    }
}
