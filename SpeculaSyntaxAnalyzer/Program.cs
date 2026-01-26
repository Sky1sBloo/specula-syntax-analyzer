using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;
using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

string[] files = args;

var results = new List<object>();
var jsonOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
};

foreach (string iFile in files)
{
    LexerOutput output = LexerFileReader.ParseFile(iFile);
    ErrorsHandler errorHandler = new();
    foreach (var error in output.Errors)
    {
        errorHandler.AddError($"Lexer Error at {error.Line}:{error.CharPos}: {error.Message}");
    }

    JsonNode? rootSummary = null;

    if (output.Errors.Count == 0)
    {
        SyntaxAnalyzerRoot analyzer = new(errorHandler);
        ParseNode? node = analyzer.ReadTokens(output.Tokens);
        if (node != null && errorHandler.ErrorList.Count == 0)
        {
            rootSummary = ToJsonNode(node, jsonOptions);
        }
    }

    results.Add(new
    {
        fileInfo = output.FileInfo,
        errors = errorHandler.ErrorList,
        root = rootSummary
    });
}

Console.WriteLine(JsonSerializer.Serialize(results.Count == 1 ? results[0] : results, jsonOptions));

static JsonNode? ToJsonNode(object? value, JsonSerializerOptions options)
{
    if (value is null)
    {
        return null;
    }

    switch (value)
    {
        case string s:
            return JsonValue.Create(s);
        case bool b:
            return JsonValue.Create(b);
        case byte or sbyte or short or ushort or int or uint or long or ulong or float or double or decimal:
            return JsonValue.Create((ValueType)value);
        case Enum e:
            return JsonValue.Create(e.ToString());
    }

    if (value is System.Collections.IEnumerable enumerable && value is not string)
    {
        JsonArray array = new();
        foreach (var item in enumerable)
        {
            array.Add(ToJsonNode(item, options));
        }
        return array;
    }

    JsonObject obj = new();
    var naming = options.PropertyNamingPolicy;
    
    // Add type name as $type field
    string typeName = value.GetType().Name;
    obj["$type"] = JsonValue.Create(typeName);
    
    foreach (PropertyInfo prop in value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
    {
        if (!prop.CanRead) continue;
        string propName = naming?.ConvertName(prop.Name) ?? prop.Name;
        obj[propName] = ToJsonNode(prop.GetValue(value), options);
    }
    return obj;
}

