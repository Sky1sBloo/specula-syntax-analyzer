using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
using SpeculaSyntaxAnalyzer.LexerReader;

namespace SpeculaSyntaxAnalyzer.Tests;

[TestFixture]
public class DeclarationStatementTests
{
    private SyntaxAnalyzerRoot analyzer = new();

    [SetUp]
    public void Setup()
    {
        analyzer = new SyntaxAnalyzerRoot();
    }

    [Test]
    public void SingleDeclaration()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/SingleDeclaration.json");
        analyzer.ReadTokens(output.Tokens);
        Assert.That(analyzer.Errors.Count, Is.EqualTo(0));
    }

    [Test]
    public void SingleDeclarationWithValue()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/SingleDeclarationWithValue.json");
        analyzer.ReadTokens(output.Tokens);
        Assert.That(analyzer.Errors.Count, Is.EqualTo(0));
    }

    [Test]
    public void MultipleDeclaration()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/MultipleDeclaration.json");
        analyzer.ReadTokens(output.Tokens);
        Assert.That(analyzer.Errors.Count, Is.EqualTo(0));
    }

    [Test]
    public void NoIdentifier()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/Invalid/NoIdentifier.json");
        analyzer.ReadTokens(output.Tokens);
        Assert.That(analyzer.Errors.Count, Is.EqualTo(2));
    }

    [Test]
    public void NoType()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/Invalid/NoType.json");
        analyzer.ReadTokens(output.Tokens);
        Assert.That(analyzer.Errors.Count, Is.EqualTo(1));
    }
}
