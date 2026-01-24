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
                switch (CurrentToken.Type)
                {
                    case Token.Types.D_PAR_OP:
                        return handleFunctionCall(identifier);
                    case Token.Types.D_CBRAC_OP:
                        return handleStructInitialization(identifier);
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

            // Handle comma between parameters or closing paren; disallow trailing comma
            if (HasMoreTokens && CurrentToken.Type == Token.Types.COMMA)
            {
                incrementIndex();

                if (!HasMoreTokens || CurrentToken.Type == Token.Types.D_PAR_CLO)
                {
                    Token missing = new()
                    {
                        Type = Token.Types.UNKNOWN,
                        Line = getIndex(),
                        CharStart = 0,
                        CharEnd = 0
                    };
                    var ex = new SyntaxErrorException(["expression"], missing);
                    errorHandler.AddError(ex);
                    throw ex;
                }

                continue;
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

    private StructInitialization handleStructInitialization(string identifier)
    {
        incrementIndex();
        var keys = new PrintableList<StructKey>();
        // For empty struct initialization
        if (HasMoreTokens && CurrentToken.Type == Token.Types.D_CBRAC_CLO)
        {
            incrementIndex();
            return new StructInitialization(identifier, keys);
        }

        while (HasMoreTokens && CurrentToken.Type != Token.Types.D_CBRAC_CLO)
        {
            if (CurrentToken.Type != Token.Types.IDENT)
            {
                throw new SyntaxErrorException(["identifier"], CurrentToken);
            }
            string key = CurrentToken.Value;
            incrementIndex();

            if (!HasMoreTokens || CurrentToken.Type != Token.Types.D_COLON)
            {
                throw new SyntaxErrorException([":"], CurrentToken);
            }
            incrementIndex();

            ExpressionHandler exprHandler = new ExpressionHandler(errorHandler);
            ParseNode? valueExpr = delegateToHandler(exprHandler);
            if (valueExpr is Expression expr)
            {
                keys.Add(new StructKey(key, expr));
            }
            else
            {
                throw new SyntaxErrorException(["expression"], CurrentToken);
            }

            // for comma between key-value pairs or closing brace; disallow trailing comma
            if (HasMoreTokens && CurrentToken.Type == Token.Types.COMMA)
            {
                incrementIndex();

                if (!HasMoreTokens || CurrentToken.Type == Token.Types.D_CBRAC_CLO)
                {
                    Token missing = new()
                    {
                        Type = Token.Types.UNKNOWN,
                        Line = getIndex(),
                        CharStart = 0,
                        CharEnd = 0
                    };
                    var ex = new SyntaxErrorException(["identifier"], missing);
                    errorHandler.AddError(ex);
                    throw ex;
                }

                continue;
            }
            else if (HasMoreTokens && CurrentToken.Type != Token.Types.D_CBRAC_CLO)
            {
                throw new SyntaxErrorException([",", "}"], CurrentToken);
            }
        }
        if (!HasMoreTokens || CurrentToken.Type != Token.Types.D_CBRAC_CLO)
        {
            Token missing = new()
            {
                Type = Token.Types.UNKNOWN,
                Line = getIndex(),
                CharStart = 0,
                CharEnd = 0
            };
            throw new SyntaxErrorException(["}"], missing);
        }

        incrementIndex();
        return new StructInitialization(identifier, keys);
    }
}
