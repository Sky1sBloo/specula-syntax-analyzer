using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Handles listener body which can contain regular statements, respond nodes, and fail statements
/// </summary>
public class ListenerBodyHandler : Handler
{
    private PrintableList<ListenerBodyContent> statements = new();
    private readonly StatementHandler statementHandler;
    private readonly ListenerRespondHandler respondHandler;
    private readonly ListenerFailHandler failHandler;
    
    public ListenerBodyHandler(ErrorsHandler err) : base(err)
    {
        statementHandler = new StatementHandler(err);
        respondHandler = new ListenerRespondHandler(err);
        failHandler = new ListenerFailHandler(err);
    }

    protected override ParseNode? verifyTokens()
    {
        // reset statements for each new body parse to avoid leaking previous state
        statements = new PrintableList<ListenerBodyContent>();
        expectTokenType(Token.Types.D_CBRAC_OP);
        
        int initialErrorCount = errorHandler.ErrorList.Count;
        
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
                incrementIndex(); 
                break;
            }

            // Check for 'respond' keyword
            if (CurrentToken.Type == Token.Types.K_RESPOND)
            {
                ParseNode? respondNode = delegateToHandler(respondHandler);
                if (respondNode != null)
                {
                    statements.Add((ListenerBodyContent)respondNode);
                }
                continue;
            }

            // Check for 'fail' keyword
            if (CurrentToken.Type == Token.Types.K_FAIL)
            {
                ParseNode? failNode = delegateToHandler(failHandler);
                if (failNode != null)
                {
                    statements.Add((ListenerBodyContent)failNode);
                }
                continue;
            }

            // Delegate to StatementHandler for regular statements
            ParseNode? stmt = delegateToHandler(statementHandler);
            if (stmt != null)
            {
                statements.Add((ListenerBodyContent)(Statement)stmt);
            }
        }
        
        if (errorHandler.ErrorList.Count > initialErrorCount)
        {
            return null;
        }
        
        return new ListenerBodyNode(statements);
    }
}
