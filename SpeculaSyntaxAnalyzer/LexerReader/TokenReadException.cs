using System.Text;

namespace SpeculaSyntaxAnalyzer.LexerReader;

public class TokenReadException : ArgumentException
{
    public TokenReadException(Token token, string message)
    {
        StringBuilder outMsg = new StringBuilder();
        outMsg.AppendFormat("Token: {0} at {1}:{2}. {3}", token.Type.ToString(), token.Line, token.CharStart, message);
    }
}
