using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Themes;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class ThemesPage : ContentPage
{
    private static string _currentThemeName = "default";
    
    public ThemesPage()
    {
        InitializeComponent();
        UpdateThemeLabel();
        UpdateModeLabel();
    }

    private void OnDefaultThemeClicked(object sender, EventArgs e)
        => ApplyTheme("default", new DefaultTheme());

    private void OnDarklyThemeClicked(object sender, EventArgs e)
        => ApplyTheme("darkly", new DarklyTheme());

    private void OnSlateThemeClicked(object sender, EventArgs e)
        => ApplyTheme("slate", new SlateTheme());

    private void OnFlatlyThemeClicked(object sender, EventArgs e)
        => ApplyTheme("flatly", new FlatlyTheme());

    private void OnSketchyThemeClicked(object sender, EventArgs e)
        => ApplyTheme("sketchy", new SketchyTheme());

    private void OnVaporThemeClicked(object sender, EventArgs e)
        => ApplyTheme("vapor", new VaporTheme());

    private void OnBriteThemeClicked(object sender, EventArgs e)
        => ApplyTheme("brite", new DefaultTheme()); // TODO: Create BriteTheme later

    private void ApplyTheme(string name, ResourceDictionary theme)
    {
        _currentThemeName = name;
        
        // This is all it takes! DynamicResource bindings update automatically.
        if (Application.Current != null)
        {
            Application.Current.Resources = theme;
        }
        
        UpdateThemeLabel();
    }
    
    private void UpdateThemeLabel()
    {
        CurrentThemeLabel.Text = $"Current: {_currentThemeName}";
    }
    
    // Light/Dark/System mode handlers
    private void OnLightModeClicked(object sender, EventArgs e)
    {
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = AppTheme.Light;
            UpdateModeLabel();
        }
    }
    
    private void OnDarkModeClicked(object sender, EventArgs e)
    {
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
            UpdateModeLabel();
        }
    }
    
    private void OnSystemModeClicked(object sender, EventArgs e)
    {
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = AppTheme.Unspecified;
            UpdateModeLabel();
        }
    }
    
    private void UpdateModeLabel()
    {
        var appTheme = Application.Current?.UserAppTheme ?? AppTheme.Unspecified;
        var modeText = appTheme switch
        {
            AppTheme.Light => "Light",
            AppTheme.Dark => "Dark",
            _ => "System"
        };
        CurrentModeLabel.Text = $"Mode: {modeText}";
    }
}
