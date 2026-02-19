using MauiReactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

abstract class BasePage : Component
{
    private void OnThemeChanged(object? sender, EventArgs e)
    {
        // Force re-render when theme changes so we pick up new colors
        Invalidate();
    }

    private void OnHotReload() => Invalidate();

    protected override void OnMounted()
    {
        BootstrapTheme.ThemeChanged += OnThemeChanged;
        HotReloadService.HotReloadTriggered += OnHotReload;
        base.OnMounted();
    }

    protected override void OnWillUnmount()
    {
        BootstrapTheme.ThemeChanged -= OnThemeChanged;
        HotReloadService.HotReloadTriggered -= OnHotReload;
        base.OnWillUnmount();
    }

    protected static T GetResource<T>(string key, T fallback = default!)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is T typed)
            return typed;
        return fallback;
    }

    public abstract VisualNode RenderContent();

    public override VisualNode Render()
    {
        return ContentPage(
            RenderContent()
        ).BackgroundColor(BootstrapTheme.Current.GetBackground());
    }
}

abstract class BasePage<TState> : Component<TState> where TState : class, new()
{
    private void OnThemeChanged(object? sender, EventArgs e)
    {
        Invalidate();
    }

    private void OnHotReload() => Invalidate();

    protected override void OnMounted()
    {
        BootstrapTheme.ThemeChanged += OnThemeChanged;
        HotReloadService.HotReloadTriggered += OnHotReload;
        base.OnMounted();
    }

    protected override void OnWillUnmount()
    {
        BootstrapTheme.ThemeChanged -= OnThemeChanged;
        HotReloadService.HotReloadTriggered -= OnHotReload;
        base.OnWillUnmount();
    }

    public abstract VisualNode RenderContent();

    public override VisualNode Render()
    {
        return ContentPage(
            RenderContent()
        ).BackgroundColor(BootstrapTheme.Current.GetBackground());
    }
}
