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
        VALUE,
        SEMI_COLON
    }

    private States currentState = States.LET;
    private string identifier = "";
    private string dataType = "";

    public override void handleToken(Token token)
    {
        switch (currentState)
        {
            case States.LET:
                handleLetState(token);
                break;
            case States.IDENT:
                handleIdentState(token);
                break;
            case States.COLON:
                handleColonState(token);
                break;
            case States.TYPE:
                handleTypeState(token);
                break;
            case States.EQUALS:
                handleEqualsState(token);
                break;
            case States.VALUE:
                break;
        }
    }

    private void reset()
    {
        currentState = States.LET;
        identifier = "";
        dataType = "";
    }

    private void handleLetState(Token token)
    {
        if (token.Type != Token.Types.IDENT)
        {
            throw new SyntaxErrorException(["IDENTIFIER"], token);
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
                throw new SyntaxErrorException(["':'", "'='"], token);
        }
    }

    private void handleColonState(Token token)
    {
        switch (token.Type)
        {
            case Token.Types.IDENT:
            case Token.Types.K_TYPE:
            case Token.Types.L_FLOAT:
            case Token.Types.L_DOUBLE:
            case Token.Types.L_CHAR:
            case Token.Types.L_STRING:
            case Token.Types.L_BOOL:
                dataType = token.Value;
                currentState = States.TYPE;
                break;

            // todo handle capabilities
            default:
                throw new SyntaxErrorException(["TYPE", "["], token);
        }
    }

    private void handleTypeState(Token token)
    {
        switch (token.Type)
        {
            case Token.Types.OP_EQUALS:
                currentState = States.EQUALS;
                break;
            case Token.Types.D_SEMICOLON:
                reset();
                break;
            default:
                throw new SyntaxErrorException(["'='", "'[", ";"], token);
        }
    }

    private void handleEqualsState(Token token)
    {
        // todo handle expressions
        switch (token.Type)
        {
            case Token.Types.IDENT:
            case Token.Types.L_INT:
            case Token.Types.L_FLOAT:
            case Token.Types.L_DOUBLE:
            case Token.Types.L_CHAR:
            case Token.Types.L_STRING:
            case Token.Types.L_BOOL:
            case Token.Types.L_NULL:
                currentState = States.SEMI_COLON;
                break;
            default:
                throw new SyntaxErrorException(["VALUE", "EXPRESSION"], token);
        }
    }

    private void handleSemiColonState(Token token)
    {
        if (token.Type != Token.Types.D_SEMICOLON)
        {
            throw new SyntaxErrorException([";"], token);
        }
        reset();
    }
}
