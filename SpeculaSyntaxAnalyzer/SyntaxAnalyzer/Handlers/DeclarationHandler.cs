using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class DeclarationHandler : Handler
{
    private readonly DataTypeHandler dataTypeHandler;
    private readonly ExpressionHandler expressionHandler;

    private string identifier = "";
    private TypeNode? dataType = null;
    private Expression? value = null;

    public DeclarationHandler(ErrorsHandler errors) : base(errors)
    {
        dataTypeHandler = new(errors);
        expressionHandler = new(errors);
    }

    public void Reset()
    {
        identifier = "";
        dataType = null;
        value = null;
    }

    protected override ParseNode? verifyTokens()
    {
        if (CurrentToken.Type != Token.Types.K_LET)
        {
            throw new SyntaxErrorException(["let"], CurrentToken);
        }
        incrementIndex();
        if (CurrentToken.Type != Token.Types.IDENT)
        {
            throw new SyntaxErrorException(["IDENTIFIER"], CurrentToken);
        }
        identifier = CurrentToken.Value;
        incrementIndex();
        switch (CurrentToken.Type)
        {
            case Token.Types.D_COLON:
                return sawColonState();
            case Token.Types.OP_EQUALS:
                incrementIndex();
                return sawEqualsState();
        }
        throw new SyntaxErrorException([":", "="], CurrentToken);
    }

    private ParseNode? sawColonState()
    {
        dataType = getType();
        incrementIndex();

        switch (CurrentToken.Type)
        {
            case Token.Types.OP_EQUALS:
                incrementIndex();
                return sawEqualsState();
            case Token.Types.D_SEMICOLON:
                {
                    if (dataType == null) throw new SyntaxErrorException(["Definition of datatype"], CurrentToken);
                    value = new LiteralValue(new TypeNode(DataTypes.NULL), "null");
                    return ConstructNode();
                }
            default:
                throw new SyntaxErrorException([";", "="], CurrentToken);
        }
    }

    private ParseNode? sawEqualsState()
    {
        value = getVariableValue();
        if (value == null) return null;

        if (CurrentToken.Type != Token.Types.D_SEMICOLON)
        {
            throw new SyntaxErrorException([";"], CurrentToken);
        }
        
        // means we came directly from = without :
        if (dataType == null)
        {
            throw new SyntaxErrorException(["Type annotation (: type)"], CurrentToken);
        }
        
        return ConstructNode();
    }

    private DeclarationStatementNode ConstructNode()
    {
        if (dataType == null || value == null)
        {
            throw new InvalidOperationException("Tried to construct node of null datatype or value");
        }
        DeclarationStatementNode statementNode = new(identifier, dataType, value);
        Reset();
        return statementNode;
    }

    private TypeNode? getType()
    {
        ParseNode? parseNode = delegateToHandler(dataTypeHandler);
        if (parseNode == null) return null;
        if (parseNode is TypeNode typeNode)
        {
            return typeNode;
        }
        else throw new InvalidOperationException($"Expected type node. received: {parseNode}");
    }

    private Expression? getVariableValue()
    {
        ParseNode? parseNode = delegateToHandler(expressionHandler);

        if (parseNode == null) return null;
        if (parseNode is Expression expression)
        {
            return expression;
        }
        else throw new InvalidOperationException($"Expected expression. received : {parseNode}");
    }
}
