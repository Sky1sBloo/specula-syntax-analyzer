namespace SpeculaSyntaxAnalyzer.ParseTree;

public record ListenerNode(string Name, 
    string ContractName, 
    ValueNode Target, 
    PrintableList<ListenerBody> Body) : RootStatement;

public interface ListenerBody : ParseNode;