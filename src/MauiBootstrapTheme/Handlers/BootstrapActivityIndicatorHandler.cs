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
/// Handler extensions for ActivityIndicator control.
/// </summary>
public static class BootstrapActivityIndicatorHandler
{
    public static void Register()
    {
        ActivityIndicatorHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
        ActivityIndicatorHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
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

    private static readonly ConditionalWeakTable<object, StrongBox<double>> _originalOpacity = new();

    private static void ApplyDisabledState(IActivityIndicatorHandler handler, IActivityIndicator control)
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
}
