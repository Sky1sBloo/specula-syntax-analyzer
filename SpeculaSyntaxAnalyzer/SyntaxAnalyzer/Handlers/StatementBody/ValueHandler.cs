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
            Expression? baseExpr = null;
            string identifier = "";

            // treat identifier-based values for member access
            if (CurrentToken.Type == Token.Types.K_SELF ||
                CurrentToken.Type == Token.Types.K_THIS ||
                CurrentToken.Type == Token.Types.K_NETWORK)
            {
                identifier = CurrentToken.Value;
                incrementIndex();
                baseExpr = new IdentifierValue(identifier);
            }

            if (baseExpr == null)
            {
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
                        baseExpr = new LiteralValue(new TypeNode(dataType), CurrentToken.Value);
                        incrementIndex();
                        break;
                    case DataTypes.VOID:
                    case DataTypes.UNKNOWN:
                        throw new SyntaxErrorException(["Value"], CurrentToken);
                }

                if (baseExpr == null)
                {
                    // Identifier or start of complex value
                    if (dataType == DataTypes.IDENTIFIER)
                    {
                        identifier = CurrentToken.Value;
                    }
                    incrementIndex();

                    // Function call or struct initialization based on next token
                    if (HasMoreTokens)
                    {
                        switch (CurrentToken.Type)
                        {
                            case Token.Types.D_PAR_OP:
                                baseExpr = handleFunctionCall(identifier);
                                break;
                            case Token.Types.D_CBRAC_OP:
                                baseExpr = handleStructInitialization(identifier);
                                break;
                            default:
                                baseExpr = new IdentifierValue(identifier);
                                break;
                        }
                    }
                    else
                    {
                        baseExpr = new IdentifierValue(identifier);
                    }
                }
            }

            // Handle member access chaining: obj.member.member2
            while (HasMoreTokens && CurrentToken.Type == Token.Types.OP_PERIOD)
            {
                incrementIndex();
                if (!HasMoreTokens || CurrentToken.Type != Token.Types.IDENT)
                {
                    throw new SyntaxErrorException(["identifier"], CurrentToken);
                }
                string member = CurrentToken.Value;
                incrementIndex();
                baseExpr = new MemberAccessValue(baseExpr!, member);
            }

            return (ParseNode?)baseExpr;
        }
        catch (ArgumentException)
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
