using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ListenerHandler : Handler
{
    private readonly ExpressionHandler expressionHandler;
    private readonly ListenerBodyHandler listenerBodyHandler;

    public ListenerHandler(ErrorsHandler errorsHandler) : base(errorsHandler)
    {
        expressionHandler = new(errorsHandler);
        listenerBodyHandler = new(errorsHandler);
    }

    protected override ParseNode? verifyTokens()
    {
        expectTokenType(Token.Types.K_LISTENER);
        return parseListener();
    }

    private ListenerNode? parseListener()
    {
        // Parse target: can be any value expression
        ParseNode? targetNode = delegateToHandler(expressionHandler);
        if (targetNode == null)
            return null;
        
        ValueNode? target = (ValueNode?)targetNode;
        if (target == null) return null;

        // Expect 'using' keyword
        expectTokenType(Token.Types.K_USING);

        // Parse contract type (IDENT)
        assertTokenType(Token.Types.IDENT);
        string contractName = CurrentToken.Value;
        incrementIndex();

        // Expect 'as' keyword
        expectTokenType(Token.Types.K_AS);

        // Parse listener name (IDENT)
        assertTokenType(Token.Types.IDENT);
        string listenerName = CurrentToken.Value;
        incrementIndex();

        // Expect opening brace for body
        expectTokenType(Token.Types.D_CBRAC_OP);

        // Parse body
        PrintableList<ListenerBody> bodyItems = new();
        while (HasMoreTokens && CurrentToken.Type != Token.Types.D_CBRAC_CLO)
        {
            ParseNode? bodyPartNode = delegateToHandler(listenerBodyHandler);
            if (bodyPartNode == null)
                break;
            
            if (bodyPartNode is ListenerBody bodyPart)
            {
                bodyItems.Add(bodyPart);
            }
            else
            {
                throw new SyntaxErrorException(["Listener Body"], CurrentToken);
            }
        }

        // Expect closing brace
        expectTokenType(Token.Types.D_CBRAC_CLO);

        return new ListenerNode(listenerName, contractName, target, bodyItems);
    }
}
