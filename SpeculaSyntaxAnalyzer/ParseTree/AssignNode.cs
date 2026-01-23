namespace SpeculaSyntaxAnalyzer.ParseTree;

public record AssignmentStatementNode(string Identifier, Expression value) : Statement;
public record AssignPlusEqNode(string Identifier, Expression value) : Statement;
public record AssignMinusEqNode(string Identifier, Expression value) : Statement;
public record AssignMulEqNode(string Identifier, Expression value) : Statement;
public record AssignDivEqNode(string Identifier, Expression value) : Statement;
public record AssignModEqNode(string Identifier, Expression value) : Statement;

