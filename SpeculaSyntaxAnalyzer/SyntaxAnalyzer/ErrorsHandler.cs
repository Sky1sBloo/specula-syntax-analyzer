namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class ErrorsHandler
{
    public List<string> ErrorList { get; private set; } = new();
    public bool SuppressErrors { get; set; } = false;

    public void AddError(string errorMsg)
    {
        if (!SuppressErrors)
        {
            ErrorList.Add(errorMsg);
        }
    }

    public void AddError(SyntaxErrorException exception)
    {
        if (!SuppressErrors)
        {
            ErrorList.Add(exception.Message);
        }
    }

    public void ClearErrors()
    {
        ErrorList.Clear();
    }
}
