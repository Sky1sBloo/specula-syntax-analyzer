namespace SpeculaSyntaxAnalyzer.ParseTree;

public record StructField (string identifier, TypeDefinitionNode typeDef) : ParseNode;
public record StructDefNode (string structName, PrintableList<StructField> fields) : ParseNode;
public record InterfaceFuncNode (string funcName, PrintableList<FuncParam> parameters, TypeDefinitionNode returnType) : ParseNode;
public record InterfaceDefNode (string interfaceName, PrintableList<InterfaceFuncNode> methods) : ParseNode;
public record ImplDefNode (string structName, PrintableList<FuncDef> methods) : ParseNode;