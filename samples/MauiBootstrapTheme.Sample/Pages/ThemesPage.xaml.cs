using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Themes.Default;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class ThemesPage : ContentPage
{
    public ThemesPage()
    {
        InitializeComponent();
        
        // Apply current theme to page background and update label
        var theme = BootstrapTheme.Current;
        this.BackgroundColor = theme.GetBackground();
        CurrentThemeLabel.Text = $"Current: {theme.Name}";
        
        // Update mode label based on current setting
        UpdateModeLabel();
    }

    private void OnDefaultThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new DefaultTheme().GetTheme());

    private void OnDarklyThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new DarklyTheme().GetTheme());

    private void OnSlateThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new SlateTheme().GetTheme());

    private void OnFlatlyThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new FlatlyTheme().GetTheme());

    private void OnSketchyThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new SketchyTheme().GetTheme());

    private void OnVaporThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new VaporTheme().GetTheme());

    private void OnBriteThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new BriteTheme().GetTheme());

    private void ApplyTheme(BootstrapTheme theme)
    {
        BootstrapTheme.SetTheme(theme);
        
        // Refresh the entire app to apply theme globally
        App.RefreshTheme();
    }
    
    // Light/Dark/System mode handlers
    private void OnLightModeClicked(object sender, EventArgs e)
    {
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = AppTheme.Light;
            // Refresh to apply new mode
            App.RefreshTheme();
        }
    }
    
    private void OnDarkModeClicked(object sender, EventArgs e)
    {
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
            // Refresh to apply new mode
            App.RefreshTheme();
        }
    }
    
    private void OnSystemModeClicked(object sender, EventArgs e)
    {
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = AppTheme.Unspecified;
            // Refresh to apply new mode
            App.RefreshTheme();
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
