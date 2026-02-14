using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Services;

/// <summary>
/// Service for managing theme switching at runtime.
/// Uses the new ResourceDictionary-based approach for instant UI updates.
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
    /// Uses BootstrapTheme.Apply() which updates both the ResourceDictionary
    /// and BootstrapTheme.Current (for handler-based styling).
    /// </summary>
    public void ApplyTheme(string themeName)
    {
        _currentThemeName = themeName.ToLowerInvariant();
        
        // Single call handles everything: ResourceDictionary + BootstrapTheme.Current sync
        BootstrapTheme.Apply(_currentThemeName);
        
        // Notify listeners
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
}
