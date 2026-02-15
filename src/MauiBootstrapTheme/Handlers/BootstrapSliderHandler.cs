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
/// Handler extensions for Slider control.
/// </summary>
public static class BootstrapSliderHandler
{
    public static void Register()
    {
        SliderHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    }

    private static void ApplyBootstrapStyle(ISliderHandler handler, ISlider slider)
    {
        var theme = BootstrapTheme.Current;
        var view = slider as Slider;
        
        var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
        var accentColor = GetAccentColor(variant, theme);
        var trackColor = theme.Outline;

#if ANDROID
        ApplyAndroid(handler, accentColor, trackColor, theme);
#elif IOS || MACCATALYST
        ApplyiOS(handler, accentColor, trackColor, theme);
#elif WINDOWS
        ApplyWindows(handler, accentColor, trackColor, theme);
#endif
    }

#if ANDROID
    private static void ApplyAndroid(ISliderHandler handler, Color accentColor, Color trackColor, BootstrapTheme theme)
    {
        var seekBar = handler.PlatformView;
        if (seekBar == null) return;

        // Progress tint (filled portion)
        seekBar.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(accentColor.ToPlatform());
        
        // Background tint (unfilled portion)
        seekBar.ProgressBackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(trackColor.ToPlatform());
        
        // Thumb tint
        seekBar.ThumbTintList = Android.Content.Res.ColorStateList.ValueOf(accentColor.ToPlatform());
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(ISliderHandler handler, Color accentColor, Color trackColor, BootstrapTheme theme)
    {
        var uiSlider = handler.PlatformView;
        if (uiSlider == null) return;

        uiSlider.MinimumTrackTintColor = accentColor.ToPlatform();
        uiSlider.MaximumTrackTintColor = trackColor.ToPlatform();
        uiSlider.ThumbTintColor = accentColor.ToPlatform();
    }
#endif

#if WINDOWS
    private static void ApplyWindows(ISliderHandler handler, Color accentColor, Color trackColor, BootstrapTheme theme)
    {
        var slider = handler.PlatformView;
        if (slider == null) return;

        // Windows Slider has limited styling through the handler
        slider.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(accentColor.ToWindowsColor());
    }
#endif

    private static Color GetAccentColor(BootstrapVariant variant, BootstrapTheme theme) => variant switch
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
