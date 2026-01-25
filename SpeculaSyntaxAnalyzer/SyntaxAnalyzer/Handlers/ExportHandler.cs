using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ExportHandler : Handler
{
    private readonly StatementHandler statementHandler;
    public ExportHandler(ErrorsHandler errors) : base(errors)
    {
        statementHandler = new StatementHandler(errors);
    }

    protected override ParseNode? verifyTokens()
    {
        if (CurrentToken.Type != Token.Types.D_BRAC_OP)
        {
            throw new SyntaxErrorException(["["], CurrentToken);
        }
        incrementIndex();
        switch (CurrentToken.Type)
        {
            case Token.Types.K_EXPORT:
                return handleExportStatement();
            case Token.Types.K_EXPORT_DEFAULT:
                return handleExportDefaultStatement();
            default:
                throw new SyntaxErrorException(["export", "export default"], CurrentToken);
        }
    }

    private ExportModuleNode? handleExportStatement()
    {
        incrementIndex();
        if (CurrentToken.Type != Token.Types.D_BRAC_CLO)
        {
            throw new SyntaxErrorException(["]"], CurrentToken);
        }
        incrementIndex();
        ParseNode? stmt = delegateToHandler(statementHandler);
        if (stmt == null) return null;
        return new ExportNode((Statement)stmt);
    }

    private ExportModuleNode? handleExportDefaultStatement()
    {
        incrementIndex();
        if (CurrentToken.Type != Token.Types.D_BRAC_CLO)
        {
            throw new SyntaxErrorException(["]"], CurrentToken);
        }
        incrementIndex();
        ParseNode? stmt = delegateToHandler(statementHandler);
        if (stmt == null) return null;
        return new ExportDefaultNode((Statement)stmt);
    }
}