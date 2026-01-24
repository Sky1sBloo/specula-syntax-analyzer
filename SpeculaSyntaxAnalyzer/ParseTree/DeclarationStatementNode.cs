using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

namespace SpeculaSyntaxAnalyzer.ParseTree;

public record DeclarationStatementNode(string Identifier, VarDefinition varDefinition, Expression value) : Statement, ForInit;
