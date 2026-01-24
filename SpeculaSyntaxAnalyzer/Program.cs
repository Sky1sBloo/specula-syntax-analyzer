using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;
using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
string[] files = args;

foreach (string iFile in files)
{
    LexerOutput output = LexerFileReader.ParseFile(iFile);
    SyntaxAnalyzerRoot analyzer = new();
    ParseNode? node = analyzer.ReadTokens(output.Tokens);
    if (node != null) {
        if (node is BodyNode bodyNode)
        {
            Console.WriteLine($"Body contains {bodyNode.statements.Count} statements");
            foreach (var stmt in bodyNode.statements)
            {
                Console.WriteLine(stmt);
            }
        }
        else
        {
            Console.WriteLine("Root node is not a BodyNode");
        }
    } else
    {
        Console.WriteLine("No parse tree generated");
    }

    foreach (var error in analyzer.ErrorHandler.ErrorList)
    {
        Console.WriteLine(error);
    }
}
