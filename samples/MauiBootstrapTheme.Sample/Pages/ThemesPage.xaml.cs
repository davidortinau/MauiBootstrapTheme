using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Themes;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class ThemesPage : ContentPage
{
    private static string _currentThemeName = "bootstrap";
    
    public ThemesPage()
    {
        InitializeComponent();
        UpdateThemeLabel();
    }

    private void OnDefaultThemeClicked(object sender, EventArgs e)
        => ApplyTheme("bootstrap", new DefaultTheme());

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
        => ApplyTheme("brite", new BriteTheme());

    private void ApplyTheme(string name, ResourceDictionary theme)
    {
        _currentThemeName = name;
        BootstrapTheme.Apply(theme);
        UpdateThemeLabel();
    }
    
    private void UpdateThemeLabel()
    {
        CurrentThemeLabel.Text = $"Current: {_currentThemeName}";
    }
}
