using System.Runtime.CompilerServices;
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
        string identifier  = "";
        // assignment statement
        ParseNode? node = delegateToHandler(varAssignHandler);
        if (node != null)
        {
            return node;
        } 
        return null;
    }

    protected ParseNode? handleAssignment(string identifier)
    {
        return delegateToHandler(varAssignHandler);
    }
}