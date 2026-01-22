namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class SyntaxAnalyzerRoot 
{
    public enum States
    {
        START,
        DECL
    }
    public States CurrentState { get; private set; } = States.START;

    private DeclarationStatementHandler declarationStatementHandler;

    public SyntaxAnalyzerRoot()
    {
        declarationStatementHandler = new();

        declarationStatementHandler.Finished += backToStartState;
    }

    private void backToStartState()
    {
        CurrentState = States.START;
    }

    public void ReadTokens(List<Token> tokens)
    {
        foreach (var token in tokens)
        {
        }
    }

    /// For state handlers 
    private void handleToken(Token token)
    {
        switch (CurrentState)
        {
            case States.START:
                handleStartState(token);
                break;
            case States.DECL:
                handleDeclState(token);
                break;
        }
    }

    private void handleStartState(Token token)
    {
        switch (token.Type)
        {
            case Token.Types.K_LET:
                CurrentState = States.DECL;
                declarationStatementHandler.handleToken(token);
                break;
        }
    }

    private void handleDeclState(Token token)
    {
        declarationStatementHandler.handleToken(token);
    }

}
