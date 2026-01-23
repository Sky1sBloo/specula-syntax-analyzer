namespace SpeculaSyntaxAnalyzer.ParseTree;

public record AssignmentStatementNode(string Identifier, Expression value) : Statement, ForInit;
public record AssignPlusEqNode(string Identifier, Expression value) : Statement, ForInit;
public record AssignMinusEqNode(string Identifier, Expression value) : Statement, ForInit;
public record AssignMulEqNode(string Identifier, Expression value) : Statement, ForInit;
public record AssignDivEqNode(string Identifier, Expression value) : Statement, ForInit;
public record AssignModEqNode(string Identifier, Expression value) : Statement, ForInit;

