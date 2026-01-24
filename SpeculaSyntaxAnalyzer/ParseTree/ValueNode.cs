namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface ValueNode : Expression;
public interface Movable;  // be able to use move, ref, share assignments

public record LiteralValue(TypeNode type, string value) : ValueNode;
public record IdentifierValue(string value) : ValueNode, Movable;
public record FunctionCallValue(string identifier, PrintableList<Expression> funcParams) : ValueNode, Movable;

public record StructKey(string key, Expression value);
public record StructInitialization(string identifier, PrintableList<StructKey> keys) : ValueNode;
