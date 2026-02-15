using System.Text.RegularExpressions;

namespace MauiBootstrapTheme.Build.Parsing;

/// <summary>
/// Parses Bootstrap/Bootswatch CSS files and extracts theme data for MAUI ResourceDictionary generation.
/// Handles minified CSS, CSS variables, gradients, shadows, dark mode overrides, and font-family.
/// </summary>
public class BootstrapCssParser
{
    /// <summary>
    /// Parses a Bootstrap CSS file and returns structured theme data.
    /// </summary>
    public BootstrapThemeData Parse(string cssContent, string themeName)
    {
        var data = new BootstrapThemeData { Name = themeName };

        // 1. Extract :root / [data-bs-theme=light] variables
        data.LightVariables = ExtractRootVariables(cssContent);

        // 2. Extract [data-bs-theme=dark] variables
        data.DarkVariables = ExtractDarkVariables(cssContent);

        // 3. Extract button component rules (colors, gradients, shadows)
        data.ButtonRules = ExtractButtonRules(cssContent);

        // 4. Extract dark-mode button overrides
        data.DarkButtonRules = ExtractDarkButtonRules(cssContent);

        // 5. Extract card rules
        data.CardRules = ExtractCardRules(cssContent);

        // 6. Extract font-family
        data.FontFamily = ExtractFontFamily(cssContent);

        // 7. Derive semantic colors from CSS variables
        DeriveSemanticColors(data);

        return data;
    }

    private Dictionary<string, string> ExtractRootVariables(string css)
    {
        var vars = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Match :root{...} or :root,[data-bs-theme=light]{...}
        var rootPattern = @":root(?:,\[data-bs-theme=light\])?\s*\{([^}]+)\}";
        var match = Regex.Match(css, rootPattern);
        if (match.Success)
        {
            ParseCssVariableBlock(match.Groups[1].Value, vars);
        }

        return vars;
    }

    private Dictionary<string, string> ExtractDarkVariables(string css)
    {
        var vars = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Match [data-bs-theme=dark]{...} — the first/main block with CSS variables
        var darkPattern = @"\[data-bs-theme=dark\]\s*\{(color-scheme:dark;[^}]+)\}";
        var match = Regex.Match(css, darkPattern);
        if (match.Success)
        {
            ParseCssVariableBlock(match.Groups[1].Value, vars);
        }

        return vars;
    }

    private void ParseCssVariableBlock(string block, Dictionary<string, string> vars)
    {
        var pattern = @"(--bs-[a-zA-Z0-9-]+)\s*:\s*([^;]+)";
        foreach (Match match in Regex.Matches(block, pattern))
        {
            vars[match.Groups[1].Value] = match.Groups[2].Value.Trim();
        }
    }

    private Dictionary<string, ButtonRule> ExtractButtonRules(string css)
    {
        var rules = new Dictionary<string, ButtonRule>(StringComparer.OrdinalIgnoreCase);

        var variants = new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" };

        foreach (var variant in variants)
        {
            var rule = new ButtonRule { Variant = variant };

            // Extract CSS variable-based rule: .btn-{variant}{--bs-btn-color:...;--bs-btn-bg:...;...}
            var varPattern = $@"\.btn-{Regex.Escape(variant)}\{{(--bs-btn-[^}}]+)\}}";
            foreach (Match m in Regex.Matches(css, varPattern))
            {
                var block = m.Groups[1].Value;
                ParseButtonVars(block, rule);
            }

            // Extract gradient: .btn-{variant}{background-image:linear-gradient(...);...}
            var gradPattern = $@"\.btn-{Regex.Escape(variant)}\{{[^}}]*background-image:\s*linear-gradient\(([^)]+)\)[^}}]*\}}";
            var gradMatch = Regex.Match(css, gradPattern);
            if (gradMatch.Success)
            {
                rule.Gradient = gradMatch.Groups[1].Value.Trim();
            }

            // Extract box-shadow: .btn-{variant}{box-shadow:...}
            var shadowPattern = $@"\.btn-{Regex.Escape(variant)}\{{box-shadow:([^}}]+)\}}";
            var shadowMatch = Regex.Match(css, shadowPattern);
            if (shadowMatch.Success)
            {
                rule.BoxShadow = shadowMatch.Groups[1].Value.Trim();
            }

            // Extract text-shadow: .btn-{variant}{text-shadow:...}
            var textShadowPattern = $@"\.btn-{Regex.Escape(variant)}\{{text-shadow:([^}}]+)\}}";
            var textShadowMatch = Regex.Match(css, textShadowPattern);
            if (textShadowMatch.Success)
            {
                rule.TextShadow = textShadowMatch.Groups[1].Value.Trim();
            }

            rules[variant] = rule;
        }

        // Outline variants
        foreach (var variant in variants)
        {
            var rule = new ButtonRule { Variant = $"outline-{variant}" };
            var varPattern = $@"\.btn-outline-{Regex.Escape(variant)}\{{(--bs-btn-[^}}]+)\}}";
            foreach (Match m in Regex.Matches(css, varPattern))
            {
                ParseButtonVars(m.Groups[1].Value, rule);
            }
            rules[$"outline-{variant}"] = rule;
        }

        return rules;
    }

