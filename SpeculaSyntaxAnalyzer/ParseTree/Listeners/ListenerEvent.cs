namespace SpeculaSyntaxAnalyzer.ParseTree;

public enum ListenerEventType
{
    BEFORE,
    AFTER
}
public record ListenerEventNode(string Name, ListenerEventType EventType, BodyNode Body) : ParseNode;

public record ListenerFailEventNode(string Name, PrintableList<FuncParam> Parameters, BodyNode Body) : ParseNode;