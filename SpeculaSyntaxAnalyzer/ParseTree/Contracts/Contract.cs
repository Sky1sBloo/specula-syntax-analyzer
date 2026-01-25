namespace SpeculaSyntaxAnalyzer.ParseTree;

public record ContractNode(string Identifier,
    InitStateNode InitialState, 
    PrintableList<RoleNode> Roles,
    PrintableList<StateTransitionsNode> StateTransitions,
    PrintableList<ContractMessageEventsNode> MessageEvents,
    PrintableList<ContractEventNodes> Events
    ) : ParseNode;