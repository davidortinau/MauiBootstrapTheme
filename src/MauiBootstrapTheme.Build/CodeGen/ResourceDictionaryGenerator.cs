using System.Globalization;
using System.Text;

namespace MauiBootstrapTheme.Build.CodeGen;

/// <summary>
/// Generates MAUI ResourceDictionary XAML + code-behind from parsed Bootstrap CSS theme data.
/// Output is compatible with XAML Source Gen (<MauiXamlInflator>SourceGen</MauiXamlInflator>).
/// </summary>
public class ResourceDictionaryGenerator
{
    // Known Bootswatch font → MAUI platform font mapping
    private static readonly Dictionary<string, string> FontMapping = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Neucha", "MarkerFelt-Wide" },       // Sketchy → closest macOS/iOS hand-drawn font
        { "Cabin Sketch", "MarkerFelt-Wide" },  // Sketchy headings
        { "Lato", "Avenir Next" },
        { "Open Sans", "HelveticaNeue" },
        { "Source Sans Pro", "HelveticaNeue" },
        { "Nunito", "Avenir Next" },
        { "Poppins", "Avenir Next" },
        { "Montserrat", "Avenir Next" },
        { "Inter", "HelveticaNeue" },
        { "Fira Sans", "HelveticaNeue" },
        { "IBM Plex Sans", "HelveticaNeue" },
        { "Roboto", "HelveticaNeue" },
    };

    /// <summary>
    /// Generates the XAML content for a ResourceDictionary theme file.
    /// </summary>
    public string GenerateXaml(Parsing.BootstrapThemeData data, string @namespace)
    {
        var className = ToPascalCase(data.Name) + "Theme";
        var sb = new StringBuilder();

        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
        sb.AppendLine("<!-- Auto-generated from CSS by MauiBootstrapTheme.Build. Do not edit. -->");
        sb.AppendLine("<ResourceDictionary xmlns=\"http://schemas.microsoft.com/dotnet/2021/maui\"");
        sb.AppendLine("                    xmlns:x=\"http://schemas.microsoft.com/winfx/2009/xaml\"");
        sb.AppendLine("                    xmlns:bs=\"clr-namespace:MauiBootstrapTheme.Theming;assembly=MauiBootstrapTheme\"");
        sb.AppendLine($"                    x:Class=\"{@namespace}.{className}\">");
        sb.AppendLine();

        // Semantic colors
        EmitSection(sb, "SEMANTIC COLORS");
        EmitColor(sb, "Primary", data.Primary ?? "#0d6efd");
        EmitColor(sb, "Secondary", data.Secondary ?? "#6c757d");
        EmitColor(sb, "Success", data.Success ?? "#198754");
        EmitColor(sb, "Danger", data.Danger ?? "#dc3545");
        EmitColor(sb, "Warning", data.Warning ?? "#ffc107");
        EmitColor(sb, "Info", data.Info ?? "#0dcaf0");
        EmitColor(sb, "Light", data.Light ?? "#f8f9fa");
        EmitColor(sb, "Dark", data.Dark ?? "#212529");
        sb.AppendLine();

        // On-colors
        EmitSection(sb, "ON-COLORS");
        EmitOnColors(sb, data);
        sb.AppendLine();

        // Surface colors
        EmitSection(sb, "SURFACE COLORS");
        EmitSurfaceColors(sb, data);
        sb.AppendLine();

        // Dark mode colors (if this theme has meaningful dark overrides)
        if (data.HasDarkMode)
        {
            EmitSection(sb, "DARK MODE OVERRIDES");
            EmitDarkModeColors(sb, data);
            sb.AppendLine();
        }

        // Gray scale
        EmitSection(sb, "GRAY SCALE");
        EmitGrayScale(sb, data);
        sb.AppendLine();

        // Typography
        EmitSection(sb, "TYPOGRAPHY");
        EmitTypography(sb, data);
        sb.AppendLine();

        // Spacing & sizing
        EmitSection(sb, "SPACING & SIZING");
        EmitSpacing(sb, data);
        sb.AppendLine();

        // Implicit styles
        EmitSection(sb, "IMPLICIT STYLES");
        EmitImplicitStyles(sb, data);
        sb.AppendLine();

        // Button variant styles
        EmitSection(sb, "BUTTON VARIANT STYLES");
        EmitButtonVariantStyles(sb, data);
        sb.AppendLine();

        // Outline button styles
        EmitSection(sb, "OUTLINE BUTTON VARIANTS");
        EmitOutlineButtonStyles(sb, data);
        sb.AppendLine();

        // Size variants
        EmitSection(sb, "SIZE VARIANTS");
        EmitSizeVariants(sb);
        sb.AppendLine();

        // Card & component styles
        EmitSection(sb, "CARD & COMPONENT STYLES");
        EmitComponentStyles(sb, data);
        sb.AppendLine();

        // Color variant cards (text-bg-*)
        EmitSection(sb, "COLOR VARIANT CARDS");
        EmitColorVariantCards(sb);
        sb.AppendLine();

        // Badge backgrounds (bg-*)
        EmitSection(sb, "BADGE BACKGROUNDS");
        EmitBadgeBackgrounds(sb);
        sb.AppendLine();

        // On-color label styles
        EmitSection(sb, "ON-COLOR LABEL STYLES");
        EmitOnColorLabels(sb);
        sb.AppendLine();

        // Heading label styles
        EmitSection(sb, "HEADING STYLES");
        EmitHeadingStyles(sb, data);
        sb.AppendLine();

        // Text styles
        EmitSection(sb, "TEXT STYLES");
        EmitTextStyles(sb);
        sb.AppendLine();

        // Progress bar variants
        EmitSection(sb, "PROGRESS BAR VARIANTS");
        EmitProgressBarStyles(sb);

        sb.AppendLine();
        sb.AppendLine("</ResourceDictionary>");

        return sb.ToString();
    }

    /// <summary>
    /// Generates the code-behind (.xaml.cs) for the ResourceDictionary.
    /// </summary>
    public string GenerateCodeBehind(Parsing.BootstrapThemeData data, string @namespace)
    {
        var className = ToPascalCase(data.Name) + "Theme";
        return $@"// Auto-generated from CSS by MauiBootstrapTheme.Build. Do not edit.
namespace {@namespace};

public partial class {className} : ResourceDictionary
{{
    public {className}() => InitializeComponent();
}}
";
    }

    /// <summary>
    /// Generates a pure C# ResourceDictionary (no XAML needed).
    /// Avoids XAML compilation timing issues with build-time code generation.
    /// </summary>
    public string GenerateCSharpResourceDictionary(Parsing.BootstrapThemeData data, string @namespace)
    {
        var className = ToPascalCase(data.Name) + "Theme";
        var sb = new StringBuilder();

        sb.AppendLine("// Auto-generated from CSS by MauiBootstrapTheme.Build. Do not edit.");
        sb.AppendLine("using Microsoft.Maui.Controls;");
        sb.AppendLine("using Microsoft.Maui.Controls.Shapes;");
        sb.AppendLine("using Microsoft.Maui.Graphics;");
        sb.AppendLine("using MauiBootstrapTheme.Theming;");
        sb.AppendLine("#pragma warning disable CS8604");
        sb.AppendLine();
        sb.AppendLine($"namespace {@namespace};");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Generated Bootstrap theme ResourceDictionary from CSS.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"public class {className} : ResourceDictionary");
        sb.AppendLine("{");
        sb.AppendLine($"    private static object DR(string key) => new Microsoft.Maui.Controls.Xaml.DynamicResourceExtension {{ Key = key }}.ProvideValue(null!);");
        sb.AppendLine();
        sb.AppendLine($"    public {className}()");
        sb.AppendLine("    {");

        // Semantic colors
        EmitCsColor(sb, "Primary", data.Primary ?? "#0d6efd");
        EmitCsColor(sb, "Secondary", data.Secondary ?? "#6c757d");
        EmitCsColor(sb, "Success", data.Success ?? "#198754");
        EmitCsColor(sb, "Danger", data.Danger ?? "#dc3545");
        EmitCsColor(sb, "Warning", data.Warning ?? "#ffc107");
        EmitCsColor(sb, "Info", data.Info ?? "#0dcaf0");
        EmitCsColor(sb, "Light", data.Light ?? "#f8f9fa");
        EmitCsColor(sb, "Dark", data.Dark ?? "#212529");
        sb.AppendLine();

        // On-colors
        var variants = new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" };
        foreach (var v in variants)
        {
            var onColor = data.OnColors.TryGetValue(v, out var c) ? NormalizeHexColor(c) : (v == "warning" || v == "info" || v == "light" ? "#000000" : "#ffffff");
            EmitCsColor(sb, $"On{ToPascalCase(v)}", onColor);
        }
        sb.AppendLine();

        // Surface colors
        var bodyBg = data.BodyBackground ?? "#ffffff";
        var bodyColor = data.BodyColor ?? "#212529";
        var borderColor = data.BorderColor ?? "#dee2e6";
        EmitCsColor(sb, "Background", bodyBg);
        var surface = data.CardRules.Background ?? data.BodyBackground ?? "#ffffff";
        if (surface.Contains("var(")) surface = bodyBg;
        EmitCsColor(sb, "Surface", surface);
        EmitCsColor(sb, "OnBackground", bodyColor);
        EmitCsColor(sb, "OnSurface", bodyColor);

        var headingColor = data.HeadingColor;
        if (headingColor == null || headingColor == "inherit") headingColor = bodyColor;
        EmitCsColor(sb, "HeadingColor", headingColor);
        EmitCsColor(sb, "HeadingColorAlt", headingColor);
        EmitCsColor(sb, "Outline", borderColor);
        var outlineVariant = data.LightVariables.TryGetValue("--bs-border-color-translucent", out var ov)
            ? NormalizeHexColor(ov) : AdjustColorBrightness(borderColor, -10);
        EmitCsColor(sb, "OutlineVariant", outlineVariant);
        EmitCsColor(sb, "Muted", data.Secondary ?? "#6c757d");
        sb.AppendLine();

        // Dark mode color keys stored for reference (used by ApplyThemeMode)
        if (data.HasDarkMode)
        {
            sb.AppendLine("        // Dark mode override values stored as reference keys");
            if (data.DarkBodyBackground != null) EmitCsColor(sb, "DarkBackground", data.DarkBodyBackground);
            if (data.DarkBodyColor != null) EmitCsColor(sb, "DarkOnBackground", data.DarkBodyColor);
            if (data.DarkSecondaryBg != null) EmitCsColor(sb, "DarkSurface", data.DarkSecondaryBg);
            if (data.DarkBorderColor != null) EmitCsColor(sb, "DarkOutline", data.DarkBorderColor);
            sb.AppendLine();
        }

        // Gray scale
        var lv = data.LightVariables;
        EmitCsColor(sb, "Gray100", GetColorOrDefault(lv, "--bs-gray-100", "#f8f9fa"));
        EmitCsColor(sb, "Gray200", GetColorOrDefault(lv, "--bs-gray-200", "#e9ecef"));
        EmitCsColor(sb, "Gray300", GetColorOrDefault(lv, "--bs-gray-300", "#dee2e6"));
        EmitCsColor(sb, "Gray400", GetColorOrDefault(lv, "--bs-gray-400", "#ced4da"));
        EmitCsColor(sb, "Gray500", GetColorOrDefault(lv, "--bs-gray-500", "#adb5bd"));
        EmitCsColor(sb, "Gray600", GetColorOrDefault(lv, "--bs-gray-600", "#6c757d"));
        EmitCsColor(sb, "Gray700", GetColorOrDefault(lv, "--bs-gray-700", "#495057"));
        EmitCsColor(sb, "Gray800", GetColorOrDefault(lv, "--bs-gray-800", "#343a40"));
        EmitCsColor(sb, "Gray900", GetColorOrDefault(lv, "--bs-gray-900", "#212529"));
        sb.AppendLine();

        // Typography
        var fontFamily = "";
        if (data.FontFamily != null)
        {
            fontFamily = FontMapping.TryGetValue(data.FontFamily, out var mapped) ? mapped : data.FontFamily;
        }
        EmitCsString(sb, "FontFamily", fontFamily);
        var baseFontSize = data.BodyFontSize != null ? CssToDevicePixels(data.BodyFontSize) : 16;
        EmitCsDouble(sb, "FontSizeBase", baseFontSize);
        EmitCsDouble(sb, "FontSizeSm", data.BtnFontSizeSm != null ? CssToDevicePixels(data.BtnFontSizeSm) : baseFontSize * 0.875);
        EmitCsDouble(sb, "FontSizeLg", data.BtnFontSizeLg != null ? CssToDevicePixels(data.BtnFontSizeLg) : baseFontSize * 1.25);
        EmitCsDouble(sb, "FontSizeLead", baseFontSize * 1.25);
        EmitCsDouble(sb, "FontSizeH1", data.FontSizeH1 != null ? CssToDevicePixels(data.FontSizeH1) : baseFontSize * 2.5);
        EmitCsDouble(sb, "FontSizeH2", data.FontSizeH2 != null ? CssToDevicePixels(data.FontSizeH2) : baseFontSize * 2);
        EmitCsDouble(sb, "FontSizeH3", data.FontSizeH3 != null ? CssToDevicePixels(data.FontSizeH3) : baseFontSize * 1.75);
        EmitCsDouble(sb, "FontSizeH4", data.FontSizeH4 != null ? CssToDevicePixels(data.FontSizeH4) : baseFontSize * 1.5);
        EmitCsDouble(sb, "FontSizeH5", data.FontSizeH5 != null ? CssToDevicePixels(data.FontSizeH5) : baseFontSize * 1.25);
        EmitCsDouble(sb, "FontSizeH6", data.FontSizeH6 != null ? CssToDevicePixels(data.FontSizeH6) : baseFontSize);
        sb.AppendLine();

        // Spacing & sizing
        EmitCsInt(sb, "CornerRadius", (int)Math.Round(CssToDevicePixels(data.BorderRadius ?? "0.375rem")));
        EmitCsInt(sb, "CornerRadiusSm", (int)Math.Round(CssToDevicePixels(data.BorderRadiusSm ?? "0.25rem")));
        EmitCsInt(sb, "CornerRadiusLg", (int)Math.Round(CssToDevicePixels(data.BorderRadiusLg ?? "0.5rem")));
        // BtnBorderWidth may be a var() reference — skip it if so and fall back to BorderWidth
        var effectiveBtnBorderWidth = data.BtnBorderWidth != null && !data.BtnBorderWidth.StartsWith("var(")
            ? data.BtnBorderWidth : null;
        var baseBorderW = CssToDevicePixels(effectiveBtnBorderWidth ?? data.BorderWidth ?? "1px");
        EmitCsDouble(sb, "BorderWidth", baseBorderW);

        var btnPadX = data.BtnPaddingX != null ? CssToDevicePixels(data.BtnPaddingX) : 12;
        var btnPadY = data.BtnPaddingY != null ? CssToDevicePixels(data.BtnPaddingY) : 6;
        var btnPadXSm = data.BtnPaddingXSm != null ? CssToDevicePixels(data.BtnPaddingXSm) : 8;
        var btnPadYSm = data.BtnPaddingYSm != null ? CssToDevicePixels(data.BtnPaddingYSm) : 4;
        var btnPadXLg = data.BtnPaddingXLg != null ? CssToDevicePixels(data.BtnPaddingXLg) : 16;
        var btnPadYLg = data.BtnPaddingYLg != null ? CssToDevicePixels(data.BtnPaddingYLg) : 8;
        var borderW = baseBorderW;
        var buttonLineHeight = ParseLineHeightFactor(
            lv.TryGetValue("--bs-btn-line-height", out var btnLh) ? btnLh
            : (lv.TryGetValue("--bs-body-line-height", out var bodyLh) ? bodyLh : null),
            1.5);
        var inputFontSize = data.FormControlRule.FontSize != null ? CssToDevicePixels(data.FormControlRule.FontSize) : baseFontSize;
        var inputLineHeight = ParseLineHeightFactor(data.FormControlRule.LineHeight, 1.5);
        var inputBorderW = data.FormControlRule.BorderWidth != null ? CssToDevicePixels(data.FormControlRule.BorderWidth) : baseBorderW;
        var (inputPadX, inputPadY) = ResolvePadding(data.FormControlRule.Padding, btnPadX, btnPadY);

        var buttonHeight = btnPadY * 2 + (baseFontSize * buttonLineHeight) + borderW * 2;
        EmitCsDouble(sb, "ButtonHeight", buttonHeight);
        var buttonHeightSm = btnPadYSm * 2 + ((data.BtnFontSizeSm != null ? CssToDevicePixels(data.BtnFontSizeSm) : baseFontSize * 0.875) * buttonLineHeight) + borderW * 2;
        var buttonHeightLg = btnPadYLg * 2 + ((data.BtnFontSizeLg != null ? CssToDevicePixels(data.BtnFontSizeLg) : baseFontSize * 1.25) * buttonLineHeight) + borderW * 2;
        EmitCsDouble(sb, "ButtonHeightSm", buttonHeightSm);
        EmitCsDouble(sb, "ButtonHeightLg", buttonHeightLg);
        // Pre-calculate pill radii per size (half of height for perfect pill shape)
        EmitCsInt(sb, "CornerRadiusPill", (int)Math.Ceiling(buttonHeight / 2));
        EmitCsInt(sb, "CornerRadiusPillSm", (int)Math.Ceiling(buttonHeightSm / 2));
        EmitCsInt(sb, "CornerRadiusPillLg", (int)Math.Ceiling(buttonHeightLg / 2));
        EmitCsDouble(sb, "InputHeight", inputPadY * 2 + (inputFontSize * inputLineHeight) + inputBorderW * 2);
        EmitCsDouble(sb, "InputHeightSm", btnPadYSm * 2 + ((baseFontSize * 0.875) * inputLineHeight) + inputBorderW * 2);
        EmitCsDouble(sb, "InputHeightLg", btnPadYLg * 2 + ((baseFontSize * 1.25) * inputLineHeight) + inputBorderW * 2);
        sb.AppendLine($"        this[\"ButtonPadding\"] = new Thickness({btnPadX.ToString(CultureInfo.InvariantCulture)}, {btnPadY.ToString(CultureInfo.InvariantCulture)});");
        sb.AppendLine($"        this[\"ButtonPaddingSm\"] = new Thickness({btnPadXSm.ToString(CultureInfo.InvariantCulture)}, {btnPadYSm.ToString(CultureInfo.InvariantCulture)});");
        sb.AppendLine($"        this[\"ButtonPaddingLg\"] = new Thickness({btnPadXLg.ToString(CultureInfo.InvariantCulture)}, {btnPadYLg.ToString(CultureInfo.InvariantCulture)});");
        sb.AppendLine($"        this[\"InputPadding\"] = new Thickness({inputPadX.ToString(CultureInfo.InvariantCulture)}, {inputPadY.ToString(CultureInfo.InvariantCulture)});");

        var progressBg = data.ProgressBg ?? data.SecondaryBg ?? "#e9ecef";
        var inputBackground = data.FormControlRule.Background ?? data.BodyBackground ?? "#ffffff";
        var inputText = data.FormControlRule.Color ?? data.BodyColor ?? "#212529";
        var placeholderColor = data.FormControlRule.PlaceholderColor
            ?? (data.LightVariables.TryGetValue("--bs-secondary-color", out var sc) ? NormalizeHexColor(sc) : "#6c757d");
        EmitCsColor(sb, "ProgressBackground", progressBg);
        EmitCsColor(sb, "InputBackground", inputBackground);
        EmitCsColor(sb, "InputText", inputText);
        EmitCsColor(sb, "PlaceholderColor", placeholderColor);
        sb.AppendLine();

        // Implicit styles
        EmitCsImplicitStyles(sb, data);

        // Button variant styles
        EmitCsButtonVariantStyles(sb, data);

        // Outline button styles
        EmitCsOutlineButtonStyles(sb, data);

        // Size variants
        EmitCsSizeVariants(sb);

        // Component styles
        EmitCsComponentStyles(sb, data);

        // Color variant cards
        EmitCsColorVariantCards(sb, data);

        // Badge backgrounds
        EmitCsBadgeBackgrounds(sb);

        // On-color labels
        EmitCsOnColorLabels(sb);

        // Heading styles
        EmitCsHeadingStyles(sb, data);

        // Text styles
        EmitCsTextStyles(sb);

        // Progress bar variants
        EmitCsProgressBarStyles(sb);

        // Dark mode support
        if (data.HasDarkMode)
        {
            sb.AppendLine();
            sb.AppendLine("        // Apply initial theme mode based on current system theme");
            sb.AppendLine("        if (Application.Current != null)");
            sb.AppendLine("        {");
            sb.AppendLine("            ApplyThemeMode(Application.Current.RequestedTheme);");
            sb.AppendLine($"            var weakSelf = new WeakReference<{className}>(this);");
            sb.AppendLine("            Application.Current.RequestedThemeChanged += (s, e) =>");
            sb.AppendLine("            {");
            sb.AppendLine("                if (weakSelf.TryGetTarget(out var self))");
            sb.AppendLine("                    self.ApplyThemeMode(e.RequestedTheme);");
            sb.AppendLine("            };");
            sb.AppendLine("        }");
        }

        sb.AppendLine("    }");

        // Generate ApplyThemeMode method for themes with dark mode
        if (data.HasDarkMode)
        {
            sb.AppendLine();
            EmitApplyThemeModeMethod(sb, data);
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    #region Dark Mode Support

    private void EmitApplyThemeModeMethod(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Applies light or dark mode color overrides from the CSS [data-bs-theme=dark] block.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    private void ApplyThemeMode(AppTheme theme)");
        sb.AppendLine("    {");
        sb.AppendLine("        if (!Microsoft.Maui.ApplicationModel.MainThread.IsMainThread)");
        sb.AppendLine("        {");
        sb.AppendLine("            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() => ApplyThemeMode(theme));");
        sb.AppendLine("            return;");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine("        if (theme == AppTheme.Dark)");
        sb.AppendLine("        {");

        // Background and surface colors
        EmitDarkOverride(sb, "Background", data.DarkBodyBackground, data.BodyBackground ?? "#ffffff");
        EmitDarkOverride(sb, "OnBackground", data.DarkBodyColor, data.BodyColor ?? "#212529");
        EmitDarkOverride(sb, "Surface", data.DarkSecondaryBg, data.CardRules.Background);
        EmitDarkOverride(sb, "OnSurface", data.DarkBodyColor, data.BodyColor ?? "#212529");
        EmitDarkOverride(sb, "Outline", data.DarkBorderColor, data.BorderColor ?? "#dee2e6");

        var darkHeading = data.DarkHeadingColor;
        if (darkHeading == null || darkHeading == "inherit") darkHeading = data.DarkBodyColor;
        EmitDarkOverride(sb, "HeadingColor", darkHeading, null);
        EmitDarkOverride(sb, "HeadingColorAlt", darkHeading, null);

        // Input colors for dark mode
        if (data.DarkBodyBackground != null || data.DarkSecondaryBg != null)
        {
            var darkInputBg = data.FormControlRule.Background ?? data.DarkSecondaryBg ?? data.DarkBodyBackground ?? "#303030";
            var darkPlaceholder = data.FormControlRule.PlaceholderColor
                ?? (data.DarkVariables.TryGetValue("--bs-secondary-color", out var darkSc) ? NormalizeHexColor(darkSc) : "#FF999999");
            sb.AppendLine($"            this[\"InputBackground\"] = Color.FromArgb(\"{NormalizeHexColor(darkInputBg)}\");");
            sb.AppendLine($"            this[\"InputText\"] = Color.FromArgb(\"{NormalizeHexColor(data.FormControlRule.Color ?? data.DarkBodyColor ?? "#ffffff")}\");");
            sb.AppendLine($"            this[\"PlaceholderColor\"] = Color.FromArgb(\"{NormalizeHexColor(darkPlaceholder)}\");");
            sb.AppendLine($"            this[\"ProgressBackground\"] = Color.FromArgb(\"#FF464545\");");
        }

        // Dark-mode button overrides (e.g., Sketchy dark .btn-primary)
        foreach (var kvp in data.DarkButtonRules)
        {
            var pascal = ToPascalCase(kvp.Key);
            if (kvp.Value.Background != null)
                sb.AppendLine($"            this[\"{pascal}\"] = Color.FromArgb(\"{NormalizeHexColor(kvp.Value.Background)}\");");
            if (kvp.Value.Color != null)
                sb.AppendLine($"            this[\"On{pascal}\"] = Color.FromArgb(\"{NormalizeHexColor(kvp.Value.Color)}\");");
        }

        sb.AppendLine("        }");
        sb.AppendLine("        else");
        sb.AppendLine("        {");

        // Restore light mode colors
        sb.AppendLine($"            this[\"Background\"] = Color.FromArgb(\"{NormalizeHexColor(data.BodyBackground ?? "#ffffff")}\");");
        sb.AppendLine($"            this[\"OnBackground\"] = Color.FromArgb(\"{NormalizeHexColor(data.BodyColor ?? "#212529")}\");");
        var surface = data.CardRules.Background ?? data.BodyBackground ?? "#ffffff";
        if (surface.Contains("var(")) surface = data.BodyBackground ?? "#ffffff";
        sb.AppendLine($"            this[\"Surface\"] = Color.FromArgb(\"{NormalizeHexColor(surface)}\");");
        sb.AppendLine($"            this[\"OnSurface\"] = Color.FromArgb(\"{NormalizeHexColor(data.BodyColor ?? "#212529")}\");");
        sb.AppendLine($"            this[\"Outline\"] = Color.FromArgb(\"{NormalizeHexColor(data.BorderColor ?? "#dee2e6")}\");");

        var headingColor = data.HeadingColor;
        if (headingColor == null || headingColor == "inherit") headingColor = data.BodyColor ?? "#212529";
        sb.AppendLine($"            this[\"HeadingColor\"] = Color.FromArgb(\"{NormalizeHexColor(headingColor)}\");");
        sb.AppendLine($"            this[\"HeadingColorAlt\"] = Color.FromArgb(\"{NormalizeHexColor(headingColor)}\");");

        var inputBg = data.FormControlRule.Background ?? data.BodyBackground ?? "#ffffff";
        sb.AppendLine($"            this[\"InputBackground\"] = Color.FromArgb(\"{NormalizeHexColor(inputBg)}\");");
        sb.AppendLine($"            this[\"InputText\"] = Color.FromArgb(\"{NormalizeHexColor(data.FormControlRule.Color ?? data.BodyColor ?? "#212529")}\");");
        var lightPlaceholder = data.FormControlRule.PlaceholderColor
            ?? (data.LightVariables.TryGetValue("--bs-secondary-color", out var lightSc)
                ? NormalizeHexColor(lightSc)
                : "#FF6c757d");
        sb.AppendLine($"            this[\"PlaceholderColor\"] = Color.FromArgb(\"{lightPlaceholder}\");");
        var lightProgressBg = data.ProgressBg ?? data.SecondaryBg ?? "#e9ecef";
        sb.AppendLine($"            this[\"ProgressBackground\"] = Color.FromArgb(\"{NormalizeHexColor(lightProgressBg)}\");");

        // Restore light-mode button colors for any variants that had dark overrides
        foreach (var kvp in data.DarkButtonRules)
        {
            var pascal = ToPascalCase(kvp.Key);
            var lightColor = pascal switch
            {
                "Primary" => data.Primary,
                "Secondary" => data.Secondary,
                "Success" => data.Success,
                "Danger" => data.Danger,
                "Warning" => data.Warning,
                "Info" => data.Info,
                "Light" => data.Light,
                "Dark" => data.Dark,
                _ => null
            };
            if (lightColor != null)
                sb.AppendLine($"            this[\"{pascal}\"] = Color.FromArgb(\"{NormalizeHexColor(lightColor)}\");");

            var lightOnColor = data.OnColors.TryGetValue(kvp.Key, out var oc) ? oc : null;
            if (lightOnColor != null)
                sb.AppendLine($"            this[\"On{pascal}\"] = Color.FromArgb(\"{NormalizeHexColor(lightOnColor)}\");");
        }

        sb.AppendLine("        }");
        sb.AppendLine("    }");
    }

    private void EmitDarkOverride(StringBuilder sb, string key, string? darkValue, string? fallback)
    {
        if (darkValue != null)
        {
            sb.AppendLine($"            this[\"{key}\"] = Color.FromArgb(\"{NormalizeHexColor(darkValue)}\");");
        }
    }

    #endregion

    #region C# Emit Helpers

    private void EmitCsColor(StringBuilder sb, string key, string value)
    {
        sb.AppendLine($"        this[\"{key}\"] = Color.FromArgb(\"{NormalizeHexColor(value)}\");");
    }

    private void EmitCsDouble(StringBuilder sb, string key, double value)
    {
        sb.AppendLine($"        this[\"{key}\"] = {value.ToString(CultureInfo.InvariantCulture)}d;");
    }

    private void EmitCsInt(StringBuilder sb, string key, int value)
    {
        sb.AppendLine($"        this[\"{key}\"] = {value};");
    }

    private void EmitCsString(StringBuilder sb, string key, string value)
    {
        sb.AppendLine($"        this[\"{key}\"] = \"{value}\";");
    }

    private void EmitCsStyle(StringBuilder sb, string targetType, string? cssClass, Action<StringBuilder> emitSetters)
    {
        if (cssClass != null)
            sb.AppendLine($"        var style_{cssClass.Replace("-", "_")}_{targetType} = new Style(typeof({targetType})) {{ Class = \"{cssClass}\" }};");
        else
            sb.AppendLine($"        var style_{targetType.ToLower()} = new Style(typeof({targetType}));");

        emitSetters(sb);

        var varName = cssClass != null ? $"style_{cssClass.Replace("-", "_")}_{targetType}" : $"style_{targetType.ToLower()}";
        sb.AppendLine($"        Add({varName});");
        sb.AppendLine();
    }

    private void EmitCsSetter(StringBuilder sb, string varName, string property, string valueExpr)
    {
        sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = {property}, Value = {valueExpr} }});");
    }

    private void EmitCsDynamicSetter(StringBuilder sb, string varName, string targetType, string property, string resourceKey)
    {
        sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = {targetType}.{property}Property, Value = DR(\"{resourceKey}\") }});");
    }

    private void EmitCsImplicitStyles(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        // Button implicit style
        sb.AppendLine("        // Implicit styles");
        sb.AppendLine("        var style_button = new Style(typeof(Button));");
        if (data.HasGradientButtons && data.ButtonRules.TryGetValue("primary", out var pBtn) && pBtn.Gradient != null)
        {
            EmitCsGradientSetter(sb, "style_button", pBtn.Gradient);
        }
        else
        {
            sb.AppendLine("        style_button.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = DR(\"Primary\") });");
        }
        sb.AppendLine("        style_button.Setters.Add(new Setter { Property = Button.TextColorProperty, Value = DR(\"OnPrimary\") });");
        sb.AppendLine("        style_button.Setters.Add(new Setter { Property = Button.CornerRadiusProperty, Value = DR(\"CornerRadius\") });");
        sb.AppendLine("        style_button.Setters.Add(new Setter { Property = Button.FontFamilyProperty, Value = DR(\"FontFamily\") });");
        sb.AppendLine("        style_button.Setters.Add(new Setter { Property = Button.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        style_button.Setters.Add(new Setter { Property = Button.PaddingProperty, Value = DR(\"ButtonPadding\") });");
        sb.AppendLine("        style_button.Setters.Add(new Setter { Property = Button.MinimumHeightRequestProperty, Value = DR(\"ButtonHeight\") });");

        var hasBorder = data.ButtonRules.TryGetValue("primary", out var primaryBtn) && primaryBtn.BorderColor != null;
        if (hasBorder)
        {
            sb.AppendLine("        style_button.Setters.Add(new Setter { Property = Button.BorderWidthProperty, Value = DR(\"BorderWidth\") });");
            sb.AppendLine("        style_button.Setters.Add(new Setter { Property = Button.BorderColorProperty, Value = Colors.Transparent });");
        }
        else
        {
            sb.AppendLine("        style_button.Setters.Add(new Setter { Property = Button.BorderWidthProperty, Value = 0d });");
        }

        if (data.HasGlowButtons && data.ButtonRules.TryGetValue("primary", out var glowBtn) && glowBtn.BoxShadow != null)
        {
            EmitCsShadowSetter(sb, "style_button", data.Primary ?? "#6f42c1");
        }
        else if (data.BtnBaseBoxShadow != null)
        {
            // Resting-state offset shadow (e.g., Brite's "3px 3px 0 0 #000")
            EmitCsBoxShadowSetter(sb, "style_button", data.BtnBaseBoxShadow, data.Dark ?? "#000");
        }
        else
        {
            // Explicitly clear Shadow to prevent stale values persisting across theme switches.
            // Use an invisible shadow (Opacity 0) because setting null via Style Setter
            // does not reliably clear the property when switching from a theme that set it.
            sb.AppendLine("        style_button.Setters.Add(new Setter { Property = Button.ShadowProperty, Value = new Shadow { Brush = Colors.Transparent, Offset = new Point(0, 0), Radius = 0, Opacity = 0f } });");
        }

        sb.AppendLine("        Add(style_button);");
        sb.AppendLine();

        // Label
        sb.AppendLine("        var style_label = new Style(typeof(Label));");
        sb.AppendLine("        style_label.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = DR(\"OnBackground\") });");
        sb.AppendLine("        style_label.Setters.Add(new Setter { Property = Label.FontFamilyProperty, Value = DR(\"FontFamily\") });");
        sb.AppendLine("        style_label.Setters.Add(new Setter { Property = Label.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        Add(style_label);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_label = new Style(typeof(Label)) { Class = \"form-label\" };");
        sb.AppendLine("        style_form_label.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = DR(\"OnBackground\") });");
        sb.AppendLine("        style_form_label.Setters.Add(new Setter { Property = Label.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        Add(style_form_label);");
        sb.AppendLine();

        // Entry
        sb.AppendLine("        var style_entry = new Style(typeof(Entry));");
        sb.AppendLine("        style_entry.Setters.Add(new Setter { Property = Entry.TextColorProperty, Value = DR(\"InputText\") });");
        sb.AppendLine("        style_entry.Setters.Add(new Setter { Property = Entry.BackgroundProperty, Value = DR(\"InputBackground\") });");
        sb.AppendLine("        style_entry.Setters.Add(new Setter { Property = Entry.PlaceholderColorProperty, Value = DR(\"PlaceholderColor\") });");
        sb.AppendLine("        style_entry.Setters.Add(new Setter { Property = Entry.FontFamilyProperty, Value = DR(\"FontFamily\") });");
        sb.AppendLine("        style_entry.Setters.Add(new Setter { Property = Entry.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        style_entry.Setters.Add(new Setter { Property = Entry.MinimumHeightRequestProperty, Value = DR(\"InputHeight\") });");
        sb.AppendLine("        Add(style_entry);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_control_entry = new Style(typeof(Entry)) { Class = \"form-control\" };");
        sb.AppendLine("        style_form_control_entry.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Default });");
        sb.AppendLine("        style_form_control_entry.Setters.Add(new Setter { Property = Entry.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        style_form_control_entry.Setters.Add(new Setter { Property = Entry.MinimumHeightRequestProperty, Value = DR(\"InputHeight\") });");
        sb.AppendLine("        Add(style_form_control_entry);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_control_lg_entry = new Style(typeof(Entry)) { Class = \"form-control-lg\" };");
        sb.AppendLine("        style_form_control_lg_entry.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Large });");
        sb.AppendLine("        style_form_control_lg_entry.Setters.Add(new Setter { Property = Entry.FontSizeProperty, Value = DR(\"FontSizeLg\") });");
        sb.AppendLine("        style_form_control_lg_entry.Setters.Add(new Setter { Property = Entry.MinimumHeightRequestProperty, Value = DR(\"InputHeightLg\") });");
        sb.AppendLine("        Add(style_form_control_lg_entry);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_control_sm_entry = new Style(typeof(Entry)) { Class = \"form-control-sm\" };");
        sb.AppendLine("        style_form_control_sm_entry.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Small });");
        sb.AppendLine("        style_form_control_sm_entry.Setters.Add(new Setter { Property = Entry.FontSizeProperty, Value = DR(\"FontSizeSm\") });");
        sb.AppendLine("        style_form_control_sm_entry.Setters.Add(new Setter { Property = Entry.MinimumHeightRequestProperty, Value = DR(\"InputHeightSm\") });");
        sb.AppendLine("        Add(style_form_control_sm_entry);");
        sb.AppendLine();

        // Picker
        sb.AppendLine("        var style_picker = new Style(typeof(Picker));");
        sb.AppendLine("        style_picker.Setters.Add(new Setter { Property = Picker.TextColorProperty, Value = DR(\"InputText\") });");
        sb.AppendLine("        style_picker.Setters.Add(new Setter { Property = Picker.TitleColorProperty, Value = DR(\"PlaceholderColor\") });");
        sb.AppendLine("        style_picker.Setters.Add(new Setter { Property = Picker.FontFamilyProperty, Value = DR(\"FontFamily\") });");
        sb.AppendLine("        style_picker.Setters.Add(new Setter { Property = Picker.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        style_picker.Setters.Add(new Setter { Property = Picker.MinimumHeightRequestProperty, Value = DR(\"InputHeight\") });");
        sb.AppendLine("        Add(style_picker);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_select_picker = new Style(typeof(Picker)) { Class = \"form-select\" };");
        sb.AppendLine("        style_form_select_picker.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Default });");
        sb.AppendLine("        style_form_select_picker.Setters.Add(new Setter { Property = Picker.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        style_form_select_picker.Setters.Add(new Setter { Property = Picker.MinimumHeightRequestProperty, Value = DR(\"InputHeight\") });");
        sb.AppendLine("        Add(style_form_select_picker);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_select_lg_picker = new Style(typeof(Picker)) { Class = \"form-select-lg\" };");
        sb.AppendLine("        style_form_select_lg_picker.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Large });");
        sb.AppendLine("        style_form_select_lg_picker.Setters.Add(new Setter { Property = Picker.FontSizeProperty, Value = DR(\"FontSizeLg\") });");
        sb.AppendLine("        style_form_select_lg_picker.Setters.Add(new Setter { Property = Picker.MinimumHeightRequestProperty, Value = DR(\"InputHeightLg\") });");
        sb.AppendLine("        Add(style_form_select_lg_picker);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_select_sm_picker = new Style(typeof(Picker)) { Class = \"form-select-sm\" };");
        sb.AppendLine("        style_form_select_sm_picker.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Small });");
        sb.AppendLine("        style_form_select_sm_picker.Setters.Add(new Setter { Property = Picker.FontSizeProperty, Value = DR(\"FontSizeSm\") });");
        sb.AppendLine("        style_form_select_sm_picker.Setters.Add(new Setter { Property = Picker.MinimumHeightRequestProperty, Value = DR(\"InputHeightSm\") });");
        sb.AppendLine("        Add(style_form_select_sm_picker);");
        sb.AppendLine();

        // DatePicker
        sb.AppendLine("        var style_datepicker = new Style(typeof(DatePicker));");
        sb.AppendLine("        style_datepicker.Setters.Add(new Setter { Property = DatePicker.TextColorProperty, Value = DR(\"InputText\") });");
        sb.AppendLine("        style_datepicker.Setters.Add(new Setter { Property = DatePicker.FontFamilyProperty, Value = DR(\"FontFamily\") });");
        sb.AppendLine("        style_datepicker.Setters.Add(new Setter { Property = DatePicker.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        style_datepicker.Setters.Add(new Setter { Property = DatePicker.MinimumHeightRequestProperty, Value = DR(\"InputHeight\") });");
        sb.AppendLine("        Add(style_datepicker);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_control_datepicker = new Style(typeof(DatePicker)) { Class = \"form-control\" };");
        sb.AppendLine("        style_form_control_datepicker.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Default });");
        sb.AppendLine("        style_form_control_datepicker.Setters.Add(new Setter { Property = DatePicker.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        style_form_control_datepicker.Setters.Add(new Setter { Property = DatePicker.MinimumHeightRequestProperty, Value = DR(\"InputHeight\") });");
        sb.AppendLine("        Add(style_form_control_datepicker);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_control_lg_datepicker = new Style(typeof(DatePicker)) { Class = \"form-control-lg\" };");
        sb.AppendLine("        style_form_control_lg_datepicker.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Large });");
        sb.AppendLine("        style_form_control_lg_datepicker.Setters.Add(new Setter { Property = DatePicker.FontSizeProperty, Value = DR(\"FontSizeLg\") });");
        sb.AppendLine("        style_form_control_lg_datepicker.Setters.Add(new Setter { Property = DatePicker.MinimumHeightRequestProperty, Value = DR(\"InputHeightLg\") });");
        sb.AppendLine("        Add(style_form_control_lg_datepicker);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_control_sm_datepicker = new Style(typeof(DatePicker)) { Class = \"form-control-sm\" };");
        sb.AppendLine("        style_form_control_sm_datepicker.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Small });");
        sb.AppendLine("        style_form_control_sm_datepicker.Setters.Add(new Setter { Property = DatePicker.FontSizeProperty, Value = DR(\"FontSizeSm\") });");
        sb.AppendLine("        style_form_control_sm_datepicker.Setters.Add(new Setter { Property = DatePicker.MinimumHeightRequestProperty, Value = DR(\"InputHeightSm\") });");
        sb.AppendLine("        Add(style_form_control_sm_datepicker);");
        sb.AppendLine();

        // TimePicker
        sb.AppendLine("        var style_timepicker = new Style(typeof(TimePicker));");
        sb.AppendLine("        style_timepicker.Setters.Add(new Setter { Property = TimePicker.TextColorProperty, Value = DR(\"InputText\") });");
        sb.AppendLine("        style_timepicker.Setters.Add(new Setter { Property = TimePicker.FontFamilyProperty, Value = DR(\"FontFamily\") });");
        sb.AppendLine("        style_timepicker.Setters.Add(new Setter { Property = TimePicker.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        style_timepicker.Setters.Add(new Setter { Property = TimePicker.MinimumHeightRequestProperty, Value = DR(\"InputHeight\") });");
        sb.AppendLine("        Add(style_timepicker);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_control_timepicker = new Style(typeof(TimePicker)) { Class = \"form-control\" };");
        sb.AppendLine("        style_form_control_timepicker.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Default });");
        sb.AppendLine("        style_form_control_timepicker.Setters.Add(new Setter { Property = TimePicker.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        style_form_control_timepicker.Setters.Add(new Setter { Property = TimePicker.MinimumHeightRequestProperty, Value = DR(\"InputHeight\") });");
        sb.AppendLine("        Add(style_form_control_timepicker);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_control_lg_timepicker = new Style(typeof(TimePicker)) { Class = \"form-control-lg\" };");
        sb.AppendLine("        style_form_control_lg_timepicker.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Large });");
        sb.AppendLine("        style_form_control_lg_timepicker.Setters.Add(new Setter { Property = TimePicker.FontSizeProperty, Value = DR(\"FontSizeLg\") });");
        sb.AppendLine("        style_form_control_lg_timepicker.Setters.Add(new Setter { Property = TimePicker.MinimumHeightRequestProperty, Value = DR(\"InputHeightLg\") });");
        sb.AppendLine("        Add(style_form_control_lg_timepicker);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_control_sm_timepicker = new Style(typeof(TimePicker)) { Class = \"form-control-sm\" };");
        sb.AppendLine("        style_form_control_sm_timepicker.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Small });");
        sb.AppendLine("        style_form_control_sm_timepicker.Setters.Add(new Setter { Property = TimePicker.FontSizeProperty, Value = DR(\"FontSizeSm\") });");
        sb.AppendLine("        style_form_control_sm_timepicker.Setters.Add(new Setter { Property = TimePicker.MinimumHeightRequestProperty, Value = DR(\"InputHeightSm\") });");
        sb.AppendLine("        Add(style_form_control_sm_timepicker);");
        sb.AppendLine();

        // Editor
        sb.AppendLine("        var style_editor = new Style(typeof(Editor));");
        sb.AppendLine("        style_editor.Setters.Add(new Setter { Property = Editor.TextColorProperty, Value = DR(\"InputText\") });");
        sb.AppendLine("        style_editor.Setters.Add(new Setter { Property = Editor.BackgroundProperty, Value = DR(\"InputBackground\") });");
        sb.AppendLine("        style_editor.Setters.Add(new Setter { Property = Editor.FontFamilyProperty, Value = DR(\"FontFamily\") });");
        sb.AppendLine("        style_editor.Setters.Add(new Setter { Property = Editor.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        style_editor.Setters.Add(new Setter { Property = Editor.MinimumHeightRequestProperty, Value = DR(\"InputHeight\") });");
        sb.AppendLine("        Add(style_editor);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_control_editor = new Style(typeof(Editor)) { Class = \"form-control\" };");
        sb.AppendLine("        style_form_control_editor.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Default });");
        sb.AppendLine("        style_form_control_editor.Setters.Add(new Setter { Property = Editor.FontSizeProperty, Value = DR(\"FontSizeBase\") });");
        sb.AppendLine("        style_form_control_editor.Setters.Add(new Setter { Property = Editor.MinimumHeightRequestProperty, Value = DR(\"InputHeight\") });");
        sb.AppendLine("        Add(style_form_control_editor);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_control_lg_editor = new Style(typeof(Editor)) { Class = \"form-control-lg\" };");
        sb.AppendLine("        style_form_control_lg_editor.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Large });");
        sb.AppendLine("        style_form_control_lg_editor.Setters.Add(new Setter { Property = Editor.FontSizeProperty, Value = DR(\"FontSizeLg\") });");
        sb.AppendLine("        style_form_control_lg_editor.Setters.Add(new Setter { Property = Editor.MinimumHeightRequestProperty, Value = DR(\"InputHeightLg\") });");
        sb.AppendLine("        Add(style_form_control_lg_editor);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_control_sm_editor = new Style(typeof(Editor)) { Class = \"form-control-sm\" };");
        sb.AppendLine("        style_form_control_sm_editor.Setters.Add(new Setter { Property = Bootstrap.SizeProperty, Value = BootstrapSize.Small });");
        sb.AppendLine("        style_form_control_sm_editor.Setters.Add(new Setter { Property = Editor.FontSizeProperty, Value = DR(\"FontSizeSm\") });");
        sb.AppendLine("        style_form_control_sm_editor.Setters.Add(new Setter { Property = Editor.MinimumHeightRequestProperty, Value = DR(\"InputHeightSm\") });");
        sb.AppendLine("        Add(style_form_control_sm_editor);");
        sb.AppendLine();

        // Check/Radio/Switch/Range classes
        sb.AppendLine("        var style_form_check_checkbox = new Style(typeof(CheckBox)) { Class = \"form-check-input\" };");
        sb.AppendLine("        style_form_check_checkbox.Setters.Add(new Setter { Property = CheckBox.ColorProperty, Value = DR(\"Primary\") });");
        sb.AppendLine("        Add(style_form_check_checkbox);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_check_radio = new Style(typeof(RadioButton)) { Class = \"form-check-input\" };");
        sb.AppendLine("        style_form_check_radio.Setters.Add(new Setter { Property = RadioButton.TextColorProperty, Value = DR(\"OnBackground\") });");
        sb.AppendLine("        Add(style_form_check_radio);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_switch = new Style(typeof(Switch)) { Class = \"form-switch\" };");
        sb.AppendLine("        style_form_switch.Setters.Add(new Setter { Property = Switch.OnColorProperty, Value = DR(\"Primary\") });");
        sb.AppendLine("        Add(style_form_switch);");
        sb.AppendLine();
        sb.AppendLine("        var style_form_range = new Style(typeof(Slider)) { Class = \"form-range\" };");
        sb.AppendLine("        style_form_range.Setters.Add(new Setter { Property = Slider.MinimumTrackColorProperty, Value = DR(\"Primary\") });");
        sb.AppendLine("        style_form_range.Setters.Add(new Setter { Property = Slider.ThumbColorProperty, Value = DR(\"Primary\") });");
        sb.AppendLine("        style_form_range.Setters.Add(new Setter { Property = Slider.MaximumTrackColorProperty, Value = DR(\"OutlineVariant\") });");
        sb.AppendLine("        Add(style_form_range);");
        sb.AppendLine();

        // ProgressBar
        sb.AppendLine("        var style_progressbar = new Style(typeof(ProgressBar));");
        sb.AppendLine("        style_progressbar.Setters.Add(new Setter { Property = ProgressBar.ProgressColorProperty, Value = DR(\"Primary\") });");
        sb.AppendLine("        style_progressbar.Setters.Add(new Setter { Property = ProgressBar.MinimumHeightRequestProperty, Value = 16d });");
        sb.AppendLine("        Add(style_progressbar);");
        sb.AppendLine();

        // Border
        sb.AppendLine("        var style_border = new Style(typeof(Border));");
        sb.AppendLine("        style_border.Setters.Add(new Setter { Property = Border.BackgroundProperty, Value = Colors.Transparent });");
        sb.AppendLine("        style_border.Setters.Add(new Setter { Property = Border.StrokeProperty, Value = Colors.Transparent });");
        sb.AppendLine("        style_border.Setters.Add(new Setter { Property = Border.StrokeThicknessProperty, Value = 0d });");
        sb.AppendLine("        Add(style_border);");
        sb.AppendLine();

        // ContentPage
        sb.AppendLine("        var style_page = new Style(typeof(ContentPage)) { ApplyToDerivedTypes = true };");
        sb.AppendLine("        style_page.Setters.Add(new Setter { Property = ContentPage.BackgroundProperty, Value = DR(\"Background\") });");
        sb.AppendLine("        Add(style_page);");
        sb.AppendLine();

        // Shell
        sb.AppendLine("        var style_shell = new Style(typeof(Shell)) { ApplyToDerivedTypes = true };");
        sb.AppendLine("        style_shell.Setters.Add(new Setter { Property = Shell.BackgroundColorProperty, Value = DR(\"Primary\") });");
        sb.AppendLine("        style_shell.Setters.Add(new Setter { Property = Shell.ForegroundColorProperty, Value = DR(\"OnPrimary\") });");
        sb.AppendLine("        style_shell.Setters.Add(new Setter { Property = Shell.TitleColorProperty, Value = DR(\"OnPrimary\") });");
        sb.AppendLine("        style_shell.Setters.Add(new Setter { Property = Shell.TabBarBackgroundColorProperty, Value = DR(\"Background\") });");
        sb.AppendLine("        style_shell.Setters.Add(new Setter { Property = Shell.TabBarForegroundColorProperty, Value = DR(\"Primary\") });");
        sb.AppendLine("        style_shell.Setters.Add(new Setter { Property = Shell.TabBarUnselectedColorProperty, Value = DR(\"Gray500\") });");
        sb.AppendLine("        style_shell.Setters.Add(new Setter { Property = Shell.FlyoutBackgroundColorProperty, Value = DR(\"Background\") });");
        sb.AppendLine("        Add(style_shell);");
        sb.AppendLine();
    }

    private void EmitCsButtonVariantStyles(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        sb.AppendLine("        // Button variant styles");
        var variants2 = new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" };
        foreach (var v in variants2)
        {
            var pascal = ToPascalCase(v);
            var varName = $"style_btn_{v}";
            sb.AppendLine($"        var {varName} = new Style(typeof(Button)) {{ Class = \"btn-{v}\" }};");

            if (data.ButtonRules.TryGetValue(v, out var rule) && rule.Gradient != null)
            {
                EmitCsGradientSetter(sb, varName, rule.Gradient);
            }
            else
            {
                sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.BackgroundProperty, Value = DR(\"{pascal}\") }});");
            }
            sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.TextColorProperty, Value = DR(\"On{pascal}\") }});");

            // Set border color from CSS --bs-btn-border-color (matches background for solid buttons)
            if (data.ButtonRules.TryGetValue(v, out var borderRule) && borderRule.BorderColor != null)
            {
                sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.BorderColorProperty, Value = Color.FromArgb(\"{NormalizeHexColor(borderRule.BorderColor)}\") }});");
            }
            else
            {
                sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.BorderColorProperty, Value = DR(\"{pascal}\") }});");
            }

            if (data.ButtonRules.TryGetValue(v, out var glowRule) && glowRule.BoxShadow != null)
            {
                var color = v switch
                {
                    "primary" => data.Primary,
                    "secondary" => data.Secondary,
                    "success" => data.Success,
                    "danger" => data.Danger,
                    "warning" => data.Warning,
                    "info" => data.Info,
                    _ => null
                };
                if (color != null)
                    EmitCsShadowSetter(sb, varName, color);
            }

            sb.AppendLine($"        Add({varName});");
            sb.AppendLine();
        }
    }

    private void EmitCsOutlineButtonStyles(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        sb.AppendLine("        // Outline button variants");
        var variants3 = new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" };
        foreach (var v in variants3)
        {
            var pascal = ToPascalCase(v);
            var varName = $"style_btnout_{v}";
            data.ButtonRules.TryGetValue($"outline-{v}", out var rule);
            sb.AppendLine($"        var {varName} = new Style(typeof(Button)) {{ Class = \"btn-outline-{v}\" }};");
            if (!string.IsNullOrWhiteSpace(rule?.Background))
                sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.BackgroundProperty, Value = Color.FromArgb(\"{NormalizeHexColor(rule.Background)}\") }});");
            else
                sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.BackgroundProperty, Value = Colors.Transparent }});");

            if (!string.IsNullOrWhiteSpace(rule?.Color))
                sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.TextColorProperty, Value = Color.FromArgb(\"{NormalizeHexColor(rule.Color)}\") }});");
            else
                sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.TextColorProperty, Value = DR(\"{pascal}\") }});");

            if (!string.IsNullOrWhiteSpace(rule?.BorderColor))
                sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.BorderColorProperty, Value = Color.FromArgb(\"{NormalizeHexColor(rule.BorderColor)}\") }});");
            else
                sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.BorderColorProperty, Value = DR(\"{pascal}\") }});");

            if (!string.IsNullOrWhiteSpace(rule?.BorderWidth))
                sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.BorderWidthProperty, Value = {CssToDevicePixels(rule.BorderWidth).ToString(CultureInfo.InvariantCulture)}d }});");
            else
                sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.BorderWidthProperty, Value = DR(\"BorderWidth\") }});");
            sb.AppendLine($"        Add({varName});");
            sb.AppendLine();
        }
    }

    private void EmitCsSizeVariants(StringBuilder sb)
    {
        sb.AppendLine("        // Size variants");
        sb.AppendLine("        var style_btn_lg = new Style(typeof(Button)) { Class = \"btn-lg\" };");
        sb.AppendLine("        style_btn_lg.Setters.Add(new Setter { Property = Button.FontSizeProperty, Value = DR(\"FontSizeLg\") });");
        sb.AppendLine("        style_btn_lg.Setters.Add(new Setter { Property = Button.PaddingProperty, Value = DR(\"ButtonPaddingLg\") });");
        sb.AppendLine("        style_btn_lg.Setters.Add(new Setter { Property = Button.MinimumHeightRequestProperty, Value = DR(\"ButtonHeightLg\") });");
        sb.AppendLine("        style_btn_lg.Setters.Add(new Setter { Property = Button.CornerRadiusProperty, Value = DR(\"CornerRadiusLg\") });");
        sb.AppendLine("        Add(style_btn_lg);");
        sb.AppendLine();

        sb.AppendLine("        var style_btn_sm = new Style(typeof(Button)) { Class = \"btn-sm\" };");
        sb.AppendLine("        style_btn_sm.Setters.Add(new Setter { Property = Button.FontSizeProperty, Value = DR(\"FontSizeSm\") });");
        sb.AppendLine("        style_btn_sm.Setters.Add(new Setter { Property = Button.PaddingProperty, Value = DR(\"ButtonPaddingSm\") });");
        sb.AppendLine("        style_btn_sm.Setters.Add(new Setter { Property = Button.MinimumHeightRequestProperty, Value = DR(\"ButtonHeightSm\") });");
        sb.AppendLine("        style_btn_sm.Setters.Add(new Setter { Property = Button.CornerRadiusProperty, Value = DR(\"CornerRadiusSm\") });");
        sb.AppendLine("        Add(style_btn_sm);");
        sb.AppendLine();

        sb.AppendLine("        var style_btn_pill = new Style(typeof(Button)) { Class = \"btn-pill\" };");
        sb.AppendLine("        style_btn_pill.Setters.Add(new Setter { Property = Button.CornerRadiusProperty, Value = DR(\"CornerRadiusPill\") });");
        sb.AppendLine("        Add(style_btn_pill);");
        sb.AppendLine();

        // Pill + size combos: correct corner radius for each button height
        sb.AppendLine("        var style_btn_pill_lg = new Style(typeof(Button)) { Class = \"btn-pill-lg\" };");
        sb.AppendLine("        style_btn_pill_lg.Setters.Add(new Setter { Property = Button.FontSizeProperty, Value = DR(\"FontSizeLg\") });");
        sb.AppendLine("        style_btn_pill_lg.Setters.Add(new Setter { Property = Button.PaddingProperty, Value = DR(\"ButtonPaddingLg\") });");
        sb.AppendLine("        style_btn_pill_lg.Setters.Add(new Setter { Property = Button.MinimumHeightRequestProperty, Value = DR(\"ButtonHeightLg\") });");
        sb.AppendLine("        style_btn_pill_lg.Setters.Add(new Setter { Property = Button.CornerRadiusProperty, Value = DR(\"CornerRadiusPillLg\") });");
        sb.AppendLine("        Add(style_btn_pill_lg);");
        sb.AppendLine();

        sb.AppendLine("        var style_btn_pill_sm = new Style(typeof(Button)) { Class = \"btn-pill-sm\" };");
        sb.AppendLine("        style_btn_pill_sm.Setters.Add(new Setter { Property = Button.FontSizeProperty, Value = DR(\"FontSizeSm\") });");
        sb.AppendLine("        style_btn_pill_sm.Setters.Add(new Setter { Property = Button.PaddingProperty, Value = DR(\"ButtonPaddingSm\") });");
        sb.AppendLine("        style_btn_pill_sm.Setters.Add(new Setter { Property = Button.MinimumHeightRequestProperty, Value = DR(\"ButtonHeightSm\") });");
        sb.AppendLine("        style_btn_pill_sm.Setters.Add(new Setter { Property = Button.CornerRadiusProperty, Value = DR(\"CornerRadiusPillSm\") });");
        sb.AppendLine("        Add(style_btn_pill_sm);");
        sb.AppendLine();
    }

    private void EmitCsComponentStyles(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        var cardBorderRadius = data.CardRules.BorderRadius ?? data.BorderRadius ?? "0.375rem";
        if (cardBorderRadius.Contains("var("))
            cardBorderRadius = data.BorderRadius ?? "0.375rem";
        var cardCornerRadius = (int)Math.Round(CssToDevicePixels(cardBorderRadius));
        var cardPadX = data.CardSpacerX != null ? CssToDevicePixels(data.CardSpacerX) : 16;
        var cardPadY = data.CardSpacerY != null ? CssToDevicePixels(data.CardSpacerY) : 16;

        sb.AppendLine("        // Card & component styles");
        sb.AppendLine("        var style_card = new Style(typeof(Border)) { Class = \"card\" };");
        sb.AppendLine("        style_card.Setters.Add(new Setter { Property = Border.BackgroundProperty, Value = DR(\"Surface\") });");
        sb.AppendLine("        style_card.Setters.Add(new Setter { Property = Border.StrokeProperty, Value = DR(\"Outline\") });");
        sb.AppendLine("        style_card.Setters.Add(new Setter { Property = Border.StrokeThicknessProperty, Value = DR(\"BorderWidth\") });");
        sb.AppendLine($"        style_card.Setters.Add(new Setter {{ Property = Border.StrokeShapeProperty, Value = new RoundRectangle {{ CornerRadius = {cardCornerRadius} }} }});");
        sb.AppendLine($"        style_card.Setters.Add(new Setter {{ Property = Border.PaddingProperty, Value = new Thickness({cardPadX.ToString(CultureInfo.InvariantCulture)}, {cardPadY.ToString(CultureInfo.InvariantCulture)}) }});");
        sb.AppendLine("        Add(style_card);");
        sb.AppendLine();

        sb.AppendLine("        var style_shadow = new Style(typeof(VisualElement)) { Class = \"shadow\", ApplyToDerivedTypes = true };");
        sb.AppendLine("        style_shadow.Setters.Add(new Setter { Property = VisualElement.ShadowProperty, Value = new Shadow { Brush = Colors.Black, Offset = new Point(0, 8), Radius = 16, Opacity = 0.15f } });");
        sb.AppendLine("        Add(style_shadow);");
        sb.AppendLine();

        sb.AppendLine("        var style_shadow_sm = new Style(typeof(VisualElement)) { Class = \"shadow-sm\", ApplyToDerivedTypes = true };");
        sb.AppendLine("        style_shadow_sm.Setters.Add(new Setter { Property = VisualElement.ShadowProperty, Value = new Shadow { Brush = Colors.Black, Offset = new Point(0, 2), Radius = 4, Opacity = 0.075f } });");
        sb.AppendLine("        Add(style_shadow_sm);");
        sb.AppendLine();

        sb.AppendLine("        var style_shadow_lg = new Style(typeof(VisualElement)) { Class = \"shadow-lg\", ApplyToDerivedTypes = true };");
        sb.AppendLine("        style_shadow_lg.Setters.Add(new Setter { Property = VisualElement.ShadowProperty, Value = new Shadow { Brush = Colors.Black, Offset = new Point(0, 16), Radius = 48, Opacity = 0.175f } });");
        sb.AppendLine("        Add(style_shadow_lg);");
        sb.AppendLine();

        var badgeCornerRadius = (int)Math.Round(CssToDevicePixels(data.BorderRadius ?? "0.375rem"));
        sb.AppendLine("        var style_badge = new Style(typeof(Border)) { Class = \"badge\" };");
        sb.AppendLine("        style_badge.Setters.Add(new Setter { Property = Border.StrokeThicknessProperty, Value = 0d });");
        sb.AppendLine($"        style_badge.Setters.Add(new Setter {{ Property = Border.StrokeShapeProperty, Value = new RoundRectangle {{ CornerRadius = {badgeCornerRadius} }} }});");
        sb.AppendLine("        style_badge.Setters.Add(new Setter { Property = Border.PaddingProperty, Value = new Thickness(8, 4) });");
        sb.AppendLine("        Add(style_badge);");
        sb.AppendLine();
    }

    private void EmitCsColorVariantCards(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        var crVal = data.CardRules.BorderRadius ?? data.BorderRadius ?? "0.375rem";
        if (crVal.Contains("var(")) crVal = data.BorderRadius ?? "0.375rem";
        var cr = (int)Math.Round(CssToDevicePixels(crVal));
        var padX = data.CardSpacerX != null ? CssToDevicePixels(data.CardSpacerX) : 16;
        var padY = data.CardSpacerY != null ? CssToDevicePixels(data.CardSpacerY) : 16;

        sb.AppendLine("        // Color variant cards");
        var variants4 = new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" };
        foreach (var v in variants4)
        {
            var pascal = ToPascalCase(v);
            var varName = $"style_textbg_{v}";
            sb.AppendLine($"        var {varName} = new Style(typeof(Border)) {{ Class = \"text-bg-{v}\" }};");
            sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Border.BackgroundProperty, Value = DR(\"{pascal}\") }});");
            sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Border.StrokeThicknessProperty, Value = 0d }});");
            sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Border.StrokeShapeProperty, Value = new RoundRectangle {{ CornerRadius = {cr} }} }});");
            sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Border.PaddingProperty, Value = new Thickness({padX.ToString(CultureInfo.InvariantCulture)}, {padY.ToString(CultureInfo.InvariantCulture)}) }});");
            sb.AppendLine($"        Add({varName});");
        }
        sb.AppendLine();
    }

    private void EmitCsBadgeBackgrounds(StringBuilder sb)
    {
        sb.AppendLine("        // Badge backgrounds");
        var variants5 = new[] { "primary", "secondary", "success", "danger", "warning", "info" };
        foreach (var v in variants5)
        {
            var pascal = ToPascalCase(v);
            var varName = $"style_bg_{v}";
            sb.AppendLine($"        var {varName} = new Style(typeof(Border)) {{ Class = \"bg-{v}\" }};");
            sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Border.BackgroundProperty, Value = DR(\"{pascal}\") }});");
            sb.AppendLine($"        Add({varName});");
        }
        sb.AppendLine();
    }

    private void EmitCsOnColorLabels(StringBuilder sb)
    {
        sb.AppendLine("        // On-color label styles");
        var variants6 = new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" };
        foreach (var v in variants6)
        {
            var pascal = ToPascalCase(v);
            var varName = $"style_on_{v}";
            sb.AppendLine($"        var {varName} = new Style(typeof(Label)) {{ Class = \"on-{v}\" }};");
            sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Label.TextColorProperty, Value = DR(\"On{pascal}\") }});");
            sb.AppendLine($"        Add({varName});");
        }
        sb.AppendLine();
    }

    private void EmitCsHeadingStyles(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        sb.AppendLine("        // Heading styles");
        for (int i = 1; i <= 6; i++)
        {
            var colorKey = i == 1 ? "HeadingColor" : "HeadingColorAlt";
            var varName = $"style_h{i}";
            sb.AppendLine($"        var {varName} = new Style(typeof(Label)) {{ Class = \"h{i}\" }};");
            sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Label.FontSizeProperty, Value = DR(\"FontSizeH{i}\") }});");
            sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Label.FontAttributesProperty, Value = FontAttributes.Bold }});");
            sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Label.TextColorProperty, Value = DR(\"{colorKey}\") }});");
            sb.AppendLine($"        Add({varName});");
            sb.AppendLine();
        }
    }

    private void EmitCsTextStyles(StringBuilder sb)
    {
        sb.AppendLine("        // Text styles");
        sb.AppendLine("        var style_lead = new Style(typeof(Label)) { Class = \"lead\" };");
        sb.AppendLine("        style_lead.Setters.Add(new Setter { Property = Label.FontSizeProperty, Value = DR(\"FontSizeLead\") });");
        sb.AppendLine("        style_lead.Setters.Add(new Setter { Property = Label.LineHeightProperty, Value = 1.5 });");
        sb.AppendLine("        Add(style_lead);");
        sb.AppendLine("        var style_small = new Style(typeof(Label)) { Class = \"small\" };");
        sb.AppendLine("        style_small.Setters.Add(new Setter { Property = Label.FontSizeProperty, Value = DR(\"FontSizeSm\") });");
        sb.AppendLine("        Add(style_small);");
        sb.AppendLine("        var style_form_text = new Style(typeof(Label)) { Class = \"form-text\" };");
        sb.AppendLine("        style_form_text.Setters.Add(new Setter { Property = Label.FontSizeProperty, Value = DR(\"FontSizeSm\") });");
        sb.AppendLine("        style_form_text.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = DR(\"Muted\") });");
        sb.AppendLine("        Add(style_form_text);");
        sb.AppendLine("        var style_form_check_label = new Style(typeof(Label)) { Class = \"form-check-label\" };");
        sb.AppendLine("        style_form_check_label.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = DR(\"OnBackground\") });");
        sb.AppendLine("        Add(style_form_check_label);");
        sb.AppendLine("        var style_mark = new Style(typeof(Label)) { Class = \"mark\" };");
        sb.AppendLine("        style_mark.Setters.Add(new Setter { Property = Label.BackgroundProperty, Value = Color.FromArgb(\"#fcf8e3\") });");
        sb.AppendLine("        style_mark.Setters.Add(new Setter { Property = Label.PaddingProperty, Value = new Thickness(4, 2) });");
        sb.AppendLine("        Add(style_mark);");
        var variants = new[] { "primary", "secondary", "success", "danger", "warning", "info" };
        foreach (var v in variants)
        {
            var pascal = ToPascalCase(v);
            sb.AppendLine($"        var style_text_{v} = new Style(typeof(Label)) {{ Class = \"text-{v}\" }};");
            sb.AppendLine($"        style_text_{v}.Setters.Add(new Setter {{ Property = Label.TextColorProperty, Value = DR(\"{pascal}\") }});");
            sb.AppendLine($"        Add(style_text_{v});");
        }
        sb.AppendLine("        var style_text_muted = new Style(typeof(Label)) { Class = \"text-muted\" };");
        sb.AppendLine("        style_text_muted.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = DR(\"Muted\") });");
        sb.AppendLine("        Add(style_text_muted);");
        sb.AppendLine("        var style_text_dark = new Style(typeof(Label)) { Class = \"text-dark\" };");
        sb.AppendLine("        style_text_dark.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = DR(\"Dark\") });");
        sb.AppendLine("        Add(style_text_dark);");
        sb.AppendLine("        var style_text_white = new Style(typeof(Label)) { Class = \"text-white\" };");
        sb.AppendLine("        style_text_white.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = Colors.White });");
        sb.AppendLine("        Add(style_text_white);");
        sb.AppendLine("        var style_text_center = new Style(typeof(Label)) { Class = \"text-center\" };");
        sb.AppendLine("        style_text_center.Setters.Add(new Setter { Property = Label.HorizontalTextAlignmentProperty, Value = TextAlignment.Center });");
        sb.AppendLine("        Add(style_text_center);");
        sb.AppendLine();
    }

    private void EmitCsProgressBarStyles(StringBuilder sb)
    {
        sb.AppendLine("        // Progress bar variants");
        var variants7 = new[] { "primary", "success", "danger" };
        foreach (var v in variants7)
        {
            var pascal = ToPascalCase(v);
            var varName = $"style_progress_{v}";
            sb.AppendLine($"        var {varName} = new Style(typeof(ProgressBar)) {{ Class = \"progress-{v}\" }};");
            sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = ProgressBar.ProgressColorProperty, Value = DR(\"{pascal}\") }});");
            sb.AppendLine($"        Add({varName});");
        }
        sb.AppendLine();
    }

    private void EmitCsGradientSetter(StringBuilder sb, string varName, string cssGradient)
    {
        var stops = ParseGradientStops(cssGradient);
        if (stops.Count >= 2)
        {
            sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.BackgroundProperty, Value = new LinearGradientBrush");
            sb.AppendLine("        {");
            sb.AppendLine("            StartPoint = new Point(0, 0), EndPoint = new Point(0, 1),");
            sb.AppendLine("            GradientStops = new GradientStopCollection");
            sb.AppendLine("            {");
            foreach (var (color, offset) in stops)
            {
                sb.AppendLine($"                new GradientStop(Color.FromArgb(\"{color}\"), {offset.ToString("F2", CultureInfo.InvariantCulture)}f),");
            }
            sb.AppendLine("            }");
            sb.AppendLine("        }});");
        }
    }

    private void EmitCsShadowSetter(StringBuilder sb, string varName, string color)
    {
        sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.ShadowProperty, Value = new Shadow {{ Brush = Color.FromArgb(\"{NormalizeHexColor(color)}\"), Offset = new Point(0, 0), Radius = 8, Opacity = 0.6f }} }});");
    }

    /// <summary>
    /// Parses a CSS box-shadow value like "3px 3px 0 0 #000" and emits a MAUI Shadow setter.
    /// </summary>
    private void EmitCsBoxShadowSetter(StringBuilder sb, string varName, string cssBoxShadow, string fallbackColor)
    {
        // Parse: offsetX offsetY [blurRadius] [spreadRadius] [color]
        var parts = cssBoxShadow.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        double offsetX = 0, offsetY = 0, blur = 0;
        var color = fallbackColor;

        if (parts.Length >= 2)
        {
            offsetX = CssToDevicePixels(parts[0]);
            offsetY = CssToDevicePixels(parts[1]);
        }
        if (parts.Length >= 3)
            blur = CssToDevicePixels(parts[2]);
        // parts[3] is spread (not supported in MAUI Shadow)
        // Find color (last part that starts with # or is a color name)
        for (int i = parts.Length - 1; i >= 2; i--)
        {
            if (parts[i].StartsWith("#") || parts[i].StartsWith("rgb"))
            {
                color = parts[i].StartsWith("#") ? NormalizeHexColor(parts[i]) : fallbackColor;
                break;
            }
        }

        sb.AppendLine($"        {varName}.Setters.Add(new Setter {{ Property = Button.ShadowProperty, Value = new Shadow {{ Brush = Color.FromArgb(\"{color}\"), Offset = new Point({offsetX.ToString("F0", CultureInfo.InvariantCulture)}, {offsetY.ToString("F0", CultureInfo.InvariantCulture)}), Radius = {blur.ToString("F0", CultureInfo.InvariantCulture)}, Opacity = 0.8f }} }});");
    }

    #endregion

    /// <summary>
    /// Returns a list of font warnings for fonts that need to be provided by the developer.
    /// </summary>
    public List<FontWarning> GetFontWarnings(Parsing.BootstrapThemeData data)
    {
        var warnings = new List<FontWarning>();

        if (data.FontFamily != null)
        {
            if (!FontMapping.ContainsKey(data.FontFamily))
            {
                // Unknown font — needs developer action
                var safeName = data.FontFamily.Replace(" ", "").Replace("-", "");
                warnings.Add(new FontWarning
                {
                    CssFontName = data.FontFamily,
                    Message = $"Theme '{data.Name}' requires font '{data.FontFamily}' which is not available as a system font. " +
                              $"Download the font file and add it to your project:",
                    SuggestedCode = $@"// In MauiProgram.cs, add to ConfigureFonts:
.ConfigureFonts(fonts =>
{{
    fonts.AddFont(""{data.FontFamily.ToLowerInvariant().Replace(" ", "-")}.ttf"", ""{data.FontFamily}"");
}})"
                });
            }
        }

        return warnings;
    }

    #region Emit Helpers

    private void EmitSection(StringBuilder sb, string title)
    {
        sb.AppendLine($"    <!-- ═══ {title} ═══ -->");
    }

    private void EmitColor(StringBuilder sb, string key, string value)
    {
        sb.AppendLine($"    <Color x:Key=\"{key}\">{value}</Color>");
    }

    private void EmitDouble(StringBuilder sb, string key, double value)
    {
        sb.AppendLine($"    <x:Double x:Key=\"{key}\">{value.ToString(CultureInfo.InvariantCulture)}</x:Double>");
    }

    private void EmitString(StringBuilder sb, string key, string value)
    {
        sb.AppendLine($"    <x:String x:Key=\"{key}\">{value}</x:String>");
    }

    private void EmitThickness(StringBuilder sb, string key, string value)
    {
        sb.AppendLine($"    <Thickness x:Key=\"{key}\">{value}</Thickness>");
    }

    private void EmitOnColors(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        var variants = new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" };
        foreach (var v in variants)
        {
            var onColor = data.OnColors.TryGetValue(v, out var c) ? NormalizeHexColor(c) : (v == "warning" || v == "info" || v == "light" ? "#000000" : "White");
            EmitColor(sb, $"On{ToPascalCase(v)}", onColor);
        }
    }

    private void EmitSurfaceColors(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        var bodyBg = data.BodyBackground ?? "#ffffff";
        var bodyColor = data.BodyColor ?? "#212529";
        var borderColor = data.BorderColor ?? "#dee2e6";

        EmitColor(sb, "Background", bodyBg);
        // Surface = card bg or secondary-bg or same as body-bg
        var surface = data.CardRules.Background ?? data.BodyBackground ?? "#ffffff";
        // Resolve var() references
        if (surface.Contains("var("))
            surface = bodyBg;
        EmitColor(sb, "Surface", surface);
        EmitColor(sb, "OnBackground", bodyColor);
        EmitColor(sb, "OnSurface", bodyColor);

        // HeadingColor: "inherit" means use body color
        var headingColor = data.HeadingColor;
        if (headingColor == null || headingColor == "inherit")
            headingColor = bodyColor;
        EmitColor(sb, "HeadingColor", headingColor);
        EmitColor(sb, "HeadingColorAlt", headingColor);

        EmitColor(sb, "Outline", borderColor);
        // OutlineVariant — slightly different from Outline
        var outlineVariant = data.LightVariables.TryGetValue("--bs-border-color-translucent", out var ov)
            ? NormalizeHexColor(ov) : AdjustColorBrightness(borderColor, -10);
        EmitColor(sb, "OutlineVariant", outlineVariant);

        // Muted — use secondary color or gray-600
        var muted = data.Secondary ?? "#6c757d";
        EmitColor(sb, "Muted", muted);
    }

    private void EmitDarkModeColors(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        if (data.DarkBodyBackground != null)
            EmitColor(sb, "DarkBackground", data.DarkBodyBackground);
        if (data.DarkBodyColor != null)
            EmitColor(sb, "DarkOnBackground", data.DarkBodyColor);
        if (data.DarkSecondaryBg != null)
            EmitColor(sb, "DarkSurface", data.DarkSecondaryBg);
        if (data.DarkBorderColor != null)
            EmitColor(sb, "DarkOutline", data.DarkBorderColor);

        // Dark button overrides
        foreach (var kvp in data.DarkButtonRules)
        {
            if (kvp.Value.Background != null)
                EmitColor(sb, $"Dark{ToPascalCase(kvp.Key)}ButtonBg", kvp.Value.Background);
            if (kvp.Value.Color != null)
                EmitColor(sb, $"Dark{ToPascalCase(kvp.Key)}ButtonColor", kvp.Value.Color);
        }
    }

    private void EmitGrayScale(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        var v = data.LightVariables;
        EmitColor(sb, "Gray100", GetColorOrDefault(v, "--bs-gray-100", "#f8f9fa"));
        EmitColor(sb, "Gray200", GetColorOrDefault(v, "--bs-gray-200", "#e9ecef"));
        EmitColor(sb, "Gray300", GetColorOrDefault(v, "--bs-gray-300", "#dee2e6"));
        EmitColor(sb, "Gray400", GetColorOrDefault(v, "--bs-gray-400", "#ced4da"));
        EmitColor(sb, "Gray500", GetColorOrDefault(v, "--bs-gray-500", "#adb5bd"));
        EmitColor(sb, "Gray600", GetColorOrDefault(v, "--bs-gray-600", "#6c757d"));
        EmitColor(sb, "Gray700", GetColorOrDefault(v, "--bs-gray-700", "#495057"));
        EmitColor(sb, "Gray800", GetColorOrDefault(v, "--bs-gray-800", "#343a40"));
        EmitColor(sb, "Gray900", GetColorOrDefault(v, "--bs-gray-900", "#212529"));
    }

    private void EmitTypography(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        var fontFamily = "";
        if (data.FontFamily != null)
        {
            fontFamily = FontMapping.TryGetValue(data.FontFamily, out var mapped)
                ? mapped : data.FontFamily;
        }
        EmitString(sb, "FontFamily", fontFamily);
        var baseFontSize = data.BodyFontSize != null ? CssToDevicePixels(data.BodyFontSize) : 16;
        EmitDouble(sb, "FontSizeBase", baseFontSize);
        EmitDouble(sb, "FontSizeSm", data.BtnFontSizeSm != null ? CssToDevicePixels(data.BtnFontSizeSm) : baseFontSize * 0.875);
        EmitDouble(sb, "FontSizeLg", data.BtnFontSizeLg != null ? CssToDevicePixels(data.BtnFontSizeLg) : baseFontSize * 1.25);
        EmitDouble(sb, "FontSizeLead", baseFontSize * 1.25);
        EmitDouble(sb, "FontSizeH1", data.FontSizeH1 != null ? CssToDevicePixels(data.FontSizeH1) : baseFontSize * 2.5);
        EmitDouble(sb, "FontSizeH2", data.FontSizeH2 != null ? CssToDevicePixels(data.FontSizeH2) : baseFontSize * 2);
        EmitDouble(sb, "FontSizeH3", data.FontSizeH3 != null ? CssToDevicePixels(data.FontSizeH3) : baseFontSize * 1.75);
        EmitDouble(sb, "FontSizeH4", data.FontSizeH4 != null ? CssToDevicePixels(data.FontSizeH4) : baseFontSize * 1.5);
        EmitDouble(sb, "FontSizeH5", data.FontSizeH5 != null ? CssToDevicePixels(data.FontSizeH5) : baseFontSize * 1.25);
        EmitDouble(sb, "FontSizeH6", data.FontSizeH6 != null ? CssToDevicePixels(data.FontSizeH6) : baseFontSize);
    }

    private void EmitSpacing(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        EmitDouble(sb, "CornerRadius", CssToDevicePixels(data.BorderRadius ?? "0.375rem"));
        EmitDouble(sb, "CornerRadiusSm", CssToDevicePixels(data.BorderRadiusSm ?? "0.25rem"));
        EmitDouble(sb, "CornerRadiusLg", CssToDevicePixels(data.BorderRadiusLg ?? "0.5rem"));
        var baseFontSize = data.BodyFontSize != null ? CssToDevicePixels(data.BodyFontSize) : 16;
        var btnPadX = data.BtnPaddingX != null ? CssToDevicePixels(data.BtnPaddingX) : 12;
        var btnPadY = data.BtnPaddingY != null ? CssToDevicePixels(data.BtnPaddingY) : 6;
        var btnPadXSm = data.BtnPaddingXSm != null ? CssToDevicePixels(data.BtnPaddingXSm) : 8;
        var btnPadYSm = data.BtnPaddingYSm != null ? CssToDevicePixels(data.BtnPaddingYSm) : 4;
        var btnPadXLg = data.BtnPaddingXLg != null ? CssToDevicePixels(data.BtnPaddingXLg) : 16;
        var btnPadYLg = data.BtnPaddingYLg != null ? CssToDevicePixels(data.BtnPaddingYLg) : 8;
        var effectiveBtnBorderWidth2 = data.BtnBorderWidth != null && !data.BtnBorderWidth.StartsWith("var(")
            ? data.BtnBorderWidth : null;
        var baseBorderW = CssToDevicePixels(effectiveBtnBorderWidth2 ?? data.BorderWidth ?? "1px");
        var borderW = baseBorderW;
        var buttonLineHeight = ParseLineHeightFactor(
            data.LightVariables.TryGetValue("--bs-btn-line-height", out var btnLh) ? btnLh
            : (data.LightVariables.TryGetValue("--bs-body-line-height", out var bodyLh) ? bodyLh : null),
            1.5);
        var inputFontSize = data.FormControlRule.FontSize != null ? CssToDevicePixels(data.FormControlRule.FontSize) : baseFontSize;
        var inputLineHeight = ParseLineHeightFactor(data.FormControlRule.LineHeight, 1.5);
        var inputBorderW = data.FormControlRule.BorderWidth != null ? CssToDevicePixels(data.FormControlRule.BorderWidth) : baseBorderW;
        var (inputPadX, inputPadY) = ResolvePadding(data.FormControlRule.Padding, btnPadX, btnPadY);

        var buttonHeight = btnPadY * 2 + (baseFontSize * buttonLineHeight) + borderW * 2;
        var buttonHeightSm = btnPadYSm * 2 + ((data.BtnFontSizeSm != null ? CssToDevicePixels(data.BtnFontSizeSm) : baseFontSize * 0.875) * buttonLineHeight) + borderW * 2;
        var buttonHeightLg = btnPadYLg * 2 + ((data.BtnFontSizeLg != null ? CssToDevicePixels(data.BtnFontSizeLg) : baseFontSize * 1.25) * buttonLineHeight) + borderW * 2;
        EmitDouble(sb, "CornerRadiusPill", Math.Ceiling(buttonHeight / 2));
        EmitDouble(sb, "BorderWidth", baseBorderW);
        EmitDouble(sb, "ButtonHeight", buttonHeight);
        EmitDouble(sb, "ButtonHeightSm", buttonHeightSm);
        EmitDouble(sb, "ButtonHeightLg", buttonHeightLg);
        EmitDouble(sb, "InputHeight", inputPadY * 2 + (inputFontSize * inputLineHeight) + inputBorderW * 2);
        EmitDouble(sb, "InputHeightSm", btnPadYSm * 2 + ((baseFontSize * 0.875) * inputLineHeight) + inputBorderW * 2);
        EmitDouble(sb, "InputHeightLg", btnPadYLg * 2 + ((baseFontSize * 1.25) * inputLineHeight) + inputBorderW * 2);
        EmitThickness(sb, "ButtonPadding", $"{btnPadX.ToString(CultureInfo.InvariantCulture)},{btnPadY.ToString(CultureInfo.InvariantCulture)}");
        EmitThickness(sb, "ButtonPaddingSm", $"{btnPadXSm.ToString(CultureInfo.InvariantCulture)},{btnPadYSm.ToString(CultureInfo.InvariantCulture)}");
        EmitThickness(sb, "ButtonPaddingLg", $"{btnPadXLg.ToString(CultureInfo.InvariantCulture)},{btnPadYLg.ToString(CultureInfo.InvariantCulture)}");
        EmitThickness(sb, "InputPadding", $"{inputPadX.ToString(CultureInfo.InvariantCulture)},{inputPadY.ToString(CultureInfo.InvariantCulture)}");

        // Input colors
        var inputBg = IsDarkTheme(data)
            ? (data.DarkSecondaryBg ?? data.DarkBodyBackground ?? data.FormControlRule.Background ?? "#303030")
            : (data.FormControlRule.Background ?? data.BodyBackground ?? "#ffffff");
        var inputText = IsDarkTheme(data)
            ? (data.FormControlRule.Color ?? data.DarkBodyColor ?? "#ffffff")
            : (data.FormControlRule.Color ?? data.BodyColor ?? "#212529");
        EmitColor(sb, "ProgressBackground", IsDarkTheme(data) ? "#464545" : "#e9ecef");
        EmitColor(sb, "InputBackground", inputBg);
        EmitColor(sb, "InputText", inputText);

        // PlaceholderColor for dark themes
        var placeholderColor = data.FormControlRule.PlaceholderColor ?? (IsDarkTheme(data) ? "#999999" : "#6c757d");
        EmitColor(sb, "PlaceholderColor", placeholderColor);
    }

    private void EmitImplicitStyles(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        var hasGradient = data.HasGradientButtons;
        var hasBorder = data.ButtonRules.TryGetValue("primary", out var primaryBtn) && primaryBtn.BorderColor != null;

        // Button implicit style
        sb.AppendLine("    <Style TargetType=\"Button\">");
        if (hasGradient && data.ButtonRules.TryGetValue("primary", out var pBtn) && pBtn.Gradient != null)
        {
            EmitGradientBackground(sb, pBtn.Gradient);
        }
        else
        {
            sb.AppendLine("        <Setter Property=\"Background\" Value=\"{DynamicResource Primary}\"/>");
        }
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource OnPrimary}\"/>");
        sb.AppendLine("        <Setter Property=\"CornerRadius\" Value=\"{DynamicResource CornerRadius}\"/>");
        sb.AppendLine("        <Setter Property=\"FontFamily\" Value=\"{DynamicResource FontFamily}\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeBase}\"/>");
        sb.AppendLine("        <Setter Property=\"Padding\" Value=\"{DynamicResource ButtonPadding}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource ButtonHeight}\"/>");
        if (hasBorder)
        {
            sb.AppendLine("        <Setter Property=\"BorderWidth\" Value=\"{DynamicResource BorderWidth}\"/>");
            sb.AppendLine("        <Setter Property=\"BorderColor\" Value=\"{DynamicResource Dark}\"/>");
        }
        else
        {
            sb.AppendLine("        <Setter Property=\"BorderWidth\" Value=\"0\"/>");
        }

        // Box-shadow for Vapor-style neon glow
        if (data.HasGlowButtons && data.ButtonRules.TryGetValue("primary", out var glowBtn) && glowBtn.BoxShadow != null)
        {
            EmitShadowFromBoxShadow(sb, glowBtn.BoxShadow, data.Primary ?? "#6f42c1");
        }
        else if (data.BtnBaseBoxShadow != null)
        {
            // Resting-state offset shadow (e.g., Brite's "3px 3px 0 0 #000")
            EmitShadowFromBoxShadow(sb, data.BtnBaseBoxShadow, data.Dark ?? "#000");
        }
        else
        {
            // Explicitly clear Shadow to prevent stale values persisting across theme switches.
            // Use an invisible shadow because {x:Null} doesn't reliably clear style-set properties.
            sb.AppendLine("        <Setter Property=\"Shadow\">");
            sb.AppendLine("            <Shadow Brush=\"Transparent\" Offset=\"0,0\" Radius=\"0\" Opacity=\"0\"/>");
            sb.AppendLine("        </Setter>");
        }

        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // Label
        sb.AppendLine("    <Style TargetType=\"Label\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource OnBackground}\"/>");
        sb.AppendLine("        <Setter Property=\"FontFamily\" Value=\"{DynamicResource FontFamily}\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeBase}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"form-label\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource OnBackground}\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeBase}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // Entry
        sb.AppendLine("    <Style TargetType=\"Entry\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource InputText}\"/>");
        sb.AppendLine("        <Setter Property=\"Background\" Value=\"{DynamicResource InputBackground}\"/>");
        sb.AppendLine("        <Setter Property=\"PlaceholderColor\" Value=\"{DynamicResource PlaceholderColor}\"/>");
        sb.AppendLine("        <Setter Property=\"FontFamily\" Value=\"{DynamicResource FontFamily}\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeBase}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeight}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // Entry - form-control classes
        sb.AppendLine("    <Style TargetType=\"Entry\" Class=\"form-control\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Default\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Entry\" Class=\"form-control-lg\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Large\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Entry\" Class=\"form-control-sm\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Small\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // Picker
        sb.AppendLine("    <Style TargetType=\"Picker\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource InputText}\"/>");
        sb.AppendLine("        <Setter Property=\"TitleColor\" Value=\"{DynamicResource PlaceholderColor}\"/>");
        sb.AppendLine("        <Setter Property=\"FontFamily\" Value=\"{DynamicResource FontFamily}\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeBase}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeight}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // Picker - form-select classes
        sb.AppendLine("    <Style TargetType=\"Picker\" Class=\"form-select\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Default\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Picker\" Class=\"form-select-lg\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Large\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Picker\" Class=\"form-select-sm\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Small\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // DatePicker
        sb.AppendLine("    <Style TargetType=\"DatePicker\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource InputText}\"/>");
        sb.AppendLine("        <Setter Property=\"FontFamily\" Value=\"{DynamicResource FontFamily}\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeBase}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeight}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"DatePicker\" Class=\"form-control\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Default\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeBase}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeight}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"DatePicker\" Class=\"form-control-lg\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Large\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeLg}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeightLg}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"DatePicker\" Class=\"form-control-sm\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Small\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeSm}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeightSm}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // TimePicker
        sb.AppendLine("    <Style TargetType=\"TimePicker\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource InputText}\"/>");
        sb.AppendLine("        <Setter Property=\"FontFamily\" Value=\"{DynamicResource FontFamily}\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeBase}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeight}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"TimePicker\" Class=\"form-control\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Default\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeBase}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeight}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"TimePicker\" Class=\"form-control-lg\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Large\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeLg}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeightLg}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"TimePicker\" Class=\"form-control-sm\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Small\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeSm}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeightSm}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // Editor
        sb.AppendLine("    <Style TargetType=\"Editor\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource InputText}\"/>");
        sb.AppendLine("        <Setter Property=\"Background\" Value=\"{DynamicResource InputBackground}\"/>");
        sb.AppendLine("        <Setter Property=\"FontFamily\" Value=\"{DynamicResource FontFamily}\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeBase}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeight}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Editor\" Class=\"form-control\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Default\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeBase}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeight}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Editor\" Class=\"form-control-lg\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Large\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeLg}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeightLg}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Editor\" Class=\"form-control-sm\">");
        sb.AppendLine("        <Setter Property=\"bs:Bootstrap.Size\" Value=\"Small\"/>");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeSm}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource InputHeightSm}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // Check/Radio/Switch/Range classes
        sb.AppendLine("    <Style TargetType=\"CheckBox\" Class=\"form-check-input\">");
        sb.AppendLine("        <Setter Property=\"Color\" Value=\"{DynamicResource Primary}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"RadioButton\" Class=\"form-check-input\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource OnBackground}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Switch\" Class=\"form-switch\">");
        sb.AppendLine("        <Setter Property=\"OnColor\" Value=\"{DynamicResource Primary}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Slider\" Class=\"form-range\">");
        sb.AppendLine("        <Setter Property=\"MinimumTrackColor\" Value=\"{DynamicResource Primary}\"/>");
        sb.AppendLine("        <Setter Property=\"ThumbColor\" Value=\"{DynamicResource Primary}\"/>");
        sb.AppendLine("        <Setter Property=\"MaximumTrackColor\" Value=\"{DynamicResource OutlineVariant}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // ProgressBar
        sb.AppendLine("    <Style TargetType=\"ProgressBar\">");
        sb.AppendLine("        <Setter Property=\"ProgressColor\" Value=\"{DynamicResource Primary}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"16\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // Border (default)
        sb.AppendLine("    <Style TargetType=\"Border\">");
        sb.AppendLine("        <Setter Property=\"Background\" Value=\"Transparent\"/>");
        sb.AppendLine("        <Setter Property=\"Stroke\" Value=\"Transparent\"/>");
        sb.AppendLine("        <Setter Property=\"StrokeThickness\" Value=\"0\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // ContentPage
        sb.AppendLine("    <Style TargetType=\"ContentPage\" ApplyToDerivedTypes=\"True\">");
        sb.AppendLine("        <Setter Property=\"Background\" Value=\"{DynamicResource Background}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // Shell
        sb.AppendLine("    <Style TargetType=\"Shell\" ApplyToDerivedTypes=\"True\">");
        sb.AppendLine("        <Setter Property=\"Shell.BackgroundColor\" Value=\"{DynamicResource Primary}\"/>");
        sb.AppendLine("        <Setter Property=\"Shell.ForegroundColor\" Value=\"{DynamicResource OnPrimary}\"/>");
        sb.AppendLine("        <Setter Property=\"Shell.TitleColor\" Value=\"{DynamicResource OnPrimary}\"/>");
        sb.AppendLine("        <Setter Property=\"Shell.TabBarBackgroundColor\" Value=\"{DynamicResource Background}\"/>");
        sb.AppendLine("        <Setter Property=\"Shell.TabBarForegroundColor\" Value=\"{DynamicResource Primary}\"/>");
        sb.AppendLine("        <Setter Property=\"Shell.TabBarUnselectedColor\" Value=\"{DynamicResource Gray500}\"/>");
        sb.AppendLine("        <Setter Property=\"FlyoutBackgroundColor\" Value=\"{DynamicResource Background}\"/>");
        sb.AppendLine("    </Style>");
    }

    private void EmitButtonVariantStyles(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        var variants = new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" };
        foreach (var v in variants)
        {
            var pascal = ToPascalCase(v);
            sb.AppendLine($"    <Style TargetType=\"Button\" Class=\"btn-{v}\">");

            // Check for gradient
            if (data.ButtonRules.TryGetValue(v, out var rule) && rule.Gradient != null)
            {
                EmitGradientBackground(sb, rule.Gradient);
            }
            else
            {
                sb.AppendLine($"        <Setter Property=\"Background\" Value=\"{{DynamicResource {pascal}}}\"/>");
            }
            sb.AppendLine($"        <Setter Property=\"TextColor\" Value=\"{{DynamicResource On{pascal}}}\"/>");

            // Box-shadow for glow effects
            if (data.ButtonRules.TryGetValue(v, out var glowRule) && glowRule.BoxShadow != null)
            {
                var color = v switch
                {
                    "primary" => data.Primary,
                    "secondary" => data.Secondary,
                    "success" => data.Success,
                    "danger" => data.Danger,
                    "warning" => data.Warning,
                    "info" => data.Info,
                    _ => null
                };
                if (color != null)
                    EmitShadowFromBoxShadow(sb, glowRule.BoxShadow, color);
            }

            sb.AppendLine("    </Style>");
            sb.AppendLine();
        }
    }

    private void EmitOutlineButtonStyles(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        var variants = new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" };
        foreach (var v in variants)
        {
            var pascal = ToPascalCase(v);
            data.ButtonRules.TryGetValue($"outline-{v}", out var rule);
            sb.AppendLine($"    <Style TargetType=\"Button\" Class=\"btn-outline-{v}\">");
            if (!string.IsNullOrWhiteSpace(rule?.Background))
                sb.AppendLine($"        <Setter Property=\"Background\" Value=\"{NormalizeHexColor(rule.Background)}\"/>");
            else
                sb.AppendLine("        <Setter Property=\"Background\" Value=\"Transparent\"/>");

            if (!string.IsNullOrWhiteSpace(rule?.Color))
                sb.AppendLine($"        <Setter Property=\"TextColor\" Value=\"{NormalizeHexColor(rule.Color)}\"/>");
            else
                sb.AppendLine($"        <Setter Property=\"TextColor\" Value=\"{{DynamicResource {pascal}}}\"/>");

            if (!string.IsNullOrWhiteSpace(rule?.BorderColor))
                sb.AppendLine($"        <Setter Property=\"BorderColor\" Value=\"{NormalizeHexColor(rule.BorderColor)}\"/>");
            else
                sb.AppendLine($"        <Setter Property=\"BorderColor\" Value=\"{{DynamicResource {pascal}}}\"/>");

            if (!string.IsNullOrWhiteSpace(rule?.BorderWidth))
                sb.AppendLine($"        <Setter Property=\"BorderWidth\" Value=\"{CssToDevicePixels(rule.BorderWidth).ToString(CultureInfo.InvariantCulture)}\"/>");
            else
                sb.AppendLine("        <Setter Property=\"BorderWidth\" Value=\"{DynamicResource BorderWidth}\"/>");
            sb.AppendLine("    </Style>");
            sb.AppendLine();
        }
    }

    private void EmitSizeVariants(StringBuilder sb)
    {
        sb.AppendLine("    <Style TargetType=\"Button\" Class=\"btn-lg\">");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeLg}\"/>");
        sb.AppendLine("        <Setter Property=\"Padding\" Value=\"{DynamicResource ButtonPaddingLg}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource ButtonHeightLg}\"/>");
        sb.AppendLine("        <Setter Property=\"CornerRadius\" Value=\"{DynamicResource CornerRadiusLg}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();
        sb.AppendLine("    <Style TargetType=\"Button\" Class=\"btn-sm\">");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeSm}\"/>");
        sb.AppendLine("        <Setter Property=\"Padding\" Value=\"{DynamicResource ButtonPaddingSm}\"/>");
        sb.AppendLine("        <Setter Property=\"MinimumHeightRequest\" Value=\"{DynamicResource ButtonHeightSm}\"/>");
        sb.AppendLine("        <Setter Property=\"CornerRadius\" Value=\"{DynamicResource CornerRadiusSm}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();
        sb.AppendLine("    <Style TargetType=\"Button\" Class=\"btn-pill\">");
        sb.AppendLine("        <Setter Property=\"CornerRadius\" Value=\"{DynamicResource CornerRadiusPill}\"/>");
        sb.AppendLine("    </Style>");
    }

    private void EmitComponentStyles(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        // card
        sb.AppendLine("    <Style TargetType=\"Border\" Class=\"card\">");
        sb.AppendLine("        <Setter Property=\"Background\" Value=\"{DynamicResource Surface}\"/>");
        sb.AppendLine("        <Setter Property=\"Stroke\" Value=\"{DynamicResource Outline}\"/>");
        sb.AppendLine("        <Setter Property=\"StrokeThickness\" Value=\"{DynamicResource BorderWidth}\"/>");
        sb.AppendLine("        <Setter Property=\"StrokeShape\" Value=\"RoundRectangle 6\"/>");
        sb.AppendLine("        <Setter Property=\"Padding\" Value=\"16\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // shadow
        sb.AppendLine("    <Style TargetType=\"Border\" Class=\"shadow\">");
        sb.AppendLine("        <Setter Property=\"Shadow\">");
        sb.AppendLine("            <Setter.Value>");
        sb.AppendLine("                <Shadow Brush=\"Black\" Offset=\"0,2\" Radius=\"8\" Opacity=\"0.15\" />");
        sb.AppendLine("            </Setter.Value>");
        sb.AppendLine("        </Setter>");
        sb.AppendLine("    </Style>");
        sb.AppendLine();

        // badge
        sb.AppendLine("    <Style TargetType=\"Border\" Class=\"badge\">");
        sb.AppendLine("        <Setter Property=\"StrokeThickness\" Value=\"0\"/>");
        sb.AppendLine("        <Setter Property=\"StrokeShape\" Value=\"RoundRectangle 50\"/>");
        sb.AppendLine("        <Setter Property=\"Padding\" Value=\"8,4\"/>");
        sb.AppendLine("    </Style>");
    }

    private void EmitColorVariantCards(StringBuilder sb)
    {
        var variants = new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" };
        foreach (var v in variants)
        {
            var pascal = ToPascalCase(v);
            sb.AppendLine($"    <Style TargetType=\"Border\" Class=\"text-bg-{v}\">");
            sb.AppendLine($"        <Setter Property=\"Background\" Value=\"{{DynamicResource {pascal}}}\"/>");
            sb.AppendLine("        <Setter Property=\"StrokeThickness\" Value=\"0\"/>");
            sb.AppendLine("        <Setter Property=\"StrokeShape\" Value=\"RoundRectangle 6\"/>");
            sb.AppendLine("        <Setter Property=\"Padding\" Value=\"16\"/>");
            sb.AppendLine("    </Style>");
        }
    }

    private void EmitBadgeBackgrounds(StringBuilder sb)
    {
        var variants = new[] { "primary", "secondary", "success", "danger", "warning", "info" };
        foreach (var v in variants)
        {
            var pascal = ToPascalCase(v);
            sb.AppendLine($"    <Style TargetType=\"Border\" Class=\"bg-{v}\">");
            sb.AppendLine($"        <Setter Property=\"Background\" Value=\"{{DynamicResource {pascal}}}\"/>");
            sb.AppendLine("    </Style>");
        }
    }

    private void EmitOnColorLabels(StringBuilder sb)
    {
        var variants = new[] { "primary", "secondary", "success", "danger", "warning", "info", "light", "dark" };
        foreach (var v in variants)
        {
            var pascal = ToPascalCase(v);
            sb.AppendLine($"    <Style TargetType=\"Label\" Class=\"on-{v}\">");
            sb.AppendLine($"        <Setter Property=\"TextColor\" Value=\"{{DynamicResource On{pascal}}}\"/>");
            sb.AppendLine("    </Style>");
        }
    }

    private void EmitHeadingStyles(StringBuilder sb, Parsing.BootstrapThemeData data)
    {
        for (int i = 1; i <= 6; i++)
        {
            var colorKey = i == 1 ? "HeadingColor" : "HeadingColorAlt";
            sb.AppendLine($"    <Style TargetType=\"Label\" Class=\"h{i}\">");
            sb.AppendLine($"        <Setter Property=\"FontSize\" Value=\"{{DynamicResource FontSizeH{i}}}\"/>");
            sb.AppendLine("        <Setter Property=\"FontAttributes\" Value=\"Bold\"/>");
            sb.AppendLine($"        <Setter Property=\"TextColor\" Value=\"{{DynamicResource {colorKey}}}\"/>");
            sb.AppendLine("    </Style>");
            sb.AppendLine();
        }
    }

    private void EmitTextStyles(StringBuilder sb)
    {
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"lead\">");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeLead}\"/>");
        sb.AppendLine("        <Setter Property=\"LineHeight\" Value=\"1.5\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"small\">");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeSm}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"form-text\">");
        sb.AppendLine("        <Setter Property=\"FontSize\" Value=\"{DynamicResource FontSizeSm}\"/>");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource Muted}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"form-check-label\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource OnBackground}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"mark\">");
        sb.AppendLine("        <Setter Property=\"Background\" Value=\"#fcf8e3\"/>");
        sb.AppendLine("        <Setter Property=\"Padding\" Value=\"4,2\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"text-primary\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource Primary}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"text-secondary\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource Secondary}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"text-success\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource Success}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"text-danger\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource Danger}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"text-warning\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource Warning}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"text-info\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource Info}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"text-muted\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource Muted}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"text-dark\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"{DynamicResource Dark}\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"text-white\">");
        sb.AppendLine("        <Setter Property=\"TextColor\" Value=\"White\"/>");
        sb.AppendLine("    </Style>");
        sb.AppendLine("    <Style TargetType=\"Label\" Class=\"text-center\">");
        sb.AppendLine("        <Setter Property=\"HorizontalTextAlignment\" Value=\"Center\"/>");
        sb.AppendLine("    </Style>");
    }

    private void EmitProgressBarStyles(StringBuilder sb)
    {
        var variants = new[] { "primary", "success", "danger" };
        foreach (var v in variants)
        {
            var pascal = ToPascalCase(v);
            sb.AppendLine($"    <Style TargetType=\"ProgressBar\" Class=\"progress-{v}\">");
            sb.AppendLine($"        <Setter Property=\"ProgressColor\" Value=\"{{DynamicResource {pascal}}}\"/>");
            sb.AppendLine("    </Style>");
            sb.AppendLine();
        }
    }

    private void EmitGradientBackground(StringBuilder sb, string cssGradient)
    {
        // Parse CSS linear-gradient(#44494d, #3a3f44 20%, #2e3236)
        var stops = ParseGradientStops(cssGradient);
        if (stops.Count >= 2)
        {
            sb.AppendLine("        <Setter Property=\"Background\">");
            sb.AppendLine("            <Setter.Value>");
            sb.AppendLine("                <LinearGradientBrush StartPoint=\"0,0\" EndPoint=\"0,1\">");
            foreach (var (color, offset) in stops)
            {
                sb.AppendLine($"                    <GradientStop Color=\"{color}\" Offset=\"{offset.ToString("F2", CultureInfo.InvariantCulture)}\"/>");
            }
            sb.AppendLine("                </LinearGradientBrush>");
            sb.AppendLine("            </Setter.Value>");
            sb.AppendLine("        </Setter>");
        }
    }

    private void EmitShadowFromBoxShadow(StringBuilder sb, string cssBoxShadow, string themeColor)
    {
        // Parse simplified box-shadow: 0 0 2px rgba(r,g,b,.9), 0 0 4px rgba(r,g,b,.4), ...
        // We take the most prominent shadow (largest spread) and convert to MAUI Shadow
        // MAUI Shadow only supports a single shadow, so we pick the best one
        var color = themeColor;
        sb.AppendLine("        <Setter Property=\"Shadow\">");
        sb.AppendLine("            <Setter.Value>");
        sb.AppendLine($"                <Shadow Brush=\"{color}\" Offset=\"0,0\" Radius=\"8\" Opacity=\"0.6\" />");
        sb.AppendLine("            </Setter.Value>");
        sb.AppendLine("        </Setter>");
    }

    #endregion

    #region Utility Methods

    private List<(string Color, double Offset)> ParseGradientStops(string gradient)
    {
        var stops = new List<(string, double)>();

        // Remove direction if present (e.g., "180deg," or just starts with colors)
        var parts = SplitTopLevelByComma(gradient);

        // Filter out direction keywords
        var colorParts = new List<string>();
        foreach (var part in parts)
        {
            if (part.EndsWith("deg") || part == "to bottom" || part == "to top" || part == "to right" || part == "to left")
                continue;
            colorParts.Add(part);
        }

        if (colorParts.Count == 0) return stops;

        for (int i = 0; i < colorParts.Count; i++)
        {
            var part = colorParts[i];
            // Split color from percentage: "#44494d" or "#3a3f44 20%"
            var spaceIdx = part.LastIndexOf(' ');
            string color;
            double offset;

            if (spaceIdx > 0 && part.EndsWith("%"))
            {
                color = part[..spaceIdx].Trim();
                var pctStr = part[(spaceIdx + 1)..].TrimEnd('%');
                offset = double.TryParse(pctStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var pct)
                    ? pct / 100.0 : (double)i / Math.Max(1, colorParts.Count - 1);
            }
            else
            {
                color = part;
                offset = (double)i / Math.Max(1, colorParts.Count - 1);
            }

            stops.Add((NormalizeHexColor(color), offset));
        }

        return stops;
    }

    private static List<string> SplitTopLevelByComma(string value)
    {
        var parts = new List<string>();
        var depth = 0;
        var start = 0;

        for (var i = 0; i < value.Length; i++)
        {
            var ch = value[i];
            if (ch == '(') depth++;
            else if (ch == ')' && depth > 0) depth--;
            else if (ch == ',' && depth == 0)
            {
                parts.Add(value[start..i].Trim());
                start = i + 1;
            }
        }

        if (start < value.Length)
            parts.Add(value[start..].Trim());

        return parts;
    }

    private bool IsDarkTheme(Parsing.BootstrapThemeData data)
    {
        // Heuristic: if body-bg is dark (starts with low hex values), it's a dark theme
        var bg = data.BodyBackground ?? "#ffffff";
        if (bg.StartsWith("#") && bg.Length >= 7)
        {
            if (int.TryParse(bg.Substring(1, 2), NumberStyles.HexNumber, null, out var r) &&
                int.TryParse(bg.Substring(3, 2), NumberStyles.HexNumber, null, out var g) &&
                int.TryParse(bg.Substring(5, 2), NumberStyles.HexNumber, null, out var b))
            {
                var luminance = 0.299 * r + 0.587 * g + 0.114 * b;
                return luminance < 128;
            }
        }
        return false;
    }

    private string NormalizeHexColor(string value)
    {
        value = value.Trim();
        if (value.StartsWith("#")) return value;
        if (value.Equals("white", StringComparison.OrdinalIgnoreCase)) return "#ffffff";
        if (value.Equals("black", StringComparison.OrdinalIgnoreCase)) return "#000000";
        if (value.Equals("transparent", StringComparison.OrdinalIgnoreCase)) return "Transparent";
        if (value.StartsWith("rgba("))
        {
            // rgba(33, 37, 41, 0.75) → #BF212529 (ARGB hex)
            var inner = value.Replace("rgba(", "").TrimEnd(')');
            var parts = inner.Split(',');
            if (parts.Length == 4
                && int.TryParse(parts[0].Trim(), out var r)
                && int.TryParse(parts[1].Trim(), out var g)
                && int.TryParse(parts[2].Trim(), out var b)
                && double.TryParse(parts[3].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var a))
            {
                var alpha = (int)Math.Round(a * 255);
                return $"#{alpha:X2}{r:X2}{g:X2}{b:X2}";
            }
            return "#000000";
        }
        if (value.StartsWith("rgb("))
        {
            var inner = value.Replace("rgb(", "").TrimEnd(')');
            var parts = inner.Split(',');
            if (parts.Length == 3
                && int.TryParse(parts[0].Trim(), out var r)
                && int.TryParse(parts[1].Trim(), out var g)
                && int.TryParse(parts[2].Trim(), out var b))
            {
                return $"#{r:X2}{g:X2}{b:X2}";
            }
        }
        return value;
    }

    private string AdjustColorBrightness(string hexColor, int amount)
    {
        if (!hexColor.StartsWith("#") || hexColor.Length < 7) return hexColor;
        try
        {
            var r = Math.Clamp(int.Parse(hexColor.Substring(1, 2), NumberStyles.HexNumber) + amount, 0, 255);
            var g = Math.Clamp(int.Parse(hexColor.Substring(3, 2), NumberStyles.HexNumber) + amount, 0, 255);
            var b = Math.Clamp(int.Parse(hexColor.Substring(5, 2), NumberStyles.HexNumber) + amount, 0, 255);
            return $"#{r:X2}{g:X2}{b:X2}";
        }
        catch { return hexColor; }
    }

    private string GetColorOrDefault(Dictionary<string, string> vars, string key, string defaultValue)
    {
        return vars.TryGetValue(key, out var v) ? NormalizeHexColor(v) : defaultValue;
    }

    private double CssToDevicePixels(string cssValue)
    {
        cssValue = cssValue.Trim();
        if (cssValue.EndsWith("px"))
        {
            if (double.TryParse(cssValue.Replace("px", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out var px))
                return px;
        }
        if (cssValue.EndsWith("rem"))
        {
            if (double.TryParse(cssValue.Replace("rem", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out var rem))
                return Math.Round(rem * 16, 1); // 1rem = 16px base
        }
        if (cssValue.EndsWith("em"))
        {
            if (double.TryParse(cssValue.Replace("em", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out var em))
                return Math.Round(em * 16, 1);
        }
        if (double.TryParse(cssValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var raw))
            return raw;
        return 1;
    }

    private (double X, double Y) ResolvePadding(string? cssPadding, double fallbackX, double fallbackY)
    {
        if (string.IsNullOrWhiteSpace(cssPadding))
            return (fallbackX, fallbackY);

        var parts = cssPadding.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 0)
            return (fallbackX, fallbackY);

        return parts.Length switch
        {
            1 => (CssToDevicePixels(parts[0]), CssToDevicePixels(parts[0])),
            2 => (CssToDevicePixels(parts[1]), CssToDevicePixels(parts[0])),
            3 => (CssToDevicePixels(parts[1]), CssToDevicePixels(parts[0])),
            _ => (CssToDevicePixels(parts[1]), CssToDevicePixels(parts[0]))
        };
    }

    private double ParseLineHeightFactor(string? lineHeight, double fallback)
    {
        if (string.IsNullOrWhiteSpace(lineHeight))
            return fallback;

        var normalized = lineHeight.Trim();
        if (normalized.Equals("normal", StringComparison.OrdinalIgnoreCase))
            return fallback;

        if (double.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var factor))
            return factor;

        if (normalized.EndsWith("px", StringComparison.OrdinalIgnoreCase) ||
            normalized.EndsWith("rem", StringComparison.OrdinalIgnoreCase) ||
            normalized.EndsWith("em", StringComparison.OrdinalIgnoreCase))
        {
            var px = CssToDevicePixels(normalized);
            return px / 16d;
        }

        return fallback;
    }

    private string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var parts = input.Split('-', '_');
        return string.Concat(parts.Select(p =>
            p.Length > 0 ? char.ToUpper(p[0]) + p[1..] : ""));
    }

    #endregion
}

public class FontWarning
{
    public string CssFontName { get; set; } = "";
    public string Message { get; set; } = "";
    public string SuggestedCode { get; set; } = "";
}
