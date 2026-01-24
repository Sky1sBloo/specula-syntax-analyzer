using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Contains everything that is within {} with expressions
/// </summary>
public class BodyHandler : Handler
{
    private List<Statement> statements = new();
    private readonly ExpressionHandler expressionHandler;
    public BodyHandler(ErrorsHandler err) : base(err)
    {
        expressionHandler = new(err);
    }

    protected override ParseNode? verifyTokens()
    {
        // reset statements for each new body parse to avoid leaking previous state
        statements = new();
        if (CurrentToken.Type != Token.Types.D_CBRAC_OP)
        {
            throw new SyntaxErrorException(["{"], CurrentToken);
        }
        incrementIndex();
        while (true)
        {
            if (!HasMoreTokens)
            {
                Token missing = new()
                {
                    Type = Token.Types.UNKNOWN,
                    Line = getIndex(),
                    CharStart = 0,
                    CharEnd = 0
                };
                throw new SyntaxErrorException(["}"], missing);
            }

            if (CurrentToken.Type == Token.Types.D_CBRAC_CLO)
            {
                break;
            }

            switch (CurrentToken.Type)
            {
                case Token.Types.K_LET:
                    handleDeclarationStmt();
                    break;
                case Token.Types.IDENT:
                    if (!handleVarAssignStmt())
                    {
                        handleExpressionStmt();
                    }
                    break;
                case Token.Types.K_IF:
                    handleIfStatement();
                    break;
                case Token.Types.K_FOR:
                    handleForLoop();
                    break;
                default:
                    if (!tryHandleExpressionStmt())
                    {
                        throw new SyntaxErrorException(
                            ["statement", "expression", "declaration"],
                            CurrentToken);
                    }
                    break;
            }
            /// todo
            /// var ass
            /// if
            /// for
            /// while
            /// do
            /// call func
            /// spawn thread

            incrementIndex();
        }
        return new BodyNode(statements);
    }

    private void handleDeclarationStmt()
    {
        ParseNode? node = delegateToHandler(new DeclarationHandler(errorHandler));
        if (node == null)
        {
            return;
        }
        if (node is DeclarationStatementNode declarationStatementNode)
        {
            statements.Add(declarationStatementNode);
        }
        else
        {
            throw new InvalidOperationException("Node is not declaration statement");
        }
    }

    private bool handleVarAssignStmt()
    {
        ParseNode? node = tryDelegateToHandler(new VarAssignHandler(errorHandler));
        if (node == null)
        {
            return false;
        }
        if (node is Statement statementNode)
        {
            statements.Add(statementNode);
        }
        else
        {
            throw new InvalidOperationException("Node is not expression for var assign");
        }
        return true;
    }

    private void handleIfStatement()
    {
        ParseNode? node = delegateToHandler(new IfStatementHandler(errorHandler));
        if (node == null)
        {
            return;
        }
        if (node is Statement statementNode)
        {
            statements.Add(statementNode);
        }
        else
        {
            throw new InvalidOperationException("Node is not if statement");
        }
    }

    private void handleForLoop()
    {
        ParseNode? node = delegateToHandler(new ForLoopHandler(errorHandler));
        if (node == null)
        {
            return;
        }
        if (node is Statement statementNode)
        {
            statements.Add(statementNode);
        }
        else
        {
            throw new InvalidOperationException("Node is not for loop");
        }
    }

    private bool handleExpressionStmt()
    {
        ParseNode? node = delegateToHandler(new ExpressionHandler(errorHandler));
        if (node == null)
        {
            return false;
        }
        if (node is Expression expressionNode)
        {
            statements.Add(expressionNode);
        }
        else
        {
            throw new InvalidOperationException("Node is not expression statement");
        }
        return true;
    }

    /// <summary>
    /// Tries to parse an expression statement without recording errors
    /// Returns true if successful, false otherwise
    /// </summary>
    private bool tryHandleExpressionStmt()
    {
        ParseNode? result = tryDelegateToHandler(expressionHandler);
        if (result == null)
        {
            return false;
        }

        if (result is Expression expressionNode)
        {
            statements.Add(expressionNode);
            return true;
        }
        return false;
    }
}
