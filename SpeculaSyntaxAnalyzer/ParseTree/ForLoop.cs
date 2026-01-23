namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface ForInit;
public record ForLoop(ForInit init, Expression expression, BodyNode Body) : Statement;
