using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Themes.Default;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class ThemesPage : ContentPage
{
    public ThemesPage()
    {
        InitializeComponent();
        
        // Apply current theme to page background
        var theme = BootstrapTheme.Current;
        this.BackgroundColor = theme.Background;
    }

    private void OnDefaultThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new DefaultTheme().GetTheme(), "bootstrap");

    private void OnDarklyThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new DarklyTheme().GetTheme(), "darkly");

    private void OnSlateThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new SlateTheme().GetTheme(), "slate");

    private void OnFlatlyThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new FlatlyTheme().GetTheme(), "flatly");

    private void OnSketchyThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new SketchyTheme().GetTheme(), "sketchy");

    private void OnVaporThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new VaporTheme().GetTheme(), "vapor");

    private void OnBriteThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new BriteTheme().GetTheme(), "brite");

    private void ApplyTheme(BootstrapTheme theme, string themeName)
    {
        BootstrapTheme.SetTheme(theme);
        
        // Refresh the entire app to apply theme globally
        App.RefreshTheme();
    }
}