    private Dictionary<string, ButtonRule> ExtractDarkButtonRules(string css)
    {
        var rules = new Dictionary<string, ButtonRule>(StringComparer.OrdinalIgnoreCase);

        var variants = new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" };

        foreach (var variant in variants)
        {
            // [data-bs-theme=dark] .btn-{variant}{...}
            var pattern = $@"\[data-bs-theme=dark\]\s*\.btn-{Regex.Escape(variant)}\{{([^}}]+)\}}";
            var match = Regex.Match(css, pattern);
            if (match.Success)
            {
                var rule = new ButtonRule { Variant = variant };
                ParseButtonVars(match.Groups[1].Value, rule);
                rules[variant] = rule;
            }

            // Outline dark variants
            var outlinePattern = $@"\[data-bs-theme=dark\]\s*\.btn-outline-{Regex.Escape(variant)}\{{([^}}]+)\}}";
            var outlineMatch = Regex.Match(css, outlinePattern);
            if (outlineMatch.Success)
            {
                var rule = new ButtonRule { Variant = $"outline-{variant}" };
                ParseButtonVars(outlineMatch.Groups[1].Value, rule);
                rules[$"outline-{variant}"] = rule;
            }
        }

        return rules;
    }

    private void ParseButtonVars(string block, ButtonRule rule)
    {
        var vars = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var pattern = @"(--bs-btn-[a-z-]+)\s*:\s*([^;]+)";
        foreach (Match m in Regex.Matches(block, pattern))
        {
            vars[m.Groups[1].Value] = m.Groups[2].Value.Trim();
        }

        if (vars.TryGetValue("--bs-btn-color", out var color)) rule.Color = color;
        if (vars.TryGetValue("--bs-btn-bg", out var bg)) rule.Background = bg;
        if (vars.TryGetValue("--bs-btn-border-color", out var bc)) rule.BorderColor = bc;
    }

    private CardRule ExtractCardRules(string css)
    {
        var rule = new CardRule();

        // .card{--bs-card-bg:...; --bs-card-border-color:...; ...}
        var cardPattern = @"\.card\{(--bs-card-[^}]+)\}";
        var match = Regex.Match(css, cardPattern);
        if (match.Success)
        {
            var block = match.Groups[1].Value;
            var vars = new Dictionary<string, string>();
            var pattern = @"(--bs-card-[a-z-]+)\s*:\s*([^;]+)";
            foreach (Match m in Regex.Matches(block, pattern))
            {
                vars[m.Groups[1].Value] = m.Groups[2].Value.Trim();
            }

            if (vars.TryGetValue("--bs-card-bg", out var bg)) rule.Background = bg;
            if (vars.TryGetValue("--bs-card-border-color", out var bc)) rule.BorderColor = bc;
            if (vars.TryGetValue("--bs-card-border-width", out var bw)) rule.BorderWidth = bw;
            if (vars.TryGetValue("--bs-card-border-radius", out var br)) rule.BorderRadius = br;
            if (vars.TryGetValue("--bs-card-cap-bg", out var capBg)) rule.HeaderBackground = capBg;
        }

        return rule;
    }

