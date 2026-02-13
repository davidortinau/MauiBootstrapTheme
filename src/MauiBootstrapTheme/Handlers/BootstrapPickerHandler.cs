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
/// Handler extensions for Picker control.
/// </summary>
public static class BootstrapPickerHandler
{
    public static void Register()
    {
        PickerHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    }

    private static void ApplyBootstrapStyle(IPickerHandler handler, IPicker picker)
    {
        var theme = BootstrapTheme.Current;
        var view = picker as Picker;
        
        var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
        var size = view != null ? Bootstrap.GetSize(view) : BootstrapSize.Default;
        
        var cornerRadius = GetCornerRadiusForSize(size, theme);
        var borderColor = GetBorderColorForVariant(variant, theme);

#if ANDROID
        ApplyAndroid(handler, cornerRadius, borderColor, theme);
#elif IOS || MACCATALYST
        ApplyiOS(handler, cornerRadius, borderColor, theme);
#elif WINDOWS
        ApplyWindows(handler, cornerRadius, borderColor, theme);
#endif
    }

#if ANDROID
    private static void ApplyAndroid(IPickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme)
    {
        var picker = handler.PlatformView;
        if (picker == null) return;

        var density = picker.Context?.Resources?.DisplayMetrics?.Density ?? 1;
        
        var drawable = new GradientDrawable();
        drawable.SetShape(ShapeType.Rectangle);
        drawable.SetCornerRadius((float)(cornerRadius * density));
        drawable.SetStroke((int)(theme.BorderWidth * density), borderColor.ToPlatform());
        drawable.SetColor(theme.GetSurface().ToPlatform());
        
        picker.Background = drawable;
        
        var px = (int)(theme.InputPaddingX * density);
        var py = (int)(theme.InputPaddingY * density);
        picker.SetPadding(px, py, px, py);
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(IPickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme)
    {
        var textField = handler.PlatformView;
        if (textField == null) return;

        textField.BorderStyle = UITextBorderStyle.None;
        textField.Layer.BorderColor = borderColor.ToCGColor();
        textField.Layer.BorderWidth = (nfloat)theme.BorderWidth;
        textField.Layer.CornerRadius = (nfloat)cornerRadius;
        textField.Layer.MasksToBounds = true;
        textField.BackgroundColor = theme.GetSurface().ToPlatform();
    }
#endif

#if WINDOWS
    private static void ApplyWindows(IPickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme)
    {
        var comboBox = handler.PlatformView;
        if (comboBox == null) return;

        comboBox.BorderBrush = new SolidColorBrush(borderColor.ToWindowsColor());
        comboBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(theme.BorderWidth);
        comboBox.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(cornerRadius);
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
        BootstrapVariant.Primary => theme.Primary,
        _ => theme.GetOutline()
    };
}
