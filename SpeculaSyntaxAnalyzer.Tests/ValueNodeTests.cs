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
        Assert.That(literalValue.value, Is.EqualTo("3.14f"));
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

    [Test]
    public void FunctionCommaEnd()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/Invalid/FunctionCallCommaEnd.json");
        Assert.Throws<SyntaxErrorException>(() => 
        {
            HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        });

        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(2));
    }

    [Test]
    public void StructInitNoKeys()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/StructInitNoKeys.json");
        HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<StructInitialization>());
        
        var structInit = (StructInitialization)node.node!;
        Assert.That(structInit.identifier, Is.EqualTo("structTest"));
        Assert.That(structInit.keys.Count, Is.EqualTo(0));
    }

    [Test]
    public void StructInitOneKey()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/StructInitOneKey.json");
        HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<StructInitialization>());
        
        var structInit = (StructInitialization)node.node!;
        Assert.That(structInit.identifier, Is.EqualTo("structTest"));
        Assert.That(structInit.keys.Count, Is.EqualTo(1));
        Assert.That(structInit.keys[0].key, Is.EqualTo("x"));
        Assert.That(structInit.keys[0].value, Is.TypeOf<LiteralValue>());
    }

    [Test]
    public void StructInitMultipleKeys()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/StructInitMultipleKeys.json");
        HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<StructInitialization>());
        
        var structInit = (StructInitialization)node.node!;
        Assert.That(structInit.identifier, Is.EqualTo("structTest"));
        Assert.That(structInit.keys.Count, Is.EqualTo(2));
        Assert.That(structInit.keys[0].key, Is.EqualTo("value"));
        Assert.That(structInit.keys[0].value, Is.TypeOf<IdentifierValue>());
        Assert.That(structInit.keys[1].key, Is.EqualTo("param2"));
        Assert.That(structInit.keys[1].value, Is.TypeOf<LiteralValue>());
    }

    [Test]
    public void StructInitUndefinedKey()
    {
        var output = LexerFileReader.ParseFile("Samples/Value/Invalid/StructInitUndefinedKey.json");
        Assert.Throws<SyntaxErrorException>(() => 
        {
            HandlerOutput node = valueHandler.HandleToken(output.Tokens, 0);
        });

        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(2));
    }
}
