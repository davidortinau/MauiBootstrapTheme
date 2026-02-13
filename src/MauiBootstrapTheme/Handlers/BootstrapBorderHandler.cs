using MauiBootstrapTheme.Theming;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Controls.Shapes;
#if ANDROID
using Android.Graphics.Drawables;
using Microsoft.Maui.Platform;
#elif IOS || MACCATALYST
using UIKit;
using Microsoft.Maui.Platform;
#elif WINDOWS
using Microsoft.Maui.Platform;
#endif

namespace MauiBootstrapTheme.Handlers;

/// <summary>
/// Handler extensions for Border control.
/// Supports card styling with shadows, variants, and Bootstrap-style borders.
/// </summary>
public static class BootstrapBorderHandler
{
    public static void Register()
    {
        BorderHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    }

    private static void ApplyBootstrapStyle(IBorderHandler handler, IBorderView border)
    {
        var theme = BootstrapTheme.Current;
        var view = border as Border;
        if (view == null) return;

        var variant = Bootstrap.GetVariant(view);
        var shadow = Bootstrap.GetShadow(view);
        var backgroundVariant = Bootstrap.GetBackgroundVariant(view);

        // Apply background variant
        if (backgroundVariant != BootstrapVariant.Default)
        {
            view.BackgroundColor = theme.GetVariantColor(backgroundVariant);
        }
        else
        {
            view.BackgroundColor = theme.GetSurface();
        }

        // Apply border color based on variant
        if (variant != BootstrapVariant.Default)
        {
            view.Stroke = new SolidColorBrush(theme.GetVariantColor(variant));
        }
        else
        {
            view.Stroke = new SolidColorBrush(theme.GetOutline());
        }

        // Apply corner radius and stroke thickness from theme
        view.StrokeThickness = theme.BorderWidth;
        view.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(theme.CornerRadius) };

        // Apply shadow
        ApplyShadow(view, shadow, theme);
    }

    private static void ApplyShadow(Border border, BootstrapShadow shadowLevel, BootstrapTheme theme)
    {
        if (shadowLevel == BootstrapShadow.None)
        {
            border.Shadow = null;
            return;
        }

        float blur, offsetY, opacity;
        switch (shadowLevel)
        {
            case BootstrapShadow.Small:
                blur = theme.ShadowSmBlur;
                offsetY = theme.ShadowSmOffsetY;
                opacity = theme.ShadowSmOpacity;
                break;
            case BootstrapShadow.Large:
                blur = theme.ShadowLgBlur;
                offsetY = theme.ShadowLgOffsetY;
                opacity = theme.ShadowLgOpacity;
                break;
            default: // Default
                blur = theme.ShadowBlur;
                offsetY = theme.ShadowOffsetY;
                opacity = theme.ShadowOpacity;
                break;
        }

        border.Shadow = new Shadow
        {
            Brush = new SolidColorBrush(Colors.Black.WithAlpha(opacity)),
            Offset = new Point(0, offsetY),
            Radius = blur
        };
    }
}
