namespace SpeculaSyntaxAnalyzer.ParseTree;

public record DeclarationStatementNode(string Identifier, TypeNode dataType, Expression value) : Statement;
