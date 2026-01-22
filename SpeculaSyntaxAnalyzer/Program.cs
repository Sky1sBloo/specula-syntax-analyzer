using SpeculaSyntaxAnalyzer.LexerReader;
string[] files = args;

foreach (string iFile in files)
{
    LexerOutput output = LexerFileReader.ParseFile(iFile);
    foreach (var token in output.Tokens)
    {
        Console.WriteLine(token.Type.ToString());
    }
}
