namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class SyntaxAnalyzer
{
    private enum States
    {
        START,
        DECL
    }
    private States currentState = States.START;

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
                break;
        }
    }
}
