using MauiBootstrapTheme.Handlers;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Extensions;

/// <summary>
/// Extension methods for MauiAppBuilder to configure Bootstrap theming.
/// </summary>
public static class MauiAppBuilderExtensions
{
    /// <summary>
    /// Configures the app to use Bootstrap theming with the default Bootstrap 5 theme.
    /// Registers platform-specific handlers and syncs BootstrapTheme.Current from Application.Resources on startup.
    /// </summary>
    /// <param name="builder">The MauiAppBuilder instance.</param>
    /// <returns>The MauiAppBuilder instance for chaining.</returns>
    public static MauiAppBuilder UseBootstrapTheme(this MauiAppBuilder builder)
    {
        RegisterHandlers();

        // Sync BootstrapTheme.Current from Application.Resources once the app is created.
        // This ensures handlers reading from the singleton get the correct theme values
        // even before the user switches themes via BootstrapTheme.Apply().
        builder.Services.AddSingleton<IMauiInitializeService>(new BootstrapThemeSyncService());

        return builder;
    }

    /// <summary>
    /// Configures the app to use Bootstrap theming with a custom theme.
    /// </summary>
    /// <param name="builder">The MauiAppBuilder instance.</param>
    /// <param name="theme">The Bootstrap theme to use.</param>
    /// <returns>The MauiAppBuilder instance for chaining.</returns>
    public static MauiAppBuilder UseBootstrapTheme(this MauiAppBuilder builder, BootstrapTheme theme)
    {
        BootstrapTheme.SetTheme(theme);
        return builder.UseBootstrapTheme(_ => { });
    }

    /// <summary>
    /// Configures the app to use Bootstrap theming with custom options.
    /// </summary>
    /// <param name="builder">The MauiAppBuilder instance.</param>
    /// <param name="configure">Action to configure Bootstrap theme options.</param>
    /// <returns>The MauiAppBuilder instance for chaining.</returns>
    public static MauiAppBuilder UseBootstrapTheme(this MauiAppBuilder builder, Action<BootstrapThemeOptions> configure)
    {
        var options = new BootstrapThemeOptions();
        configure(options);

        // Apply theme if custom one provided
        if (options.Theme != null)
        {
            BootstrapTheme.SetTheme(options.Theme);
        }
        else if (!string.IsNullOrEmpty(options.CssPath))
        {
            // Load from CSS file (runtime parsing - less optimal than build-time)
            var theme = LoadThemeFromCss(options.CssPath);
            BootstrapTheme.SetTheme(theme);
        }

        // Register handler customizations
        RegisterHandlers();

        return builder;
    }

    /// <summary>
    /// Configures the app to use a pre-built theme type.
    /// </summary>
    /// <typeparam name="TTheme">The theme type that provides Bootstrap values.</typeparam>
    /// <param name="builder">The MauiAppBuilder instance.</param>
    /// <returns>The MauiAppBuilder instance for chaining.</returns>
    public static MauiAppBuilder UseBootstrapTheme<TTheme>(this MauiAppBuilder builder) 
        where TTheme : IBootstrapThemeProvider, new()
    {
        var provider = new TTheme();
        BootstrapTheme.SetTheme(provider.GetTheme());
        RegisterHandlers();
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

    private static BootstrapTheme LoadThemeFromCss(string cssPath)
    {
        // Note: For optimal performance, use the build-time CSS parser instead
        // This runtime parsing is provided for flexibility but has startup cost
        
        try
        {
            if (File.Exists(cssPath))
            {
                var cssContent = File.ReadAllText(cssPath);
                var variables = ParseCssVariables(cssContent);
                return BootstrapTheme.FromCssVariables(variables);
            }
        }
        catch
        {
            // Fall back to default theme on error
        }

        return BootstrapTheme.CreateDefault();
    }

    private static Dictionary<string, string> ParseCssVariables(string css)
    {
        var variables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        
        // Simple regex-based parser for CSS custom properties
        // Production should use ExCSS or similar
        var pattern = @"--bs-([a-zA-Z0-9-]+)\s*:\s*([^;]+);";
        var matches = System.Text.RegularExpressions.Regex.Matches(css, pattern);
        
        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            var name = $"--bs-{match.Groups[1].Value}";
            var value = match.Groups[2].Value.Trim();
            variables[name] = value;
        }
        
        return variables;
    }
}

/// <summary>
/// Options for configuring Bootstrap theming.
/// </summary>
public class BootstrapThemeOptions
{
    /// <summary>
    /// Path to a Bootstrap CSS file to parse for theme values.
    /// </summary>
    public string? CssPath { get; set; }

    /// <summary>
    /// A pre-configured Bootstrap theme to use.
    /// </summary>
    public BootstrapTheme? Theme { get; set; }

    /// <summary>
    /// Whether to enable dark mode variant support.
    /// </summary>
    public bool EnableDarkMode { get; set; } = true;
}

/// <summary>
/// Interface for pre-built theme packages to provide their theme.
/// </summary>
public interface IBootstrapThemeProvider
{
    /// <summary>
    /// Gets the Bootstrap theme provided by this package.
    /// </summary>
    BootstrapTheme GetTheme();
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
