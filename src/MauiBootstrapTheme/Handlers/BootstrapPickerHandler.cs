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
        if (size == BootstrapSize.Default && view?.StyleClass?.Count > 0)
            size = InferSizeFromStyleClass(view.StyleClass);
        
        var cornerRadius = GetCornerRadiusForSize(size, theme);
        var borderColor = GetBorderColorForVariant(variant, theme);
        var minHeight = GetMinHeightForSize(size, theme);
        var fontSize = GetFontSizeForSize(size, theme);
        var (paddingX, paddingY) = GetPaddingForSize(size, theme);

#if ANDROID
        ApplyAndroid(handler, cornerRadius, borderColor, theme, minHeight, fontSize, paddingX, paddingY);
#elif IOS || MACCATALYST
        ApplyiOS(handler, cornerRadius, borderColor, theme, fontSize, paddingX);
#elif WINDOWS
        ApplyWindows(handler, cornerRadius, borderColor, theme, minHeight, fontSize, paddingX, paddingY);
#endif
    }

#if ANDROID
    private static void ApplyAndroid(IPickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme,
        double minHeight, double fontSize, double paddingX, double paddingY)
    {
        var picker = handler.PlatformView;
        if (picker == null) return;

        var density = picker.Context?.Resources?.DisplayMetrics?.Density ?? 1;
        
        var drawable = new GradientDrawable();
        drawable.SetShape(ShapeType.Rectangle);
        drawable.SetCornerRadius((float)(cornerRadius * density));
        drawable.SetStroke((int)(theme.BorderWidth * density), borderColor.ToPlatform());
        drawable.SetColor(theme.InputBackground.ToPlatform());
        
        picker.Background = drawable;
        picker.SetTextColor(theme.InputText.ToPlatform());
        picker.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)fontSize);
        picker.SetMinHeight((int)(minHeight * density));
        
        var px = (int)(paddingX * density);
        var py = (int)(paddingY * density);
        picker.SetPadding(px, py, px, py);
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(IPickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme,
        double fontSize, double paddingX)
    {
        var textField = handler.PlatformView;
        if (textField == null) return;

        textField.BorderStyle = UITextBorderStyle.None;
        textField.Layer.BorderColor = borderColor.ToCGColor();
        textField.Layer.BorderWidth = (nfloat)theme.BorderWidth;
        textField.Layer.CornerRadius = (nfloat)cornerRadius;
        textField.Layer.MasksToBounds = true;
        textField.BackgroundColor = theme.InputBackground.ToPlatform();
        textField.TextColor = theme.InputText.ToPlatform();
        textField.Font = UIFont.SystemFontOfSize((nfloat)fontSize);

        var leftPadding = new UIView(new CoreGraphics.CGRect(0, 0, paddingX, 1));
        var rightPadding = new UIView(new CoreGraphics.CGRect(0, 0, paddingX, 1));
        textField.LeftView = leftPadding;
        textField.LeftViewMode = UITextFieldViewMode.Always;
        textField.RightView = rightPadding;
        textField.RightViewMode = UITextFieldViewMode.Always;
    }
#endif

#if WINDOWS
    private static void ApplyWindows(IPickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme,
        double minHeight, double fontSize, double paddingX, double paddingY)
    {
        var comboBox = handler.PlatformView;
        if (comboBox == null) return;

        comboBox.BorderBrush = new SolidColorBrush(borderColor.ToWindowsColor());
        comboBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(theme.BorderWidth);
        comboBox.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(cornerRadius);
        comboBox.Foreground = new SolidColorBrush(theme.InputText.ToWindowsColor());
        comboBox.Background = new SolidColorBrush(theme.InputBackground.ToWindowsColor());
        comboBox.FontSize = fontSize;
        comboBox.MinHeight = minHeight;
        comboBox.Padding = new Microsoft.UI.Xaml.Thickness(paddingX, paddingY, paddingX, paddingY);
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

    private static BootstrapSize InferSizeFromStyleClass(IList<string> styleClasses)
    {
        foreach (var item in styleClasses)
        {
            foreach (var token in item.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (token is "form-select-sm" or "form-control-sm" or "btn-sm")
                    return BootstrapSize.Small;
                if (token is "form-select-lg" or "form-control-lg" or "btn-lg")
                    return BootstrapSize.Large;
            }
        }

        return BootstrapSize.Default;
    }
}
