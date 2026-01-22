namespace SpeculaSyntaxAnalyzer.ParseTree;

public abstract record ValueNode : ParseNode;

public record LiteralValue(string value) : ValueNode;
public record IdentifierValue(string value) : ValueNode;
public record FunctionCallValue(string value) : ValueNode;

