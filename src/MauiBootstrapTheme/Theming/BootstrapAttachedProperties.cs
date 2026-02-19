namespace MauiBootstrapTheme.Theming;

/// <summary>
/// Attached properties for applying Bootstrap styling to individual controls.
/// </summary>
public static class Bootstrap
{
    // ══════════════════════════════════════════════════════════════════
    // VARIANT & SIZE (Existing)
    // ══════════════════════════════════════════════════════════════════
    
    public static readonly BindableProperty VariantProperty =
        BindableProperty.CreateAttached(
            "Variant",
            typeof(BootstrapVariant),
            typeof(Bootstrap),
            BootstrapVariant.Default,
            propertyChanged: OnStylePropertyChanged);

    public static BootstrapVariant GetVariant(BindableObject view) =>
        (BootstrapVariant)view.GetValue(VariantProperty);

    public static void SetVariant(BindableObject view, BootstrapVariant value) =>
        view.SetValue(VariantProperty, value);

    public static readonly BindableProperty SizeProperty =
        BindableProperty.CreateAttached(
            "Size",
            typeof(BootstrapSize),
            typeof(Bootstrap),
            BootstrapSize.Default,
            propertyChanged: OnStylePropertyChanged);

    public static BootstrapSize GetSize(BindableObject view) =>
        (BootstrapSize)view.GetValue(SizeProperty);

    public static void SetSize(BindableObject view, BootstrapSize value) =>
        view.SetValue(SizeProperty, value);

    public static readonly BindableProperty IsOutlinedProperty =
        BindableProperty.CreateAttached(
            "IsOutlined",
            typeof(bool),
            typeof(Bootstrap),
            false,
            propertyChanged: OnStylePropertyChanged);

    public static bool GetIsOutlined(BindableObject view) =>
        (bool)view.GetValue(IsOutlinedProperty);

    public static void SetIsOutlined(BindableObject view, bool value) =>
        view.SetValue(IsOutlinedProperty, value);

    public static readonly BindableProperty IsPillProperty =
        BindableProperty.CreateAttached(
            "IsPill",
            typeof(bool),
            typeof(Bootstrap),
            false,
            propertyChanged: OnStylePropertyChanged);

    public static bool GetIsPill(BindableObject view) =>
        (bool)view.GetValue(IsPillProperty);

    public static void SetIsPill(BindableObject view, bool value) =>
        view.SetValue(IsPillProperty, value);

    // ══════════════════════════════════════════════════════════════════
    // TYPOGRAPHY (Labels)
    // ══════════════════════════════════════════════════════════════════

    /// <summary>Heading level 1-6 (like h1-h6 in HTML).</summary>
    public static readonly BindableProperty HeadingProperty =
        BindableProperty.CreateAttached(
            "Heading",
            typeof(int),
            typeof(Bootstrap),
            0,
            propertyChanged: OnTypographyChanged);

    public static int GetHeading(BindableObject view) =>
        (int)view.GetValue(HeadingProperty);

    public static void SetHeading(BindableObject view, int value) =>
        view.SetValue(HeadingProperty, value);

    /// <summary>Text style (Lead, Small, Muted, Mark).</summary>
    public static readonly BindableProperty TextStyleProperty =
        BindableProperty.CreateAttached(
            "TextStyle",
            typeof(BootstrapTextStyle),
            typeof(Bootstrap),
            BootstrapTextStyle.Normal,
            propertyChanged: OnTypographyChanged);

    public static BootstrapTextStyle GetTextStyle(BindableObject view) =>
        (BootstrapTextStyle)view.GetValue(TextStyleProperty);

    public static void SetTextStyle(BindableObject view, BootstrapTextStyle value) =>
        view.SetValue(TextStyleProperty, value);

    /// <summary>Text color variant.</summary>
    public static readonly BindableProperty TextColorVariantProperty =
        BindableProperty.CreateAttached(
            "TextColorVariant",
            typeof(BootstrapVariant),
            typeof(Bootstrap),
            BootstrapVariant.Default,
            propertyChanged: OnTypographyChanged);

    public static BootstrapVariant GetTextColorVariant(BindableObject view) =>
        (BootstrapVariant)view.GetValue(TextColorVariantProperty);

    public static void SetTextColorVariant(BindableObject view, BootstrapVariant value) =>
        view.SetValue(TextColorVariantProperty, value);

    // ══════════════════════════════════════════════════════════════════
    // SPACING (Margin & Padding 0-5)
    // ══════════════════════════════════════════════════════════════════

    /// <summary>Bootstrap margin level (0-5). -1 means not set.</summary>
    public static readonly BindableProperty MarginLevelProperty =
        BindableProperty.CreateAttached(
            "MarginLevel",
            typeof(int),
            typeof(Bootstrap),
            -1,
            propertyChanged: OnSpacingChanged);

    public static int GetMarginLevel(BindableObject view) =>
        (int)view.GetValue(MarginLevelProperty);

    public static void SetMarginLevel(BindableObject view, int value) =>
        view.SetValue(MarginLevelProperty, value);

