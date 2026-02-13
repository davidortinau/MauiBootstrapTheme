using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Themes.Default;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class ThemesPage : ContentPage
{
    public ThemesPage()
    {
        InitializeComponent();
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
        CurrentThemeLabel.Text = $"Current: {themeName}";
        
        this.BackgroundColor = theme.Background;
        PreviewFrame.BackgroundColor = theme.Surface;
        PreviewFrame.Stroke = new SolidColorBrush(theme.GetOutline());
        PreviewLabel.TextColor = theme.GetOnSurface();
        
        RefreshControl(PreviewEntry);
        RefreshControl(PreviewBtn1);
        RefreshControl(PreviewBtn2);
        RefreshControl(PreviewBtn3);
        RefreshControl(PreviewBtn4);
        RefreshControl(PreviewBtn5);
    }

    private void RefreshControl(VisualElement control)
        => control.Handler?.UpdateValue("BootstrapStyle");
}
