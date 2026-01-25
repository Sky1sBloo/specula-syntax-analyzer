namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface Comparison : Expression;

public record EqCompExpression(Expression lhs, Expression rhs) : Comparison;
public record NotEqCompExpression(Expression lhs, Expression rhs) : Comparison;
public record GtCompExpression(Expression lhs, Expression rhs) : Comparison;
public record LtCompExpression(Expression lhs, Expression rhs) : Comparison;
public record GteCompExpression(Expression lhs, Expression rhs) : Comparison;
public record LteCompExpression(Expression lhs, Expression rhs) : Comparison;
