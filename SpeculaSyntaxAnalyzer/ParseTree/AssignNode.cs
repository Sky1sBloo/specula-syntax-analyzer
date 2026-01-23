namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface Assignment : Statement;
public record AssignmentStatementNode(string Identifier, Expression value) : Assignment, ForInit;
public record AssignPlusEqNode(string Identifier, Expression value) : Assignment, ForInit;
public record AssignMinusEqNode(string Identifier, Expression value) : Assignment, ForInit;
public record AssignMulEqNode(string Identifier, Expression value) : Assignment, ForInit;
public record AssignDivEqNode(string Identifier, Expression value) : Assignment, ForInit;
public record AssignModEqNode(string Identifier, Expression value) : Assignment, ForInit;

