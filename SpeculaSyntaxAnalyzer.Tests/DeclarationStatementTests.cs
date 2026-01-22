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
        Assert.That(output.Tokens.Count, Is.EqualTo(7));
        Assert.DoesNotThrow(() =>
        {
            analyzer.ReadTokens(output.Tokens);
        });
    }

    [Test]
    public void MultipleDeclaration()
    {

    }
}