    private string? ExtractFontFamily(string css)
    {
        var rootVars = ExtractRootVariables(css);

        // First check --bs-body-font-family
        if (rootVars.TryGetValue("--bs-body-font-family", out var ff))
        {
            // If it references var(--bs-font-sans-serif), resolve that variable
            if (ff.Contains("var(--bs-font-sans-serif)"))
            {
                if (rootVars.TryGetValue("--bs-font-sans-serif", out var sansSerif))
                    ff = sansSerif;
                else
                    return null;
            }
            else if (ff.Contains("var("))
            {
                return null;
            }

            // Extract the first font name
            var firstFont = ff.Split(',')[0].Trim().Trim('"').Trim('\'');
            // Skip system fonts
            if (IsSystemFont(firstFont))
                return null;
            return string.IsNullOrEmpty(firstFont) ? null : firstFont;
        }

        // Also check --bs-font-sans-serif directly for themes that set custom fonts there
        if (rootVars.TryGetValue("--bs-font-sans-serif", out var sans))
        {
            var firstFont = sans.Split(',')[0].Trim().Trim('"').Trim('\'');
            if (!IsSystemFont(firstFont) && !string.IsNullOrEmpty(firstFont))
                return firstFont;
        }

        return null;
    }

    private bool IsSystemFont(string fontName)
    {
        var systemFonts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "-apple-system", "system-ui", "BlinkMacSystemFont", "Segoe UI",
            "Roboto", "Helvetica Neue", "Arial", "sans-serif",
            "Helvetica", "Times New Roman", "Georgia"
        };
        return systemFonts.Contains(fontName);
    }

    private void DeriveSemanticColors(BootstrapThemeData data)
    {
        var v = data.LightVariables;

        // Semantic colors
        data.Primary = GetColor(v, "--bs-primary");
        data.Secondary = GetColor(v, "--bs-secondary");
        data.Success = GetColor(v, "--bs-success");
        data.Danger = GetColor(v, "--bs-danger");
        data.Warning = GetColor(v, "--bs-warning");
        data.Info = GetColor(v, "--bs-info");
        data.Light = GetColor(v, "--bs-light");
        data.Dark = GetColor(v, "--bs-dark");

        // Surface colors
        data.BodyBackground = GetColor(v, "--bs-body-bg");
        data.BodyColor = GetColor(v, "--bs-body-color");
        data.BorderColor = GetColor(v, "--bs-border-color");
        data.HeadingColor = GetVar(v, "--bs-heading-color");
        data.LinkColor = GetColor(v, "--bs-link-color");

        // Sizing
        data.BorderWidth = GetVar(v, "--bs-border-width");
        data.BorderRadius = GetVar(v, "--bs-border-radius");
        data.BorderRadiusSm = GetVar(v, "--bs-border-radius-sm");
        data.BorderRadiusLg = GetVar(v, "--bs-border-radius-lg");

        // Derive On-colors from button rules
        foreach (var variant in new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" })
        {
            if (data.ButtonRules.TryGetValue(variant, out var rule) && rule.Color != null)
            {
                data.OnColors[variant] = rule.Color;
            }
        }

        // Dark mode overrides
        var dv = data.DarkVariables;
        data.DarkBodyBackground = GetColor(dv, "--bs-body-bg");
        data.DarkBodyColor = GetColor(dv, "--bs-body-color");
        data.DarkHeadingColor = GetVar(dv, "--bs-heading-color");
        data.DarkBorderColor = GetColor(dv, "--bs-border-color");
        data.DarkSecondaryBg = GetColor(dv, "--bs-secondary-bg");

        // Dark semantic colors
        data.DarkPrimary = GetColor(dv, "--bs-primary");
        data.DarkSecondary = GetColor(dv, "--bs-secondary");
        data.DarkSuccess = GetColor(dv, "--bs-success");
        data.DarkDanger = GetColor(dv, "--bs-danger");
        data.DarkWarning = GetColor(dv, "--bs-warning");
        data.DarkInfo = GetColor(dv, "--bs-info");
        data.DarkLight = GetColor(dv, "--bs-light");
        data.DarkDark = GetColor(dv, "--bs-dark");
        data.DarkLinkColor = GetColor(dv, "--bs-link-color");
    }

    private string? GetColor(Dictionary<string, string> vars, string key)
    {
        if (vars.TryGetValue(key, out var value))
        {
            // Resolve simple var() references
            if (value.StartsWith("var("))
            {
                var refKey = value.Replace("var(", "").TrimEnd(')');
                if (vars.TryGetValue(refKey, out var resolved))
                    return NormalizeColor(resolved);
                return null;
            }
            return NormalizeColor(value);
        }
        return null;
    }

    private string? GetVar(Dictionary<string, string> vars, string key)
    {
        return vars.TryGetValue(key, out var value) ? value : null;
    }

    private string NormalizeColor(string value)
    {
        value = value.Trim();
        if (value.StartsWith("#")) return value;
        if (value == "inherit") return value;

        // Handle named colors
        return value switch
        {
            "white" => "#ffffff",
            "black" => "#000000",
            "transparent" => "Transparent",
            _ => value
        };
    }
}

