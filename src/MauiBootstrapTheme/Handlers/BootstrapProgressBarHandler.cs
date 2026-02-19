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

using System.Runtime.CompilerServices;

namespace MauiBootstrapTheme.Handlers;

/// <summary>
/// Handler extensions for ProgressBar control.
/// </summary>
public static class BootstrapProgressBarHandler
{
    public static void Register()
    {
        ProgressBarHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
        ProgressBarHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
    }

    private static void ApplyBootstrapStyle(IProgressBarHandler handler, IProgress progressBar)
    {
        try
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
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BootstrapTheme: Handler error: {ex.Message}");
        }
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
        
        // Instead of scaling which distorts corners, use a constraint or frame adjustment
        // However, UIProgressView height is stubborn.
        // A better approach for consistent height and corners is to use a layer mask or just accept the transform limitation
        // But for "Highest-impact minimal code fixes", maybe we can just fix the corner radius logic slightly
        // or ensure we aren't distorting it too much.
        
        // Actually, let's try to set the frame height if possible via layout, but handler is limited.
        // The scale transform is the standard workaround for UIProgressView height.
        
        var scaleY = (nfloat)(theme.ProgressHeight / 4.0);
        progressView.Transform = CGAffineTransform.MakeScale(1.0f, scaleY);
        
        // To fix distorted corners, we can try to apply corner radius to the layer *after* transform? 
        // No, layer transform applies to everything.
        
        // If we want perfect corners, we might need to not use UIProgressView or accept the distortion.
        // But let's look at the metrics again. "Default,controls" is bad.
        // Maybe the issue is simpler: colors or sizing mismatch.
        
        progressView.ClipsToBounds = true;
        progressView.Layer.CornerRadius = (nfloat)(theme.CornerRadius / scaleY);
        progressView.Layer.MasksToBounds = true;
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

    private static readonly ConditionalWeakTable<object, StrongBox<double>> _originalOpacity = new();

    private static void ApplyDisabledState(IProgressBarHandler handler, IProgress control)
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
