using System.Text.Json;
using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;
using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

string[] files = args;

var results = new List<object>();
var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

foreach (string iFile in files)
{
    LexerOutput output = LexerFileReader.ParseFile(iFile);
    ErrorsHandler errorHandler = new();
    foreach (var error in output.Errors)
    {
        errorHandler.AddError($"Lexer Error at {error.Line}:{error.CharPos}: {error.Message}");
    }

    object? rootSummary = null;

    if (output.Errors.Count == 0)
    {
        SyntaxAnalyzerRoot analyzer = new(errorHandler);
        ParseNode? node = analyzer.ReadTokens(output.Tokens);
        if (node != null && errorHandler.ErrorList.Count == 0)
        {
            rootSummary = BuildRootSummary(node);
        }
    }

    results.Add(new
    {
        file = output.FileInfo,
        errors = errorHandler.ErrorList,
        root = rootSummary
    });
}

Console.WriteLine(JsonSerializer.Serialize(results.Count == 1 ? results[0] : results, jsonOptions));

static object? BuildRootSummary(ParseNode node)
{
    if (node is RootNode root)
    {
        return new
        {
            statements = root.Statements.Select(stmt => stmt?.ToString()).ToList()
        };
    }

    return node.ToString();
}

