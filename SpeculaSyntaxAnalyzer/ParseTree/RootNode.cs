namespace SpeculaSyntaxAnalyzer.ParseTree;

public record RootNode(PrintableList<RootStatement> statements) : ParseNode;
