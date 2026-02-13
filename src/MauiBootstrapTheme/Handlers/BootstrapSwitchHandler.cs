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

namespace MauiBootstrapTheme.Handlers;

/// <summary>
/// Handler extensions for Switch control.
/// </summary>
public static class BootstrapSwitchHandler
{
    public static void Register()
    {
        SwitchHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    }

    private static void ApplyBootstrapStyle(ISwitchHandler handler, ISwitch sw)
    {
        var theme = BootstrapTheme.Current;
        var view = sw as Switch;
        
        var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
        var onColor = GetOnColor(variant, theme);
        var offColor = theme.GetOutline();

#if ANDROID
        ApplyAndroid(handler, onColor, offColor, theme);
#elif IOS || MACCATALYST
        ApplyiOS(handler, onColor, offColor, theme);
#elif WINDOWS
        ApplyWindows(handler, onColor, offColor, theme);
#endif
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
                theme.GetSurface().ToPlatform()
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
        sw.ThumbTintColor = theme.GetSurface().ToPlatform();
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
}
