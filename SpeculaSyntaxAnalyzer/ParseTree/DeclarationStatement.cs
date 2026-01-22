namespace SpeculaSyntaxAnalyzer.ParseTree;

[Statement]
public class DeclarationStatement 
{
    public required string Identifier { get; set; }
    public required string Type { get; set; }
    public required string Value { get; set; }
}
