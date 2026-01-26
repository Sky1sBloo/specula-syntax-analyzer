namespace SpeculaSyntaxAnalyzer.ParseTree;


public record ListenerMessageEventNode(string Name, PrintableList<FuncParam> Parameters, BodyNode Body) : ParseNode;
