namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface ForInit;
public record ForLoop(Expression Condition, BodyNode Body) : Statement;
