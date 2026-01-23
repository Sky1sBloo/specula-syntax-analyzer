using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;
using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
string[] files = args;

foreach (string iFile in files)
{
    LexerOutput output = LexerFileReader.ParseFile(iFile);
    SyntaxAnalyzerRoot analyzer = new();
    List<ParseNode> nodes = analyzer.ReadTokens(output.Tokens);
    foreach (var node in nodes)
    {
        Console.WriteLine(node);
    }
}
