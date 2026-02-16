using MauiBootstrapTheme.Build.Parsing;
using MauiBootstrapTheme.Build.CodeGen;

var cssDir = args[0];
var outDir = args[1];
Directory.CreateDirectory(outDir);

var parser = new BootstrapCssParser();
var generator = new ResourceDictionaryGenerator();
var validationFailures = new List<string>();

foreach (var cssFile in Directory.GetFiles(cssDir, "*.min.css"))
{
    var fileName = Path.GetFileNameWithoutExtension(cssFile);
    if (fileName.EndsWith(".min")) fileName = fileName[..^4];

    Console.WriteLine($"\n=== {fileName.ToUpper()} ===");
    var css = File.ReadAllText(cssFile);
    var data = parser.Parse(css, fileName);

    Console.WriteLine($"  Primary: {data.Primary}");
    Console.WriteLine($"  BodyBg: {data.BodyBackground}");
    Console.WriteLine($"  BodyColor: {data.BodyColor}");
    Console.WriteLine($"  Font: {data.FontFamily ?? "(system)"}");
    Console.WriteLine($"  Dark vars: {data.DarkVariables.Count}");
    Console.WriteLine($"  Gradient btns: {data.HasGradientButtons}");
    Console.WriteLine($"  Glow btns: {data.HasGlowButtons}");
    Console.WriteLine($"  FormControl bg: {data.FormControlRule.Background ?? "(missing)"}");
    Console.WriteLine($"  FormControl border: {data.FormControlRule.BorderColor ?? "(missing)"}");

    if (data.OnColors.Count > 0)
    {
        Console.Write("  On-colors: ");
        foreach (var kv in data.OnColors) Console.Write($"{kv.Key}={kv.Value} ");
        Console.WriteLine();
    }

    if (data.HasGradientButtons)
    {
        foreach (var kv in data.ButtonRules)
            if (kv.Value.Gradient != null)
                Console.WriteLine($"  Gradient {kv.Key}: {kv.Value.Gradient}");
    }

    if (data.DarkButtonRules.Count > 0)
    {
        Console.Write("  Dark btn overrides: ");
        foreach (var kv in data.DarkButtonRules)
            Console.Write($"{kv.Key}(bg={kv.Value.Background},color={kv.Value.Color}) ");
        Console.WriteLine();
    }

    // Generate XAML
    var xaml = generator.GenerateXaml(data, "TestNamespace");
    var pascal = char.ToUpper(fileName[0]) + fileName[1..];
    var xamlPath = Path.Combine(outDir, $"{pascal}Theme.xaml");
    File.WriteAllText(xamlPath, xaml);
    Console.WriteLine($"  XAML: {new FileInfo(xamlPath).Length} bytes");

    // Cross-theme mapping guards: detect parser/generator regressions for input controls
    if (string.IsNullOrWhiteSpace(data.FormControlRule.Background))
        validationFailures.Add($"{fileName}: missing .form-control background-color parse");
    if (string.IsNullOrWhiteSpace(data.FormControlRule.BorderColor))
        validationFailures.Add($"{fileName}: missing .form-control border-color parse");
    if (!xaml.Contains("Class=\"form-control\"", StringComparison.Ordinal))
        validationFailures.Add($"{fileName}: generated XAML missing form-control class style");
    if (xaml.Contains("InputBackground\">var(", StringComparison.OrdinalIgnoreCase))
        validationFailures.Add($"{fileName}: InputBackground unresolved var() in generated XAML");

    // Font warnings
    var warnings = generator.GetFontWarnings(data);
    foreach (var w in warnings)
        Console.WriteLine($"  ⚠️  {w.Message}");
}

Console.WriteLine("\n✅ All themes generated successfully.");
if (validationFailures.Count > 0)
{
    Console.Error.WriteLine("\n❌ Validation failures:");
    foreach (var failure in validationFailures)
        Console.Error.WriteLine($" - {failure}");
    Environment.Exit(1);
}
