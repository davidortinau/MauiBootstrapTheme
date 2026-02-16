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
/// Handler extensions for DatePicker control.
/// </summary>
public static class BootstrapDatePickerHandler
{
    public static void Register()
    {
        DatePickerHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    }

    private static void ApplyBootstrapStyle(IDatePickerHandler handler, IDatePicker datePicker)
    {
        var theme = BootstrapTheme.Current;
        var view = datePicker as DatePicker;
        
        var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
        var size = view != null ? Bootstrap.GetSize(view) : BootstrapSize.Default;
        
        var cornerRadius = GetCornerRadiusForSize(size, theme);
        var borderColor = GetBorderColorForVariant(variant, theme);
        var minHeight = GetMinHeightForSize(size, theme);
        var fontSize = GetFontSizeForSize(size, theme);
        var (paddingX, paddingY) = GetPaddingForSize(size, theme);

#if ANDROID
        ApplyAndroid(handler, cornerRadius, borderColor, theme, minHeight, fontSize, paddingX, paddingY);
#elif IOS || MACCATALYST
        ApplyiOS(handler, cornerRadius, borderColor, theme);
#elif WINDOWS
        ApplyWindows(handler, cornerRadius, borderColor, theme, minHeight, fontSize, paddingX, paddingY);
#endif
    }

#if ANDROID
    private static void ApplyAndroid(IDatePickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme,
        double minHeight, double fontSize, double paddingX, double paddingY)
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
        editText.SetTextColor(theme.InputText.ToPlatform());
        editText.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)fontSize);
        editText.SetMinHeight((int)(minHeight * density));
        
        var px = (int)(paddingX * density);
        var py = (int)(paddingY * density);
        editText.SetPadding(px, py, px, py);
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(IDatePickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme)
    {
        var datePicker = handler.PlatformView;
        if (datePicker == null) return;

        datePicker.TintColor = theme.Primary.ToPlatform();
        datePicker.Layer.BorderColor = borderColor.ToCGColor();
        datePicker.Layer.BorderWidth = (nfloat)theme.BorderWidth;
        datePicker.Layer.CornerRadius = (nfloat)cornerRadius;
        datePicker.Layer.MasksToBounds = true;
        datePicker.BackgroundColor = theme.InputBackground.ToPlatform();
    }
#endif

#if WINDOWS
    private static void ApplyWindows(IDatePickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme,
        double minHeight, double fontSize, double paddingX, double paddingY)
    {
        var calendarDatePicker = handler.PlatformView;
        if (calendarDatePicker == null) return;

        calendarDatePicker.BorderBrush = new SolidColorBrush(borderColor.ToWindowsColor());
        calendarDatePicker.BorderThickness = new Microsoft.UI.Xaml.Thickness(theme.BorderWidth);
        calendarDatePicker.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(cornerRadius);
        calendarDatePicker.Foreground = new SolidColorBrush(theme.InputText.ToWindowsColor());
        calendarDatePicker.Background = new SolidColorBrush(theme.InputBackground.ToWindowsColor());
        calendarDatePicker.FontSize = fontSize;
        calendarDatePicker.MinHeight = minHeight;
        calendarDatePicker.Padding = new Microsoft.UI.Xaml.Thickness(paddingX, paddingY, paddingX, paddingY);
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

    private static double GetMinHeightForSize(BootstrapSize size, BootstrapTheme theme) => size switch
    {
        BootstrapSize.Small => theme.InputMinHeightSm,
        BootstrapSize.Large => theme.InputMinHeightLg,
        _ => theme.InputMinHeight
    };

    private static double GetFontSizeForSize(BootstrapSize size, BootstrapTheme theme) => size switch
    {
        BootstrapSize.Small => theme.FontSizeSm,
        BootstrapSize.Large => theme.FontSizeLg,
        _ => theme.FontSizeBase
    };

    private static (double X, double Y) GetPaddingForSize(BootstrapSize size, BootstrapTheme theme) => size switch
    {
        BootstrapSize.Small => (theme.InputPaddingXSm, theme.InputPaddingYSm),
        BootstrapSize.Large => (theme.InputPaddingXLg, theme.InputPaddingYLg),
        _ => (theme.InputPaddingX, theme.InputPaddingY)
    };
}
