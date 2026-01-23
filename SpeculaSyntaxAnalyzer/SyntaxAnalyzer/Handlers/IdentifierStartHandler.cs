using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

/// <summary>
/// Used for recognizing possible expressions starting with an identifier
/// </summary>
public class IdentifierStartHandler : Handler
{
    public IdentifierStartHandler(ErrorsHandler errors) : base(errors)
    {
    }

    protected override ParseNode? verifyTokens()
    {
        return null;
    } 
}