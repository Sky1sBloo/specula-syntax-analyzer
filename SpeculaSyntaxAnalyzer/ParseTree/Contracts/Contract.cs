namespace SpeculaSyntaxAnalyzer.ParseTree;

public record ContractNode(string Identifier,
    InitStateNode InitialState, 
    RolesNode Roles,
    PrintableList<StateTransitionsNode> StateTransitions,
    PrintableList<ContractMessageEventsNode> MessageEvents,
    PrintableList<ContractEventNode> Events
    ) : ParseNode;