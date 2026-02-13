namespace MauiBootstrapTheme.Sample.Blazor;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage()) { Title = "Bootstrap Theme Blazor" };
    }
}
