using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Contains everything that is within {} with expressions
/// </summary>
public class BodyHandler : Handler
{
    private List<Statement> statements = new();
    public BodyHandler(ErrorsHandler err) : base(err)
    {
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
                    handleVarAssignStmt();
                    break;
                case Token.Types.K_IF:
                    handleIfStatement();
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

    private void handleVarAssignStmt()
    {
        ParseNode? node = delegateToHandler(new VarAssignHandler(errorHandler));
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
            throw new InvalidOperationException("Node is not expression for var assign");
        }
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
}
