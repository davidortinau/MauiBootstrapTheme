using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Keep sample comparisons deterministic against Blazor baseline (light mode CSS rendering).
        UserAppTheme = AppTheme.Light;
        
        // Load the default generated theme from CSS
        Resources.MergedDictionaries.Add(new Themes.DefaultTheme());
        
        // Sync BootstrapTheme.Current from the ResourceDictionary
        // so handlers reading the singleton have correct theme values on startup.
        BootstrapTheme.SyncFromResources(Resources);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
