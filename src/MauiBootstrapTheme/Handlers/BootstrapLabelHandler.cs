using MauiBootstrapTheme.Theming;
using Microsoft.Maui.Handlers;
#if ANDROID
using Microsoft.Maui.Platform;
#elif IOS || MACCATALYST
using UIKit;
using Microsoft.Maui.Platform;
#elif WINDOWS
using Microsoft.Maui.Platform;
#endif

using System.Runtime.CompilerServices;

namespace MauiBootstrapTheme.Handlers;

/// <summary>
/// Handler extensions for Label control.
/// Supports headings (H1-H6), text styles, badges, and color variants.
/// </summary>
public static class BootstrapLabelHandler
{
    public static void Register()
    {
        LabelHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
        LabelHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
    }

    private static void ApplyBootstrapStyle(ILabelHandler handler, ILabel label)
    {
        try
        {
            var theme = BootstrapTheme.Current;
            var view = label as Label;
            if (view == null) return;

            var heading = Bootstrap.GetHeading(view);
            var textStyle = Bootstrap.GetTextStyle(view);
            var textColorVariant = Bootstrap.GetTextColorVariant(view);
            var badgeVariant = Bootstrap.GetBadge(view);
            var variant = Bootstrap.GetVariant(view);

            // Apply heading styles
            if (heading > 0 && heading <= 6)
            {
                ApplyHeading(view, heading, theme);
            }

            // Apply text styles
            if (textStyle != BootstrapTextStyle.Normal)
            {
                ApplyTextStyle(view, textStyle, theme);
            }

            // Apply color variant for text
            if (textColorVariant != BootstrapVariant.Default)
            {
                view.TextColor = theme.GetVariantColor(textColorVariant);
            }

            // Apply badge styling (when Badge property is set to a variant other than Default)
            if (badgeVariant != BootstrapVariant.Default)
            {
                ApplyBadge(view, badgeVariant, theme);
            }

            // Apply font family to ALL labels if theme specifies one (and not already set)
            if (!string.IsNullOrEmpty(theme.FontFamily) && string.IsNullOrEmpty(view.FontFamily))
            {
                view.FontFamily = theme.FontFamily;
            }

            // Apply text color for labels that don't have any specific variant/badge/heading
            // Only apply if the handler's theme OnBackground differs from what the style set
            // Skip this — the implicit Label style in ResourceDictionary already sets TextColor
            // via {DynamicResource OnBackground}. We should not override it here.
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BootstrapTheme: Handler error: {ex.Message}");
        }
    }

    private static void ApplyHeading(Label label, int level, BootstrapTheme theme)
    {
        var fontSize = theme.GetHeadingFontSize(level);
        label.FontSize = fontSize;
        label.FontAttributes = FontAttributes.Bold;
        // TextColor is handled by StyleClass h1-h6 via {DynamicResource HeadingColor}
        label.LineHeight = theme.LineHeightHeading;
        
        // Apply font family if specified
        if (!string.IsNullOrEmpty(theme.FontFamily))
        {
            label.FontFamily = theme.FontFamily;
        }
        
        // Add bottom margin based on heading level
        var marginBottom = level <= 2 ? 16.0 : (level <= 4 ? 12.0 : 8.0);
        label.Margin = new Thickness(0, 0, 0, marginBottom);
    }

    private static void ApplyTextStyle(Label label, BootstrapTextStyle style, BootstrapTheme theme)
    {
        switch (style)
        {
            case BootstrapTextStyle.Lead:
                label.FontSize = theme.FontSizeLead;
                label.TextColor = theme.GetText();
                label.LineHeight = theme.LineHeightLead;
                break;
            case BootstrapTextStyle.Small:
                label.FontSize = theme.FontSizeSmall;
                break;
            case BootstrapTextStyle.Muted:
                label.TextColor = theme.GetMuted();
                break;
            case BootstrapTextStyle.Mark:
                label.Background = new SolidColorBrush(Color.FromArgb("#fcf8e3"));
                label.Padding = new Thickness(4, 2);
                break;
        }
        
        // Apply font family if specified
        if (!string.IsNullOrEmpty(theme.FontFamily))
        {
            label.FontFamily = theme.FontFamily;
        }
    }

    private static void ApplyBadge(Label label, BootstrapVariant variant, BootstrapTheme theme)
    {
        var bgColor = theme.GetVariantColor(variant == BootstrapVariant.Default ? BootstrapVariant.Primary : variant);
        var textColor = ShouldUseDarkText(bgColor) ? Colors.Black : Colors.White;
        
        label.Background = new SolidColorBrush(bgColor);
        label.TextColor = textColor;
        label.FontSize = 12;
        label.Padding = new Thickness(8, 4);
        
        // Note: Corner radius requires Border wrapper or handler extension
    }

    private static bool ShouldUseDarkText(Color bgColor)
    {
        // Calculate relative luminance
        var luminance = 0.299 * bgColor.Red + 0.587 * bgColor.Green + 0.114 * bgColor.Blue;
        return luminance > 0.5;
    }

    private static readonly ConditionalWeakTable<object, StrongBox<double>> _originalOpacity = new();

    private static void ApplyDisabledState(ILabelHandler handler, ILabel control)
    {
        try
        {
            if (control is not VisualElement ve) return;
            var theme = BootstrapTheme.Current;

            if (!ve.IsEnabled)
            {
                _originalOpacity.GetOrCreateValue(control).Value = ve.Opacity;
                ve.Opacity = theme.DisabledOpacity;
            }
            else if (_originalOpacity.TryGetValue(control, out var box))
            {
                ve.Opacity = box.Value;
                _originalOpacity.Remove(control);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BootstrapTheme: Handler error: {ex.Message}");
        }
    }
}
