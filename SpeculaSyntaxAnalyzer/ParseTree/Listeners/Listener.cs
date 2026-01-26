namespace SpeculaSyntaxAnalyzer.ParseTree;

public record ListenerNode(string Name, 
    string ContractName, 
    ValueNode Target, 
    PrintableList<ListenerMessageEventNode> MessageEvents,
    PrintableList<ListenerEventNode> Events,
    PrintableList<ListenerFailEventNode> FailEvents);