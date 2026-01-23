namespace SpeculaSyntaxAnalyzer.ParseTree;

public record DeclarationStatementNode(string Identifier, TypeNode dataType, ParseNode value) : Statement;
