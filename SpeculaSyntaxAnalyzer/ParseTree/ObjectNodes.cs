namespace SpeculaSyntaxAnalyzer.ParseTree;

public record StructField (string identifier, TypeDefinitionNode typeDef) : ParseNode;
public record StructDefNode (string structName, PrintableList<StructField> fields) : RootStatement;
public record InterfaceSelfParam(string identifier) : ParamNode;  // used for self parameter in interface methods

public interface InterfaceFuncNode : ParseNode; 
public record InterfaceFuncReturnSelfNode(string funcName, PrintableList<ParamNode> parameters) : InterfaceFuncNode;
public record InterfaceFuncReturnNode (string funcName, PrintableList<ParamNode> parameters, TypeDefinitionNode returnType) : InterfaceFuncNode;
public record InterfaceDefNode (string interfaceName, PrintableList<InterfaceFuncNode> methods) : RootStatement;

public record ImplDefNode (string structName, string interfaceName, PrintableList<FuncDef> methods) : RootStatement;