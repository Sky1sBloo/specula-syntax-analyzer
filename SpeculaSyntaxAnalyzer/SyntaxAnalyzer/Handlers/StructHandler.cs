using System.Reflection.Metadata;
using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class StructHandler: Handler
{
    private readonly DataTypeHandler dataTypeHandler;
    private readonly CapabilityHandler capabilityHandler;
    
    public StructHandler(ErrorsHandler errors) : base(errors)
    {
        dataTypeHandler = new DataTypeHandler(errors);
        capabilityHandler = new CapabilityHandler(errors);
    }

    protected override ParseNode? verifyTokens()
    {
        expectTokenType(Token.Types.K_STRUCT);
        assertTokenType(Token.Types.IDENT);
        string structName = CurrentToken.Value;
        incrementIndex();
        expectTokenType(Token.Types.D_CBRAC_OP);
        PrintableList<StructField> fields = new();
        while (CurrentToken.Type != Token.Types.D_CBRAC_CLO)
        {
            expectTokenType(Token.Types.K_LET);
            assertTokenType(Token.Types.IDENT);
            string fieldName = CurrentToken.Value;
            incrementIndex();
            assertTokenType(Token.Types.D_COLON);
            incrementIndex();
            
            // Parse type directly
            TypeNode? dataType = (TypeNode?)delegateToHandler(dataTypeHandler);
            if (dataType == null)
            {
                throw new SyntaxErrorException(["TYPE"], CurrentToken);
            }
            
            // Parse optional capabilities
            Capabilities? capabilities = null;
            if (HasMoreTokens && CurrentToken.Type == Token.Types.D_BRAC_OP)
            {
                capabilities = (Capabilities?)delegateToHandler(capabilityHandler);
            }
            
            if (capabilities == null)
            {
                capabilities = CapabilityHandler.GenerateDefaultCapabilities();
            }
            
            TypeDefinitionNode fieldType = new TypeDefinitionNode(dataType, capabilities);
            expectTokenType(Token.Types.D_SEMICOLON);
            fields.Add(new StructField(fieldName, fieldType));
        }

        incrementIndex();
        return new StructDefNode(structName, fields);
    }
}