using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class SyntaxAnalyzerRoot
{
    public readonly ErrorsHandler ErrorHandler;
    private BodyHandler bodyHandler;

    private int i = 0;

    public SyntaxAnalyzerRoot()
    {
        ErrorHandler = new();
        bodyHandler = new BodyHandler(ErrorHandler);
    }

    public ParseNode? ReadTokens(List<Token> tokens)
    {
        // Wrap tokens with { } to use BodyHandler
        List<Token> wrappedTokens =
        [
            new Token
            {
                Type = Token.Types.D_CBRAC_OP,
                Value = "{",
                Line = 0,
                CharStart = 0,
                CharEnd = 0
            },
            .. tokens,
            new Token
            {
                Type = Token.Types.D_CBRAC_CLO,
                Value = "}",
                Line = tokens.Count > 0 ? tokens[^1].Line : 0,
                CharStart = 0,
                CharEnd = 0
            },
        ];
        return delegateToHandler(bodyHandler, wrappedTokens);
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
            while (i < token.Count && token[i].Type != Token.Types.D_SEMICOLON)
            {
                i++;
            }
            if (i < token.Count && token[i].Type == Token.Types.D_SEMICOLON)
            {
                i++;
            }
        }
        return null;
    }
}
