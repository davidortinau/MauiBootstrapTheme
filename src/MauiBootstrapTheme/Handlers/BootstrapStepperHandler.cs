using MauiBootstrapTheme.Theming;
using Microsoft.Maui.Handlers;
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
/// Handler extensions for Stepper control.
/// </summary>
public static class BootstrapStepperHandler
{
    public static void Register()
    {
        StepperHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
        StepperHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
    }

    private static void ApplyBootstrapStyle(IStepperHandler handler, IStepper stepper)
    {
        try
        {
            var theme = BootstrapTheme.Current;
            var view = stepper as Stepper;

            var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
            var accentColor = GetAccentColor(variant, theme);

#if ANDROID
            ApplyAndroid(handler, accentColor, theme);
#elif IOS || MACCATALYST
            ApplyiOS(handler, accentColor, theme);
#elif WINDOWS
            ApplyWindows(handler, accentColor, theme);
#endif
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BootstrapTheme: Handler error: {ex.Message}");
        }
    }

#if ANDROID
    private static void ApplyAndroid(IStepperHandler handler, Color accentColor, BootstrapTheme theme)
    {
        // Android Stepper is rendered as two buttons
        var platformView = handler.PlatformView;
        if (platformView == null) return;

        // Set background tint via the container
        platformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(IStepperHandler handler, Color accentColor, BootstrapTheme theme)
    {
        var uiStepper = handler.PlatformView;
        if (uiStepper == null) return;

        uiStepper.TintColor = accentColor.ToPlatform();
    }
#endif

#if WINDOWS
    private static void ApplyWindows(IStepperHandler handler, Color accentColor, BootstrapTheme theme)
    {
        // Windows Stepper has limited styling options through handler
        var platformView = handler.PlatformView;
        if (platformView == null) return;
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

    private static readonly ConditionalWeakTable<object, StrongBox<double>> _originalOpacity = new();

    private static void ApplyDisabledState(IStepperHandler handler, IStepper control)
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
