using MauiBootstrapTheme.Theming;
using MauiReactor;
using MauiReactor.Internals;

namespace MauiBootstrapTheme.Reactor;

/// <summary>
/// Fluent extension methods for applying Bootstrap styling to MauiReactor components.
/// These extensions set the underlying attached properties on MAUI controls.
/// </summary>
public static class BootstrapExtensions
{
    // ══════════════════════════════════════════════════════════════════
    // VARIANT & SIZE
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets the Bootstrap color variant for this control.
    /// </summary>
    public static T BootstrapVariant<T>(this T node, BootstrapVariant variant) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
    {
        node.Set(Bootstrap.VariantProperty, variant);
        return node;
    }

    /// <summary>
    /// Sets the Bootstrap size variant for this control.
    /// </summary>
    public static T BootstrapSize<T>(this T node, BootstrapSize size) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
    {
        node.Set(Bootstrap.SizeProperty, size);
        return node;
    }

    /// <summary>
    /// Makes this button an outline-style button.
    /// </summary>
    public static T BootstrapOutlined<T>(this T node, bool isOutlined = true) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
    {
        node.Set(Bootstrap.IsOutlinedProperty, isOutlined);
        return node;
    }

    /// <summary>
    /// Makes this button a pill-shaped button.
    /// </summary>
    public static T BootstrapPill<T>(this T node, bool isPill = true) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
    {
        node.Set(Bootstrap.IsPillProperty, isPill);
        return node;
    }

    // ══════════════════════════════════════════════════════════════════
    // TYPOGRAPHY
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets the heading level (1-6) for a Label, like h1-h6 in HTML.
    /// </summary>
    public static T BootstrapHeading<T>(this T node, int level) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
    {
        node.Set(Bootstrap.HeadingProperty, level);
        return node;
    }

    /// <summary>
    /// Sets the text style (Lead, Small, Muted, Mark) for a Label.
    /// </summary>
    public static T BootstrapTextStyle<T>(this T node, BootstrapTextStyle style) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
    {
        node.Set(Bootstrap.TextStyleProperty, style);
        return node;
    }

    /// <summary>
    /// Sets the text color variant for a Label.
    /// </summary>
    public static T BootstrapTextColor<T>(this T node, BootstrapVariant variant) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
    {
        node.Set(Bootstrap.TextColorVariantProperty, variant);
        return node;
    }

    // ══════════════════════════════════════════════════════════════════
    // BADGES
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Makes this Label display as a badge with the specified variant.
    /// </summary>
    public static T BootstrapBadge<T>(this T node, BootstrapVariant variant) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
    {
        node.Set(Bootstrap.BadgeProperty, variant);
        return node;
    }

    // ══════════════════════════════════════════════════════════════════
    // CONTAINERS
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets the shadow level for a Border (card styling).
    /// </summary>
    public static T BootstrapShadow<T>(this T node, BootstrapShadow shadow) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
    {
        node.Set(Bootstrap.ShadowProperty, shadow);
        return node;
    }

    /// <summary>
    /// Sets the background color variant for a container.
    /// </summary>
    public static T BootstrapBackground<T>(this T node, BootstrapVariant variant) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
    {
        node.Set(Bootstrap.BackgroundVariantProperty, variant);
        return node;
    }

    // ══════════════════════════════════════════════════════════════════
    // SPACING
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets the Bootstrap margin level (0-5).
    /// 0=0px, 1=4px, 2=8px, 3=16px, 4=24px, 5=48px
    /// </summary>
    public static T BootstrapMargin<T>(this T node, int level) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
    {
        node.Set(Bootstrap.MarginLevelProperty, level);
        return node;
    }

    /// <summary>
    /// Sets the Bootstrap padding level (0-5).
    /// 0=0px, 1=4px, 2=8px, 3=16px, 4=24px, 5=48px
    /// </summary>
    public static T BootstrapPadding<T>(this T node, int level) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
    {
        node.Set(Bootstrap.PaddingLevelProperty, level);
        return node;
    }

    // ══════════════════════════════════════════════════════════════════
    // CONVENIENCE SHORTCUTS
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Shortcut for BootstrapVariant(Primary).
    /// </summary>
    public static T BsPrimary<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapVariant(Theming.BootstrapVariant.Primary);

    /// <summary>
    /// Shortcut for BootstrapVariant(Secondary).
    /// </summary>
    public static T BsSecondary<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapVariant(Theming.BootstrapVariant.Secondary);

    /// <summary>
    /// Shortcut for BootstrapVariant(Success).
    /// </summary>
    public static T BsSuccess<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapVariant(Theming.BootstrapVariant.Success);

    /// <summary>
    /// Shortcut for BootstrapVariant(Danger).
    /// </summary>
    public static T BsDanger<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapVariant(Theming.BootstrapVariant.Danger);

    /// <summary>
    /// Shortcut for BootstrapVariant(Warning).
    /// </summary>
    public static T BsWarning<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapVariant(Theming.BootstrapVariant.Warning);

    /// <summary>
    /// Shortcut for BootstrapVariant(Info).
    /// </summary>
    public static T BsInfo<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapVariant(Theming.BootstrapVariant.Info);

    /// <summary>
    /// Shortcut for BootstrapSize(Small).
    /// </summary>
    public static T BsSmall<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapSize(Theming.BootstrapSize.Small);

    /// <summary>
    /// Shortcut for BootstrapSize(Large).
    /// </summary>
    public static T BsLarge<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapSize(Theming.BootstrapSize.Large);

    /// <summary>
    /// Shortcut for BootstrapHeading(1).
    /// </summary>
    public static T BsH1<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapHeading(1);

    /// <summary>
    /// Shortcut for BootstrapHeading(2).
    /// </summary>
    public static T BsH2<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapHeading(2);

    /// <summary>
    /// Shortcut for BootstrapHeading(3).
    /// </summary>
    public static T BsH3<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapHeading(3);

    /// <summary>
    /// Shortcut for BootstrapHeading(4).
    /// </summary>
    public static T BsH4<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapHeading(4);

    /// <summary>
    /// Shortcut for BootstrapHeading(5).
    /// </summary>
    public static T BsH5<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapHeading(5);

    /// <summary>
    /// Shortcut for BootstrapHeading(6).
    /// </summary>
    public static T BsH6<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapHeading(6);

    /// <summary>
    /// Shortcut for BootstrapTextStyle(Lead).
    /// </summary>
    public static T BsLead<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapTextStyle(Theming.BootstrapTextStyle.Lead);

    /// <summary>
    /// Shortcut for BootstrapTextStyle(Muted).
    /// </summary>
    public static T BsMuted<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapTextStyle(Theming.BootstrapTextStyle.Muted);

    /// <summary>
    /// Shortcut for BootstrapShadow(Small).
    /// </summary>
    public static T BsShadowSm<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapShadow(Theming.BootstrapShadow.Small);

    /// <summary>
    /// Shortcut for BootstrapShadow(Default).
    /// </summary>
    public static T BsShadow<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapShadow(Theming.BootstrapShadow.Default);

    /// <summary>
    /// Shortcut for BootstrapShadow(Large).
    /// </summary>
    public static T BsShadowLg<T>(this T node) 
        where T : VisualNode, IVisualNodeWithAttachedProperties
        => node.BootstrapShadow(Theming.BootstrapShadow.Large);
}
