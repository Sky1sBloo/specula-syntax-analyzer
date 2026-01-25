
namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface ModuleNode : ParseNode;

public record ExportNode(Statement statement) : ModuleNode;
public record ImportDefaultNode(string moduleName, string alias) : ModuleNode;
public record ImportNode(string moduleName, PrintableList<string> identifiers) : ModuleNode;