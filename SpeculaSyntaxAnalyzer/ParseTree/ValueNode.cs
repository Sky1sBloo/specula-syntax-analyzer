namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface ValueNode : Expression;

public record LiteralValue(TypeNode type, string value) : ValueNode;
public record IdentifierValue(string value) : ValueNode;
public record FunctionCallValue(string identifier, PrintableList<string> funcParams) : ValueNode;

