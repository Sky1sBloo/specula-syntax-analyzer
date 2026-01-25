namespace SpeculaSyntaxAnalyzer.ParseTree;

public record ContractEventNodes(RoleNode From, RoleNode To, PrintableList<ContractEventNode> Events) : ParseNode;
public interface ContractEventNode : ParseNode;
public record ContractAutoResetEventNode(PrintableList<StateNode> States) : ContractEventNode;
public record ContractAutoMoveEventNode(PrintableList<StateNode> States, StateNode TargetState) : ContractEventNode;
public record ContractFailEventNode(string Identifier, PrintableList<FuncParam> Parameters) : ContractEventNode;