using MauiBootstrapTheme.Theming;
using Microsoft.Maui.Handlers;
#if ANDROID
using Android.Graphics.Drawables;
using Microsoft.Maui.Platform;
#elif IOS || MACCATALYST
using UIKit;
using Microsoft.Maui.Platform;
#elif WINDOWS
using Microsoft.UI.Xaml.Media;
using Microsoft.Maui.Platform;
#endif

namespace MauiBootstrapTheme.Handlers;

/// <summary>
/// Handler extensions for TimePicker control.
/// </summary>
public static class BootstrapTimePickerHandler
{
    public static void Register()
    {
        TimePickerHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    }

    private static void ApplyBootstrapStyle(ITimePickerHandler handler, ITimePicker timePicker)
    {
        var theme = BootstrapTheme.Current;
        var view = timePicker as TimePicker;
        
        var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
        var size = view != null ? Bootstrap.GetSize(view) : BootstrapSize.Default;
        
        var cornerRadius = GetCornerRadiusForSize(size, theme);
        var borderColor = GetBorderColorForVariant(variant, theme);

#if ANDROID
        ApplyAndroid(handler, cornerRadius, borderColor, theme);
#elif IOS || MACCATALYST
        ApplyiOS(handler, borderColor, theme);
#elif WINDOWS
        ApplyWindows(handler, cornerRadius, borderColor, theme);
#endif
    }

#if ANDROID
    private static void ApplyAndroid(ITimePickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme)
    {
        var editText = handler.PlatformView;
        if (editText == null) return;

        var density = editText.Context?.Resources?.DisplayMetrics?.Density ?? 1;
        
        var drawable = new GradientDrawable();
        drawable.SetShape(ShapeType.Rectangle);
        drawable.SetCornerRadius((float)(cornerRadius * density));
        drawable.SetStroke((int)(theme.BorderWidth * density), borderColor.ToPlatform());
        drawable.SetColor(theme.InputBackground.ToPlatform());
        
        editText.Background = drawable;
        
        var px = (int)(theme.InputPaddingX * density);
        var py = (int)(theme.InputPaddingY * density);
        editText.SetPadding(px, py, px, py);
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(ITimePickerHandler handler, Color borderColor, BootstrapTheme theme)
    {
        // iOS/Mac TimePicker uses UIDatePicker which doesn't have typical text field styling
        // The styling is limited for this native control
        var datePicker = handler.PlatformView;
        if (datePicker == null) return;

        datePicker.TintColor = theme.Primary.ToPlatform();
    }
#endif

#if WINDOWS
    private static void ApplyWindows(ITimePickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme)
    {
        var timePicker = handler.PlatformView;
        if (timePicker == null) return;

        timePicker.BorderBrush = new SolidColorBrush(borderColor.ToWindowsColor());
        timePicker.BorderThickness = new Microsoft.UI.Xaml.Thickness(theme.BorderWidth);
        timePicker.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(cornerRadius);
    }
#endif

    private static double GetCornerRadiusForSize(BootstrapSize size, BootstrapTheme theme) => size switch
    {
        BootstrapSize.Small => theme.CornerRadiusSm,
        BootstrapSize.Large => theme.CornerRadiusLg,
        _ => theme.CornerRadius
    };

    private static Color GetBorderColorForVariant(BootstrapVariant variant, BootstrapTheme theme) => variant switch
    {
        BootstrapVariant.Danger => theme.Danger,
        BootstrapVariant.Success => theme.Success,
        BootstrapVariant.Warning => theme.Warning,
        _ => theme.Outline
    };
}
