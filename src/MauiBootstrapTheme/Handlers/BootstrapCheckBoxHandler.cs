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

namespace MauiBootstrapTheme.Handlers;

/// <summary>
/// Handler extensions for CheckBox control.
/// </summary>
public static class BootstrapCheckBoxHandler
{
    public static void Register()
    {
        CheckBoxHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    }

    private static void ApplyBootstrapStyle(ICheckBoxHandler handler, ICheckBox checkBox)
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
                theme.GetOutline().ToPlatform()
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
}
