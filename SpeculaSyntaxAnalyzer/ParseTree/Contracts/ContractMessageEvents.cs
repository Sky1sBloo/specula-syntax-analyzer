namespace SpeculaSyntaxAnalyzer.ParseTree;

public record ContractMessageEventNodes(RoleNode From, RoleNode To, PrintableList<ContractMessageEventNode> Events) : ParseNode;
public record ContractMessageEventNode(string Name, PrintableList<FuncParam> Parameters, StateNode InitialState, StateNode NextState) : ParseNode;