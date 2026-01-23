using SpeculaSyntaxAnalyzer.ParseTree;

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

    public ParseNode? prevHandleNode;

    private DeclarationStatementHandler declarationStatementHandler = new();
    private ValueHandler valueHandler = new();

    private Token? currentToken;

    public SyntaxAnalyzerRoot()
    {
        declarationStatementHandler.Finished += node => PopState(node);
        declarationStatementHandler.DelegateToState += newState => PushState(newState);

        valueHandler.Finished += node => PopState(node);

        StateStack.Push(States.START);
    }

    public ParseNode? ReadTokens(List<Token> tokens)
    {
        foreach (var token in tokens)
        {
            currentToken = token;
            handleToken(token);
        }

        if (prevHandleNode == null)
        {
            Errors.Add("Tokens did not produce an output");
        }
        return prevHandleNode;
    }

    /// Used to delegate current handler to another
    public void PushState(States newState)
    {
        StateStack.Push(newState);
    }

    public void PopState(ParseNode node)
    {
        StateStack.Pop();
        prevHandleNode = node;
        if (currentToken == null)
        {
            throw new InvalidOperationException("Tried to pop state with an empty token");
        }
        handleToken(currentToken);
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
                case States.VALUE:
                    handleValueState(token);
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
                declarationStatementHandler.handleToken(token, prevHandleNode);
                break;
        }
    }

    private void handleDeclState(Token token)
    {
        declarationStatementHandler.handleToken(token, prevHandleNode);
    }

    private void handleValueState(Token token)
    {
        valueHandler.handleToken(token, prevHandleNode);
    }
}
