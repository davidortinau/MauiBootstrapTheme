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
    {
        ApplyTheme(new DefaultTheme().GetTheme(), "Default Bootstrap 5");
    }

    private void OnDarklyThemeClicked(object sender, EventArgs e)
    {
        ApplyTheme(new DarklyTheme().GetTheme(), "Darkly (Dark Mode)");
    }

    private void OnCyborgThemeClicked(object sender, EventArgs e)
    {
        ApplyTheme(new CyborgTheme().GetTheme(), "Cyborg (Cyberpunk)");
    }

    private void OnMintyThemeClicked(object sender, EventArgs e)
    {
        ApplyTheme(new MintyTheme().GetTheme(), "Minty (Fresh Pastels)");
    }

    private void OnSlateThemeClicked(object sender, EventArgs e)
    {
        ApplyTheme(new SlateTheme().GetTheme(), "Slate (Professional Gray)");
    }

    private void ApplyTheme(BootstrapTheme theme, string themeName)
    {
        // Set the global theme
        BootstrapTheme.SetTheme(theme);
        
        // Update the current theme label
        CurrentThemeLabel.Text = $"Current: {themeName}";
        
        // Update page background for dark themes
        this.BackgroundColor = theme.Background;
        PreviewFrame.BackgroundColor = theme.Surface;
        PreviewFrame.BorderColor = theme.GetOutline();
        PreviewLabel.TextColor = theme.GetOnSurface();
        
        // Force handler updates by triggering property changes
        // This demonstrates runtime theme switching
        RefreshControl(PreviewEntry);
        RefreshControl(PreviewBtn1);
        RefreshControl(PreviewBtn2);
        RefreshControl(PreviewBtn3);
        RefreshControl(PreviewBtn4);
        RefreshControl(PreviewBtn5);
        RefreshControl(PreviewEditor);
    }

    private void RefreshControl(VisualElement control)
    {
        // Force the handler to re-apply styling
        control.Handler?.UpdateValue("BootstrapStyle");
    }
}
