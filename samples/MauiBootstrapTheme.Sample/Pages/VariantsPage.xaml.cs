using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class VariantsPage : ContentPage
{
    public VariantsPage()
    {
        InitializeComponent();
        this.BackgroundColor = BootstrapTheme.Current.GetBackground();
    }
}
