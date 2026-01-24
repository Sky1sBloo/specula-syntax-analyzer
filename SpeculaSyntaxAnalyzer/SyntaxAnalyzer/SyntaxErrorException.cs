using System.Text;
namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class SyntaxErrorException : Exception
{
    public override string Message { get; }

    public SyntaxErrorException(List<string> expectedTokens, Token receivedToken)
    {
        StringBuilder outMsg = new StringBuilder();
        outMsg.Append("Expected token: ");
        foreach (string token in expectedTokens)
        {
            outMsg.AppendFormat("{0}, ", token);
        }
        outMsg.Length--;

        outMsg.AppendFormat(" Received: {0}. ", receivedToken.Type.ToString());
        outMsg.Append(getTokenPos(receivedToken));
        Message = outMsg.ToString();

    }

    public SyntaxErrorException(string message)
    {
        Message = message;
    }

    private string getTokenPos(Token token)
    {
        return $" at {token.Line}:{token.CharStart}";
    }
}