    /// <summary>Bootstrap padding level (0-5). -1 means not set.</summary>
    public static readonly BindableProperty PaddingLevelProperty =
        BindableProperty.CreateAttached(
            "PaddingLevel",
            typeof(int),
            typeof(Bootstrap),
            -1,
            propertyChanged: OnSpacingChanged);

    public static int GetPaddingLevel(BindableObject view) =>
        (int)view.GetValue(PaddingLevelProperty);

    public static void SetPaddingLevel(BindableObject view, int value) =>
        view.SetValue(PaddingLevelProperty, value);

    // ══════════════════════════════════════════════════════════════════
    // CONTAINERS (Background, Shadow)
    // ══════════════════════════════════════════════════════════════════

    /// <summary>Background color variant.</summary>
    public static readonly BindableProperty BackgroundVariantProperty =
        BindableProperty.CreateAttached(
            "BackgroundVariant",
            typeof(BootstrapVariant),
            typeof(Bootstrap),
            BootstrapVariant.Default,
            propertyChanged: OnContainerChanged);

    public static BootstrapVariant GetBackgroundVariant(BindableObject view) =>
        (BootstrapVariant)view.GetValue(BackgroundVariantProperty);

    public static void SetBackgroundVariant(BindableObject view, BootstrapVariant value) =>
        view.SetValue(BackgroundVariantProperty, value);

    /// <summary>Shadow level.</summary>
    public static readonly BindableProperty ShadowProperty =
        BindableProperty.CreateAttached(
            "Shadow",
            typeof(BootstrapShadow),
            typeof(Bootstrap),
            BootstrapShadow.None,
            propertyChanged: OnContainerChanged);

    public static BootstrapShadow GetShadow(BindableObject view) =>
        (BootstrapShadow)view.GetValue(ShadowProperty);

    public static void SetShadow(BindableObject view, BootstrapShadow value) =>
        view.SetValue(ShadowProperty, value);

    // ══════════════════════════════════════════════════════════════════
    // BADGES
    // ══════════════════════════════════════════════════════════════════

    /// <summary>Badge variant (makes Label display as a badge).</summary>
    public static readonly BindableProperty BadgeProperty =
        BindableProperty.CreateAttached(
            "Badge",
            typeof(BootstrapVariant),
            typeof(Bootstrap),
            BootstrapVariant.Default,
            propertyChanged: OnBadgeChanged);

    public static BootstrapVariant GetBadge(BindableObject view) =>
        (BootstrapVariant)view.GetValue(BadgeProperty);

    public static void SetBadge(BindableObject view, BootstrapVariant value) =>
        view.SetValue(BadgeProperty, value);

    // ══════════════════════════════════════════════════════════════════
    // NAVIGATION
    // ══════════════════════════════════════════════════════════════════

    /// <summary>Navigation style (Light or Dark).</summary>
    public static readonly BindableProperty NavStyleProperty =
        BindableProperty.CreateAttached(
            "NavStyle",
            typeof(BootstrapNavStyle),
            typeof(Bootstrap),
            BootstrapNavStyle.Light,
            propertyChanged: OnNavStyleChanged);

    public static BootstrapNavStyle GetNavStyle(BindableObject view) =>
        (BootstrapNavStyle)view.GetValue(NavStyleProperty);

    public static void SetNavStyle(BindableObject view, BootstrapNavStyle value) =>
        view.SetValue(NavStyleProperty, value);

    // ══════════════════════════════════════════════════════════════════
    // PROPERTY CHANGED HANDLERS
    // ══════════════════════════════════════════════════════════════════

