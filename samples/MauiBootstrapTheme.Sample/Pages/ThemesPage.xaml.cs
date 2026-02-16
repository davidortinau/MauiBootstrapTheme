using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class ThemesPage : ContentPage
{
    private static string _currentThemeName = "default";
    
    public ThemesPage()
    {
        InitializeComponent();
        ApplyTopThemeButtonShadowBindings();
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
        ApplyTopThemeButtonShadowBindings();
        UpdateThemeLabel();
    }
    
    private void UpdateThemeLabel()
    {
        CurrentThemeLabel.Text = $"Current: {_currentThemeName}";
    }

    private void ApplyTopThemeButtonShadowBindings()
    {
        BindShadow(ThemeButtonDefault, "BtnShadowPrimary");
        BindShadow(ThemeButtonDarkly, "BtnShadowDark");
        BindShadow(ThemeButtonSlate, "BtnShadowSecondary");
        BindShadow(ThemeButtonFlatly, "BtnShadowInfo");
        BindShadow(ThemeButtonSketchy, "BtnShadowWarning");
        BindShadow(ThemeButtonVapor, "BtnShadowDanger");
        BindShadow(ThemeButtonBrite, "BtnShadowSuccess");
    }

    private static void BindShadow(Button button, string resourceKey)
    {
        button.RemoveDynamicResource(Button.ShadowProperty);
        button.ClearValue(Button.ShadowProperty);
        button.SetDynamicResource(Button.ShadowProperty, resourceKey);
    }
}
