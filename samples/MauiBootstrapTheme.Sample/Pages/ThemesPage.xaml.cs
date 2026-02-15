using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class ThemesPage : ContentPage
{
    private static string _currentThemeName = "default";
    
    public ThemesPage()
    {
        InitializeComponent();
        UpdateThemeLabel();
    }

    private void OnDefaultThemeClicked(object sender, EventArgs e)
        => ApplyTheme("default");

    private void OnDarklyThemeClicked(object sender, EventArgs e)
        => ApplyTheme("darkly");

    private void OnSlateThemeClicked(object sender, EventArgs e)
        => ApplyTheme("slate");

    private void OnFlatlyThemeClicked(object sender, EventArgs e)
        => ApplyTheme("flatly");

    private void OnSketchyThemeClicked(object sender, EventArgs e)
        => ApplyTheme("sketchy");

    private void OnVaporThemeClicked(object sender, EventArgs e)
        => ApplyTheme("vapor");

    private void OnBriteThemeClicked(object sender, EventArgs e)
        => ApplyTheme("brite");

    private void ApplyTheme(string name)
    {
        _currentThemeName = name;
        BootstrapTheme.Apply(name);
        UpdateThemeLabel();
    }
    
    private void UpdateThemeLabel()
    {
        CurrentThemeLabel.Text = $"Current: {_currentThemeName}";
    }
}
