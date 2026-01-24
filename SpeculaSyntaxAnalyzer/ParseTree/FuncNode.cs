namespace SpeculaSyntaxAnalyzer.ParseTree;

public record FuncDefNode(string Identifier, bool isAsync, PrintableList<FuncParam> Parameters, TypeDefinitionNode ReturnType, BodyNode body) : Statement;
public record FuncParam(string Identifier, TypeDefinitionNode Definition) : ParseNode;