/// <summary>
/// Parsed theme data extracted from a Bootstrap CSS file.
/// </summary>
public class BootstrapThemeData
{
    public string Name { get; set; } = "";

    // Raw CSS variables
    public Dictionary<string, string> LightVariables { get; set; } = new();
    public Dictionary<string, string> DarkVariables { get; set; } = new();

    // Semantic colors (from :root)
    public string? Primary { get; set; }
    public string? Secondary { get; set; }
    public string? Success { get; set; }
    public string? Danger { get; set; }
    public string? Warning { get; set; }
    public string? Info { get; set; }
    public string? Light { get; set; }
    public string? Dark { get; set; }

    // On-colors (derived from btn-{variant} --bs-btn-color)
    public Dictionary<string, string> OnColors { get; set; } = new();

    // Surface colors
    public string? BodyBackground { get; set; }
    public string? BodyColor { get; set; }
    public string? BorderColor { get; set; }
    public string? HeadingColor { get; set; }
    public string? LinkColor { get; set; }

    // Dark mode surface colors
    public string? DarkBodyBackground { get; set; }
    public string? DarkBodyColor { get; set; }
    public string? DarkHeadingColor { get; set; }
    public string? DarkBorderColor { get; set; }
    public string? DarkSecondaryBg { get; set; }

    // Dark mode semantic colors
    public string? DarkPrimary { get; set; }
    public string? DarkSecondary { get; set; }
    public string? DarkSuccess { get; set; }
    public string? DarkDanger { get; set; }
    public string? DarkWarning { get; set; }
    public string? DarkInfo { get; set; }
    public string? DarkLight { get; set; }
    public string? DarkDark { get; set; }
    public string? DarkLinkColor { get; set; }

    // Sizing (raw CSS values, need unit conversion)
    public string? BorderWidth { get; set; }
    public string? BorderRadius { get; set; }
    public string? BorderRadiusSm { get; set; }
    public string? BorderRadiusLg { get; set; }

    // Font
    public string? FontFamily { get; set; }

    // Component rules
    public Dictionary<string, ButtonRule> ButtonRules { get; set; } = new();
    public Dictionary<string, ButtonRule> DarkButtonRules { get; set; } = new();
    public CardRule CardRules { get; set; } = new();

    /// <summary>Whether this theme has meaningful [data-bs-theme=dark] overrides.</summary>
    public bool HasDarkMode => DarkVariables.Count > 0;

    /// <summary>Whether any buttons use linear-gradient.</summary>
    public bool HasGradientButtons => ButtonRules.Values.Any(r => r.Gradient != null);

    /// <summary>Whether any buttons use box-shadow.</summary>
    public bool HasGlowButtons => ButtonRules.Values.Any(r => r.BoxShadow != null);
}

public class ButtonRule
{
    public string Variant { get; set; } = "";
    public string? Color { get; set; }
    public string? Background { get; set; }
    public string? BorderColor { get; set; }
    public string? Gradient { get; set; }
    public string? BoxShadow { get; set; }
    public string? TextShadow { get; set; }
}

public class CardRule
{
    public string? Background { get; set; }
    public string? BorderColor { get; set; }
    public string? BorderWidth { get; set; }
    public string? BorderRadius { get; set; }
    public string? HeaderBackground { get; set; }
}
