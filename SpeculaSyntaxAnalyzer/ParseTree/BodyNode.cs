namespace SpeculaSyntaxAnalyzer.ParseTree;

public record BodyNode(List<Statement> statements) : Statement;
