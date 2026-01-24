namespace SpeculaSyntaxAnalyzer.ParseTree;


public record TypeDefinitionNode(DataTypes type, Capabilities capabilities) : ParseNode;