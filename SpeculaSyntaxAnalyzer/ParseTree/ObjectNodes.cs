namespace SpeculaSyntaxAnalyzer.ParseTree;

public record StructField (string identifier, TypeDefinitionNode typeDef) : ParseNode;
public record StructDefNode (string structName, PrintableList<StructField> fields) : ParseNode;
