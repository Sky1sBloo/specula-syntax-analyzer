namespace SpeculaSyntaxAnalyzer.ParseTree;

public enum ListenerEventType
{
    BEFORE,
    AFTER
}
public record ListenerEventNode(string Name, ListenerEventType EventType, ListenerBodyNode Body) : ParseNode;

public record ListenerFailEventNode(string Name, FuncParams Parameters, ListenerBodyNode Body) : ParseNode;