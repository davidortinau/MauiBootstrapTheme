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
/// Handler extensions for RadioButton control.
/// </summary>
public static class BootstrapRadioButtonHandler
{
    public static void Register()
    {
        RadioButtonHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
        RadioButtonHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
    }

    private static void ApplyBootstrapStyle(IRadioButtonHandler handler, IRadioButton radioButton)
    {
        var theme = BootstrapTheme.Current;
        var view = radioButton as RadioButton;
        
        var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
        var accentColor = GetAccentColor(variant, theme);

#if ANDROID
        ApplyAndroid(handler, accentColor, theme);
#elif IOS || MACCATALYST
        // iOS doesn't have a native radio button, MAUI uses custom rendering
        ApplyiOS(handler, accentColor, theme);
#elif WINDOWS
        ApplyWindows(handler, accentColor, theme);
#endif
    }

#if ANDROID
    private static void ApplyAndroid(IRadioButtonHandler handler, Color accentColor, BootstrapTheme theme)
    {
        // RadioButton on Android uses a ContentViewGroup as platform view
        // Limited styling through handler - use MAUI properties instead
        var radioButton = handler.VirtualView as RadioButton;
        if (radioButton != null)
        {
            radioButton.TextColor = theme.GetText();
        }
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(IRadioButtonHandler handler, Color accentColor, BootstrapTheme theme)
    {
        // MAUI uses a custom ContentView-based RadioButton on iOS
        var radioButton = handler.VirtualView as RadioButton;
        if (radioButton != null)
        {
            radioButton.TextColor = theme.GetText();
        }
    }
#endif

#if WINDOWS
    private static void ApplyWindows(IRadioButtonHandler handler, Color accentColor, BootstrapTheme theme)
    {
        var radioButton = handler.PlatformView;
        if (radioButton == null) return;

        radioButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(accentColor.ToWindowsColor());
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

    private static void ApplyDisabledState(IRadioButtonHandler handler, IRadioButton control)
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
