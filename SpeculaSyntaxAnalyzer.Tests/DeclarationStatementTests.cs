using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.Tests;

[TestFixture]
public class DeclarationStatementTests
{
    private SyntaxAnalyzerRoot analyzer = new();

    [SetUp]
    public void Setup()
    {
        analyzer.Reset();
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
        Assert.That(analyzer.ErrorHandler.ErrorList.Count, Is.EqualTo(0));
    }

    [Test]
    public void NoIdentifier()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/Invalid/NoIdentifier.json");
        analyzer.ReadTokens(output.Tokens);
        Assert.That(analyzer.ErrorHandler.ErrorList.Count, Is.EqualTo(2));
    }

    [Test]
    public void NoType()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/Invalid/NoType.json");
        analyzer.ReadTokens(output.Tokens);
        Assert.That(analyzer.ErrorHandler.ErrorList.Count, Is.EqualTo(1));
    }
}
