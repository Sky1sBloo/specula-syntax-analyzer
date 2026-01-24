using SpeculaSyntaxAnalyzer.ParseTree;

namespace SpeculaSyntaxAnalyzer.SyntaxAnalyzer;

public class CapabilityHandler : Handler
{
    public CapabilityHandler(ErrorsHandler errors) : base(errors) { }

    protected override ParseNode? verifyTokens()
    {
        if (CurrentToken.Type != Token.Types.D_BRAC_OP)
        {
            throw new SyntaxErrorException(["["], CurrentToken);
        }
        incrementIndex();
        PrintableList<Capability> capabilities = new();
        while (true)
        {
            CapabilityTypes type;
            PrintableList<string> configuration = new();

            if (!HasMoreTokens)
            {
                Token missing = new()
                {
                    Type = Token.Types.UNKNOWN,
                    Line = getIndex(),
                    CharStart = 0,
                    CharEnd = 0
                };
                throw new SyntaxErrorException(["]"], missing);
            }

            if (CurrentToken.Type == Token.Types.D_BRAC_CLO)
            {
                break;
            }

            switch (CurrentToken.Type)
            {
                case Token.Types.K_OWN:
                    type = CapabilityTypes.OWN;
                    break;
                case Token.Types.K_MOVE:
                    type = CapabilityTypes.MOVE;
                    break;
                case Token.Types.K_SHARED:
                    type = CapabilityTypes.SHARED;
                    break;
                case Token.Types.K_VIEW:
                    type = CapabilityTypes.VIEW;
                    break;
                case Token.Types.K_SHARE:
                    type = CapabilityTypes.SHARE;
                    break;
                case Token.Types.K_MUT:
                    type = CapabilityTypes.MUT;
                    break;
                case Token.Types.K_CONST:
                    type = CapabilityTypes.CONST;
                    break;
                case Token.Types.K_THR_LOCAL:
                    type = CapabilityTypes.THR_LOCAL;
                    break;
                case Token.Types.K_SYNC:
                    type = CapabilityTypes.SYNC;
                    break;
                case Token.Types.K_INFER:
                    type = CapabilityTypes.INFER;
                    break;
                case Token.Types.K_NETWORK:
                    type = CapabilityTypes.NETWORK;
                    break;
                default:
                    throw new SyntaxErrorException(["OWN", "CONST", "REF"], CurrentToken);
            }
            incrementIndex();
            // For [own, network[json]]
            if (CurrentToken.Type == Token.Types.D_BRAC_OP)
            {
                incrementIndex();
                while (true)
                {
                    if (!HasMoreTokens)
                    {
                        Token missing = new()
                        {
                            Type = Token.Types.UNKNOWN,
                            Line = getIndex(),
                            CharStart = 0,
                            CharEnd = 0
                        };
                        throw new SyntaxErrorException(["]"], missing);
                    }

                    if (CurrentToken.Type == Token.Types.D_BRAC_CLO)
                        break;

                    if (CurrentToken.Type == Token.Types.IDENT)
                    {
                        configuration.Add(CurrentToken.Value);
                        incrementIndex();
                    }
                    else
                        throw new SyntaxErrorException(["IDENTIFIER"], CurrentToken);
                }
            }
            capabilities.Add(new Capability(type, configuration));
        }
        incrementIndex();
        return new Capabilities(capabilities);
    }
}