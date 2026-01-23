using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;

namespace SpeculaSyntaxAnalyzer.Tests;

[TestFixture]
public class DeclarationStatementTests
{
    private SyntaxAnalyzerRoot analyzer = new();

    [SetUp]
    public void Setup()
    {
        analyzer = new();
    }

    [Test]
    public void SingleDeclaration()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/SingleDeclaration.json");
        List<ParseNode> node = analyzer.ReadTokens(output.Tokens);
        Assert.That(node.Count, Is.EqualTo(1));
        Assert.That(analyzer.ErrorHandler.ErrorList.Count, Is.EqualTo(0));
    }

    [Test]
    public void SingleDeclarationWithValue()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/SingleDeclarationWithValue.json");
        List<ParseNode> node = analyzer.ReadTokens(output.Tokens);
        Assert.That(node.Count, Is.EqualTo(1));
        Assert.That(analyzer.ErrorHandler.ErrorList.Count, Is.EqualTo(0));
    }

    [Test]
    public void MultipleDeclaration()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/MultipleDeclaration.json");
        List<ParseNode> node = analyzer.ReadTokens(output.Tokens);
        Assert.That(node.Count, Is.EqualTo(3));
        Assert.That(analyzer.ErrorHandler.ErrorList.Count, Is.EqualTo(0));
    }

    [Test]
    public void ExpressionValues()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/Expressions.json");
        List<ParseNode> node = analyzer.ReadTokens(output.Tokens);
        foreach (var n in node)
            Console.WriteLine(n);
        Assert.That(analyzer.ErrorHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.Count, Is.EqualTo(7));
    }
 

    [Test]
    public void NoIdentifier()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/Invalid/NoIdentifier.json");
        List<ParseNode> node = analyzer.ReadTokens(output.Tokens);

        Assert.That(node.Count, Is.EqualTo(0));
        Assert.That(analyzer.ErrorHandler.ErrorList.Count, Is.EqualTo(2));
    }

    [Test]
    public void NoType()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/Invalid/NoType.json");
        List<ParseNode> node = analyzer.ReadTokens(output.Tokens);
        foreach (var err in analyzer.ErrorHandler.ErrorList)
        {
            Console.WriteLine(err);
        }
        Assert.That(node.Count, Is.EqualTo(0));
        Assert.That(analyzer.ErrorHandler.ErrorList.Count, Is.EqualTo(2));
    }
}
