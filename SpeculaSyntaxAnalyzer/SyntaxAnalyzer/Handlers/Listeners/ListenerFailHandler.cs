using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Handles listener fail statement: fail low_battery; or fail low_battery(true, a);
/// </summary>
public class ListenerFailHandler : Handler
{
    private readonly FuncParamsHandler paramsHandler;
    
    public ListenerFailHandler(ErrorsHandler err) : base(err)
    {
        paramsHandler = new FuncParamsHandler(err);
    }

    protected override ParseNode? verifyTokens()
    {
        // expect 'fail' keyword
        assertTokenType(Token.Types.K_FAIL);
        incrementIndex();
        
        // expect identifier (fail name)
        assertTokenType(Token.Types.IDENT);
        string failName = CurrentToken.Value;
        incrementIndex();
        
        FuncParams? parameters = null;
        
        // Check if there are parameters
        if (HasMoreTokens && CurrentToken.Type == Token.Types.D_PAR_OP)
        {
            parameters = (FuncParams?)delegateToHandler(paramsHandler);
            if (parameters == null) return null;
        }
        else
        {
            parameters = new FuncParams(new PrintableList<FuncParam>());
        }
        
        // expect semicolon
        assertTokenType(Token.Types.D_SEMICOLON);
        incrementIndex();
        
        return new ListenerFail(failName, parameters);
    }
}
