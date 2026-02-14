using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    /// <summary>
    /// Recreates the shell to apply theme changes globally.
    /// Call this after BootstrapTheme.SetTheme() to refresh all controls.
    /// </summary>
    public static void RefreshTheme()
    {
        if (Current?.Windows.FirstOrDefault()?.Page is Shell currentShell)
        {
            var currentRoute = currentShell.CurrentState?.Location?.ToString() ?? "//controls";
            
            // Recreate the shell with new theme
            var newShell = new AppShell();
            Current.Windows[0].Page = newShell;
            
            // Navigate back to the same page
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await Task.Delay(50); // Allow shell to initialize
                    await newShell.GoToAsync(currentRoute);
                }
                catch { /* Ignore navigation errors */ }
            });
        }
    }
}
