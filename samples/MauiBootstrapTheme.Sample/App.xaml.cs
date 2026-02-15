using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        
        // Sync BootstrapTheme.Current from the ResourceDictionary loaded in App.xaml
        // so handlers reading the singleton have correct theme values on startup.
        BootstrapTheme.SyncFromResources(Resources);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
