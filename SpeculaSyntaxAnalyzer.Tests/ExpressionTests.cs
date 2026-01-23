using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.Tests;

[TestFixture]
public class ExpressionStatementTests
{
    private ExpressionHandler expressionHandler;
    private ErrorsHandler errorsHandler;

    [SetUp]
    public void Setup()
    {
        errorsHandler = new();
        expressionHandler = new(errorsHandler);
    }

    [Test]
    public void BinaryExpressions()
    {
        var output = LexerFileReader.ParseFile("Samples/Expression/BinaryExpressions/Addition.json");
        HandlerOutput addNode = expressionHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(addNode.node, Is.InstanceOf<Expression>()); 
    }
}