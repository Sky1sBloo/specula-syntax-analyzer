namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface IfNode : ParseNode;

public record ConditionalStatement(IfStatementNode ifStatement, PrintableList<ElseIfStatementNode> elseIfStatement, ElseStatement? elseStatement) : Statement;
public record IfStatementNode(Expression Condition, BodyNode Body) : IfNode;
public record ElseIfStatementNode(Expression Condition, BodyNode Body) : IfNode;
public record ElseStatement(BodyNode body) : IfNode;