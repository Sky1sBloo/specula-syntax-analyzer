using System.Text;

namespace SpeculaSyntaxAnalyzer.LexerReader;

public class LexerReadException : ArgumentException
{
    public LexerReadException(Token token, string message)
    {
        StringBuilder outMsg = new StringBuilder();
        outMsg.AppendFormat("Token: {0} at {1}:{2}. {3}", token.Type.ToString(), token.Line, token.CharStart, message);
    }
}
