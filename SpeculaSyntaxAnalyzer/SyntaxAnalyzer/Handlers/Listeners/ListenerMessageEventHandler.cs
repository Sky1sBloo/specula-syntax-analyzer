using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ListenerMessageEventHandler : Handler
{
    private readonly BodyHandler bodyHandler;
    private readonly FuncParamsHandler paramsHandler;
    public ListenerMessageEventHandler(ErrorsHandler errorsHandler) : base(errorsHandler)
    {
        bodyHandler = new(errorsHandler, true);
        paramsHandler = new(errorsHandler, typesOptional: true, openingToken: Token.Types.D_CBRAC_OP, closingToken: Token.Types.D_CBRAC_CLO);
    }

    protected override ParseNode? verifyTokens()
    {
        expectTokenType(Token.Types.K_ON);
        assertTokenType(Token.Types.IDENT);
        string name = CurrentToken.Value;
        incrementIndex();
        
        FuncParams? parameters;
        
        // Check if there's a parameter block (opening brace with actual parameters or followed by another brace)
        if (HasMoreTokens && CurrentToken.Type == Token.Types.D_CBRAC_OP)
        {
            // Peek ahead to see if there's content before the closing brace
            // If next token after { is }, then this is an empty parameter list (or could be the body)
            // We need to look further to disambiguate
            int savedIndex = getIndex();
            incrementIndex(); // consume the opening brace
            
            if (HasMoreTokens && CurrentToken.Type == Token.Types.D_CBRAC_CLO)
            {
                // Empty braces { } - this could be:
                // 1. Empty parameters with a body following: { } { body }
                // 2. Just the body with no parameters: { body }
                // Look ahead one more token
                incrementIndex(); // consume the closing brace
                
                if (HasMoreTokens && CurrentToken.Type == Token.Types.D_CBRAC_OP)
                {
                    // It's empty parameters followed by body
                    // Reset to before the first {
                    setIndex(savedIndex);
                    FuncParams? parsedParams = (FuncParams?)delegateToHandler(paramsHandler);
                    if (parsedParams == null)
                        return null;
                    parameters = parsedParams;
                }
                else
                {
                    // Empty {} is the body, not parameters
                    // Reset to before the first {
                    setIndex(savedIndex);
                    parameters = new FuncParams(new PrintableList<FuncParam>());
                }
            }
            else
            {
                // There's content between { and }, so parse as parameters
                // Reset to before the first {
                setIndex(savedIndex);
                FuncParams? parsedParams = (FuncParams?)delegateToHandler(paramsHandler);
                if (parsedParams == null)
                    return null;
                parameters = parsedParams;
            }
        }
        else
        {
            // No opening brace, use empty parameters
            parameters = new FuncParams(new PrintableList<FuncParam>());
        }
        
        BodyNode? body = parseFunctionBody();
        if (body == null) 
            return null;
        
        return new ListenerMessageEventNode(name, parameters, body);
    }

    private BodyNode? parseFunctionBody()
    {
        ParseNode? bodyNode = delegateToHandler(bodyHandler);
        if (bodyNode == null)
        {
            return null;
        }
        return (BodyNode?)bodyNode;
    }
}