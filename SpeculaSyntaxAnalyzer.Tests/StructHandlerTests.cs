using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.Tests;

[TestFixture]
public class StructHandlerTests
{
    private SyntaxAnalyzerRoot analyzer = new();

    [SetUp]
    public void Setup()
    {
        analyzer = new();
    }

    [Test]
    public void SimpleDefinitionParses()
    {
        var output = LexerFileReader.ParseFile("Samples/Struct/SimpleDefinition.json");
        ParseNode? node = analyzer.ReadTokens(output.Tokens);
        Assert.That(analyzer.ErrorHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node, Is.Not.Null);
        Assert.That(node, Is.TypeOf<RootNode>());
        var rootNode = (RootNode)node!;
        Assert.That(rootNode.statements.Count, Is.EqualTo(1));
        var structNode = rootNode.statements[0] as StructDefNode;
        Assert.That(structNode, Is.Not.Null);
        Assert.That(structNode!.structName, Is.EqualTo("User"));
        Assert.That(structNode.fields.Count, Is.EqualTo(2));

        Assert.That(structNode.fields[0].identifier, Is.EqualTo("id"));
        Assert.That(structNode.fields[0].typeDef.DataType.DataType, Is.EqualTo(DataTypes.INT));
        Assert.That(structNode.fields[0].typeDef.Capabilities.capabilityList.Count, Is.EqualTo(2));
        Assert.That(structNode.fields[0].typeDef.Capabilities.capabilityList[0].type, Is.EqualTo(CapabilityTypes.OWN));
        Assert.That(structNode.fields[0].typeDef.Capabilities.capabilityList[1].type, Is.EqualTo(CapabilityTypes.CONST));

        Assert.That(structNode.fields[1].identifier, Is.EqualTo("name"));
        Assert.That(structNode.fields[1].typeDef.DataType.DataType, Is.EqualTo(DataTypes.STRING));
        Assert.That(structNode.fields[1].typeDef.Capabilities.capabilityList.Count, Is.EqualTo(2));
        Assert.That(structNode.fields[1].typeDef.Capabilities.capabilityList[0].type, Is.EqualTo(CapabilityTypes.OWN));
        Assert.That(structNode.fields[1].typeDef.Capabilities.capabilityList[1].type, Is.EqualTo(CapabilityTypes.CONST));
    }

    [Test]
    public void StructWithCapabilities()
    {
        var output = LexerFileReader.ParseFile("Samples/Struct/CapabilityDefinition.json");
        ParseNode? node = analyzer.ReadTokens(output.Tokens);
        Assert.That(analyzer.ErrorHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node, Is.Not.Null);
        var rootNode = (RootNode)node!;
        Assert.That(rootNode.statements.Count, Is.EqualTo(1));
        var structNode = rootNode.statements[0] as StructDefNode;
        Assert.That(structNode, Is.Not.Null);
        Assert.That(structNode!.structName, Is.EqualTo("Flags"));
        Assert.That(structNode.fields.Count, Is.EqualTo(2));

        Assert.That(structNode.fields[0].identifier, Is.EqualTo("active"));
        Assert.That(structNode.fields[0].typeDef.DataType.DataType, Is.EqualTo(DataTypes.BOOL));
        Assert.That(structNode.fields[0].typeDef.Capabilities.capabilityList.Count, Is.EqualTo(1));
        Assert.That(structNode.fields[0].typeDef.Capabilities.capabilityList[0].type, Is.EqualTo(CapabilityTypes.OWN));

        Assert.That(structNode.fields[1].identifier, Is.EqualTo("mode"));
        Assert.That(structNode.fields[1].typeDef.DataType.DataType, Is.EqualTo(DataTypes.STRING));
        Assert.That(structNode.fields[1].typeDef.Capabilities.capabilityList.Count, Is.EqualTo(1));
        Assert.That(structNode.fields[1].typeDef.Capabilities.capabilityList[0].type, Is.EqualTo(CapabilityTypes.CONST));
    }

    [Test]
    public void NestedStructWithSharedCapability()
    {
        var output = LexerFileReader.ParseFile("Samples/Struct/Nested.json");
        ParseNode? node = analyzer.ReadTokens(output.Tokens);
        Assert.That(analyzer.ErrorHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node, Is.Not.Null);
        var rootNode = (RootNode)node!;
        Assert.That(rootNode.statements.Count, Is.EqualTo(1));
        var structNode = rootNode.statements[0] as StructDefNode;
        Assert.That(structNode, Is.Not.Null);
        Assert.That(structNode!.structName, Is.EqualTo("Nested"));
        Assert.That(structNode.fields.Count, Is.EqualTo(1));

        Assert.That(structNode.fields[0].identifier, Is.EqualTo("inner"));
        Assert.That(structNode.fields[0].typeDef.DataType.DataType, Is.EqualTo(DataTypes.IDENTIFIER));
        Assert.That(structNode.fields[0].typeDef.Capabilities.capabilityList.Count, Is.EqualTo(1));
        Assert.That(structNode.fields[0].typeDef.Capabilities.capabilityList[0].type, Is.EqualTo(CapabilityTypes.SHARED));
    }

    [Test]
    public void EmptyStructThrows()
    {
        var output = LexerFileReader.ParseFile("Samples/Struct/Invalid/EmptyStruct.json");
        Assert.Throws<InvalidOperationException>(() => analyzer.ReadTokens(output.Tokens));
    }

    [Test]
    public void MissingClosingBraceThrows()
    {
        var output = LexerFileReader.ParseFile("Samples/Struct/Invalid/MissingClosingBrace.json");
        Assert.Throws<InvalidOperationException>(() => analyzer.ReadTokens(output.Tokens));
    }

    [Test]
    public void FieldWithoutLetThrows()
    {
        var output = LexerFileReader.ParseFile("Samples/Struct/Invalid/NoLet.json");
        Assert.Throws<SyntaxErrorException>(() => analyzer.ReadTokens(output.Tokens));
    }

    [Test]
    public void TrailingCommaInCapabilitiesThrows()
    {
        var output = LexerFileReader.ParseFile("Samples/Struct/Invalid/TrailingComma.json");
        Assert.Throws<SyntaxErrorException>(() => analyzer.ReadTokens(output.Tokens));
    }
}
