using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.Tests;

[TestFixture]
public class CapabilityTests
{
    private CapabilityHandler capabilityHandler;
    private ErrorsHandler errorsHandler;

    [SetUp]
    public void Setup()
    {
        errorsHandler = new();
        capabilityHandler = new(errorsHandler);
    }

    [Test]
    public void SingleCapability()
    {
        var output = LexerFileReader.ParseFile("Samples/Capability/SingleCapability.json");
        HandlerOutput node = capabilityHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<Capabilities>());
        
        var capabilities = (Capabilities)node.node!;
        Assert.That(capabilities.capabilityList.Count, Is.EqualTo(1));
        Assert.That(capabilities.capabilityList[0].type, Is.EqualTo(CapabilityTypes.OWN));
        Assert.That(capabilities.capabilityList[0].configuration.Count, Is.EqualTo(0));
    }

    [Test]
    public void MultipleCapability()
    {
        var output = LexerFileReader.ParseFile("Samples/Capability/MultipleCapability.json");
        HandlerOutput node = capabilityHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<Capabilities>());
        
        var capabilities = (Capabilities)node.node!;
        Assert.That(capabilities.capabilityList.Count, Is.EqualTo(2));
        Assert.That(capabilities.capabilityList[0].type, Is.EqualTo(CapabilityTypes.OWN));
        Assert.That(capabilities.capabilityList[0].configuration.Count, Is.EqualTo(0));
        Assert.That(capabilities.capabilityList[1].type, Is.EqualTo(CapabilityTypes.MUT));
        Assert.That(capabilities.capabilityList[1].configuration.Count, Is.EqualTo(0));
    }

    [Test]
    public void CapabilityWithSettings()
    {
        var output = LexerFileReader.ParseFile("Samples/Capability/CapabilityWithSettings.json");
        HandlerOutput node = capabilityHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<Capabilities>());
        
        var capabilities = (Capabilities)node.node!;
        Assert.That(capabilities.capabilityList.Count, Is.EqualTo(3));
        
        Assert.That(capabilities.capabilityList[0].type, Is.EqualTo(CapabilityTypes.OWN));
        Assert.That(capabilities.capabilityList[0].configuration.Count, Is.EqualTo(0));
        
        Assert.That(capabilities.capabilityList[1].type, Is.EqualTo(CapabilityTypes.NETWORK));
        Assert.That(capabilities.capabilityList[1].configuration.Count, Is.EqualTo(1));
        Assert.That(capabilities.capabilityList[1].configuration[0], Is.EqualTo("json"));
        
        Assert.That(capabilities.capabilityList[2].type, Is.EqualTo(CapabilityTypes.SHARED));
        Assert.That(capabilities.capabilityList[2].configuration.Count, Is.EqualTo(1));
        Assert.That(capabilities.capabilityList[2].configuration[0], Is.EqualTo("this"));
    }

    [Test]
    public void EmptyCapability()
    {
        var output = LexerFileReader.ParseFile("Samples/Capability/Invalid/EmptyCapability.json");
        Assert.Throws<SyntaxErrorException>(() => 
        {
            HandlerOutput node = capabilityHandler.HandleToken(output.Tokens, 0);
        });

        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(1));
    }

    [Test]
    public void TrailingComma()
    {
        var output = LexerFileReader.ParseFile("Samples/Capability/Invalid/TrailingComma.json");
        Assert.Throws<SyntaxErrorException>(() => 
        {
            HandlerOutput node = capabilityHandler.HandleToken(output.Tokens, 0);
        });

        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(1));
    }

    [Test]
    public void UnknownCapability()
    {
        var output = LexerFileReader.ParseFile("Samples/Capability/Invalid/UnknownCapability.json");
        Assert.Throws<SyntaxErrorException>(() => 
        {
            HandlerOutput node = capabilityHandler.HandleToken(output.Tokens, 0);
        });

        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(1));
    }

    [Test]
    public void StackedCapability()
    {
        var output = LexerFileReader.ParseFile("Samples/Capability/Invalid/StackedCapability.json");
        Assert.Throws<SyntaxErrorException>(() => 
        {
            HandlerOutput node = capabilityHandler.HandleToken(output.Tokens, 0);
        });

        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(1));
    }
}
