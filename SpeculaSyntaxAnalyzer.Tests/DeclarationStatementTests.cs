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
        Assert.DoesNotThrow(() =>
        {
            analyzer.ReadTokens(output.Tokens);
        });
    }

    [Test]
    public void SingleDeclarationWithValue()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/SingleDeclarationWithValue.json");
        Assert.DoesNotThrow(() =>
        {
            analyzer.ReadTokens(output.Tokens);
        });
    }

    [Test]
    public void MultipleDeclaration()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/MultipleDeclaration.json");
        Assert.DoesNotThrow(() =>
        {
            analyzer.ReadTokens(output.Tokens);
        });
    }

    [Test]
    public void NoIdentifier()
    {
        var output = LexerFileReader.ParseFile("Samples/Declaration/Invalid/NoIdentifier.json");
        Assert.Throws<SyntaxErrorException>(() =>
        {
            analyzer.ReadTokens(output.Tokens);
        });

    }
}
