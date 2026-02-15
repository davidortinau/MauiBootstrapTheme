using MauiBootstrapTheme.Build.CodeGen;
using MauiBootstrapTheme.Build.Parsing;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MauiBootstrapTheme.Build.Tasks;

/// <summary>
/// MSBuild task that parses Bootstrap CSS files and generates MAUI ResourceDictionary XAML + code-behind.
/// Supports multiple CSS files. Compatible with XAML Source Gen.
/// </summary>
public class GenerateBootstrapThemeTask : Microsoft.Build.Utilities.Task
{
    /// <summary>
    /// The Bootstrap CSS file(s) to parse. Supports multiple via item groups.
    /// </summary>
    [Required]
    public ITaskItem[]? CssFiles { get; set; }

    /// <summary>
    /// The output directory for generated XAML and code-behind files.
    /// </summary>
    [Required]
    public string? OutputDirectory { get; set; }

    /// <summary>
    /// The namespace for generated code.
    /// </summary>
    public string Namespace { get; set; } = "MauiBootstrapTheme.Generated";

    /// <summary>
    /// Output: Generated XAML files (for MauiXaml item group).
    /// </summary>
    [Output]
    public ITaskItem[]? GeneratedXamlFiles { get; set; }

    /// <summary>
    /// Output: Generated C# code-behind files.
    /// </summary>
    [Output]
    public ITaskItem[]? GeneratedCsFiles { get; set; }

    public override bool Execute()
    {
        if (CssFiles == null || CssFiles.Length == 0)
        {
            Log.LogMessage(MessageImportance.Normal, "No CSS files specified for theme generation.");
            return true;
        }

        if (string.IsNullOrEmpty(OutputDirectory))
        {
            Log.LogError("OutputDirectory is required.");
            return false;
        }

        Directory.CreateDirectory(OutputDirectory);

        var parser = new BootstrapCssParser();
        var generator = new ResourceDictionaryGenerator();
        var generatedXaml = new List<ITaskItem>();
        var generatedCs = new List<ITaskItem>();

        foreach (var cssItem in CssFiles)
        {
            var cssFile = cssItem.ItemSpec;
            if (!File.Exists(cssFile))
            {
                Log.LogError($"CSS file not found: {cssFile}");
                return false;
            }

            // Derive theme name from filename: "darkly.min.css" → "darkly", "bootstrap.css" → "bootstrap"
            var fileName = Path.GetFileNameWithoutExtension(cssFile);
            if (fileName.EndsWith(".min", StringComparison.OrdinalIgnoreCase))
                fileName = fileName[..^4]; // Remove .min
            var themeName = fileName.ToLowerInvariant();

            // Allow override via metadata
            var nameOverride = cssItem.GetMetadata("ThemeName");
            if (!string.IsNullOrEmpty(nameOverride))
                themeName = nameOverride;

            Log.LogMessage(MessageImportance.Normal, $"Parsing Bootstrap CSS: {cssFile} → theme '{themeName}'");

            try
            {
                var cssContent = File.ReadAllText(cssFile);
                var themeData = parser.Parse(cssContent, themeName);

                // Log stats
                Log.LogMessage(MessageImportance.Normal,
                    $"  Variables: {themeData.LightVariables.Count} light, {themeData.DarkVariables.Count} dark");
                Log.LogMessage(MessageImportance.Normal,
                    $"  Features: gradient={themeData.HasGradientButtons}, glow={themeData.HasGlowButtons}, dark-mode={themeData.HasDarkMode}");

                if (themeData.FontFamily != null)
                    Log.LogMessage(MessageImportance.Normal, $"  Font: {themeData.FontFamily}");

                // Generate pure C# ResourceDictionary (no XAML needed)
                var csContent = generator.GenerateCSharpResourceDictionary(themeData, Namespace);
                var className = ToPascalCase(themeName) + "Theme";
                var csPath = Path.Combine(OutputDirectory, $"{className}.g.cs");
                File.WriteAllText(csPath, csContent);
                generatedCs.Add(new TaskItem(csPath));
                Log.LogMessage(MessageImportance.Normal, $"  Generated: {csPath}");

                // Emit font warnings
                var fontWarnings = generator.GetFontWarnings(themeData);
                foreach (var warning in fontWarnings)
                {
                    Log.LogWarning(
                        subcategory: "BootstrapTheme",
                        warningCode: "BT0001",
                        helpKeyword: null,
                        file: cssFile,
                        lineNumber: 0,
                        columnNumber: 0,
                        endLineNumber: 0,
                        endColumnNumber: 0,
                        message: $"{warning.Message}\n{warning.SuggestedCode}");
                }
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex, showStackTrace: true);
                return false;
            }
        }

        GeneratedXamlFiles = generatedXaml.ToArray();
        GeneratedCsFiles = generatedCs.ToArray();

        Log.LogMessage(MessageImportance.High,
            $"MauiBootstrapTheme: Generated {generatedXaml.Count} theme(s) from CSS.");

        return true;
    }

    private string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var parts = input.Split('-', '_');
        return string.Concat(parts.Select(p =>
            p.Length > 0 ? char.ToUpper(p[0]) + p[1..] : ""));
    }
}
