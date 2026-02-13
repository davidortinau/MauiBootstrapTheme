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

namespace MauiBootstrapTheme.Handlers;

/// <summary>
/// Handler extensions for Stepper control.
/// </summary>
public static class BootstrapStepperHandler
{
    public static void Register()
    {
        StepperHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    }

    private static void ApplyBootstrapStyle(IStepperHandler handler, IStepper stepper)
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
}
