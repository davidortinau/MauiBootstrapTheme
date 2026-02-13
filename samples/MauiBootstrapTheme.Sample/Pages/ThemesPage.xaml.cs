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
        => ApplyTheme(new DefaultTheme().GetTheme(), "Default Bootstrap 5");

    private void OnDarklyThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new DarklyTheme().GetTheme(), "Darkly");

    private void OnCyborgThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new CyborgTheme().GetTheme(), "Cyborg");

    private void OnMintyThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new MintyTheme().GetTheme(), "Minty");

    private void OnSlateThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new SlateTheme().GetTheme(), "Slate");

    private void OnFlatlyThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new FlatlyTheme().GetTheme(), "Flatly");

    private void OnSketchyThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new SketchyTheme().GetTheme(), "Sketchy");

    private void OnVaporThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new VaporTheme().GetTheme(), "Vapor");

    private void OnBriteThemeClicked(object sender, EventArgs e)
        => ApplyTheme(new BriteTheme().GetTheme(), "Brite");

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
        RefreshControl(PreviewEditor);
    }

    private void RefreshControl(VisualElement control)
        => control.Handler?.UpdateValue("BootstrapStyle");
}
