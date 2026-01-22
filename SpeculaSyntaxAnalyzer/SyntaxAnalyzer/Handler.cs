namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public enum HandlerStatus 
{
    PENDING,
    FINISHED
}

public interface Handler
{
    public HandlerStatus handleToken(Token token);
}
