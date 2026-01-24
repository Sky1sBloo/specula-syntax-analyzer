namespace SpeculaSyntaxAnalyzer.ParseTree;

public enum CapabilityTypes
{
    OWN,
    MOVE,
    SHARED,
    VIEW,
    SHARE,
    MUT,
    CONST,
    THR_LOCAL,
    SYNC,
    INFER,
    NETWORK
}
public record Capabilities(PrintableList<Capability> capabilityList) : ParseNode;

public record Capability(CapabilityTypes type, PrintableList<string> configuration) : ParseNode;