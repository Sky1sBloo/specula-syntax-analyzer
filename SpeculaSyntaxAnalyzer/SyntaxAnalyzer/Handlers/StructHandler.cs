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

    protected override ParseNode? verifyTokens()
    {
        assertTokenType(Token.Types.K_STRUCT);
        incrementIndex();
        assertTokenType(Token.Types.IDENT);
        string structName = CurrentToken.Value;
        incrementIndex();
        expectTokenType(Token.Types.D_CBRAC_OP);
        PrintableList<StructField> fields = new();
        while (CurrentToken.Type != Token.Types.D_CBRAC_CLO)
        {
            assertTokenType(Token.Types.IDENT);
            string fieldName = CurrentToken.Value;
            incrementIndex();
            expectTokenType(Token.Types.D_COLON);
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
        throw new NotImplementedException();
    }

    private TypeDefinitionNode? parseTypeDefinition()
    {
        ParseNode? typeNode = delegateToHandler(typeHandler);
        return typeNode as TypeDefinitionNode;
    }
}