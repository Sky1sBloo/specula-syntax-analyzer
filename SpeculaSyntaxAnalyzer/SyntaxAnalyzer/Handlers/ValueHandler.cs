using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ValueHandler : Handler
{
    public ValueHandler(ErrorsHandler errors) : base(errors)
    {
    }

    protected override ParseNode? verifyTokens()
    {
        try
        {
            string identifier = "";
            DataTypes dataType = DataTypeHandler.InferDataTypeFromTokenLiteral(CurrentToken);
            switch (dataType)
            {
                case DataTypes.INT:
                case DataTypes.FLOAT:
                case DataTypes.DOUBLE:
                case DataTypes.CHAR:
                case DataTypes.BOOL:
                case DataTypes.STRING:
                case DataTypes.NULL:
                    {
                        var literalValue = new LiteralValue(new TypeNode(dataType), CurrentToken.Value);
                        incrementIndex();
                        return literalValue;
                    }
                case DataTypes.VOID:
                case DataTypes.UNKNOWN:
                    throw new SyntaxErrorException(["Value"], CurrentToken);
            }

            if (dataType == DataTypes.IDENTIFIER)
            {
                identifier = CurrentToken.Value;
            }
            incrementIndex();

            // Check if we're at the end of tokens before accessing CurrentToken
            if (HasMoreTokens)
            {
                // Handle function calls
                if (CurrentToken.Type == Token.Types.D_PAR_OP)
                {
                    return handleFunctionCall(identifier);
                }
            }
            return new IdentifierValue(identifier);
        } catch (ArgumentException)
        {
            throw new SyntaxErrorException(["Value Type"], CurrentToken);
        }
    }

    private FunctionCallValue handleFunctionCall(string identifier)
    {
        incrementIndex();
        var parameters = new PrintableList<Expression>();

        // For empty param list
        if (HasMoreTokens && CurrentToken.Type == Token.Types.D_PAR_CLO)
        {
            incrementIndex();
            return new FunctionCallValue(identifier, parameters);
        }

        while (HasMoreTokens && CurrentToken.Type != Token.Types.D_PAR_CLO)
        {
            ExpressionHandler exprHandler = new ExpressionHandler(errorHandler);
            ParseNode? paramExpr = delegateToHandler(exprHandler);
            
            if (paramExpr is Expression expr)
            {
                parameters.Add(expr);
            }
            else
            {
                throw new SyntaxErrorException(["expression"], CurrentToken);
            }

            // Handle comma between parameters or closing paren
            if (HasMoreTokens && CurrentToken.Type == Token.Types.COMMA)
            {
                incrementIndex();
            }
            else if (HasMoreTokens && CurrentToken.Type != Token.Types.D_PAR_CLO)
            {
                throw new SyntaxErrorException([",", ")"], CurrentToken);
            }
        }

        if (!HasMoreTokens || CurrentToken.Type != Token.Types.D_PAR_CLO)
        {
            Token missing = new()
            {
                Type = Token.Types.UNKNOWN,
                Line = getIndex(),
                CharStart = 0,
                CharEnd = 0
            };
            throw new SyntaxErrorException([")"], missing);
        }

        incrementIndex();
        return new FunctionCallValue(identifier, parameters);
    }
}
