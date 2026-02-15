using MauiBootstrapTheme.Handlers;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Extensions;

/// <summary>
/// Extension methods for MauiAppBuilder to configure Bootstrap theming.
/// </summary>
public static class MauiAppBuilderExtensions
{
    /// <summary>
    /// Configures the app to use Bootstrap theming.
    /// Registers platform-specific handlers and syncs BootstrapTheme.Current from Application.Resources on startup.
    /// </summary>
    public static MauiAppBuilder UseBootstrapTheme(this MauiAppBuilder builder)
    {
        RegisterHandlers();
        builder.Services.AddSingleton<IMauiInitializeService>(new BootstrapThemeSyncService());
        return builder;
    }

    /// <summary>
    /// Configures the app to use Bootstrap theming with the specified themes.
    /// The first theme in the list is the default.
    /// </summary>
    public static MauiAppBuilder UseBootstrapTheme(this MauiAppBuilder builder, Action<BootstrapThemeOptions> configure)
    {
        var options = new BootstrapThemeOptions();
        configure(options);

        // Register all themes
        foreach (var (name, factory) in options.Themes)
        {
            BootstrapTheme.RegisterTheme(name, factory);
        }

        RegisterHandlers();
        builder.Services.AddSingleton<IMauiInitializeService>(new BootstrapThemeSyncService());
        return builder;
    }

    private static void RegisterHandlers()
    {
        // Input controls
        BootstrapEntryHandler.Register();
        BootstrapButtonHandler.Register();
        BootstrapEditorHandler.Register();
        BootstrapSearchBarHandler.Register();
        BootstrapPickerHandler.Register();
        BootstrapDatePickerHandler.Register();
        BootstrapTimePickerHandler.Register();
        BootstrapCheckBoxHandler.Register();
        BootstrapSwitchHandler.Register();
        BootstrapRadioButtonHandler.Register();
        BootstrapSliderHandler.Register();
        BootstrapStepperHandler.Register();
        
        // Display controls
        BootstrapLabelHandler.Register();
        BootstrapProgressBarHandler.Register();
        BootstrapActivityIndicatorHandler.Register();
        
        // Container controls
        BootstrapBorderHandler.Register();
    }
}

/// <summary>
/// Options for configuring Bootstrap theming.
/// </summary>
public class BootstrapThemeOptions
{
    internal List<(string Name, Func<ResourceDictionary> Factory)> Themes { get; } = new();

    /// <summary>
    /// Registers a theme by name and factory. The first registered theme is the default.
    /// </summary>
    /// <typeparam name="T">A ResourceDictionary type generated from CSS.</typeparam>
    /// <param name="name">Theme name for runtime switching.</param>
    public BootstrapThemeOptions AddTheme<T>(string name) where T : ResourceDictionary, new()
    {
        Themes.Add((name, () => new T()));
        return this;
    }

    /// <summary>
    /// Registers a theme by name and factory function.
    /// </summary>
    public BootstrapThemeOptions AddTheme(string name, Func<ResourceDictionary> factory)
    {
        Themes.Add((name, factory));
        return this;
    }
}

/// <summary>
/// Syncs BootstrapTheme.Current from Application.Resources on app startup.
/// </summary>
internal class BootstrapThemeSyncService : IMauiInitializeService
{
    public void Initialize(IServiceProvider services)
    {
        // Defer to after Application is created so Resources are available
        Microsoft.Maui.Dispatching.DispatcherProvider.Current.GetForCurrentThread()?.DispatchDelayed(
            TimeSpan.FromMilliseconds(50),
            () =>
            {
                if (Application.Current?.Resources != null)
                {
                    BootstrapTheme.SyncFromResources(Application.Current.Resources);
                }
            });
    }
}
