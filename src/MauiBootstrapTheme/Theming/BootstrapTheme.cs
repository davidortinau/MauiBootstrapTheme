namespace MauiBootstrapTheme.Theming;

/// <summary>
/// Represents the current Bootstrap theme with colors, spacing, and typography values.
/// Values are populated from parsed CSS or from a pre-built theme package.
/// </summary>
public class BootstrapTheme
{
    private static BootstrapTheme? _current;
    
    /// <summary>
    /// Gets the current active theme. Returns default Bootstrap 5 values if no theme is set.
    /// </summary>
    public static BootstrapTheme Current => _current ??= CreateDefault();

    /// <summary>
    /// Sets the current theme.
    /// </summary>
    public static void SetTheme(BootstrapTheme theme) => _current = theme;

    // ── Colors ──
    
    public Color Primary { get; set; } = Color.FromArgb("#0d6efd");
    public Color Secondary { get; set; } = Color.FromArgb("#6c757d");
    public Color Success { get; set; } = Color.FromArgb("#198754");
    public Color Danger { get; set; } = Color.FromArgb("#dc3545");
    public Color Warning { get; set; } = Color.FromArgb("#ffc107");
    public Color Info { get; set; } = Color.FromArgb("#0dcaf0");
    public Color Light { get; set; } = Color.FromArgb("#f8f9fa");
    public Color Dark { get; set; } = Color.FromArgb("#212529");
    
    public Color Background { get; set; } = Color.FromArgb("#ffffff");
    public Color OnBackground { get; set; } = Color.FromArgb("#212529");
    public Color Surface { get; set; } = Color.FromArgb("#ffffff");
    public Color OnSurface { get; set; } = Color.FromArgb("#212529");
    public Color Outline { get; set; } = Color.FromArgb("#dee2e6");
    public Color OutlineVariant { get; set; } = Color.FromArgb("#ced4da");
    
    public Color OnPrimary { get; set; } = Colors.White;
    public Color OnSecondary { get; set; } = Colors.White;
    public Color OnSuccess { get; set; } = Colors.White;
    public Color OnDanger { get; set; } = Colors.White;
    public Color OnWarning { get; set; } = Color.FromArgb("#212529");
    public Color OnInfo { get; set; } = Color.FromArgb("#212529");
    
    // ── Dark Mode Colors ──
    
    public Color DarkBackground { get; set; } = Color.FromArgb("#212529");
    public Color DarkOnBackground { get; set; } = Color.FromArgb("#dee2e6");
    public Color DarkSurface { get; set; } = Color.FromArgb("#343a40");
    public Color DarkOnSurface { get; set; } = Color.FromArgb("#f8f9fa");
    public Color DarkOutline { get; set; } = Color.FromArgb("#495057");

    // ── Spacing & Sizing ──
    
    public double CornerRadius { get; set; } = 6.0;
    public double CornerRadiusSm { get; set; } = 4.0;
    public double CornerRadiusLg { get; set; } = 8.0;
    public double CornerRadiusPill { get; set; } = 50.0;
    
    public double BorderWidth { get; set; } = 1.0;
    public double BorderWidthLg { get; set; } = 2.0;
    
    public double Spacer { get; set; } = 16.0;
    public double SpacerSm { get; set; } = 8.0;
    public double SpacerLg { get; set; } = 24.0;
    
    // ── Typography ──
    
    public double FontSizeBase { get; set; } = 16.0;
    public double FontSizeSm { get; set; } = 14.0;
    public double FontSizeLg { get; set; } = 20.0;
    public double FontSizeH1 { get; set; } = 40.0;
    public double FontSizeH2 { get; set; } = 32.0;
    public double FontSizeH3 { get; set; } = 28.0;
    public double FontSizeH4 { get; set; } = 24.0;
    public double FontSizeH5 { get; set; } = 20.0;
    public double FontSizeH6 { get; set; } = 16.0;
    
    // ── Control-Specific ──
    
