namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ValueHandler : Handler
{
    private enum States
    {
        START,
        FOUND_IDENT
    }

    private States currentState = States.START;
    public override void handleToken(Token token)
    {
        switch (currentState)
        {
            case States.START:
                handleStartState(token);
                break;
        }
    }

    public void handleStartState(Token token)
    {
        switch (token.Type)
        {
            case Token.Types.K_TYPE:
            case Token.Types.L_FLOAT:
            case Token.Types.L_DOUBLE:
            case Token.Types.L_CHAR:
            case Token.Types.L_STRING:
            case Token.Types.L_BOOL:
                end();
                break;
            case Token.Types.IDENT:
                currentState = States.FOUND_IDENT;
                break;
        }
    }

    public void handleIdentState(Token token)
    {
        switch (token.Type)
        {
            case Token.Types.D_PAR_OP:
                SetDelegateToState(SyntaxAnalyzerRoot.States.FUNC_CALL);
                break;
            case Token.Types.D_CBRAC_OP:
                SetDelegateToState(SyntaxAnalyzerRoot.States.STRUCT_BUILDER);
                break;
            default:
                end();
                break;
        }
    }

    private void end()
    {
        currentState = States.START;
    }
}

