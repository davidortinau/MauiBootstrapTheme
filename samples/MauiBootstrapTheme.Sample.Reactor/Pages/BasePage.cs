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

    protected override void OnMounted()
    {
        BootstrapTheme.ThemeChanged += OnThemeChanged;
        base.OnMounted();
    }

    protected override void OnWillUnmount()
    {
        BootstrapTheme.ThemeChanged -= OnThemeChanged;
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

abstract class BasePage<TState> : Component<TState> where TState : class, new()
{
    private void OnThemeChanged(object? sender, EventArgs e)
    {
        Invalidate();
    }

    protected override void OnMounted()
    {
        BootstrapTheme.ThemeChanged += OnThemeChanged;
        base.OnMounted();
    }

    protected override void OnWillUnmount()
    {
        BootstrapTheme.ThemeChanged -= OnThemeChanged;
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
