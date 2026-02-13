using MauiBootstrapTheme.Theming;
using Microsoft.Maui.Handlers;
#if ANDROID
using Android.Graphics.Drawables;
using AndroidX.Core.Content;
using Microsoft.Maui.Platform;
#elif IOS || MACCATALYST
using UIKit;
using CoreGraphics;
using Microsoft.Maui.Platform;
#elif WINDOWS
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Maui.Platform;
#endif

namespace MauiBootstrapTheme.Handlers;

/// <summary>
/// Handler extensions for Entry control to support Bootstrap styling.
/// Adds BorderColor, CornerRadius, and BorderWidth support that stock MAUI Entry lacks.
/// </summary>
public static class BootstrapEntryHandler
{
    /// <summary>
    /// Registers the Bootstrap Entry handler customizations.
    /// </summary>
    public static void Register()
    {
        EntryHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    }

    private static void ApplyBootstrapStyle(IEntryHandler handler, IEntry entry)
    {
        var theme = BootstrapTheme.Current;
        var view = entry as Entry;
        
        var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
        var size = view != null ? Bootstrap.GetSize(view) : BootstrapSize.Default;
        var isPill = view != null && Bootstrap.GetIsPill(view);
        
        var cornerRadius = isPill ? theme.CornerRadiusPill : GetCornerRadiusForSize(size, theme);
        var borderColor = GetBorderColorForVariant(variant, theme);
        var backgroundColor = theme.GetSurface();
        var textColor = theme.GetOnSurface();
        var minHeight = GetMinHeightForSize(size, theme);
        var fontSize = GetFontSizeForSize(size, theme);
        var (paddingX, paddingY) = GetPaddingForSize(size, theme);

#if ANDROID
        ApplyAndroid(handler, cornerRadius, borderColor, backgroundColor, textColor, theme, minHeight, fontSize, paddingX, paddingY);
#elif IOS || MACCATALYST
        ApplyiOS(handler, cornerRadius, borderColor, backgroundColor, textColor, theme, minHeight, fontSize, paddingX, paddingY);
#elif WINDOWS
        ApplyWindows(handler, cornerRadius, borderColor, backgroundColor, textColor, theme, minHeight, fontSize, paddingX, paddingY);
#endif
    }

#if ANDROID
    private static void ApplyAndroid(IEntryHandler handler, double cornerRadius, Color borderColor, 
        Color backgroundColor, Color textColor, BootstrapTheme theme, double minHeight, double fontSize,
        double paddingX, double paddingY)
    {
        var editText = handler.PlatformView;
        if (editText == null) return;

        var density = editText.Context?.Resources?.DisplayMetrics?.Density ?? 1;

        var drawable = new GradientDrawable();
        drawable.SetShape(ShapeType.Rectangle);
        drawable.SetCornerRadius((float)(cornerRadius * density));
        drawable.SetStroke((int)(theme.BorderWidth * density), borderColor.ToPlatform());
        drawable.SetColor(backgroundColor.ToPlatform());
        
        editText.Background = drawable;
        editText.SetTextColor(textColor.ToPlatform());
        
        // Apply font size
        editText.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)fontSize);
        
        // Apply min height
        editText.SetMinHeight((int)(minHeight * density));
        
        // Padding
        var px = (int)(paddingX * density);
        var py = (int)(paddingY * density);
        editText.SetPadding(px, py, px, py);
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(IEntryHandler handler, double cornerRadius, Color borderColor, 
        Color backgroundColor, Color textColor, BootstrapTheme theme, double minHeight, double fontSize,
        double paddingX, double paddingY)
    {
        var textField = handler.PlatformView;
        if (textField == null) return;

        // Remove default border style
        textField.BorderStyle = UITextBorderStyle.None;
        
        // Apply Bootstrap styling via layer
        textField.Layer.BorderColor = borderColor.ToCGColor();
        textField.Layer.BorderWidth = (nfloat)theme.BorderWidth;
        textField.Layer.CornerRadius = (nfloat)cornerRadius;
        textField.Layer.MasksToBounds = true;
        textField.BackgroundColor = backgroundColor.ToPlatform();
        textField.TextColor = textColor.ToPlatform();
        
        // Apply font size
        textField.Font = UIFont.SystemFontOfSize((nfloat)fontSize);
        
        // Padding via left/right views
        var leftPadding = new UIView(new CGRect(0, 0, paddingX, 1));
        var rightPadding = new UIView(new CGRect(0, 0, paddingX, 1));
        textField.LeftView = leftPadding;
        textField.LeftViewMode = UITextFieldViewMode.Always;
        textField.RightView = rightPadding;
        textField.RightViewMode = UITextFieldViewMode.Always;
        
        // Set frame height - entry needs HeightRequest set on MAUI side for this to work properly
        // The handler can't directly set intrinsic size, but ensuring padding helps
    }
#endif

#if WINDOWS
    private static void ApplyWindows(IEntryHandler handler, double cornerRadius, Color borderColor, 
        Color backgroundColor, Color textColor, BootstrapTheme theme, double minHeight, double fontSize,
        double paddingX, double paddingY)
    {
        var textBox = handler.PlatformView;
        if (textBox == null) return;

        textBox.BorderBrush = new SolidColorBrush(borderColor.ToWindowsColor());
        textBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(theme.BorderWidth);
        textBox.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(cornerRadius);
        textBox.Background = new SolidColorBrush(backgroundColor.ToWindowsColor());
        textBox.Foreground = new SolidColorBrush(textColor.ToWindowsColor());
        textBox.Padding = new Microsoft.UI.Xaml.Thickness(paddingX, paddingY, paddingX, paddingY);
        textBox.MinHeight = minHeight;
        textBox.FontSize = fontSize;
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
        _ => theme.GetOutline()
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
        BootstrapSize.Small => (8.0, 4.0),
        BootstrapSize.Large => (16.0, 8.0),
        _ => (theme.InputPaddingX, theme.InputPaddingY)
    };
}
