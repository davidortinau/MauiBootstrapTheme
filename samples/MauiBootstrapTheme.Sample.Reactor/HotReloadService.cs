using System.Reflection.Metadata;

[assembly: MetadataUpdateHandler(typeof(MauiBootstrapTheme.Sample.Reactor.HotReloadService))]

namespace MauiBootstrapTheme.Sample.Reactor;

/// <summary>
/// Bridges .NET C# Hot Reload with MauiReactor's component tree.
/// MauiReactor v4 NuGet does not ship a MetadataUpdateHandler,
/// so without this, code changes apply in-memory but components
/// never re-render.
/// </summary>
internal static class HotReloadService
{
    public static event Action? HotReloadTriggered;

#pragma warning disable IDE0051 // Called by .NET Hot Reload infrastructure
    static void ClearCache(Type[]? _) { }

    static void UpdateApplication(Type[]? _)
    {
        System.Diagnostics.Debug.WriteLine("[BootstrapTheme] Hot Reload triggered — invalidating components");
        MainThread.BeginInvokeOnMainThread(() => HotReloadTriggered?.Invoke());
    }
#pragma warning restore IDE0051
}
