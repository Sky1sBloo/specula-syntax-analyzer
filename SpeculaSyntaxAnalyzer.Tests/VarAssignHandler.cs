using SpeculaSyntaxAnalyzer.SyntaxAnalyzer;
using SpeculaSyntaxAnalyzer.LexerReader;
using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.Tests;

[TestFixture]
public class VarAssignTests
{
    private VarAssignHandler varAssignHandler;
    private ErrorsHandler errorsHandler;

    [SetUp]
    public void Setup()
    {
        errorsHandler = new();
        varAssignHandler = new(errorsHandler);
    }

    [Test]
    public void AssignmentOperators()
    {
        // x = 5
        var output = LexerFileReader.ParseFile("Samples/VarAssign/SimpleAssignment.json");
        HandlerOutput node = varAssignHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.InstanceOf<Expression>());

        // x += 3
        Setup(); 
        output = LexerFileReader.ParseFile("Samples/VarAssign/AddAssignment.json");
        node = varAssignHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.InstanceOf<Expression>());

        // y -= 2
        Setup(); 
        output = LexerFileReader.ParseFile("Samples/VarAssign/SubtractAssignment.json");
        node = varAssignHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.InstanceOf<Expression>());

        // z *= 4
        Setup(); 
        output = LexerFileReader.ParseFile("Samples/VarAssign/MultiplyAssignment.json");
        node = varAssignHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.InstanceOf<Expression>());

        // w /= 2
        Setup(); 
        output = LexerFileReader.ParseFile("Samples/VarAssign/DivideAssignment.json");
        node = varAssignHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.InstanceOf<Expression>());

        // m %= 3
        Setup(); 
        output = LexerFileReader.ParseFile("Samples/VarAssign/ModuloAssignment.json");
        node = varAssignHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.InstanceOf<Expression>());
    }

    [Test]
    public void ExpressionAssignment()
    {
        // x = 5 + 3
        var output = LexerFileReader.ParseFile("Samples/VarAssign/ExpressionAssignment.json");
        HandlerOutput node = varAssignHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<AddExpression>());
        var addExpr = (AddExpression)node.node!;
        Assert.That(addExpr.lhs, Is.TypeOf<LiteralValue>());
        Assert.That(addExpr.rhs, Is.TypeOf<LiteralValue>());
    }

    [Test]
    public void CompoundExpressionAssignment()
    {
        // x += 5 * 2 should parse as AddAssign with expression 5 * 2
        var output = LexerFileReader.ParseFile("Samples/VarAssign/CompoundExpressionAssignment.json");
        HandlerOutput node = varAssignHandler.HandleToken(output.Tokens, 0);
        Assert.That(errorsHandler.ErrorList.Count, Is.EqualTo(0));
        Assert.That(node.node, Is.TypeOf<MultExpression>());
        var multExpr = (MultExpression)node.node!;
        Assert.That(multExpr.lhs, Is.TypeOf<LiteralValue>());
        Assert.That(multExpr.rhs, Is.TypeOf<LiteralValue>());
    }

    [Test]
    public void MissingIdentifier()
    {
        // Missing identifier should throw error
        var tokens = new List<Token>
        {
            new() { Type = Token.Types.OP_EQUALS, Value = "=", Line = 1, CharStart = 1, CharEnd = 2 },
            new() { Type = Token.Types.L_INT, Value = "5", Line = 1, CharStart = 3, CharEnd = 4 }
        };
        
        Assert.Throws<SyntaxErrorException>(() => varAssignHandler.HandleToken(tokens, 0));
    }

    [Test]
    public void InvalidAssignmentNoRHS()
    {
        // Assignment without right-hand side should fail
        var tokens = new List<Token>
        {
            new() { Type = Token.Types.IDENT, Value = "x", Line = 1, CharStart = 1, CharEnd = 2 },
            new() { Type = Token.Types.OP_EQUALS, Value = "=", Line = 1, CharStart = 3, CharEnd = 4 }
            // Missing expression after equals
        };
        
        // Should throw an exception since there's no expression after the equals
        Assert.Throws<ArgumentOutOfRangeException>(() => varAssignHandler.HandleToken(tokens, 0));
    }
}
