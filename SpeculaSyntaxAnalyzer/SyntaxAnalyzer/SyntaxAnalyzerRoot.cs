using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class SyntaxAnalyzerRoot
{
    private List<ParseNode> root;

    public readonly ErrorsHandler ErrorHandler;

    private int i = 0;

    public SyntaxAnalyzerRoot()
    {
        ErrorHandler = new();
        root = new();
    }

    public List<ParseNode> ReadTokens(List<Token> tokens)
    {
        while (i < tokens.Count)
        {
            ParseNode? node = tokens[i].Type switch
            {
                Token.Types.K_LET => delegateToHandler(new DeclarationHandler(ErrorHandler), tokens),
                _ => throw new SyntaxErrorException(["Start Symbol"], tokens[i])
            };
            if (node != null)
            {
                root.Add(node);
            }
            i++;
        }
        return root;
    }

    private ParseNode? delegateToHandler(Handler handler, List<Token> token)
    {
        try
        {
            HandlerOutput output = handler.HandleToken(token, i);
            i = output.endIndex;
            return output.node;
        }
        catch (SyntaxErrorException ex)
        {
            ErrorHandler.AddError(ex);
        }
        return null;
    }

}
