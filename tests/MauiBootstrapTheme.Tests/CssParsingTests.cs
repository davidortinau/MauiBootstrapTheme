using System.Text.RegularExpressions;
using MauiBootstrapTheme.Build.CodeGen;
using MauiBootstrapTheme.Build.Parsing;
using Xunit;

namespace MauiBootstrapTheme.Tests;

public class CssParsingTests
{
    [Fact]
    public void Parser_CapturesGroupedVaporVariantShadows()
    {
        var parser = new BootstrapCssParser();
        var data = parser.Parse(ReadThemeCss("vapor"), "vapor");

        Assert.False(string.IsNullOrWhiteSpace(data.ButtonRules["danger"].BoxShadow));
        Assert.False(string.IsNullOrWhiteSpace(data.ButtonRules["info"].BoxShadow));
        Assert.False(string.IsNullOrWhiteSpace(data.ButtonRules["light"].BoxShadow));
    }

    [Fact]
    public void Generator_EmitsBriteSolidShadows_AndOutlineClears()
    {
        var parser = new BootstrapCssParser();
        var generator = new ResourceDictionaryGenerator();
        var data = parser.Parse(ReadThemeCss("brite"), "brite");
        var generated = generator.GenerateCSharpResourceDictionary(data, "TestNamespace");

        foreach (var variant in new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" })
        {
            Assert.Matches(
                $@"this\[""BtnShadow{ToPascal(variant)}""\] = new Shadow \{{ Brush = Color\.FromArgb\(""#000""\), Offset = new Point\(3, 3\), Radius = 0, Opacity = 1f \}};",
                generated);
            Assert.Contains(
                $"style_btn_{variant}.Setters.Add(new Setter {{ Property = Button.ShadowProperty, Value = DR(\"BtnShadow{ToPascal(variant)}\") }});",
                generated);
        }

        foreach (var variant in new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" })
        {
            Assert.Contains(
                $"style_btnout_{variant}.Setters.Add(new Setter {{ Property = Button.ShadowProperty, Value = DR(\"BtnShadowOutline{ToPascal(variant)}\") }});",
                generated);
            Assert.Contains(
                $"this[\"BtnShadowOutline{ToPascal(variant)}\"] = new Shadow {{ Brush = Colors.Transparent, Offset = new Point(0, 0), Radius = 0, Opacity = 0f }};",
                generated);
        }
    }

    [Fact]
    public void Generator_ClearsSolidVariantShadows_ForDefaultTheme()
    {
        var parser = new BootstrapCssParser();
        var generator = new ResourceDictionaryGenerator();
        var data = parser.Parse(ReadThemeCss("default"), "default");
        var generated = generator.GenerateCSharpResourceDictionary(data, "TestNamespace");

        foreach (var variant in new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" })
        {
            Assert.Contains(
                $"this[\"BtnShadow{ToPascal(variant)}\"] = new Shadow {{ Brush = Colors.Transparent, Offset = new Point(0, 0), Radius = 0, Opacity = 0f }};",
                generated);
            Assert.Contains(
                $"style_btn_{variant}.Setters.Add(new Setter {{ Property = Button.ShadowProperty, Value = DR(\"BtnShadow{ToPascal(variant)}\") }});",
                generated);
        }
    }

    private static string ToPascal(string variant) =>
        char.ToUpperInvariant(variant[0]) + variant[1..];

    private static string ReadThemeCss(string themeName)
    {
        var cssThemeName = themeName.Equals("default", StringComparison.OrdinalIgnoreCase) ? "bootstrap" : themeName;
        var root = FindRepositoryRoot();
        var path = Path.Combine(
            root,
            "samples",
            "MauiBootstrapTheme.Sample.Blazor",
            "wwwroot",
            "css",
            "themes",
            $"{cssThemeName}.min.css");
        return File.ReadAllText(path);
    }

    private static string FindRepositoryRoot()
    {
        var current = AppContext.BaseDirectory;
        while (!string.IsNullOrWhiteSpace(current))
        {
            if (File.Exists(Path.Combine(current, "MauiBootstrapTheme.sln")))
                return current;

            current = Directory.GetParent(current)?.FullName ?? string.Empty;
        }

        throw new DirectoryNotFoundException("Unable to locate repository root (MauiBootstrapTheme.sln).");
    }
}
