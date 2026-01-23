namespace SpeculaSyntaxAnalyzer.ParseTree;

public enum DataTypes
{
    INT,
    FLOAT,
    DOUBLE,
    CHAR,
    BOOL,
    STRING,
    VOID,
    NULL,
    IDENTIFIER,
    UNKNOWN
}
public record TypeNode(DataTypes type) : ParseNode;

