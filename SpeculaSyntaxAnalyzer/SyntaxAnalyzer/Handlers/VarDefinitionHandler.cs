using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Handler for variable settings such as datatype and capabilities
/// </summary>
public class VarDefinitionHandler : Handler
{
    private readonly DataTypeHandler dataTypeHandler;
    private readonly CapabilityHandler capabilityHandler;

    public VarDefinitionHandler(ErrorsHandler errors) : base(errors)
    {
        dataTypeHandler = new(errors);
        capabilityHandler = new(errors);
    }

    protected override ParseNode? verifyTokens()
    {
        if (CurrentToken.Type != Token.Types.D_COLON)
        {
            throw new SyntaxErrorException([":"], CurrentToken);
        }
        incrementIndex();
        
        TypeNode? dataType;
        switch (CurrentToken.Type)
        {
            case Token.Types.K_TYPE:
            case Token.Types.IDENT:
                {
                    dataType = getType();
                    if (dataType == null) return null;
                    incrementIndex();
                    if (HasMoreTokens && CurrentToken.Type == Token.Types.D_BRAC_OP)
                    {
                        Capabilities? capabilities = getCapabilities();
                        if (capabilities == null) return null;
                        incrementIndex();
                        return new TypeDefinitionNode(dataType.type, capabilities);
                    }
                    else
                    {
                        Capabilities defaultCapabilities = generateDefaultCapabilities();
                        return new TypeDefinitionNode(dataType.type, defaultCapabilities);
                    }
                }
            default:
                throw new SyntaxErrorException(["TYPE", "IDENTIFIER"], CurrentToken);
        }
    }

    private TypeNode? getType()
    {
        TypeNode typeNode = new TypeNode(DataTypeHandler.TokenTypeToDataType(CurrentToken));
        return typeNode;
    }

    private Capabilities? getCapabilities()
    {
        ParseNode? parseNode = delegateToHandler(capabilityHandler);
        if (parseNode == null) return null;
        if (parseNode is Capabilities capabilities)
        {
            return capabilities;
        }
        else throw new InvalidOperationException($"Expected capabilities node. received: {parseNode}");
    }

    private Capabilities generateDefaultCapabilities()
    {
        return new Capabilities(new PrintableList<Capability>()
        {
            new Capability(CapabilityTypes.OWN, new PrintableList<string>()),
            new Capability(CapabilityTypes.CONST, new PrintableList<string>())
        });
    }
}