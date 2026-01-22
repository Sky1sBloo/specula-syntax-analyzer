namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public abstract class Handler
{
    public event Action? Finished;
    public event Action<SyntaxAnalyzerRoot.States>? DelegateToState;

    public abstract void handleToken(Token token);
    protected void SetHandlerFinished() => Finished?.Invoke();
    protected void SetDelegateToState(SyntaxAnalyzerRoot.States state) => DelegateToState?.Invoke(state);
}
