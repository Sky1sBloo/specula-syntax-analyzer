namespace SpeculaSyntaxAnalyzer.ParseTree;

public record FuncDefNode(string Identifier, PrintableList<FuncParam> Parameters, TypeDefinitionNode ReturnType, BodyNode body) : ParseNode;
public record FuncParam(string Identifier, TypeDefinitionNode Definition) : ParseNode;