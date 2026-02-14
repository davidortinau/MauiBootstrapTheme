using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class CardsPage : ContentPage
{
    public CardsPage()
    {
        InitializeComponent();
        this.BackgroundColor = BootstrapTheme.Current.GetBackground();
    }
}
