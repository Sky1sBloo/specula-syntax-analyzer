using SpeculaSyntaxAnalyzer.ParseTree;
namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public abstract class Handler
{
    public event Action<ParseNode>? Finished;
    public event Action<SyntaxAnalyzerRoot.States>? DelegateToState;

    public abstract void handleToken(Token token, ParseNode? prevNode);
    protected void SetHandlerFinished(ParseNode node) => Finished?.Invoke(node);
    protected void SetDelegateToState(SyntaxAnalyzerRoot.States state) => DelegateToState?.Invoke(state);
}
