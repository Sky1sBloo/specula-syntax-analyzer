
namespace SpeculaSyntaxAnalyzer.ParseTree;

public interface ModuleNode : RootStatement;

public interface ExportModuleNode : ModuleNode;
public record ExportNode(RootStatement statement) : ExportModuleNode;
public record ExportDefaultNode(RootStatement statement) : ExportModuleNode;

public interface ImportModuleNode : ModuleNode;
public record ImportNodes(PrintableList<ImportModuleNode> imports) : ModuleNode;
public record ImportAliasNode(string moduleName, string alias) : ImportModuleNode;
public record ImportNode(string moduleName, PrintableList<string> identifiers) : ImportModuleNode;