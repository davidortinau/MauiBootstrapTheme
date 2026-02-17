using MauiBootstrapTheme.Theming;
using MauiReactor;
using MauiReactor.Internals;

namespace MauiBootstrapTheme.Reactor;

/// <summary>
/// Fluent extension methods for applying Bootstrap styling to MauiReactor components.
/// </summary>
public static class BootstrapExtensions
{
    // ══════════════════════════════════════════════════════════════════
    // VARIANT
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets the color variant for this control.
    /// </summary>
    public static T Variant<T>(this T node, BootstrapVariant variant) 
        where T : VisualNode, IVisualNode
    {
        node.Set(Bootstrap.VariantProperty, variant);
        return node;
    }

    /// <summary>Primary color variant.</summary>
    public static T Primary<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Variant(BootstrapVariant.Primary);

    /// <summary>Secondary color variant.</summary>
    public static T Secondary<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Variant(BootstrapVariant.Secondary);

    /// <summary>Success color variant.</summary>
    public static T Success<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Variant(BootstrapVariant.Success);

    /// <summary>Danger color variant.</summary>
    public static T Danger<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Variant(BootstrapVariant.Danger);

    /// <summary>Warning color variant.</summary>
    public static T Warning<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Variant(BootstrapVariant.Warning);

    /// <summary>Info color variant.</summary>
    public static T Info<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Variant(BootstrapVariant.Info);

    /// <summary>Light color variant.</summary>
    public static T Light<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Variant(BootstrapVariant.Light);

    /// <summary>Dark color variant.</summary>
    public static T Dark<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Variant(BootstrapVariant.Dark);

    // ══════════════════════════════════════════════════════════════════
    // SIZE
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets the size variant for this control.
    /// </summary>
    public static T Size<T>(this T node, BootstrapSize size) 
        where T : VisualNode, IVisualNode
    {
        node.Set(Bootstrap.SizeProperty, size);
        return node;
    }

    /// <summary>Small size variant.</summary>
    public static T Small<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Size(BootstrapSize.Small);

    /// <summary>Large size variant.</summary>
    public static T Large<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Size(BootstrapSize.Large);

    // ══════════════════════════════════════════════════════════════════
    // BUTTON STYLES
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Makes this button an outline-style button.
    /// </summary>
    public static T Outlined<T>(this T node, bool isOutlined = true) 
        where T : VisualNode, IVisualNode
    {
        node.Set(Bootstrap.IsOutlinedProperty, isOutlined);
        return node;
    }

    /// <summary>
    /// Makes this button a pill-shaped button.
    /// </summary>
    public static T Pill<T>(this T node, bool isPill = true) 
        where T : VisualNode, IVisualNode
    {
        node.Set(Bootstrap.IsPillProperty, isPill);
        return node;
    }

    // ══════════════════════════════════════════════════════════════════
    // TYPOGRAPHY
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets the heading level (1-6) for a Label.
    /// </summary>
    public static T Heading<T>(this T node, int level) 
        where T : VisualNode, IVisualNode
    {
        node.Set(Bootstrap.HeadingProperty, level);
        return node;
    }

    /// <summary>Heading level 1.</summary>
    public static T H1<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Heading(1);

    /// <summary>Heading level 2.</summary>
    public static T H2<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Heading(2);

    /// <summary>Heading level 3.</summary>
    public static T H3<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Heading(3);

    /// <summary>Heading level 4.</summary>
    public static T H4<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Heading(4);

    /// <summary>Heading level 5.</summary>
    public static T H5<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Heading(5);

    /// <summary>Heading level 6.</summary>
    public static T H6<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Heading(6);

    /// <summary>
    /// Sets the text style for a Label.
    /// </summary>
    public static T TextStyle<T>(this T node, BootstrapTextStyle style) 
        where T : VisualNode, IVisualNode
    {
        node.Set(Bootstrap.TextStyleProperty, style);
        return node;
    }

