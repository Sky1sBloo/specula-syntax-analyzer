using System.Text;
namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class SyntaxErrorException : Exception
{
    public override string Message { get; }

    public SyntaxErrorException(Token token, string message)
    {
        StringBuilder outMsg = new StringBuilder();
        outMsg.AppendFormat("Token: {0} at {1}:{2}. {3}", token.Type.ToString(), token.Line, token.CharStart, message);
        Message = outMsg.ToString();
    }

    public SyntaxErrorException(string message)
    {
        Message = message;
    }

}