    public double ButtonPaddingX { get; set; } = 12.0;
    public double ButtonPaddingY { get; set; } = 6.0;
    public double ButtonPaddingXLg { get; set; } = 16.0;
    public double ButtonPaddingYLg { get; set; } = 8.0;
    public double ButtonPaddingXSm { get; set; } = 8.0;
    public double ButtonPaddingYSm { get; set; } = 4.0;
    
    public double InputPaddingX { get; set; } = 12.0;
    public double InputPaddingY { get; set; } = 6.0;
    public double InputMinHeight { get; set; } = 38.0;
    public double InputMinHeightLg { get; set; } = 48.0;
    public double InputMinHeightSm { get; set; } = 31.0;

    // ── Helper Methods ──
    
    /// <summary>
    /// Gets a color value respecting the current app theme (light/dark mode).
    /// </summary>
    public Color GetBackground() => IsDarkMode ? DarkBackground : Background;
    public Color GetOnBackground() => IsDarkMode ? DarkOnBackground : OnBackground;
    public Color GetSurface() => IsDarkMode ? DarkSurface : Surface;
    public Color GetOnSurface() => IsDarkMode ? DarkOnSurface : OnSurface;
    public Color GetOutline() => IsDarkMode ? DarkOutline : Outline;
    
    private static bool IsDarkMode => Application.Current?.RequestedTheme == AppTheme.Dark;

    /// <summary>
    /// Creates a default Bootstrap 5 theme.
    /// </summary>
    public static BootstrapTheme CreateDefault() => new();
    
    /// <summary>
    /// Creates a theme from parsed CSS values.
    /// </summary>
    public static BootstrapTheme FromCssVariables(Dictionary<string, string> variables)
    {
        var theme = new BootstrapTheme();
        
        if (variables.TryGetValue("--bs-primary", out var primary))
            theme.Primary = Color.FromArgb(primary);
        if (variables.TryGetValue("--bs-secondary", out var secondary))
            theme.Secondary = Color.FromArgb(secondary);
        if (variables.TryGetValue("--bs-success", out var success))
            theme.Success = Color.FromArgb(success);
        if (variables.TryGetValue("--bs-danger", out var danger))
            theme.Danger = Color.FromArgb(danger);
        if (variables.TryGetValue("--bs-warning", out var warning))
            theme.Warning = Color.FromArgb(warning);
        if (variables.TryGetValue("--bs-info", out var info))
            theme.Info = Color.FromArgb(info);
        if (variables.TryGetValue("--bs-light", out var light))
            theme.Light = Color.FromArgb(light);
        if (variables.TryGetValue("--bs-dark", out var dark))
            theme.Dark = Color.FromArgb(dark);
        if (variables.TryGetValue("--bs-body-bg", out var bodyBg))
            theme.Background = Color.FromArgb(bodyBg);
        if (variables.TryGetValue("--bs-body-color", out var bodyColor))
            theme.OnBackground = Color.FromArgb(bodyColor);
        if (variables.TryGetValue("--bs-border-color", out var borderColor))
            theme.Outline = Color.FromArgb(borderColor);
        if (variables.TryGetValue("--bs-border-radius", out var borderRadius))
            theme.CornerRadius = ParsePixelValue(borderRadius);
        if (variables.TryGetValue("--bs-border-radius-sm", out var borderRadiusSm))
            theme.CornerRadiusSm = ParsePixelValue(borderRadiusSm);
        if (variables.TryGetValue("--bs-border-radius-lg", out var borderRadiusLg))
            theme.CornerRadiusLg = ParsePixelValue(borderRadiusLg);
            
        return theme;
    }
    
    private static double ParsePixelValue(string value)
    {
        value = value.Trim().ToLowerInvariant();
        if (value.EndsWith("px"))
            value = value[..^2];
        else if (value.EndsWith("rem"))
        {
            value = value[..^3];
            if (double.TryParse(value, out var rem))
                return rem * 16; // Assume 16px base
        }
        
        return double.TryParse(value, out var result) ? result : 6.0;
    }
}
