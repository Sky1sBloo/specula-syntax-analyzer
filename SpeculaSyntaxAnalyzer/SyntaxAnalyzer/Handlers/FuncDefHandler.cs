using System.Runtime.CompilerServices;
using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class FuncDefHandler : Handler
{
    private readonly VarDefinitionHandler varDefinitionHandler;
    private readonly BodyHandler bodyHandler;
    public FuncDefHandler(ErrorsHandler errors) : base(errors)
    {
        varDefinitionHandler = new(errors, true);
        bodyHandler = new(errors);
    }

    protected override ParseNode? verifyTokens()
    {
        bool isAsync = false;
        if (CurrentToken.Type != Token.Types.K_FN)
        {
            throw new SyntaxErrorException(["fn"], CurrentToken);
        }
        incrementIndex();
        if (CurrentToken.Type == Token.Types.K_ASYNC)
        {
            isAsync = true;
            incrementIndex();
        }
        if (CurrentToken.Type != Token.Types.IDENT)
        {
            throw new SyntaxErrorException(["IDENTIFIER"], CurrentToken);
        }
        string funcName = CurrentToken.Value;
        incrementIndex();

        if (CurrentToken.Type != Token.Types.D_PAR_OP)
        {
            throw new SyntaxErrorException(["("], CurrentToken);
        }
        incrementIndex();
        PrintableList<FuncParam> parameters = parseParameters();

        TypeDefinitionNode? returnType = getReturnType();
        if (returnType == null)
        {
            returnType = new TypeDefinitionNode(DataTypes.VOID, CapabilityHandler.GenerateDefaultCapabilities());
        }
        BodyNode? body = parseFunctionBody();
        if (body == null)
            return null;
        return new FuncDefNode(funcName, isAsync, parameters, returnType, body);
    }

    private PrintableList<FuncParam> parseParameters()
    {
        var parameters = new PrintableList<FuncParam>();

        while (CurrentToken.Type != Token.Types.D_PAR_CLO)
        {
            if (CurrentToken.Type != Token.Types.IDENT)
            {
                throw new SyntaxErrorException(["IDENTIFIER"], CurrentToken);
            }
            string paramName = CurrentToken.Value;
            incrementIndex();

            if (CurrentToken.Type != Token.Types.D_COLON)
            {
                throw new SyntaxErrorException([":"], CurrentToken);
            }
            incrementIndex();

            TypeDefinitionNode? paramType = parseTypeDefinitionNode();
            if (paramType != null)
            {
                parameters.Add(new FuncParam(paramName, paramType));
            }


            if (CurrentToken.Type == Token.Types.COMMA)
            {
                incrementIndex();
            }
            else if (CurrentToken.Type != Token.Types.D_PAR_CLO)
            {
                throw new SyntaxErrorException([",", ")"], CurrentToken);
            }
        }

        incrementIndex();

        return parameters;
    }

    private TypeDefinitionNode? getReturnType()
    {
        if (CurrentToken.Type != Token.Types.OP_RIGHT_OP)
        {
            return null;
        }
        incrementIndex();

        return parseTypeDefinitionNode();
    }

    private TypeDefinitionNode? parseTypeDefinitionNode()
    {
        ParseNode? varDefNode = delegateToHandler(varDefinitionHandler);
        if (varDefNode == null)
        {
            return null;
        }
        if (varDefNode is TypeDefinitionNode definition)
        {
            return definition;
        }
        else
        {
            throw new InvalidOperationException("TypeDefinitionNodeHandler did not return a TypeDefinitionNode node.");
        }
    }

    private BodyNode? parseFunctionBody()
    {
        ParseNode? bodyNode = delegateToHandler(bodyHandler);
        if (bodyNode == null)
        {
            return null;
        }
        if (bodyNode is BodyNode body)
        {
            return body;
        }
        else
        {
            throw new InvalidOperationException("BodyHandler did not return a BodyNode for function body.");
        }
    }
}