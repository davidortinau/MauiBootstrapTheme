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
/// Handler extensions for Switch control.
/// </summary>
public static class BootstrapSwitchHandler
{
    public static void Register()
    {
        SwitchHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
        SwitchHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
    }

    private static void ApplyBootstrapStyle(ISwitchHandler handler, ISwitch sw)
    {
        try
        {
            var theme = BootstrapTheme.Current;
            var view = sw as Switch;

            var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
            var onColor = GetOnColor(variant, theme);
            var offColor = theme.Outline;

#if ANDROID
            ApplyAndroid(handler, onColor, offColor, theme);
#elif IOS || MACCATALYST
            ApplyiOS(handler, onColor, offColor, theme);
#elif WINDOWS
            ApplyWindows(handler, onColor, offColor, theme);
#endif
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BootstrapTheme: Handler error: {ex.Message}");
        }
    }

#if ANDROID
    private static void ApplyAndroid(ISwitchHandler handler, Color onColor, Color offColor, BootstrapTheme theme)
    {
        var sw = handler.PlatformView;
        if (sw == null) return;

        // Track color
        var trackColors = new Android.Content.Res.ColorStateList(
            new int[][] {
                new int[] { Android.Resource.Attribute.StateChecked },
                new int[] { -Android.Resource.Attribute.StateChecked }
            },
            new int[] {
                onColor.WithAlpha(0.5f).ToPlatform(),
                offColor.ToPlatform()
            });
        sw.TrackTintList = trackColors;

        // Thumb color
        var thumbColors = new Android.Content.Res.ColorStateList(
            new int[][] {
                new int[] { Android.Resource.Attribute.StateChecked },
                new int[] { -Android.Resource.Attribute.StateChecked }
            },
            new int[] {
                onColor.ToPlatform(),
                theme.Surface.ToPlatform()
            });
        sw.ThumbTintList = thumbColors;
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(ISwitchHandler handler, Color onColor, Color offColor, BootstrapTheme theme)
    {
        var sw = handler.PlatformView;
        if (sw == null) return;

        sw.OnTintColor = onColor.ToPlatform();
        sw.ThumbTintColor = theme.Surface.ToPlatform();
    }
#endif

#if WINDOWS
    private static void ApplyWindows(ISwitchHandler handler, Color onColor, Color offColor, BootstrapTheme theme)
    {
        var toggleSwitch = handler.PlatformView;
        if (toggleSwitch == null) return;

        // Windows ToggleSwitch has limited styling options
        toggleSwitch.OnContent = null;
        toggleSwitch.OffContent = null;
    }
#endif

    private static Color GetOnColor(BootstrapVariant variant, BootstrapTheme theme) => variant switch
    {
        BootstrapVariant.Primary => theme.Primary,
        BootstrapVariant.Secondary => theme.Secondary,
        BootstrapVariant.Success => theme.Success,
        BootstrapVariant.Danger => theme.Danger,
        BootstrapVariant.Warning => theme.Warning,
        BootstrapVariant.Info => theme.Info,
        _ => theme.Primary
    };

    private static readonly ConditionalWeakTable<object, StrongBox<double>> _originalOpacity = new();

    private static void ApplyDisabledState(ISwitchHandler handler, ISwitch control)
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
