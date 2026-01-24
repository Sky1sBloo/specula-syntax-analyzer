namespace SpeculaSyntaxAnalyzer.ParseTree;

public record IfStatementNode(Expression Condition, BodyNode Body) : Statement;