    private static void OnStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Button button)
            ApplyButtonVariant(button);
        else if (bindable is Entry entry)
            ApplyEntryVariant(entry);
        
        TriggerHandlerUpdate(bindable);
    }

    private static void OnTypographyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Label label)
            ApplyLabelTypography(label);
    }

    private static void OnSpacingChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is View view)
            ApplySpacing(view);
    }

    private static void OnContainerChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is View view)
            ApplyContainerStyle(view);
    }

    private static void OnBadgeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Label label)
            ApplyBadgeStyle(label);
    }

    private static void OnNavStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TriggerHandlerUpdate(bindable);
    }

    private static void TriggerHandlerUpdate(BindableObject bindable)
    {
        if (bindable is VisualElement ve)
            ve.Handler?.UpdateValue("BootstrapStyle");
    }

    // ══════════════════════════════════════════════════════════════════
    // APPLY HELPERS
    // ══════════════════════════════════════════════════════════════════

    private static void ApplyButtonVariant(Button button)
    {
        var theme = BootstrapTheme.Current;
        var variant = GetVariant(button);
        var isOutlined = GetIsOutlined(button);
        
        if (isOutlined && variant != BootstrapVariant.Default)
        {
            button.Background = Colors.Transparent;
            button.TextColor = theme.GetVariantColor(variant);
            button.BorderColor = theme.GetVariantColor(variant);
            button.BorderWidth = theme.BorderWidth;
        }
        else
        {
            var (bg, fg) = GetVariantColors(variant, theme);
            button.Background = bg;
            button.TextColor = fg;
        }
        
        button.CornerRadius = GetIsPill(button) 
            ? (int)theme.CornerRadiusPill 
            : (int)theme.CornerRadius;
    }

    private static void ApplyEntryVariant(Entry entry)
    {
        var theme = BootstrapTheme.Current;
        var variant = GetVariant(entry);
        
        if (variant == BootstrapVariant.Danger)
            entry.Background = new SolidColorBrush(theme.Danger.WithAlpha(0.1f));
        else if (variant == BootstrapVariant.Success)
            entry.Background = new SolidColorBrush(theme.Success.WithAlpha(0.1f));
    }

    private static void ApplyLabelTypography(Label label)
    {
        var theme = BootstrapTheme.Current;
        var heading = GetHeading(label);
        var textStyle = GetTextStyle(label);
        var colorVariant = GetTextColorVariant(label);

        // Apply heading
        if (heading >= 1 && heading <= 6)
        {
            label.FontSize = theme.GetHeadingFontSize(heading);
            label.FontAttributes = FontAttributes.Bold;
            label.LineHeight = theme.LineHeightHeading;
        }

        // Apply text style
        switch (textStyle)
        {
            case BootstrapTextStyle.Lead:
                label.FontSize = theme.FontSizeLead;
                label.LineHeight = theme.LineHeightLead;
                break;
            case BootstrapTextStyle.Small:
                label.FontSize = theme.FontSizeSmall;
                break;
            case BootstrapTextStyle.Muted:
                label.TextColor = theme.Muted;
                break;
            case BootstrapTextStyle.Mark:
                label.Background = theme.Mark;
                break;
        }

        // Apply color variant
        if (colorVariant != BootstrapVariant.Default)
        {
            label.TextColor = theme.GetVariantColor(colorVariant);
        }
    }

    private static void ApplySpacing(View view)
    {
        var marginLevel = GetMarginLevel(view);
        var paddingLevel = GetPaddingLevel(view);

        if (marginLevel >= 0 && marginLevel <= 5)
        {
            var margin = BootstrapTheme.GetSpacingValue(marginLevel);
            view.Margin = new Thickness(margin);
        }

        if (paddingLevel >= 0 && paddingLevel <= 5)
        {
            var padding = BootstrapTheme.GetSpacingValue(paddingLevel);
            // Padding is only available on Layout and some controls
            if (view is Layout layout)
            {
                layout.Padding = new Thickness(padding);
            }
            else if (view is Border border)
            {
                border.Padding = new Thickness(padding);
            }
            else if (view is Button button)
            {
                button.Padding = new Thickness(padding);
            }
            else if (view is Label label)
            {
                label.Padding = new Thickness(padding);
            }
        }
    }

    private static void ApplyContainerStyle(View view)
    {
        var theme = BootstrapTheme.Current;
        var bgVariant = GetBackgroundVariant(view);
        var shadow = GetShadow(view);

        // Apply background
        if (bgVariant != BootstrapVariant.Default)
        {
            view.Background = theme.GetVariantColor(bgVariant);
        }

        // Apply shadow
        view.Shadow = shadow switch
        {
            BootstrapShadow.Small => new Shadow { Brush = Colors.Black, Offset = new Point(0, 2), Radius = 4, Opacity = 0.075f },
            BootstrapShadow.Default => new Shadow { Brush = Colors.Black, Offset = new Point(0, 8), Radius = 16, Opacity = 0.15f },
            BootstrapShadow.Large => new Shadow { Brush = Colors.Black, Offset = new Point(0, 16), Radius = 48, Opacity = 0.175f },
            _ => null
        };
    }

    private static void ApplyBadgeStyle(Label label)
    {
        var theme = BootstrapTheme.Current;
        var badgeVariant = GetBadge(label);

        if (badgeVariant == BootstrapVariant.Default)
            return;

        var (bg, fg) = GetVariantColors(badgeVariant, theme);
        label.Background = bg;
        label.TextColor = fg;
        label.Padding = new Thickness(8, 4);
        label.FontSize = theme.FontSizeSmall;
    }

    // ══════════════════════════════════════════════════════════════════
    // PUBLIC HELPERS
    // ══════════════════════════════════════════════════════════════════

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
            
            _ => (theme.Surface, theme.OnSurface)
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
            BootstrapVariant.Primary or BootstrapVariant.OutlinePrimary => theme.Primary,
            BootstrapVariant.Secondary or BootstrapVariant.OutlineSecondary => theme.Secondary,
            BootstrapVariant.Success or BootstrapVariant.OutlineSuccess => theme.Success,
            BootstrapVariant.Danger or BootstrapVariant.OutlineDanger => theme.Danger,
            BootstrapVariant.Warning or BootstrapVariant.OutlineWarning => theme.Warning,
            BootstrapVariant.Info or BootstrapVariant.OutlineInfo => theme.Info,
            BootstrapVariant.Light or BootstrapVariant.OutlineLight => theme.Light,
            BootstrapVariant.Dark or BootstrapVariant.OutlineDark => theme.Dark,
            _ => theme.Outline
        };
    }
}
