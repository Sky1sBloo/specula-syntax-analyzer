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
                //todo: handle functions
                switch (CurrentToken.Type)
                {
                    case Token.Types.D_PAR_OP:
                    case Token.Types.D_CBRAC_OP:
                        break;
                }
            }
            return new IdentifierValue(identifier);
        } catch (ArgumentException)
        {
            throw new SyntaxErrorException(["Value Type"], CurrentToken);
        }
    }
}
