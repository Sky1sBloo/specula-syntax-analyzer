using System.Text.Json.Serialization;
using SpeculaSyntaxAnalyzer.LexerReader;
namespace SpeculaSyntaxAnalyzer;

public class Token
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TokenType Type { get; set; }
    public string Value { get; set; } = "";
    public int Line { get; set; }
    [JsonPropertyName("char_start")]
    public int CharStart { get; set; }
    [JsonPropertyName("char_end")]
    public int CharEnd { get; set; }
}
