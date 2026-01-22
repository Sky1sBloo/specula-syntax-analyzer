using System.Text.Json;

namespace SpeculaSyntaxAnalyzer.LexerReader;

public class LexerFileReader
{
    public static LexerOutput ParseFile(string fileName)
    {
        string fileContent;
        using (var sr = new StreamReader(fileName))
        {
            fileContent = sr.ReadToEnd();
        }
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        LexerOutput? output = JsonSerializer.Deserialize<LexerOutput>(fileContent, options);

        if (output == null)
        {
            throw new LexerReadException($"Serialize failed on file: {fileName}. Invalid format");
        }
        return output;
    }
}
