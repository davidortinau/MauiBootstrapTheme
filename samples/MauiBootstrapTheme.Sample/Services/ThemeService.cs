using MauiBootstrapTheme.Theming;
using ThemeProvider = MauiBootstrapTheme.Themes.Default;

namespace MauiBootstrapTheme.Sample.Services;

/// <summary>
/// Service for managing theme switching at runtime.
/// Uses DynamicResource approach for instant UI updates without recreating the shell.
/// </summary>
public class ThemeService
{
    private static ThemeService? _instance;
    public static ThemeService Instance => _instance ??= new ThemeService();
    
    private string _currentThemeName = "default";
    private AppTheme _userAppTheme = AppTheme.Unspecified;
    
    public string CurrentThemeName => _currentThemeName;
    public AppTheme UserAppTheme => _userAppTheme;
    
    public event EventHandler<string>? ThemeChanged;
    
    /// <summary>
    /// Available theme names for the picker.
    /// </summary>
    public static readonly string[] AvailableThemes = 
    {
        "default",
        "darkly", 
        "slate",
        "flatly",
        "sketchy",
        "vapor",
        "brite"
    };
    
    /// <summary>
    /// Applies a Bootswatch theme by name.
    /// Updates both the ResourceDictionary (for DynamicResource bindings) 
    /// and the BootstrapTheme.Current (for handler-based styling).
    /// </summary>
    public void ApplyTheme(string themeName)
    {
        _currentThemeName = themeName.ToLowerInvariant();
        
        // 1. Get the BootstrapTheme for handlers
        var bootstrapTheme = GetBootstrapTheme(_currentThemeName);
        
        // 2. Update resource dictionary values directly
        UpdateResourceValues(bootstrapTheme);
        
        // 3. Update BootstrapTheme.Current (for handler-based controls)
        BootstrapTheme.SetTheme(bootstrapTheme);
        
        // 4. Notify listeners
        ThemeChanged?.Invoke(this, _currentThemeName);
    }
    
    /// <summary>
    /// Sets the user's preferred app theme (Light, Dark, or Unspecified for system).
    /// </summary>
    public void SetAppTheme(AppTheme theme)
    {
        _userAppTheme = theme;
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = theme;
        }
    }
    
    private static BootstrapTheme GetBootstrapTheme(string themeName)
    {
        return themeName switch
        {
            "darkly" => new ThemeProvider.DarklyTheme().GetTheme(),
            "slate" => new ThemeProvider.SlateTheme().GetTheme(),
            "flatly" => new ThemeProvider.FlatlyTheme().GetTheme(),
            "sketchy" => new ThemeProvider.SketchyTheme().GetTheme(),
            "vapor" => new ThemeProvider.VaporTheme().GetTheme(),
            "brite" => new ThemeProvider.BriteTheme().GetTheme(),
            _ => new ThemeProvider.DefaultTheme().GetTheme()
        };
    }
    
    private static void UpdateResourceValues(BootstrapTheme theme)
    {
        if (Application.Current?.Resources == null) return;
        
        var resources = Application.Current.Resources;
        
        // Update semantic colors
        resources["ThemePrimary"] = theme.Primary;
        resources["ThemeSecondary"] = theme.Secondary;
        resources["ThemeSuccess"] = theme.Success;
        resources["ThemeDanger"] = theme.Danger;
        resources["ThemeWarning"] = theme.Warning;
        resources["ThemeInfo"] = theme.Info;
        resources["ThemeLight"] = theme.Light;
        resources["ThemeDark"] = theme.Dark;
        
        // Update light mode colors
        resources["ThemeLightBackground"] = theme.Background;
        resources["ThemeLightSurface"] = theme.Surface;
        resources["ThemeLightOnBackground"] = theme.OnBackground;
        resources["ThemeLightOnSurface"] = theme.OnSurface;
        resources["ThemeLightOutline"] = theme.Outline;
        resources["ThemeLightMuted"] = theme.Muted;
        
        // Update dark mode colors
        resources["ThemeDarkBackground"] = theme.DarkBackground;
        resources["ThemeDarkSurface"] = theme.DarkSurface;
        resources["ThemeDarkOnBackground"] = theme.DarkOnBackground;
        resources["ThemeDarkOnSurface"] = theme.DarkOnSurface;
        resources["ThemeDarkOutline"] = theme.DarkOutline;
        resources["ThemeDarkMuted"] = Color.FromArgb("#adb5bd"); // Dark mode muted
        
        // Update font family
        resources["ThemeFontFamily"] = theme.FontFamily ?? string.Empty;
    }
}
