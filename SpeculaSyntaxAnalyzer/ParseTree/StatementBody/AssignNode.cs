namespace SpeculaSyntaxAnalyzer.ParseTree;

public enum AssignMethod
{
    MOVE,
    SHARE,
    REF
}
public interface Assignment : Statement, ForInit;
public record AssignmentStatementNode(string Identifier, Expression value) : Assignment;
public record AssignmentMethodNode(string Identifier, AssignMethod Method, Movable value) : Assignment;
public record AssignPlusEqNode(string Identifier, Expression value) : Assignment;
public record AssignMinusEqNode(string Identifier, Expression value) : Assignment;
public record AssignMulEqNode(string Identifier, Expression value) : Assignment;
public record AssignDivEqNode(string Identifier, Expression value) : Assignment;
public record AssignModEqNode(string Identifier, Expression value) : Assignment;

