using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.Tests;

[TestFixture]
public class ConditionalStatementTests 
{
    private ConditionalStatementHandler ifHandler = null!;
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
        var output = LexerFileReader.ParseFile("Samples/Conditional/ParensConditionEmptyBody.json");
        HandlerOutput result = ifHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(result.node, Is.TypeOf<ConditionalStatement>());
        var conditionalNode = (ConditionalStatement)result.node!;
        Assert.That(conditionalNode.ifStatement.Body.statements.Count, Is.EqualTo(0));
        Assert.That(conditionalNode.elseIfStatement.Count, Is.EqualTo(0));
        Assert.That(conditionalNode.elseStatement, Is.Null);
    }

    [Test]
    public void ParensConditionWithDeclaration()
    {
        var output = LexerFileReader.ParseFile("Samples/Conditional/ParensConditionWithDeclaration.json");
        HandlerOutput result = ifHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(result.node, Is.TypeOf<ConditionalStatement>());
        var conditionalNode = (ConditionalStatement)result.node!;
        Assert.That(conditionalNode.ifStatement.Body.statements.Count, Is.EqualTo(1));
        Assert.That(conditionalNode.ifStatement.Body.statements[0], Is.InstanceOf<DeclarationStatementNode>());
        Assert.That(conditionalNode.elseIfStatement.Count, Is.EqualTo(0));
        Assert.That(conditionalNode.elseStatement, Is.Null);
    }

    [Test]
    public void IfWithElse()
    {
        var output = LexerFileReader.ParseFile("Samples/Conditional/IfWithElse.json");
        HandlerOutput result = ifHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(result.node, Is.TypeOf<ConditionalStatement>());
        var conditionalNode = (ConditionalStatement)result.node!;
        Assert.That(conditionalNode.ifStatement.Body.statements.Count, Is.EqualTo(1));
        Assert.That(conditionalNode.elseIfStatement.Count, Is.EqualTo(0));
        Assert.That(conditionalNode.elseStatement, Is.Not.Null);
        Assert.That(conditionalNode.elseStatement!.body.statements.Count, Is.EqualTo(1));
    }

    [Test]
    public void IfWithElseIf()
    {
        var output = LexerFileReader.ParseFile("Samples/Conditional/IfWithIfElse.json");
        HandlerOutput result = ifHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(result.node, Is.TypeOf<ConditionalStatement>());
        var conditionalNode = (ConditionalStatement)result.node!;
        Assert.That(conditionalNode.ifStatement.Body.statements.Count, Is.EqualTo(1));
        Assert.That(conditionalNode.elseIfStatement.Count, Is.EqualTo(1));
        Assert.That(conditionalNode.elseIfStatement[0].Body.statements.Count, Is.EqualTo(1));
        Assert.That(conditionalNode.elseStatement, Is.Null);
    }

    [Test]
    public void IfElseIfElse()
    {
        var output = LexerFileReader.ParseFile("Samples/Conditional/IfElseIfElse.json");
        HandlerOutput result = ifHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(result.node, Is.TypeOf<ConditionalStatement>());
        var conditionalNode = (ConditionalStatement)result.node!;
        Assert.That(conditionalNode.ifStatement.Body.statements.Count, Is.EqualTo(1));
        Assert.That(conditionalNode.elseIfStatement.Count, Is.EqualTo(1));
        Assert.That(conditionalNode.elseIfStatement[0].Body.statements.Count, Is.EqualTo(1));
        Assert.That(conditionalNode.elseStatement, Is.Not.Null);
        Assert.That(conditionalNode.elseStatement!.body.statements.Count, Is.EqualTo(1));
    }

    [Test]
    public void MultipleElseIf()
    {
        var output = LexerFileReader.ParseFile("Samples/Conditional/MultiElseIf.json");
        HandlerOutput result = ifHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(result.node, Is.TypeOf<ConditionalStatement>());
        var conditionalNode = (ConditionalStatement)result.node!;
        Assert.That(conditionalNode.ifStatement.Body.statements.Count, Is.EqualTo(1));
        Assert.That(conditionalNode.elseIfStatement.Count, Is.EqualTo(3));
        Assert.That(conditionalNode.elseIfStatement[0].Body.statements.Count, Is.EqualTo(1));
        Assert.That(conditionalNode.elseIfStatement[1].Body.statements.Count, Is.EqualTo(1));
        Assert.That(conditionalNode.elseIfStatement[2].Body.statements.Count, Is.EqualTo(1));
        Assert.That(conditionalNode.elseStatement, Is.Not.Null);
        Assert.That(conditionalNode.elseStatement!.body.statements.Count, Is.EqualTo(1));
    }

    [Test]
    public void NotIfStart_ShouldThrow()
    {
        var output = LexerFileReader.ParseFile("Samples/Conditional/Invalid/NotIfStart.json");
        Assert.Throws<SyntaxErrorException>(() => ifHandler.HandleToken(output.Tokens, 0));
    }

    [Test]
    public void MissingBody_ShouldThrow()
    {
        var output = LexerFileReader.ParseFile("Samples/Conditional/Invalid/MissingBody.json");
        Assert.Throws<SyntaxErrorException>(() => ifHandler.HandleToken(output.Tokens, 0));
        Assert.That(errorsHandler.ErrorList.Count, Is.GreaterThanOrEqualTo(1));
    }

    [Test]
    public void UnclosedBody_ShouldThrow()
    {
        var output = LexerFileReader.ParseFile("Samples/Conditional/Invalid/UnclosedBody.json");
        Assert.Throws<SyntaxErrorException>(() => ifHandler.HandleToken(output.Tokens, 0));
        Assert.That(errorsHandler.ErrorList.Count, Is.GreaterThanOrEqualTo(1));
    }

    [Test]
    public void NoParensCondition_ShouldThrow()
    {
        var output = LexerFileReader.ParseFile("Samples/Conditional/Invalid/NoParenthesis.json");
        Assert.Throws<SyntaxErrorException>(() => ifHandler.HandleToken(output.Tokens, 0));
        Assert.That(errorsHandler.ErrorList.Count, Is.GreaterThanOrEqualTo(1));
    }
}