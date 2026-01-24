namespace SpeculaSyntaxAnalyzer.ParseTree;

public record BodyNode(PrintableList<Statement> statements) : Statement;
