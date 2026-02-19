using MauiBootstrapTheme.Theming;
using Microsoft.Maui.Handlers;
#if ANDROID
using Microsoft.Maui.Platform;
using AndroidX.AppCompat.Widget;
#elif IOS || MACCATALYST
using UIKit;
using Microsoft.Maui.Platform;
#elif WINDOWS
using Microsoft.Maui.Platform;
#endif

using System.Runtime.CompilerServices;

namespace MauiBootstrapTheme.Handlers;

/// <summary>
/// Handler extensions for CheckBox control.
/// </summary>
public static class BootstrapCheckBoxHandler
{
    public static void Register()
    {
        CheckBoxHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
        CheckBoxHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
    }

    private static void ApplyBootstrapStyle(ICheckBoxHandler handler, ICheckBox checkBox)
    {
        try
        {
            var theme = BootstrapTheme.Current;
            var view = checkBox as CheckBox;

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
    private static void ApplyAndroid(ICheckBoxHandler handler, Color accentColor, BootstrapTheme theme)
    {
        var checkBox = handler.PlatformView;
        if (checkBox == null) return;

        // Set the button tint color for checked state
        var colorStateList = new Android.Content.Res.ColorStateList(
            new int[][] {
                new int[] { Android.Resource.Attribute.StateChecked },
                new int[] { -Android.Resource.Attribute.StateChecked }
            },
            new int[] {
                accentColor.ToPlatform(),
                theme.Outline.ToPlatform()
            });
        
        checkBox.ButtonTintList = colorStateList;
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(ICheckBoxHandler handler, Color accentColor, BootstrapTheme theme)
    {
        // MauiCheckBox on iOS has limited styling options
        var checkBox = handler.PlatformView;
        if (checkBox == null) return;

        checkBox.TintColor = accentColor.ToPlatform();
    }
#endif

#if WINDOWS
    private static void ApplyWindows(ICheckBoxHandler handler, Color accentColor, BootstrapTheme theme)
    {
        var checkBox = handler.PlatformView;
        if (checkBox == null) return;

        // Windows CheckBox styling is more limited
        checkBox.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(accentColor.ToWindowsColor());
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

    private static void ApplyDisabledState(ICheckBoxHandler handler, ICheckBox control)
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
