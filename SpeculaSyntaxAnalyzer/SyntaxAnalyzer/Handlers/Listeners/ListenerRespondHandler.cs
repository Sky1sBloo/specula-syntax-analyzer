using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Handles listener respond statement: respond Ack(true, a);
/// </summary>
public class ListenerRespondHandler : Handler
{
    private readonly FuncParamsHandler paramsHandler;
    
    public ListenerRespondHandler(ErrorsHandler err) : base(err)
    {
        paramsHandler = new FuncParamsHandler(err);
    }

    protected override ParseNode? verifyTokens()
    {
        // expect 'respond' keyword
        assertTokenType(Token.Types.K_RESPOND);
        incrementIndex();
        
        // expect identifier (response name)
        assertTokenType(Token.Types.IDENT);
        string responseName = CurrentToken.Value;
        incrementIndex();
        
        FuncParams? parameters;
        
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
        
        return new ListenerEventRespondNode(responseName, parameters);
    }
}
