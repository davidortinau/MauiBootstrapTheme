using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class FormsPage : ContentPage
{
    public FormsPage()
    {
        InitializeComponent();
        this.BackgroundColor = BootstrapTheme.Current.Background;
    }
}
