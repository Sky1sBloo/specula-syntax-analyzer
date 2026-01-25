using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ContractHandler : Handler
{
    private readonly InitStateHandler initStateHandler;
    private readonly ContractRoleHandler contractRoleHandler;
    private readonly ContractMessageEventHandler contractMessageEventHandler;
    private readonly ContractEventHandler contractEventHandler;
    public ContractHandler(ErrorsHandler errorsHandler) : base(errorsHandler)
    {
        initStateHandler = new InitStateHandler(errorsHandler);
        contractRoleHandler = new ContractRoleHandler(errorsHandler);
        contractMessageEventHandler = new ContractMessageEventHandler(errorsHandler);
        contractEventHandler = new ContractEventHandler(errorsHandler);
    }

    protected override ParseNode? verifyTokens()
    {
        InitStateNode? initStateNode = delegateToHandler(initStateHandler) as InitStateNode;
        if (initStateNode == null) return null;
        RolesNode? rolesNode = delegateToHandler(contractRoleHandler) as RolesNode;
        var messageEvents = new PrintableList<ContractMessageEventNode>();
        return null;
    }
}