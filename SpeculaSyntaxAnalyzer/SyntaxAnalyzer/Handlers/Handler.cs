using SpeculaSyntaxAnalyzer.ParseTree;
namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public struct HandlerOutput
{
    public int endIndex;
    public ParseNode? node;
}

public abstract class Handler
{
    private int i;
    protected readonly ErrorsHandler errorHandler;
    private List<Token> tokens = new();

    protected Token CurrentToken
    {
        get
        {
            if (i >= tokens.Count)
            {
                throw new InvalidOperationException($"Attempted to access token at index {i}, but only {tokens.Count} tokens available");
            }
            return tokens.ElementAt(i);
        }
    }
    protected bool HasMoreTokens => i < tokens.Count;

    public Handler(ErrorsHandler errorHandler)
    {
        this.errorHandler = errorHandler;
    }

    /// <summary>
    /// Provides the handler to handle the given token
    /// </summary>
    ///
    /// <returns> The index where it ends
    public HandlerOutput HandleToken(List<Token> tokens, int iStart)
    {
        this.tokens = tokens;
        skipToIndex(iStart);
        HandlerOutput output;
        output.node = verifyTokens();
        output.endIndex = getIndex();
        return output;
    }


    /// <summary>
    /// Verifies the token list if it follows the definition
    /// </summary>
    protected abstract ParseNode? verifyTokens();

    /// <summary>
    /// Delegates the current index to to the handler
    /// Automatically moves the index on the end of the handler
    /// </summary>
    protected ParseNode? delegateToHandler(Handler other)
    {
        try
        {
            HandlerOutput output = other.HandleToken(tokens, i);
            i = output.endIndex;
            return output.node;
        }
        catch (SyntaxErrorException ex)
        {
            errorHandler.AddError(ex);
            while (CurrentToken.Type != Token.Types.D_SEMICOLON)
            {
                incrementIndex();
            }
        }
        return null;
    }

    /// <summary>
    /// Delegates to a handler silently (without recording errors)
    /// Returns null if parsing fails and restores the original index
    /// </summary>
    protected ParseNode? delegateToHandlerSilently(Handler other)
    {
        int originalIndex = i;
        errorHandler.SuppressErrors = true;
        try
        {
            HandlerOutput output = other.HandleToken(tokens, i);
            i = output.endIndex;
            return output.node;
        }
        catch (SyntaxErrorException)
        {
            // Restore index on failure
            i = originalIndex;
            return null;
        }
        finally
        {
            errorHandler.SuppressErrors = false;
        }
    }

    protected int getIndex() { return i; }

    /// <summary>
    /// Increments the given index to the next
    /// </summary>
    protected void incrementIndex() { i++; }

    /// <summary>
    /// Skips to to the specified index 
    /// </summary>
    protected void skipToIndex(int i) { this.i = i; }
}
