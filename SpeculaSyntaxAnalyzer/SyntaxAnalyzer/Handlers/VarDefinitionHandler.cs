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

        TypeNode? dataType = getType();
        if (dataType == null) return null;

        if (HasMoreTokens && CurrentToken.Type == Token.Types.D_BRAC_OP)
        {
            Capabilities? capabilities = getCapabilities();
            if (capabilities == null) return null;
            return new TypeDefinitionNode(dataType, capabilities);
        }

        Capabilities defaultCapabilities = CapabilityHandler.GenerateDefaultCapabilities();
        return new TypeDefinitionNode(dataType, defaultCapabilities);


        /*
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
                        return new TypeDefinitionNode(dataType.DataType, capabilities);
                    }
                    else
                    {
                        Capabilities defaultCapabilities = CapabilityHandler.GenerateDefaultCapabilities();
                        return new TypeDefinitionNode(dataType.DataType, defaultCapabilities);
                    }
                }
            default:
                throw new SyntaxErrorException(["TYPE", "IDENTIFIER"], CurrentToken);
        } */
    }

    private TypeNode? getType()
    {
        ParseNode? parseNode = delegateToHandler(dataTypeHandler);
        if (parseNode == null) return null;
        return (TypeNode)parseNode;
    }

    private Capabilities? getCapabilities()
    {
        ParseNode? parseNode = delegateToHandler(capabilityHandler);
        if (parseNode == null) return null;
        return (Capabilities)parseNode;
    }
}