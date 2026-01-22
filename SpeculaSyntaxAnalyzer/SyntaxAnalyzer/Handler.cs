namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public abstract class Handler
{
    public event Action? Finished;

    public abstract void handleToken(Token token);
    protected void SetHandlerFinished() => Finished?.Invoke();
}
