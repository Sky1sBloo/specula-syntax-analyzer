namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface FuncDef : RootStatement;
public record FuncParam(string Identifier, TypeDefinitionNode Definition) : ParseNode;
// Used for reusable function shapes (param) {body}
public record FuncShapeNode(PrintableList<FuncParam> Parameters, TypeDefinitionNode ReturnType, BodyNode Body) : ParseNode;
public record FuncDefNode(string Identifier, bool isAsync, FuncShapeNode FunctionNode) : FuncDef;
public record ThreadDefNode(string Identifier, FuncShapeNode FunctionNode) : FuncDef;