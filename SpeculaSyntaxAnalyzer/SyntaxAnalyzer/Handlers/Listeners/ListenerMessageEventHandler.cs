using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ListenerMessageEventHandler : Handler
{
    private readonly ListenerBodyHandler bodyHandler;
    private readonly FuncParamsHandler paramsHandler;
    public ListenerMessageEventHandler(ErrorsHandler errorsHandler) : base(errorsHandler)
    {
        bodyHandler = new(errorsHandler);
        paramsHandler = new(errorsHandler, typesOptional: true, openingToken: Token.Types.D_CBRAC_OP, closingToken: Token.Types.D_CBRAC_CLO);
    }

    protected override ParseNode? verifyTokens()
    {
        expectTokenType(Token.Types.K_ON);
        assertTokenType(Token.Types.IDENT);
        string name = CurrentToken.Value;
        incrementIndex();
        
        FuncParams? parameters = (FuncParams?)delegateToHandler(paramsHandler);
        if (parameters == null) return null;
        
        ListenerBodyNode? body = parseFunctionBody();
        if (body == null) return null;
        return new ListenerMessageEventNode(name, parameters, body);
    }

    private ListenerBodyNode? parseFunctionBody()
    {
        ParseNode? bodyNode = delegateToHandler(bodyHandler);
        if (bodyNode == null)
        {
            return null;
        }
        return (ListenerBodyNode)bodyNode;
    }
}