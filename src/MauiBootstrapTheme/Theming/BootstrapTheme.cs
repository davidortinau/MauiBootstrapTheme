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
    /// Event raised when the theme changes. Subscribe to refresh UI components.
    /// </summary>
    public static event EventHandler? ThemeChanged;

    /// <summary>
    /// Sets the current theme and raises ThemeChanged event.
    /// </summary>
    public static void SetTheme(BootstrapTheme theme)
    {
        _current = theme;
        ThemeChanged?.Invoke(null, EventArgs.Empty);
    }

    /// <summary>
    /// Whether to respect system dark/light mode preference. Default is true.
    /// Set to false to force the theme's light mode colors regardless of system setting.
    /// </summary>
    public static bool RespectSystemTheme { get; set; } = true;

    // ── Theme Identification ──
    
    /// <summary>
    /// The name of the theme (e.g., "bootstrap", "darkly", "sketchy").
    /// Used for display and persistence.
    /// </summary>
    public string Name { get; set; } = "bootstrap";

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
    
    /// <summary>Muted text color (Bootstrap's text-muted).</summary>
    public Color Muted { get; set; } = Color.FromArgb("#6c757d");
    
    /// <summary>Mark/highlight background color.</summary>
    public Color Mark { get; set; } = Color.FromArgb("#fcf8e3");
    
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
    
    // ── Bootstrap Spacing Scale (0-5) ──
    // 0 = 0, 1 = 0.25rem (4px), 2 = 0.5rem (8px), 3 = 1rem (16px), 4 = 1.5rem (24px), 5 = 3rem (48px)
    
    /// <summary>Gets the pixel value for a Bootstrap spacing level (0-5).</summary>
    public static double GetSpacingValue(int level) => level switch
    {
        0 => 0,
        1 => 4,    // 0.25rem
        2 => 8,    // 0.5rem
        3 => 16,   // 1rem
        4 => 24,   // 1.5rem
        5 => 48,   // 3rem
        _ => 16
    };
    
    // ── Shadow Values ──
    // Bootstrap shadow CSS: box-shadow: offsetX offsetY blurRadius spreadRadius color
    // shadow-sm: 0 .125rem .25rem rgba(0,0,0,.075) = 0 2px 4px
    // shadow:    0 .5rem 1rem rgba(0,0,0,.15)     = 0 8px 16px
    // shadow-lg: 0 1rem 3rem rgba(0,0,0,.175)     = 0 16px 48px
    
    /// <summary>Small shadow (Bootstrap shadow-sm): blur=4, offset=2.</summary>
    public float ShadowSmBlur { get; set; } = 4f;
    public float ShadowSmOffsetY { get; set; } = 2f;
    public float ShadowSmOpacity { get; set; } = 0.075f;
    
    /// <summary>Default shadow (Bootstrap shadow): blur=16, offset=8.</summary>
    public float ShadowBlur { get; set; } = 16f;
    public float ShadowOffsetY { get; set; } = 8f;
    public float ShadowOpacity { get; set; } = 0.15f;
    
    /// <summary>Large shadow (Bootstrap shadow-lg): blur=48, offset=16.</summary>
    public float ShadowLgBlur { get; set; } = 48f;
    public float ShadowLgOffsetY { get; set; } = 16f;
    public float ShadowLgOpacity { get; set; } = 0.175f;
    
    // ── Typography ──
    // Note: Bootstrap uses rem units. At 16px base: 2.5rem=40px, but computed sizes are larger
    // These values match actual Bootstrap computed pixel sizes from browser measurement
    
    public double FontSizeBase { get; set; } = 16.0;
    public double FontSizeSm { get; set; } = 14.0;
    public double FontSizeLg { get; set; } = 20.0;
    public double FontSizeLead { get; set; } = 20.0;  // 1.25rem
    public double FontSizeSmall { get; set; } = 12.8; // 0.8rem
    public double FontSizeH1 { get; set; } = 40.0;    // Bootstrap 2.5rem = 40px
    public double FontSizeH2 { get; set; } = 32.0;    // Bootstrap 2rem = 32px
    public double FontSizeH3 { get; set; } = 28.0;    // Bootstrap 1.75rem = 28px
    public double FontSizeH4 { get; set; } = 24.0;    // Bootstrap 1.5rem = 24px
    public double FontSizeH5 { get; set; } = 20.0;    // Bootstrap 1.25rem = 20px
    public double FontSizeH6 { get; set; } = 16.0;    // Bootstrap 1rem = 16px
    
    // Line height multipliers (Bootstrap defaults)
    public double LineHeightBase { get; set; } = 1.5;     // Bootstrap body line-height
    public double LineHeightHeading { get; set; } = 1.2;  // Bootstrap heading line-height
    public double LineHeightLead { get; set; } = 1.5;     // Bootstrap lead line-height
    
    // Font family (null = system default, set to specific font for themed look)
    public string? FontFamily { get; set; } = null;
    
    // ── Control-Specific ──
    
    public double ButtonPaddingX { get; set; } = 12.0;
    public double ButtonPaddingY { get; set; } = 6.0;
    public double ButtonPaddingXLg { get; set; } = 16.0;
    public double ButtonPaddingYLg { get; set; } = 8.0;
    public double ButtonPaddingXSm { get; set; } = 8.0;
    public double ButtonPaddingYSm { get; set; } = 4.0;
    public double ButtonMinHeight { get; set; } = 38.0;    // Bootstrap computed button height
    public double ButtonMinHeightLg { get; set; } = 48.0;
    public double ButtonMinHeightSm { get; set; } = 31.0;
    
    public double InputPaddingX { get; set; } = 12.0;
    public double InputPaddingY { get; set; } = 6.0;
    public double InputMinHeight { get; set; } = 38.0;
    public double InputMinHeightLg { get; set; } = 48.0;
    public double InputMinHeightSm { get; set; } = 31.0;
    
    // ── Progress Bar ──
    
    public double ProgressHeight { get; set; } = 16.0;
    public double ProgressHeightSm { get; set; } = 8.0;
    public Color ProgressBackground { get; set; } = Color.FromArgb("#e9ecef");
    
    // ── Switch/CheckBox ──
    
    public double SwitchWidth { get; set; } = 48.0;
    public double SwitchHeight { get; set; } = 24.0;
    public double CheckBoxSize { get; set; } = 20.0;

    // ── Helper Methods ──
    
    /// <summary>
    /// Gets a color value respecting the current app theme (light/dark mode).
    /// </summary>
    public Color GetBackground() => IsDarkMode ? DarkBackground : Background;
    public Color GetOnBackground() => IsDarkMode ? DarkOnBackground : OnBackground;
    public Color GetSurface() => IsDarkMode ? DarkSurface : Surface;
    public Color GetOnSurface() => IsDarkMode ? DarkOnSurface : OnSurface;
    public Color GetOutline() => IsDarkMode ? DarkOutline : Outline;
    public Color GetMuted() => IsDarkMode ? DarkOnSurface.WithAlpha(0.6f) : Muted;
    
    /// <summary>
    /// Gets the appropriate text color for the current theme.
    /// Uses OnBackground directly (designed to contrast with Background).
    /// </summary>
    public Color GetText() => OnBackground;
    
    private static bool IsDarkMode => 
        RespectSystemTheme && Application.Current?.RequestedTheme == AppTheme.Dark;

    /// <summary>
    /// Gets the font size for a heading level (1-6).
    /// </summary>
    public double GetHeadingFontSize(int level) => level switch
    {
        1 => FontSizeH1,
        2 => FontSizeH2,
        3 => FontSizeH3,
        4 => FontSizeH4,
        5 => FontSizeH5,
        6 => FontSizeH6,
        _ => FontSizeBase
    };

    /// <summary>
    /// Gets the color for a variant.
    /// </summary>
    public Color GetVariantColor(BootstrapVariant variant) => variant switch
    {
        BootstrapVariant.Primary => Primary,
        BootstrapVariant.Secondary => Secondary,
        BootstrapVariant.Success => Success,
        BootstrapVariant.Danger => Danger,
        BootstrapVariant.Warning => Warning,
        BootstrapVariant.Info => Info,
        BootstrapVariant.Light => Light,
        BootstrapVariant.Dark => Dark,
        _ => GetOnSurface()
    };

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
