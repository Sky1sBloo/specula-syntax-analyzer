namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface VarDefinition : ParseNode;

public record TypeDefinitionNode(DataTypes type, Capabilities capabilities) : VarDefinition;