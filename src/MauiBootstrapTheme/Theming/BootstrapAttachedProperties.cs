namespace MauiBootstrapTheme.Theming;

/// <summary>
/// Attached properties for applying Bootstrap styling to individual controls.
/// </summary>
public static class Bootstrap
{
    // ── Variant Property ──
    
    public static readonly BindableProperty VariantProperty =
        BindableProperty.CreateAttached(
            "Variant",
            typeof(BootstrapVariant),
            typeof(Bootstrap),
            BootstrapVariant.Default,
            propertyChanged: OnVariantChanged);

    public static BootstrapVariant GetVariant(BindableObject view) =>
        (BootstrapVariant)view.GetValue(VariantProperty);

    public static void SetVariant(BindableObject view, BootstrapVariant value) =>
        view.SetValue(VariantProperty, value);

    // ── Size Property ──
    
    public static readonly BindableProperty SizeProperty =
        BindableProperty.CreateAttached(
            "Size",
            typeof(BootstrapSize),
            typeof(Bootstrap),
            BootstrapSize.Default,
            propertyChanged: OnSizeChanged);

    public static BootstrapSize GetSize(BindableObject view) =>
        (BootstrapSize)view.GetValue(SizeProperty);

    public static void SetSize(BindableObject view, BootstrapSize value) =>
        view.SetValue(SizeProperty, value);

    // ── IsOutlined Property (shorthand for outline variants) ──
    
    public static readonly BindableProperty IsOutlinedProperty =
        BindableProperty.CreateAttached(
            "IsOutlined",
            typeof(bool),
            typeof(Bootstrap),
            false,
            propertyChanged: OnIsOutlinedChanged);

    public static bool GetIsOutlined(BindableObject view) =>
        (bool)view.GetValue(IsOutlinedProperty);

    public static void SetIsOutlined(BindableObject view, bool value) =>
        view.SetValue(IsOutlinedProperty, value);

    // ── IsPill Property (full rounded corners) ──
    
    public static readonly BindableProperty IsPillProperty =
        BindableProperty.CreateAttached(
            "IsPill",
            typeof(bool),
            typeof(Bootstrap),
            false);

    public static bool GetIsPill(BindableObject view) =>
        (bool)view.GetValue(IsPillProperty);

    public static void SetIsPill(BindableObject view, bool value) =>
        view.SetValue(IsPillProperty, value);

    // ── Property Changed Handlers ──
    
    private static void OnVariantChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Button button)
            ApplyButtonVariant(button, (BootstrapVariant)newValue);
        else if (bindable is Entry entry)
            ApplyEntryVariant(entry, (BootstrapVariant)newValue);
        // Handler updates will be triggered via the handler mapper
        if (bindable is VisualElement ve)
            ve.Handler?.UpdateValue(nameof(VariantProperty));
    }

    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is VisualElement ve)
            ve.Handler?.UpdateValue(nameof(SizeProperty));
    }

    private static void OnIsOutlinedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is VisualElement ve)
            ve.Handler?.UpdateValue(nameof(IsOutlinedProperty));
    }

    // ── Apply Helpers ──
    
    private static void ApplyButtonVariant(Button button, BootstrapVariant variant)
    {
        var theme = BootstrapTheme.Current;
        var (bg, fg) = GetVariantColors(variant, theme);
        
        button.BackgroundColor = bg;
        button.TextColor = fg;
        button.CornerRadius = (int)theme.CornerRadius;
    }

    private static void ApplyEntryVariant(Entry entry, BootstrapVariant variant)
    {
        var theme = BootstrapTheme.Current;
        
        // Entry variant typically affects border color on validation states
        if (variant == BootstrapVariant.Danger)
            entry.BackgroundColor = theme.Danger.WithAlpha(0.1f);
        else if (variant == BootstrapVariant.Success)
            entry.BackgroundColor = theme.Success.WithAlpha(0.1f);
    }

    /// <summary>
    /// Gets background and foreground colors for a variant.
    /// </summary>
    public static (Color Background, Color Foreground) GetVariantColors(BootstrapVariant variant, BootstrapTheme? theme = null)
    {
        theme ??= BootstrapTheme.Current;
        
        return variant switch
        {
            BootstrapVariant.Primary => (theme.Primary, theme.OnPrimary),
            BootstrapVariant.Secondary => (theme.Secondary, theme.OnSecondary),
            BootstrapVariant.Success => (theme.Success, theme.OnSuccess),
            BootstrapVariant.Danger => (theme.Danger, theme.OnDanger),
            BootstrapVariant.Warning => (theme.Warning, theme.OnWarning),
            BootstrapVariant.Info => (theme.Info, theme.OnInfo),
            BootstrapVariant.Light => (theme.Light, theme.Dark),
            BootstrapVariant.Dark => (theme.Dark, theme.Light),
            
            BootstrapVariant.OutlinePrimary => (Colors.Transparent, theme.Primary),
            BootstrapVariant.OutlineSecondary => (Colors.Transparent, theme.Secondary),
            BootstrapVariant.OutlineSuccess => (Colors.Transparent, theme.Success),
            BootstrapVariant.OutlineDanger => (Colors.Transparent, theme.Danger),
            BootstrapVariant.OutlineWarning => (Colors.Transparent, theme.Warning),
            BootstrapVariant.OutlineInfo => (Colors.Transparent, theme.Info),
            BootstrapVariant.OutlineLight => (Colors.Transparent, theme.Light),
            BootstrapVariant.OutlineDark => (Colors.Transparent, theme.Dark),
            
            BootstrapVariant.Link => (Colors.Transparent, theme.Primary),
            
            _ => (theme.GetSurface(), theme.GetOnSurface())
        };
    }

    /// <summary>
    /// Gets the border color for an outline variant.
    /// </summary>
    public static Color GetOutlineBorderColor(BootstrapVariant variant, BootstrapTheme? theme = null)
    {
        theme ??= BootstrapTheme.Current;
        
        return variant switch
        {
            BootstrapVariant.OutlinePrimary => theme.Primary,
            BootstrapVariant.OutlineSecondary => theme.Secondary,
            BootstrapVariant.OutlineSuccess => theme.Success,
            BootstrapVariant.OutlineDanger => theme.Danger,
            BootstrapVariant.OutlineWarning => theme.Warning,
            BootstrapVariant.OutlineInfo => theme.Info,
            BootstrapVariant.OutlineLight => theme.Light,
            BootstrapVariant.OutlineDark => theme.Dark,
            _ => theme.GetOutline()
        };
    }
}
