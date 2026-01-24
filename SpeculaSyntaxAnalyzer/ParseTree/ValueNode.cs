namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface ValueNode : Expression;

public record LiteralValue(TypeNode type, string value) : ValueNode;
public record IdentifierValue(string value) : ValueNode;
public record FunctionCallValue(string identifier, PrintableList<Expression> funcParams) : ValueNode;

public record StructKey(string key, Expression value);
public record StructInitialization(string identifier, PrintableList<StructKey> keys) : ValueNode;
