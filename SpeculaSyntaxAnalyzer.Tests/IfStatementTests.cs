using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.Tests;

[TestFixture]
public class IfStatementTests
{
    private IfStatementHandler ifHandler = null!;
    private ErrorsHandler errorsHandler = null!;

    [SetUp]
    public void Setup()
    {
        errorsHandler = new();
        ifHandler = new(errorsHandler);
    }

    [Test]
    public void ParensConditionEmptyBody()
    {
        var output = LexerFileReader.ParseFile("Samples/If/ParensConditionEmptyBody.json");
        HandlerOutput result = ifHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(result.node, Is.InstanceOf<IfStatementNode>());
        var ifNode = (IfStatementNode)result.node!;
        Assert.That(ifNode.Body.statements.Count, Is.EqualTo(0));
    }

    [Test]
    public void ParensConditionWithDeclaration()
    {
        var output = LexerFileReader.ParseFile("Samples/If/ParensConditionWithDeclaration.json");
        HandlerOutput result = ifHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(result.node, Is.InstanceOf<IfStatementNode>());
        var ifNode = (IfStatementNode)result.node!;
        Assert.That(ifNode.Body.statements.Count, Is.EqualTo(1));
        Assert.That(ifNode.Body.statements[0], Is.InstanceOf<DeclarationStatementNode>());
    }

    [Test]
    public void NotIfStart_ShouldThrow()
    {
        var output = LexerFileReader.ParseFile("Samples/If/Invalid/NotIfStart.json");
        Assert.Throws<SyntaxErrorException>(() => ifHandler.HandleToken(output.Tokens, 0));
    }

    [Test]
    public void MissingBody_ShouldThrow()
    {
        var output = LexerFileReader.ParseFile("Samples/If/Invalid/MissingBody.json");
        Assert.Throws<SyntaxErrorException>(() => ifHandler.HandleToken(output.Tokens, 0));
        Assert.That(errorsHandler.ErrorList.Count, Is.GreaterThanOrEqualTo(1));
    }

    [Test]
    public void UnclosedBody_ShouldThrow()
    {
        var output = LexerFileReader.ParseFile("Samples/If/Invalid/UnclosedBody.json");
        Assert.Throws<SyntaxErrorException>(() => ifHandler.HandleToken(output.Tokens, 0));
        Assert.That(errorsHandler.ErrorList.Count, Is.GreaterThanOrEqualTo(1));
    }
}