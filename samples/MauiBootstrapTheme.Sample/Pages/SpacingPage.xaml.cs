using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class SpacingPage : ContentPage
{
    public SpacingPage()
    {
        InitializeComponent();
        this.BackgroundColor = BootstrapTheme.Current.GetBackground();
    }
}
