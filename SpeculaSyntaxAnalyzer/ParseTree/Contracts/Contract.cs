namespace SpeculaSyntaxAnalyzer.ParseTree;

public record ContractNode(string Identifier,
    InitStateNode InitialState, 
    PrintableList<RoleNode> Roles,
    PrintableList<StateTransitionsNode> StateTransitions,
    PrintableList<ContractMessageEventNodes> MessageEvents,
    PrintableList<ContractEventNodes> Events
    ) : ParseNode;