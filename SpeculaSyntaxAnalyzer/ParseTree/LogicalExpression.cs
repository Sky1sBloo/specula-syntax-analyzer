namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface Logical : Expression;

public record AndLogical(Expression lhs, Expression rhs) : Logical;
public record OrLogical(Expression lhs, Expression rhs) : Logical;
public record NotLogical(Expression node) : Logical;
