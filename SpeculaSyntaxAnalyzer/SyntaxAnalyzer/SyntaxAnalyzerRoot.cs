using System.Reflection;
using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class SyntaxAnalyzerRoot
{
    public readonly ErrorsHandler ErrorHandler;
    private readonly FuncDefHandler funcDefHandler;
    private readonly DeclarationHandler declarationHandler;
    private readonly ImportsHandler importsHandler;
    
    private PrintableList<RootStatement> statements = new();
    private List<Token> tokens = [];
    private int i = 0;

    public SyntaxAnalyzerRoot()
    {
        ErrorHandler = new();
        funcDefHandler = new FuncDefHandler(ErrorHandler);
        declarationHandler = new DeclarationHandler(ErrorHandler);
        importsHandler = new ImportsHandler(ErrorHandler);
    }

    public ParseNode? ReadTokens(List<Token> tokens)
    {
        this.tokens = tokens;
        this.i = 0;
        statements = new PrintableList<RootStatement>();

        while (i < tokens.Count)
        {
            RootStatement? stmt = parseRootStatement();
            if (stmt != null)
            {
                statements.Add(stmt);
            }
        }

        return new RootNode(statements);
    }

    private RootStatement? parseRootStatement()
    {
        if (i >= tokens.Count)
            return null;

        Token currentToken = tokens[i];

        switch (currentToken.Type)
        {
            case Token.Types.K_FN:
            case Token.Types.K_THREAD:
                return handleFuncDef();
            case Token.Types.K_LET:
                return handleDeclaration();
            case Token.Types.K_IMPORT:
                return handleImport();
            default:
                throw new SyntaxErrorException(
                    ["fn", "thread", "let", "import"],
                    currentToken);
        }
    }

    private RootStatement? handleFuncDef()
    {
        return (RootStatement?)delegateToHandler(funcDefHandler);
    }

    private RootStatement? handleDeclaration()
    {
        RootStatement? stmt = (RootStatement?)delegateToHandler(declarationHandler);
        // Declaration handler stops at semicolon, consume it
        if (stmt != null && i < tokens.Count && tokens[i].Type == Token.Types.D_SEMICOLON)
        {
            i++;
        }
        return stmt;
    }

    private ParseNode? delegateToHandler(Handler handler)
    {
        try
        {
            HandlerOutput output = handler.HandleToken(tokens, i);
            i = output.endIndex;
            return output.node;
        }
        catch (SyntaxErrorException ex)
        {
            ErrorHandler.AddError(ex);
            while (i < tokens.Count && tokens[i].Type != Token.Types.D_SEMICOLON)
            {
                i++;
            }
            if (i < tokens.Count && tokens[i].Type == Token.Types.D_SEMICOLON)
            {
                i++;
            }
        }
        return null;
    }

    private RootStatement? handleImport()
    {
        return (RootStatement?)delegateToHandler(importsHandler);
    }
}
