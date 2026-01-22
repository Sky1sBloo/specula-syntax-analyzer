using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ValueHandler : Handler
{
    private enum States
    {
        START,
        FOUND_IDENT,
        WAIT_FOR_OUTPUT  // used to receive output from other handler
    }

    private enum Type
    {
        LITERAL,
        IDENTIFIER,
        FUNCTION_CALL
    }

    private States currentState = States.START;
    private Type type = Type.LITERAL;

    public override void handleToken(Token token, ParseNode? prevNode)
    {
        switch (currentState)
        {
            case States.START:
                handleStartState(token);
                break;
            case States.FOUND_IDENT:
                handleIdentState(token);
                break;
            case States.WAIT_FOR_OUTPUT:
                handleWaitForOutputState(token, prevNode);
                break;
        }
    }

    private void end(Token token)
    {
        ValueNode node;
        switch (type)
        {
            case Type.LITERAL:
                node = new LiteralValue(token.Type.ToString(), token.Value);
                SetHandlerFinished(node);
                break;
            case Type.IDENTIFIER:
                node = new IdentifierValue(token.Value);
                break;
        }
        currentState = States.START;
        type = Type.LITERAL;
    }

    private void handleStartState(Token token)
    {
        switch (token.Type)
        {
            case Token.Types.K_TYPE:
            case Token.Types.L_FLOAT:
            case Token.Types.L_DOUBLE:
            case Token.Types.L_CHAR:
            case Token.Types.L_STRING:
            case Token.Types.L_BOOL:
                type = Type.LITERAL;
                end(token);
                break;
            case Token.Types.IDENT:
                currentState = States.FOUND_IDENT;
                break;
        }
    }

    private void handleIdentState(Token token)
    {
        switch (token.Type)
        {
            case Token.Types.D_PAR_OP:
                SetDelegateToState(SyntaxAnalyzerRoot.States.FUNC_CALL);
                currentState = States.WAIT_FOR_OUTPUT;
                break;
            case Token.Types.D_CBRAC_OP:
                SetDelegateToState(SyntaxAnalyzerRoot.States.STRUCT_BUILDER);
                currentState = States.WAIT_FOR_OUTPUT;
                break;
            default:
                type = Type.IDENTIFIER;
                end(token);
                break;
        }
    }

    private void handleWaitForOutputState(Token token, ParseNode? node)
    {
        if (node == null)
        {
            throw new SyntaxErrorException(["Value"], token);
        }
    }
}

