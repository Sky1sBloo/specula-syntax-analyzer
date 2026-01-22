namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class SyntaxAnalyzerRoot 
{
    private enum States
    {
        START,
        DECL
    }
    private States currentState = States.START;

    private DeclarationStatementHandler declarationStatementHandler;

    public SyntaxAnalyzerRoot()
    {
        declarationStatementHandler = new();

        declarationStatementHandler.Finished += backToStartState;
    }

    private void backToStartState()
    {
        currentState = States.START;
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
        switch (currentState)
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
                currentState = States.DECL;
                declarationStatementHandler.handleToken(token);
                break;
        }
    }

    private void handleDeclState(Token token)
    {
        declarationStatementHandler.handleToken(token);
    }

}
