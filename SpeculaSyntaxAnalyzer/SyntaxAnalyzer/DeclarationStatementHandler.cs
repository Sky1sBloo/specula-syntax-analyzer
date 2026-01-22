namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class DeclarationStatementHandler : Handler
{
    private enum States
    {
        LET,
        IDENT,
        COLON,
        TYPE,
        EQUALS,
        VALUE
    }

    private States currentState = States.LET;

    public void handleToken(Token token)
    {
        switch (currentState)
        {
            case States.LET:
                handleLetState(token);
                break;
            case States.IDENT:
                handleIdentState(token);
                break;
            case States.TYPE:
                break;
            case States.EQUALS:
                break;
            case States.VALUE:
                break;
        }
    }

    private void handleLetState(Token token)
    {
        if (token.Type != Token.Types.IDENT)
        {
            throw new SyntaxErrorException(token, $"Expected token IDENTIFIER. Received {token.Type.ToString()}");
        }
        currentState = States.IDENT;
    }

    private void handleIdentState(Token token)
    {
        switch (token.Type)
        {
            case Token.Types.D_COLON:
                currentState = States.COLON;
                break;
            case Token.Types.OP_EQUALS:
                currentState = States.EQUALS;
                break;
            default:
                throw new SyntaxErrorException(token, $"Expected token '='/':'. Received {token.Type.ToString()}");
        }
    }

    private void handleTypeState(Token token)
    {
        /*
        switch (token.Type)
        {
            case Token.Types.IDENT:
            case Token.Types.K_INT
        } */
    }
}
