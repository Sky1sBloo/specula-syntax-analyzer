using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;
using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
string[] files = args;

foreach (string iFile in files)
{
    LexerOutput output = LexerFileReader.ParseFile(iFile);
    ErrorsHandler errorHandler = new();
    foreach (var error in output.Errors)
    {
        errorHandler.AddError($"Lexer Error at line {error.Line}, char {error.CharPos}: {error.Message}");
    }
    SyntaxAnalyzerRoot analyzer = new(errorHandler);
    ParseNode? node = analyzer.ReadTokens(output.Tokens);
    if (node != null) {
        if (node is RootNode bodyNode)
        {
            Console.WriteLine($"Body contains {bodyNode.Statements.Count} statements");
            foreach (var stmt in bodyNode.Statements)
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

    foreach (var error in errorHandler.ErrorList)
    {
        Console.WriteLine(error);
    }
}
