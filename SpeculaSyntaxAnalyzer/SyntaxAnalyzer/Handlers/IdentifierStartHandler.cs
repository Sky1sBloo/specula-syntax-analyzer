using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Used for recognizing possible expressions starting with an identifier
/// </summary>
public class IdentifierStartHandler : Handler
{
    private readonly VarAssignHandler varAssignHandler;
    public IdentifierStartHandler(ErrorsHandler errors) : base(errors)
    {
        varAssignHandler = new(errors);
    }

    protected override ParseNode? verifyTokens()
    {
        // assignment statement
        ParseNode? node = delegateToHandler(varAssignHandler);
        if (node != null)
        {
            return node;
        } 
        return null;
    }
}