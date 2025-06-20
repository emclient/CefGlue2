using CefParser;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Text;

namespace CefGlue.Interop.Gen;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var targetApiVersion = context.AnalyzerConfigOptionsProvider.Select((config, _) =>
        {
            if (config.GlobalOptions.TryGetValue("build_property.CefApiVersion", out var version))
                return CefParser.CefParser.VersionNumberFromString(version);
            return 0;
        });

        context.RegisterSourceOutput(
            context.AdditionalTextsProvider.Where(at => Path.GetExtension(at.Path) is ".h").Collect().Combine(targetApiVersion),
            GenerateCode);
    }

    private void GenerateCode(
        SourceProductionContext context,
        (ImmutableArray<AdditionalText> AdditionalFiles, int TargetApiVersion) p)
    {
        StringBuilder sb = new StringBuilder();

        try
        {
            if (p.AdditionalFiles.Length < 1) return;

            var cp = new CefParser.CefParser([]);
            foreach (var file in p.AdditionalFiles)
            {
                sb.AppendLine(file.Path);
                cp.ParseFileData(file.GetText()?.ToString() ?? "", Path.GetFileName(file.Path));
            }

            var interopGen = new InteropGen(cp, p.TargetApiVersion);

            using (var writer = new StringWriter())
            {
                interopGen.GenerateLibCefG(writer);
                context.AddSource($"Interop\\libcef.g.cs", writer.ToString());
            }

            using (var writer = new StringWriter())
            {
                interopGen.GenerateVersionFile(writer);
                context.AddSource($"Interop\\version.g.cs", writer.ToString());
            }

            foreach (var c in cp.Classes)
            {
                using var writer = new StringWriter();
                interopGen.GenerateStructFile(c, writer);
                context.AddSource($"Interop\\Classes.g\\{CefParser.CefParser.GetCApiName(c.Name, true)}.g.cs", writer.ToString());
            }

            foreach (var c in cp.Classes)
            {
                using var writer = new StringWriter();
                interopGen.GenerateWrapper(c, writer);
                context.AddSource($"Classes.g\\{NameConverter.ToCSharpClassName(c.Name, CefParser.CefParser.TypeClass.Class)}.g.cs", writer.ToString());
            }

            foreach (var e in cp.Enums)
            {
                using var writer = new StringWriter();
                var csName = NameConverter.ToCSharpClassName(e.Name, CefParser.CefParser.TypeClass.Enum);
                interopGen.GenerateEnum(e, writer);
                context.AddSource($"Enums\\{csName}.g.cs", writer.ToString());
            }

        }
        catch (Exception ex)
        {
            context.AddSource($"exception.txt", sb.ToString() + ex.ToString());
            //throw new Exception(ex.ToString());
        }
    }
}
