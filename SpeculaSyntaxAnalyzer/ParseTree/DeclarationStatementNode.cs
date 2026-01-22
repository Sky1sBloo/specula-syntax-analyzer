namespace SpeculaSyntaxAnalyzer.ParseTree;

public record DeclarationStatementNode(string Identifier, string Type, ParseNode value) : ParseNode;
