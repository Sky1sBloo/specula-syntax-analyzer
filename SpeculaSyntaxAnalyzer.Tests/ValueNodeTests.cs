using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.Tests;

[TestFixture]
public class ValueNodeTests
{
    private ValueHandler valueHandler;
    private ErrorsHandler errorsHandler;

    [SetUp]
    public void Setup()
    {
        errorsHandler = new();
        valueHandler = new(errorsHandler);
    }

    [Test]
    public void LiteralIntValue()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/LiteralInt.json");
        HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<LiteralValue>());
        
        var literalValue = (LiteralValue)node.node!;
        Assert.That(literalValue.type.type, Is.EqualTo(DataTypes.INT));
        Assert.That(literalValue.value, Is.EqualTo("42"));
    }

    [Test]
    public void LiteralStringValue()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/LiteralString.json");
        HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<LiteralValue>());
        
        var literalValue = (LiteralValue)node.node!;
        Assert.That(literalValue.type.type, Is.EqualTo(DataTypes.STRING));
        Assert.That(literalValue.value, Is.EqualTo("\"hello\""));
    }

    [Test]
    public void LiteralBoolValue()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/LiteralBool.json");
        HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<LiteralValue>());
        
        var literalValue = (LiteralValue)node.node!;
        Assert.That(literalValue.type.type, Is.EqualTo(DataTypes.BOOL));
        Assert.That(literalValue.value, Is.EqualTo("true"));
    }

    [Test]
    public void LiteralFloatValue()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/LiteralFloat.json");
        HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<LiteralValue>());
        
        var literalValue = (LiteralValue)node.node!;
        Assert.That(literalValue.type.type, Is.EqualTo(DataTypes.FLOAT));
        Assert.That(literalValue.value, Is.EqualTo("3.14"));
    }

    [Test]
    public void IdentifierValue()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/Identifier.json");
        HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<IdentifierValue>());
        
        var identifierValue = (IdentifierValue)node.node!;
        Assert.That(identifierValue.value, Is.EqualTo("myVar"));
    }

    [Test]
    public void FunctionCallNoParameters()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/FunctionCallNoParams.json");
        HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<FunctionCallValue>());
        
        var funcCall = (FunctionCallValue)node.node!;
        Assert.That(funcCall.identifier, Is.EqualTo("print"));
        Assert.That(funcCall.funcParams.Count, Is.EqualTo(0));
    }

    [Test]
    public void FunctionCallOneParameter()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/FunctionCallOneParam.json");
        HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<FunctionCallValue>());
        
        var funcCall = (FunctionCallValue)node.node!;
        Assert.That(funcCall.identifier, Is.EqualTo("print"));
        Assert.That(funcCall.funcParams.Count, Is.EqualTo(1));
        Assert.That(funcCall.funcParams[0], Is.TypeOf<LiteralValue>());
        
        var literalParam = (LiteralValue)funcCall.funcParams[0];
        Assert.That(literalParam.value, Is.EqualTo("42"));
    }

    [Test]
    public void FunctionCallMultipleParameters()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/FunctionCallMultipleParams.json");
        HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<FunctionCallValue>());
        
        var funcCall = (FunctionCallValue)node.node!;
        Assert.That(funcCall.identifier, Is.EqualTo("print"));
        Assert.That(funcCall.funcParams.Count, Is.EqualTo(2));
        Assert.That(funcCall.funcParams[0], Is.TypeOf<LiteralValue>());
        Assert.That(funcCall.funcParams[1], Is.TypeOf<LiteralValue>());
    }
}
