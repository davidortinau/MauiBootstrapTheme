using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class ControlsPage : ContentPage
{
    public ControlsPage()
    {
        InitializeComponent();
        this.BackgroundColor = BootstrapTheme.Current.Background;
    }
}
