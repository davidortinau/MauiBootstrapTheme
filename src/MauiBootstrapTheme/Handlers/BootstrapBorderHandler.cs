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

using System.Runtime.CompilerServices;

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
        BorderHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
    }

    private static void ApplyBootstrapStyle(IBorderHandler handler, IBorderView border)
    {
        try
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
                view.Background = new SolidColorBrush(theme.GetVariantColor(backgroundVariant));
                // Also set text color for child labels
                var onColor = Bootstrap.GetVariantColors(backgroundVariant, theme).Foreground;
                foreach (var child in GetDescendants(view, 0, 10))
                {
                    if (child is Label lbl)
                        lbl.TextColor = onColor;
                }
            }
            // Don't override BackgroundColor for borders without BackgroundVariant —
            // the implicit Border style in ResourceDictionary already sets it via {DynamicResource Surface}

            // Apply border color based on variant
            if (variant != BootstrapVariant.Default)
            {
                view.Stroke = new SolidColorBrush(theme.GetVariantColor(variant));
            }
            // Don't override Stroke for borders without Variant —
            // the implicit style handles it via {DynamicResource Outline}

            // Apply corner radius and stroke thickness from theme only when variant/shadow is active
            // But only set StrokeThickness when variant is explicitly set (not for shadow-only),
            // so text-bg-* styles with StrokeThickness=0 are preserved
            if (variant != BootstrapVariant.Default || backgroundVariant != BootstrapVariant.Default)
            {
                view.StrokeThickness = theme.BorderWidth;
                view.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(theme.CornerRadius) };
            }
            else if (shadow != BootstrapShadow.None)
            {
                // Shadow-only: apply corner radius but don't override StrokeThickness
                view.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(theme.CornerRadius) };
            }

            // Apply shadow
            ApplyShadow(view, shadow, theme);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BootstrapTheme: Handler error: {ex.Message}");
        }
    }

    private static IEnumerable<VisualElement> GetDescendants(View view, int depth, int maxDepth)
    {
        if (depth >= maxDepth)
            yield break;

        if (view is IVisualTreeElement tree)
        {
            foreach (var child in tree.GetVisualChildren())
            {
                if (child is VisualElement ve)
                {
                    if (depth > 0 && ve is Border)
                        continue;

                    yield return ve;
                    if (ve is View childView)
                    {
                        foreach (var desc in GetDescendants(childView, depth + 1, maxDepth))
                            yield return desc;
                    }
                }
            }
        }
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

    private static readonly ConditionalWeakTable<object, StrongBox<double>> _originalOpacity = new();

    private static void ApplyDisabledState(IBorderHandler handler, IBorderView control)
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