    /// <summary>Lead text style - larger and lighter.</summary>
    public static T Lead<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.TextStyle(BootstrapTextStyle.Lead);

    /// <summary>Muted text style - secondary color.</summary>
    public static T Muted<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.TextStyle(BootstrapTextStyle.Muted);

    /// <summary>
    /// Sets the text color variant.
    /// </summary>
    public static T TextColor<T>(this T node, BootstrapVariant variant) 
        where T : VisualNode, IVisualNode
    {
        node.Set(Bootstrap.TextColorVariantProperty, variant);
        return node;
    }

    // ══════════════════════════════════════════════════════════════════
    // BADGES
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Makes this Label display as a badge.
    /// </summary>
    public static T Badge<T>(this T node, BootstrapVariant variant) 
        where T : VisualNode, IVisualNode
    {
        node.Set(Bootstrap.BadgeProperty, variant);
        return node;
    }

    // ══════════════════════════════════════════════════════════════════
    // SHADOWS
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets the shadow level for a Border.
    /// </summary>
    public static T Shadow<T>(this T node, BootstrapShadow shadow) 
        where T : VisualNode, IVisualNode
    {
        node.Set(Bootstrap.ShadowProperty, shadow);
        return node;
    }

    /// <summary>Small shadow.</summary>
    public static T ShadowSm<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Shadow(BootstrapShadow.Small);

    /// <summary>Default shadow.</summary>
    public static T ShadowMd<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Shadow(BootstrapShadow.Default);

    /// <summary>Large shadow.</summary>
    public static T ShadowLg<T>(this T node) 
        where T : VisualNode, IVisualNode
        => node.Shadow(BootstrapShadow.Large);

    // ══════════════════════════════════════════════════════════════════
    // BACKGROUND
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets the background color variant.
    /// </summary>
    public static T Background<T>(this T node, BootstrapVariant variant) 
        where T : VisualNode, IVisualNode
    {
        node.Set(Bootstrap.BackgroundVariantProperty, variant);
        return node;
    }

    // ══════════════════════════════════════════════════════════════════
    // SPACING
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets the margin level (0-5).
    /// 0=0px, 1=4px, 2=8px, 3=16px, 4=24px, 5=48px
    /// </summary>
    public static T MarginLevel<T>(this T node, int level) 
        where T : VisualNode, IVisualNode
    {
        node.Set(Bootstrap.MarginLevelProperty, level);
        return node;
    }

    /// <summary>
    /// Sets the padding level (0-5).
    /// 0=0px, 1=4px, 2=8px, 3=16px, 4=24px, 5=48px
    /// </summary>
    public static T PaddingLevel<T>(this T node, int level) 
        where T : VisualNode, IVisualNode
    {
        node.Set(Bootstrap.PaddingLevelProperty, level);
        return node;
    }

    // ══════════════════════════════════════════════════════════════════
    // BOOTSTRAP SIZING HELPERS
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets Bootstrap-standard height for buttons (38px default, 48px large, 31px small).
    /// </summary>
    public static MauiReactor.Button BootstrapHeight(this MauiReactor.Button node)
    {
        var theme = BootstrapTheme.Current;
        return node.HeightRequest(theme.ButtonMinHeight);
    }

    /// <summary>
    /// Sets Bootstrap-standard height for entries (38px default, 48px large, 31px small).
    /// </summary>
    public static MauiReactor.Entry BootstrapHeight(this MauiReactor.Entry node)
    {
        var theme = BootstrapTheme.Current;
        return node.HeightRequest(theme.InputMinHeight);
    }

    /// <summary>
    /// Sets Bootstrap-standard height for progress bars (16px).
    /// </summary>
    public static MauiReactor.ProgressBar BootstrapHeight(this MauiReactor.ProgressBar node)
    {
        var theme = BootstrapTheme.Current;
        return node.HeightRequest(theme.ProgressHeight);
    }
}
