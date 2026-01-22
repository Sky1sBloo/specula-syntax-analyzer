namespace SpeculaSyntaxAnalyzer.LexerReader;

public class Token
{
    public TokenType Type { get; private set; }
    public string Value { get; private set; }
    public int Line { get; private set; }
    public int CharStart { get; private set; }
    public int CharEnd { get; private set; }

    public Token(TokenType type, string value, int line, int charStart, int charEnd)
    {
        Type = type;
        Value = value;
        Line = line;
        CharStart = charStart;
        CharEnd = charEnd;
    }

    public Token(string type, string value, int line, int charStart, int charEnd)
    {
        bool isParse = Enum.TryParse<TokenType>(type, ignoreCase: true, out var typeEnum);
        Type = isParse ? typeEnum : TokenType.UNKNOWN;
        Value = value;
        Line = line;
        CharStart = charStart;
        CharEnd = charEnd;
    }
}
