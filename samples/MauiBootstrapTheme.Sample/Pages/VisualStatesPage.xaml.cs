using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Pages;

public partial class VisualStatesPage : ContentPage
{
    public VisualStatesPage()
    {
        InitializeComponent();
        this.BackgroundColor = BootstrapTheme.Current.GetBackground();
    }

    private void OnToggleDisabledClicked(object? sender, EventArgs e)
    {
        ToggleTarget.IsEnabled = !ToggleTarget.IsEnabled;
    }
}
