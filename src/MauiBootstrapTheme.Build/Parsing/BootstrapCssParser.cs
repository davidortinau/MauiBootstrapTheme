using System.Globalization;
using System.Linq;
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

        // 7. Extract input component rules
        ExtractInputRules(cssContent, data);

        // 8. Derive semantic colors from CSS variables
        DeriveSemanticColors(data);

        // 9. Extract heading sizes/weight and button sizing from raw CSS rules
        ExtractHeadingSizesFromCss(cssContent, data);
        ExtractButtonSizingFromCss(cssContent, data);
        ExtractProgressBgFromCss(cssContent, data);

        return data;
    }

    private Dictionary<string, string> ExtractRootVariables(string css)
    {
        var vars = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Match :root{...} or :root,[data-bs-theme=light]{...}
        // Use Matches to capture all :root blocks (themes may have multiple)
        var rootPattern = @":root(?:,\[data-bs-theme=light\])?\s*\{([^}]+)\}";
        foreach (Match m in Regex.Matches(css, rootPattern))
        {
            ParseCssVariableBlock(m.Groups[1].Value, vars);
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
            // Skip dark-mode overrides ([data-bs-theme=dark]) — only use light-mode values
            var varPattern = $@"\.btn-{Regex.Escape(variant)}\{{(--bs-btn-[^}}]+)\}}";
            foreach (Match m in Regex.Matches(css, varPattern))
            {
                // Skip if preceded by [data-bs-theme=dark] selector
                var precedingText = css.Substring(Math.Max(0, m.Index - 30), Math.Min(30, m.Index));
                if (precedingText.Contains("[data-bs-theme=dark]"))
                    continue;

                var block = m.Groups[1].Value;
                ParseButtonVars(block, rule);
            }

            // Extract gradient: .btn-{variant}{background-image:linear-gradient(...);...}
            var gradPattern = $@"\.btn-{Regex.Escape(variant)}\{{[^}}]*background-image:\s*linear-gradient\(([^;]+)\)\s*;[^}}]*\}}";
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

            // Extract direct color overrides (e.g., Slate .btn-outline-primary{color:#fff})
            // These are standalone blocks with direct CSS properties (no --bs-btn-* vars).
            foreach (Match dcm in Regex.Matches(css, $@"\.btn-outline-{Regex.Escape(variant)}\{{([^}}]+)\}}"))
            {
                var block = dcm.Groups[1].Value;
                // Skip blocks that contain CSS variable declarations (standard Bootstrap)
                if (block.Contains("--bs-btn-")) continue;
                // Skip dark-mode overrides
                var precedingText = css.Substring(Math.Max(0, dcm.Index - 30), Math.Min(30, dcm.Index));
                if (precedingText.Contains("[data-bs-theme=dark]")) continue;

                // Extract direct color property
                var colorMatch = Regex.Match(block, @"(?<![\w-])color:([^;}]+)");
                if (colorMatch.Success)
                {
                    var directColor = colorMatch.Groups[1].Value.Trim();
                    if (!directColor.StartsWith("var("))
                        rule.DirectColor = directColor;
                }
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
        if (vars.TryGetValue("--bs-btn-border-width", out var bw)) rule.BorderWidth = bw;
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

    private void ExtractInputRules(string css, BootstrapThemeData data)
    {
        data.FormControlRule = ExtractInputRule(css, data.LightVariables, "form-control");
        data.FormSelectRule = ExtractInputRule(css, data.LightVariables, "form-select");
    }

    private InputRule ExtractInputRule(string css, Dictionary<string, string> vars, string className)
    {
        var borderWidth = ResolveProperty(css, vars, className, null, "border-width");
        if (borderWidth == null)
        {
            var borderValue = ResolveProperty(css, vars, className, null, "border");
            if (!string.IsNullOrWhiteSpace(borderValue))
            {
                var tokens = borderValue.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                borderWidth = tokens.FirstOrDefault(t =>
                    t.EndsWith("px", StringComparison.OrdinalIgnoreCase) ||
                    t.EndsWith("rem", StringComparison.OrdinalIgnoreCase) ||
                    t.EndsWith("em", StringComparison.OrdinalIgnoreCase));
            }
        }

        return new InputRule
        {
            Background = ResolveColorProperty(css, vars, className, null, "background-color"),
            Color = ResolveColorProperty(css, vars, className, null, "color"),
            BorderColor = ResolveColorProperty(css, vars, className, null, "border-color"),
            BorderWidth = borderWidth,
            FontSize = ResolveProperty(css, vars, className, null, "font-size"),
            LineHeight = ResolveProperty(css, vars, className, null, "line-height"),
            Padding = ResolveProperty(css, vars, className, null, "padding"),
            PlaceholderColor = ResolveColorProperty(css, vars, className, "::placeholder", "color"),
            FocusBorderColor = ResolveColorProperty(css, vars, className, ":focus", "border-color"),
            FocusShadow = ResolveProperty(css, vars, className, ":focus", "box-shadow"),
            DisabledBackground = ResolveColorProperty(css, vars, className, ":disabled", "background-color"),
            DisabledColor = ResolveColorProperty(css, vars, className, ":disabled", "color")
        };
    }

    private string? ResolveColorProperty(string css, Dictionary<string, string> vars, string className, string? pseudo, string propertyName)
    {
        var value = ResolveProperty(css, vars, className, pseudo, propertyName);
        if (value == null && propertyName.Equals("border-color", StringComparison.OrdinalIgnoreCase))
        {
            var borderValue = ResolveProperty(css, vars, className, pseudo, "border");
            value = ExtractBorderColor(borderValue, vars);
        }
        return value == null ? null : NormalizeColor(value);
    }

    private string? ExtractBorderColor(string? borderValue, Dictionary<string, string> vars)
    {
        if (string.IsNullOrWhiteSpace(borderValue))
            return null;

        var rgbaMatch = Regex.Match(borderValue, @"rgba?\([^)]+\)", RegexOptions.IgnoreCase);
        if (rgbaMatch.Success)
            return rgbaMatch.Value;

        var tokens = borderValue.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        for (var i = tokens.Length - 1; i >= 0; i--)
        {
            var token = tokens[i].Trim();
            if (IsStandaloneCssVar(token))
                token = ResolveCssVarValue(vars, token) ?? token;
            if (token.StartsWith("#", StringComparison.Ordinal) ||
                token.StartsWith("rgb(", StringComparison.OrdinalIgnoreCase) ||
                token.StartsWith("rgba(", StringComparison.OrdinalIgnoreCase) ||
                token.Equals("transparent", StringComparison.OrdinalIgnoreCase) ||
                token.Equals("white", StringComparison.OrdinalIgnoreCase) ||
                token.Equals("black", StringComparison.OrdinalIgnoreCase))
            {
                return token;
            }
        }

        return tokens.Length > 0 ? tokens[^1] : null;
    }

    private string? ResolveProperty(string css, Dictionary<string, string> vars, string className, string? pseudo, string propertyName)
    {
        var classPattern = $@"(?<![\w-])\.{Regex.Escape(className)}(?![\w-])";
        var selectorPattern = pseudo == null
            ? $@"{classPattern}(?!:)"
            : $@"{classPattern}{Regex.Escape(pseudo)}";

        foreach (Match match in Regex.Matches(css, @"([^{}]+)\{([^{}]*)\}"))
        {
            var selector = match.Groups[1].Value;
            if (!Regex.IsMatch(selector, selectorPattern))
                continue;

            var propertyPattern = $@"(?:^|;)\s*{Regex.Escape(propertyName)}\s*:\s*([^;]+)";
            var propertyMatch = Regex.Match(match.Groups[2].Value, propertyPattern);
            if (!propertyMatch.Success)
                continue;

            var rawValue = propertyMatch.Groups[1].Value.Trim();
            var resolved = IsStandaloneCssVar(rawValue) ? (ResolveCssVarValue(vars, rawValue) ?? rawValue) : rawValue;
            if (resolved.Contains("var(", StringComparison.OrdinalIgnoreCase))
            {
                if (propertyName.Equals("border", StringComparison.OrdinalIgnoreCase))
                    return resolved;
                continue;
            }
            return resolved;
        }

        return null;
    }

    private static bool IsStandaloneCssVar(string value)
    {
        value = value.Trim();
        return Regex.IsMatch(value, @"^var\([^()]+\)$", RegexOptions.IgnoreCase);
    }

    private string? ExtractFontFamily(string css)
    {
        var rootVars = ExtractRootVariables(css);

        // First check --bs-body-font-family
        if (rootVars.TryGetValue("--bs-body-font-family", out var ff))
        {
            ff = ResolveCssVarValue(rootVars, ff) ?? ff;
            if (ff.Contains("var("))
                return null;

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
            sans = ResolveCssVarValue(rootVars, sans) ?? sans;
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

        // Typography
        data.BodyFontSize = GetVar(v, "--bs-body-font-size");
        data.SecondaryBg = GetColor(v, "--bs-secondary-bg");

        // Card spacing
        data.CardSpacerX = GetVar(v, "--bs-card-spacer-x");
        data.CardSpacerY = GetVar(v, "--bs-card-spacer-y");

        // Extract progress background
        ExtractProgressBg(data);

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

    /// <summary>
    /// Extracts heading font sizes and weight from the original CSS content.
    /// Called from Parse() with the raw CSS.
    /// </summary>
    private void ExtractHeadingSizesFromCss(string css, BootstrapThemeData data)
    {
        // Look for the @media (min-width:1200px) fixed sizes first (desktop sizes)
        // Pattern: .h1,h1{font-size:2.5rem}
        for (int i = 1; i <= 6; i++)
        {
            // Try the @media block fixed size first (e.g., @media (min-width:1200px){.h1,h1{font-size:2.5rem}})
            var mediaPattern = $@"@media[^{{]*min-width:\s*1200px\)[^{{]*\{{[^}}]*\.h{i},h{i}\{{font-size:([^}}]+)\}}";
            var mediaMatch = Regex.Match(css, mediaPattern);
            if (mediaMatch.Success)
            {
                var size = mediaMatch.Groups[1].Value.Trim();
                SetHeadingSize(data, i, size);
                continue;
            }

            // Fall back to the direct rule (e.g., .h5,h5{font-size:1.25rem})
            var directPattern = $@"\.h{i},h{i}\{{font-size:([^}}]+)\}}";
            var directMatch = Regex.Match(css, directPattern);
            if (directMatch.Success)
            {
                var size = directMatch.Groups[1].Value.Trim();
                // Skip calc() values — use the @media fixed fallback instead
                if (!size.StartsWith("calc("))
                    SetHeadingSize(data, i, size);
            }
        }

        // Extract heading font-weight
        var weightPattern = @"h[1-6](?:,\.h[1-6])*\{[^}]*font-weight:\s*(\d+)";
        var weightMatch = Regex.Match(css, weightPattern);
        if (weightMatch.Success)
        {
            data.HeadingFontWeight = weightMatch.Groups[1].Value;
        }
    }

    private void SetHeadingSize(BootstrapThemeData data, int level, string size)
    {
        switch (level)
        {
            case 1: data.FontSizeH1 = size; break;
            case 2: data.FontSizeH2 = size; break;
            case 3: data.FontSizeH3 = size; break;
            case 4: data.FontSizeH4 = size; break;
            case 5: data.FontSizeH5 = size; break;
            case 6: data.FontSizeH6 = size; break;
        }
    }

    /// <summary>
    /// Extracts button sizing from the .btn, .btn-sm, .btn-lg CSS rules.
    /// </summary>
    private void ExtractButtonSizingFromCss(string css, BootstrapThemeData data)
    {
        // .btn{--bs-btn-padding-x:0.75rem;--bs-btn-padding-y:0.375rem;...}
        // Use Matches to capture all .btn base blocks (themes may have multiple)
        foreach (Match bm in Regex.Matches(css, @"\.btn\{(--bs-btn-[^}]+)\}"))
        {
            var block = bm.Groups[1].Value;
            var padX = ExtractCssVar(block, "--bs-btn-padding-x");
            var padY = ExtractCssVar(block, "--bs-btn-padding-y");
            if (padX != null) data.BtnPaddingX = padX;
            if (padY != null) data.BtnPaddingY = padY;
            var bwVal = ExtractCssVar(block, "--bs-btn-border-width");
            if (bwVal != null) data.BtnBorderWidth = bwVal;
        }

        // Extract resting-state box-shadow from .btn override rules (e.g., Brite's 3px 3px offset)
        // Match .btn{...box-shadow:VALUE...} but NOT CSS variable definitions (--bs-btn-box-shadow)
        foreach (Match bsm in Regex.Matches(css, @"(?<!\w)\.btn\{[^}]*(?<!-)box-shadow:([^;}]+)[;}]"))
        {
            var shadow = bsm.Groups[1].Value.Trim();
            // Skip focus/hover shadows (var() references) and inset shadows
            if (!shadow.StartsWith("var(") && !shadow.StartsWith("inset"))
            {
                data.BtnBaseBoxShadow = shadow;
            }
        }

        // Extract direct border-color overrides on .btn (e.g., Slate's rgba(0,0,0,.6), Brite's #000)
        // These are theme-level .btn{border-color:VALUE} rules that override var(--bs-btn-border-color).
        // Skip var() references and blocks preceded by combinators (+, ~) or pseudo-state selectors.
        foreach (Match bcm in Regex.Matches(css, @"(?<![+~\w])\.btn\{[^}]*(?<!-)border-color:([^;}]+)[;}]"))
        {
            var bc = bcm.Groups[1].Value.Trim();
            if (!bc.StartsWith("var("))
            {
                data.BtnBaseBorderColor = bc;
            }
        }

        // Extract direct color overrides on .btn (e.g., Slate's .btn{color:#fff})
        // Skip var() references and combinator-preceded blocks.
        foreach (Match cm in Regex.Matches(css, @"(?<![+~\w])\.btn\{[^}]*(?<![\w-])color:([^;}]+)[;}]"))
        {
            var c = cm.Groups[1].Value.Trim();
            if (!c.StartsWith("var("))
            {
                data.BtnBaseColor = c;
            }
        }

        // .btn-sm{--bs-btn-padding-y:0.25rem;--bs-btn-padding-x:0.5rem;--bs-btn-font-size:0.875rem;...}
        var smMatch = Regex.Match(css, @"\.btn-sm\{(--bs-btn-[^}]+)\}");
        if (smMatch.Success)
        {
            var block = smMatch.Groups[1].Value;
            data.BtnPaddingXSm = ExtractCssVar(block, "--bs-btn-padding-x");
            data.BtnPaddingYSm = ExtractCssVar(block, "--bs-btn-padding-y");
            data.BtnFontSizeSm = ExtractCssVar(block, "--bs-btn-font-size");
        }

        // .btn-lg{--bs-btn-padding-y:0.5rem;--bs-btn-padding-x:1rem;--bs-btn-font-size:1.25rem;...}
        var lgMatch = Regex.Match(css, @"\.btn-lg\{(--bs-btn-[^}]+)\}");
        if (lgMatch.Success)
        {
            var block = lgMatch.Groups[1].Value;
            data.BtnPaddingXLg = ExtractCssVar(block, "--bs-btn-padding-x");
            data.BtnPaddingYLg = ExtractCssVar(block, "--bs-btn-padding-y");
            data.BtnFontSizeLg = ExtractCssVar(block, "--bs-btn-font-size");
        }
    }

    private void ExtractProgressBg(BootstrapThemeData data)
    {
        var v = data.LightVariables;
        // --bs-progress-bg from :root is often var(--bs-secondary-bg)
        var progressBg = GetVar(v, "--bs-progress-bg");
        // Only set if found in :root — component rule override happens in ExtractProgressBgFromCss
        if (progressBg != null)
            data.ProgressBg = progressBg;
        else
            data.ProgressBg = data.SecondaryBg;
    }

    private void ExtractProgressBgFromCss(string css, BootstrapThemeData data)
    {
        // .progress,.progress-stacked{--bs-progress-bg:#250d49;...}
        var match = Regex.Match(css, @"\.progress[,{][^}]*--bs-progress-bg:\s*([^;]+)");
        if (match.Success)
        {
            var val = match.Groups[1].Value.Trim();
            if (!val.StartsWith("var("))
                data.ProgressBg = val;
        }
    }

    private string? ExtractCssVar(string block, string varName)
    {
        var pattern = $@"{Regex.Escape(varName)}\s*:\s*([^;]+)";
        var match = Regex.Match(block, pattern);
        return match.Success ? match.Groups[1].Value.Trim() : null;
    }

    private string? GetColor(Dictionary<string, string> vars, string key)
    {
        if (vars.TryGetValue(key, out var value))
        {
            var resolved = ResolveCssVarValue(vars, value);
            if (string.IsNullOrWhiteSpace(resolved) || resolved.Contains("var("))
                return null;
            return NormalizeColor(resolved);
        }
        return null;
    }

    private string? GetVar(Dictionary<string, string> vars, string key)
    {
        return vars.TryGetValue(key, out var value) ? ResolveCssVarValue(vars, value) : null;
    }

    private string? ResolveCssVarValue(Dictionary<string, string> vars, string value)
    {
        var current = value.Trim();
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (var i = 0; i < 8; i++)
        {
            if (!current.StartsWith("var(", StringComparison.OrdinalIgnoreCase))
                return current;

            if (!current.EndsWith(")", StringComparison.Ordinal))
                return current;

            var inner = current.Substring(4, current.Length - 5).Trim();
            string varName;
            string? fallback = null;

            var comma = inner.IndexOf(',');
            if (comma >= 0)
            {
                varName = inner[..comma].Trim();
                fallback = inner[(comma + 1)..].Trim();
            }
            else
            {
                varName = inner;
            }

            if (!varName.StartsWith("--", StringComparison.Ordinal))
                return fallback;

            if (!visited.Add(varName))
                return fallback;

            if (vars.TryGetValue(varName, out var next))
            {
                current = next.Trim();
                continue;
            }

            return fallback;
        }

        return null;
    }

    private string NormalizeColor(string value)
    {
        value = value.Trim();
        if (value.StartsWith("#")) return value;
        if (value == "inherit") return value;
        if (value.StartsWith("rgba(", StringComparison.OrdinalIgnoreCase))
        {
            var inner = value.Replace("rgba(", "", StringComparison.OrdinalIgnoreCase).TrimEnd(')');
            var parts = inner.Split(',', StringSplitOptions.TrimEntries);
            if (parts.Length == 4 &&
                byte.TryParse(parts[0], out var r) &&
                byte.TryParse(parts[1], out var g) &&
                byte.TryParse(parts[2], out var b) &&
                double.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var a))
            {
                var alpha = (byte)Math.Clamp((int)Math.Round(a * 255), 0, 255);
                return $"#{alpha:X2}{r:X2}{g:X2}{b:X2}";
            }
        }
        if (value.StartsWith("rgb(", StringComparison.OrdinalIgnoreCase))
        {
            var inner = value.Replace("rgb(", "", StringComparison.OrdinalIgnoreCase).TrimEnd(')');
            var parts = inner.Split(',', StringSplitOptions.TrimEntries);
            if (parts.Length == 3 &&
                byte.TryParse(parts[0], out var r) &&
                byte.TryParse(parts[1], out var g) &&
                byte.TryParse(parts[2], out var b))
            {
                return $"#{r:X2}{g:X2}{b:X2}";
            }
        }

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

    // Typography (raw CSS values)
    public string? BodyFontSize { get; set; }
    public string? HeadingFontWeight { get; set; }
    public string? FontSizeH1 { get; set; }
    public string? FontSizeH2 { get; set; }
    public string? FontSizeH3 { get; set; }
    public string? FontSizeH4 { get; set; }
    public string? FontSizeH5 { get; set; }
    public string? FontSizeH6 { get; set; }

    // Button spacing (raw CSS values)
    public string? BtnPaddingX { get; set; }
    public string? BtnPaddingY { get; set; }
    public string? BtnPaddingXSm { get; set; }
    public string? BtnPaddingYSm { get; set; }
    public string? BtnPaddingXLg { get; set; }
    public string? BtnPaddingYLg { get; set; }
    public string? BtnBorderWidth { get; set; }
    public string? BtnFontSizeSm { get; set; }
    public string? BtnFontSizeLg { get; set; }
    /// <summary>Resting-state box-shadow on .btn base rule (e.g., Brite's 3px 3px offset).</summary>
    public string? BtnBaseBoxShadow { get; set; }
    /// <summary>Direct border-color override on .btn base rule (e.g., Slate's rgba(0,0,0,.6), Brite's #000).</summary>
    public string? BtnBaseBorderColor { get; set; }
    /// <summary>Direct color override on .btn base rule (e.g., Slate's #fff).</summary>
    public string? BtnBaseColor { get; set; }

    // Progress / Secondary background
    public string? SecondaryBg { get; set; }
    public string? ProgressBg { get; set; }

    // Card spacing
    public string? CardSpacerX { get; set; }
    public string? CardSpacerY { get; set; }

    // Component rules
    public Dictionary<string, ButtonRule> ButtonRules { get; set; } = new();
    public Dictionary<string, ButtonRule> DarkButtonRules { get; set; } = new();
    public CardRule CardRules { get; set; } = new();
    public InputRule FormControlRule { get; set; } = new();
    public InputRule FormSelectRule { get; set; } = new();

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
    public string? BorderWidth { get; set; }
    public string? Gradient { get; set; }
    public string? BoxShadow { get; set; }
    public string? TextShadow { get; set; }
    /// <summary>Direct CSS color property override (not --bs-btn-color variable). E.g., Slate .btn-outline-primary{color:#fff}.</summary>
    public string? DirectColor { get; set; }
}

public class CardRule
{
    public string? Background { get; set; }
    public string? BorderColor { get; set; }
    public string? BorderWidth { get; set; }
    public string? BorderRadius { get; set; }
    public string? HeaderBackground { get; set; }
}

public class InputRule
{
    public string? Background { get; set; }
    public string? Color { get; set; }
    public string? BorderColor { get; set; }
    public string? BorderWidth { get; set; }
    public string? FontSize { get; set; }
    public string? LineHeight { get; set; }
    public string? Padding { get; set; }
    public string? PlaceholderColor { get; set; }
    public string? FocusBorderColor { get; set; }
    public string? FocusShadow { get; set; }
    public string? DisabledBackground { get; set; }
    public string? DisabledColor { get; set; }
}
