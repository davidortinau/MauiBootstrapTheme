using MauiBootstrapTheme.Theming;
using Microsoft.Maui.Handlers;
#if ANDROID
using Microsoft.Maui.Platform;
#elif IOS || MACCATALYST
using UIKit;
using CoreGraphics;
using Microsoft.Maui.Platform;
#elif WINDOWS
using Microsoft.Maui.Platform;
#endif

namespace MauiBootstrapTheme.Handlers;

/// <summary>
/// Handler extensions for ProgressBar control.
/// </summary>
public static class BootstrapProgressBarHandler
{
    public static void Register()
    {
        ProgressBarHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    }

    private static void ApplyBootstrapStyle(IProgressBarHandler handler, IProgress progressBar)
    {
        var theme = BootstrapTheme.Current;
        var view = progressBar as ProgressBar;
        
        var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
        var progressColor = GetProgressColor(variant, theme);
        // Use solid track color for visibility (Bootstrap uses solid gray background)
        var trackColor = theme.ProgressBackground;

#if ANDROID
        ApplyAndroid(handler, progressColor, trackColor, theme);
#elif IOS || MACCATALYST
        ApplyiOS(handler, progressColor, trackColor, theme);
#elif WINDOWS
        ApplyWindows(handler, progressColor, trackColor, theme);
#endif
    }

#if ANDROID
    private static void ApplyAndroid(IProgressBarHandler handler, Color progressColor, Color trackColor, BootstrapTheme theme)
    {
        var progressBar = handler.PlatformView;
        if (progressBar == null) return;

        progressBar.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(progressColor.ToPlatform());
        progressBar.ProgressBackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(trackColor.ToPlatform());
        
        // Set minimum height to match Bootstrap (16px)
        var density = progressBar.Context?.Resources?.DisplayMetrics?.Density ?? 1;
        progressBar.SetMinimumHeight((int)(theme.ProgressHeight * density));
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(IProgressBarHandler handler, Color progressColor, Color trackColor, BootstrapTheme theme)
    {
        var progressView = handler.PlatformView;
        if (progressView == null) return;

        progressView.ProgressTintColor = progressColor.ToPlatform();
        progressView.TrackTintColor = trackColor.ToPlatform();
        
        // iOS UIProgressView is very thin by default (4px)
        // Scale transform to make it match Bootstrap's 16px height
        // 16px / 4px = 4x scale on Y axis
        var scale = theme.ProgressHeight / 4.0;
        progressView.Transform = CGAffineTransform.MakeScale(1.0f, (nfloat)scale);
        progressView.ClipsToBounds = true;
        progressView.Layer.CornerRadius = (nfloat)(theme.CornerRadius / 2.0);
    }
#endif

#if WINDOWS
    private static void ApplyWindows(IProgressBarHandler handler, Color progressColor, Color trackColor, BootstrapTheme theme)
    {
        var progressBar = handler.PlatformView;
        if (progressBar == null) return;

        progressBar.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(progressColor.ToWindowsColor());
        progressBar.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(trackColor.ToWindowsColor());
        progressBar.MinHeight = theme.ProgressHeight;
    }
#endif

    private static Color GetProgressColor(BootstrapVariant variant, BootstrapTheme theme) => variant switch
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
