namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class SyntaxAnalyzerRoot
{
    public enum States
    {
        START,
        DECL,
        VALUE,
        FUNC_CALL,
        STRUCT_BUILDER
    }

    public Stack<States> StateStack = new();
    public List<string> Errors { get; private set; } = new();

    private DeclarationStatementHandler declarationStatementHandler = new();
    private ValueHandler valueHandler = new();

    public SyntaxAnalyzerRoot()
    {
        declarationStatementHandler.Finished += () => StateStack.Pop();
        declarationStatementHandler.DelegateToState += newState => StateStack.Push(newState);
        valueHandler.Finished += () => {};
    }

    public void ReadTokens(List<Token> tokens)
    {
        foreach (var token in tokens)
        {
            handleToken(token);
        }
    }

    /// Used to delegate current handler to another
    public void PushState(States newState)
    {
        StateStack.Push(newState);
    }

    /// For state handlers 
    private void handleToken(Token token)
    {
        try
        {
            switch (StateStack.Peek())
            {
                case States.START:
                    handleStartState(token);
                    break;
                case States.DECL:
                    handleDeclState(token);
                    break;
            }
        }
        catch (SyntaxErrorException ex)
        {
            Errors.Add(ex.Message);
        }
    }

    private void handleStartState(Token token)
    {
        switch (token.Type)
        {
            case Token.Types.K_LET:
                PushState(States.DECL);
                declarationStatementHandler.handleToken(token);
                break;
        }
    }

    private void handleDeclState(Token token)
    {
        declarationStatementHandler.handleToken(token);
    }
}
