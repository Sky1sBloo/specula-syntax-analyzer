namespace SpeculaSyntaxAnalyzer.ParseTree;

public abstract record ValueNode : ParseNode;

public record LiteralValue(TypeNode type, string value) : ValueNode;
public record IdentifierValue(string value) : ValueNode;
public record FunctionCallValue(string identifier, List<string> funcParams) : ValueNode;

