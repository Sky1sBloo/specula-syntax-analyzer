namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface Assignment : Statement, ForInit;
public record AssignmentStatementNode(string Identifier, Expression value) : Assignment;
public record AssignPlusEqNode(string Identifier, Expression value) : Assignment;
public record AssignMinusEqNode(string Identifier, Expression value) : Assignment;
public record AssignMulEqNode(string Identifier, Expression value) : Assignment;
public record AssignDivEqNode(string Identifier, Expression value) : Assignment;
public record AssignModEqNode(string Identifier, Expression value) : Assignment;

