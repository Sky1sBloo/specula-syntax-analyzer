using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ListenerMessageEventHandler : Handler
{
    private readonly BodyHandler bodyHandler;
    private readonly FuncParamsHandler messageParamsHandler;
    private readonly FuncParamsHandler failParamsHandler;
    public ListenerMessageEventHandler(ErrorsHandler errorsHandler) : base(errorsHandler)
    {
        bodyHandler = new(errorsHandler, true);
        messageParamsHandler = new(errorsHandler, typesOptional: true, openingToken: Token.Types.D_CBRAC_OP, closingToken: Token.Types.D_CBRAC_CLO);
        failParamsHandler = new(errorsHandler, typesOptional: true, openingToken: Token.Types.D_PAR_OP, closingToken: Token.Types.D_PAR_CLO);
    }

    protected override ParseNode? verifyTokens()
    {
        expectTokenType(Token.Types.K_ON);
        switch (CurrentToken.Type)
        {
            case Token.Types.IDENT:
                return parseMessageEvent();
            case Token.Types.K_FAIL:
                return parseFailEvent();
            default:
                throw new SyntaxErrorException(
                    ["IDENT", "fail"],
                    CurrentToken
                );
        }
    }

    private ListenerMessageEventNode? parseMessageEvent()
    {
        assertTokenType(Token.Types.IDENT);
        string name = CurrentToken.Value;
        incrementIndex();

        FuncParams? parameters;

        if (HasMoreTokens && CurrentToken.Type == Token.Types.D_CBRAC_OP)
        {
            int savedIndex = getIndex();
            incrementIndex();
            if (HasMoreTokens && CurrentToken.Type == Token.Types.D_CBRAC_CLO)
            {
                incrementIndex(); 
                if (HasMoreTokens && CurrentToken.Type == Token.Types.D_CBRAC_OP)
                {
                    setIndex(savedIndex);
                    FuncParams? parsedParams = (FuncParams?)delegateToHandler(messageParamsHandler);
                    if (parsedParams == null)
                        return null;
                    parameters = parsedParams;
                }
                else
                {
                    setIndex(savedIndex);
                    parameters = new FuncParams(new PrintableList<FuncParam>());
                }
            }
            else
            {
                setIndex(savedIndex);
                FuncParams? parsedParams = (FuncParams?)delegateToHandler(messageParamsHandler);
                if (parsedParams == null)
                    return null;
                parameters = parsedParams;
            }
        }
        else
        {
            parameters = new FuncParams(new PrintableList<FuncParam>());
        }

        BodyNode? body = parseFunctionBody();
        if (body == null)
            return null;

        return new ListenerMessageEventNode(name, parameters, body);
    }
    private ListenerFailEventNode? parseFailEvent()
    {
        expectTokenType(Token.Types.K_FAIL);
        assertTokenType(Token.Types.IDENT);
        string name = CurrentToken.Value;
        incrementIndex();

        FuncParams? parameters;

        if (HasMoreTokens && CurrentToken.Type == Token.Types.D_PAR_OP)
        {
            int savedIndex = getIndex();
            incrementIndex();
            if (HasMoreTokens && CurrentToken.Type == Token.Types.D_PAR_CLO)
            {
                incrementIndex(); 
                if (HasMoreTokens && CurrentToken.Type == Token.Types.D_PAR_OP)
                {
                    setIndex(savedIndex);
                    FuncParams? parsedParams = (FuncParams?)delegateToHandler(failParamsHandler);
                    if (parsedParams == null)
                        return null;
                    parameters = parsedParams;
                }
                else
                {
                    setIndex(savedIndex);
                    parameters = new FuncParams(new PrintableList<FuncParam>());
                }
            }
            else
            {
                setIndex(savedIndex);
                FuncParams? parsedParams = (FuncParams?)delegateToHandler(failParamsHandler);
                if (parsedParams == null)
                    return null;
                parameters = parsedParams;
            }
        }
        else
        {
            parameters = new FuncParams(new PrintableList<FuncParam>());
        }

        BodyNode? body = parseFunctionBody();
        if (body == null)
            return null;

        return new ListenerFailEventNode(name, parameters, body);
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