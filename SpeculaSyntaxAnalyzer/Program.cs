using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
string[] files = args;

foreach (string iFile in files)
{
    LexerOutput output = LexerFileReader.ParseFile(iFile);
    SyntaxAnalyzerRoot analyzer = new();
    var node = analyzer.ReadTokens(output.Tokens);
}
