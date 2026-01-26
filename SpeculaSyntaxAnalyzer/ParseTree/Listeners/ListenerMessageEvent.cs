namespace SpeculaSyntaxAnalyzer.ParseTree;


public record ListenerMessageEventNode(string Name, FuncParams Parameters, ListenerBodyNode Body) : ParseNode;

public interface ListenerBodyContent : Statement;
public record ListenerBodyNode (PrintableList<ListenerBodyContent> Body) : ParseNode;
public record ListenerEventRespondNode(string Name, FuncParams Parameters) : ListenerBodyContent;
public record ListenerFail(string Name, FuncParams Parameters) : ListenerBodyContent;