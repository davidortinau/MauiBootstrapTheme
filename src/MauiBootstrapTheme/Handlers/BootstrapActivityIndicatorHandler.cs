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
/// Handler extensions for ActivityIndicator control.
/// </summary>
public static class BootstrapActivityIndicatorHandler
{
    public static void Register()
    {
        ActivityIndicatorHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    }

    private static void ApplyBootstrapStyle(IActivityIndicatorHandler handler, IActivityIndicator activityIndicator)
    {
        var theme = BootstrapTheme.Current;
        var view = activityIndicator as ActivityIndicator;
        
        var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
        var color = GetSpinnerColor(variant, theme);

#if ANDROID
        ApplyAndroid(handler, color);
#elif IOS || MACCATALYST
        ApplyiOS(handler, color);
#elif WINDOWS
        ApplyWindows(handler, color);
#endif
    }

#if ANDROID
    private static void ApplyAndroid(IActivityIndicatorHandler handler, Color color)
    {
        var progressBar = handler.PlatformView;
        if (progressBar == null) return;

        progressBar.IndeterminateTintList = Android.Content.Res.ColorStateList.ValueOf(color.ToPlatform());
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(IActivityIndicatorHandler handler, Color color)
    {
        var activityIndicator = handler.PlatformView;
        if (activityIndicator == null) return;

        activityIndicator.Color = color.ToPlatform();
    }
#endif

#if WINDOWS
    private static void ApplyWindows(IActivityIndicatorHandler handler, Color color)
    {
        var progressRing = handler.PlatformView;
        if (progressRing == null) return;

        progressRing.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(color.ToWindowsColor());
    }
#endif

    private static Color GetSpinnerColor(BootstrapVariant variant, BootstrapTheme theme) => variant switch
    {
        BootstrapVariant.Primary => theme.Primary,
        BootstrapVariant.Secondary => theme.Secondary,
        BootstrapVariant.Success => theme.Success,
        BootstrapVariant.Danger => theme.Danger,
        BootstrapVariant.Warning => theme.Warning,
        BootstrapVariant.Info => theme.Info,
        BootstrapVariant.Light => theme.GetText().WithAlpha(0.5f),
        BootstrapVariant.Dark => theme.Dark,
        _ => theme.Primary
    };
}
