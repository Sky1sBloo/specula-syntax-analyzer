using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

namespace SpeculaSyntaxAnalyzer.ParseTree;

public record DeclarationStatementNode(string Identifier, TypeDefinitionNode varDefinition, Expression value) : Statement, RootStatement, ForInit;
