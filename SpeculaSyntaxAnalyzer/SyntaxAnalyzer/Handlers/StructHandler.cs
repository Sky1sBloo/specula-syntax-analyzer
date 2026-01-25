using System.Reflection.Metadata;
using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class StructHandler: Handler
{
    private readonly VarDefinitionHandler typeHandler;
    public StructHandler(ErrorsHandler errors) : base(errors)
    {
        typeHandler = new VarDefinitionHandler(errors);
    }

    protected ParseNode? verifyTokens()
    {
        if (CurrentToken.Type != Token.Types.K_STRUCT)
        {
            throw new SyntaxErrorException(["struct"], CurrentToken);
        }
        incrementIndex();
        if (CurrentToken.Type != Token.Types.IDENT)
        {
            throw new SyntaxErrorException(["IDENTIFIER"], CurrentToken);
        }
        string structName = CurrentToken.Value;
        incrementIndex();
        if (CurrentToken.Type != Token.Types.D_CBRAC_OP)
        {
            throw new SyntaxErrorException(["{"], CurrentToken);
        }
        incrementIndex();
        PrintableList<StructField> fields = new();
        while (CurrentToken.Type != Token.Types.D_CBRAC_CLO)
        {
            if (CurrentToken.Type != Token.Types.IDENT)
            {
                throw new SyntaxErrorException(["IDENTIFIER"], CurrentToken);
            }
            string fieldName = CurrentToken.Value;
            incrementIndex();
            if (CurrentToken.Type != Token.Types.D_COLON)
            {
                throw new SyntaxErrorException([":"], CurrentToken);
            }
            incrementIndex();
            TypeDefinitionNode? fieldType = parseTypeDefinition();
            if (fieldType == null)
            {
                throw new SyntaxErrorException(["TYPE_DEFINITION"], CurrentToken);
            }
            fields.Add(new StructField(fieldName, fieldType));
            if (CurrentToken.Type == Token.Types.COMMA)
            {
                incrementIndex();
            }
        }
    }

    private TypeDefinitionNode? parseTypeDefinition()
    {
        ParseNode? typeNode = delegateToHandler(typeHandler);
        return typeNode as TypeDefinitionNode;
    }
}