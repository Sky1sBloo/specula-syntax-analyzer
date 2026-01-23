namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface Expression : ParseNode;


public record AddExpression(Expression lhs, Expression rhs) : Expression;
public record SubExpression(Expression lhs, Expression rhs) : Expression;
public record MultExpression(Expression lhs, Expression rhs) : Expression;
public record DivExpression(Expression lhs, Expression rhs) : Expression;
public record PostIncExpression(Expression node) : Expression;
public record PostDecExpression(Expression node) : Expression;
public record PreIncExpression(Expression node) : Expression;
public record PreDecExpression(Expression node) : Expression;
public record PrePosExpression(Expression node) : Expression;
public record PreNegExpression(Expression node) : Expression;

