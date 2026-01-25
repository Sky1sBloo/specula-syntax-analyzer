namespace SpeculaSyntaxAnalyzer.ParseTree;

public record StructField (string identifier, TypeDefinitionNode typeDef) : ParseNode;
public record StructDefNode (string structName, PrintableList<StructField> fields) : RootStatement;
public record InterfaceFuncNode (string funcName, PrintableList<FuncParam> parameters, TypeDefinitionNode returnType) : ParseNode;
public record InterfaceDefNode (string interfaceName, PrintableList<InterfaceFuncNode> methods) : RootStatement;
public record ImplDefNode (string structName, PrintableList<FuncDef> methods) : RootStatement;