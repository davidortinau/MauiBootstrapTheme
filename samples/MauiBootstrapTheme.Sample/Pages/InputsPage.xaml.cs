using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class InputsPage : ContentPage
{
    public InputsPage()
    {
        InitializeComponent();
        this.BackgroundColor = BootstrapTheme.Current.GetBackground();
    }
}
