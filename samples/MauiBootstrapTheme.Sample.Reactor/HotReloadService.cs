// [assembly: System.Reflection.Metadata.MetadataUpdateHandler(typeof(MauiBootstrapTheme.Sample.Reactor.HotReloadService))]

namespace MauiBootstrapTheme.Sample.Reactor;

/// <summary>
/// This service is intentionally commented out.
/// MauiReactor v3+ (and v4) supports Hot Reload natively via the
/// RuntimeHostConfigurationOption in the .csproj file.
///
/// If Hot Reload fails, uncomment this class and subscribe to
/// HotReloadTriggered in BasePage.cs to bridge the notification manually.
/// </summary>
#if false
internal static class HotReloadService
{
    public static event Action? HotReloadTriggered;

#pragma warning disable IDE0051 // Called by .NET Hot Reload infrastructure
    static void ClearCache(Type[]? _) { }

    static void UpdateApplication(Type[]? _)
    {
        // Debug logging only - standard MauiReactor invalidation should handle this.
        System.Diagnostics.Debug.WriteLine("[BootstrapTheme] Hot Reload update received via custom handler.");
        
        // Uncomment if manual invalidation is needed:
        /*
        if (MainThread.IsMainThread)
            HotReloadTriggered?.Invoke();
        else
            MainThread.BeginInvokeOnMainThread(() => HotReloadTriggered?.Invoke());
        */
    }
#pragma warning restore IDE0051
}
#endif
