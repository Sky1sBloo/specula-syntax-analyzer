namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ErrorsHandler
{
    public List<string> ErrorList { get; private set; } = new();

    public void AddError(string errorMsg)
    {
        ErrorList.Add(errorMsg);
    }

    public void AddError(SyntaxErrorException exception)
    {
        ErrorList.Add(exception.Message);
    }

    public void ClearErrors()
    {
        ErrorList.Clear();
    }
}
