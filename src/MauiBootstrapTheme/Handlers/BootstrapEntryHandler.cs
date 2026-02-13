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

#if ANDROID
        ApplyAndroid(handler, cornerRadius, borderColor, backgroundColor, textColor, theme);
#elif IOS || MACCATALYST
        ApplyiOS(handler, cornerRadius, borderColor, backgroundColor, textColor, theme);
#elif WINDOWS
        ApplyWindows(handler, cornerRadius, borderColor, backgroundColor, textColor, theme);
#endif
    }

#if ANDROID
    private static void ApplyAndroid(IEntryHandler handler, double cornerRadius, Color borderColor, 
        Color backgroundColor, Color textColor, BootstrapTheme theme)
    {
        var editText = handler.PlatformView;
        if (editText == null) return;

        var drawable = new GradientDrawable();
        drawable.SetShape(ShapeType.Rectangle);
        drawable.SetCornerRadius((float)(cornerRadius * editText.Context.Resources.DisplayMetrics.Density));
        drawable.SetStroke(
            (int)(theme.BorderWidth * editText.Context.Resources.DisplayMetrics.Density),
            borderColor.ToPlatform());
        drawable.SetColor(backgroundColor.ToPlatform());
        
        editText.Background = drawable;
        editText.SetTextColor(textColor.ToPlatform());
        
        // Padding
        var paddingPx = (int)(theme.InputPaddingX * editText.Context.Resources.DisplayMetrics.Density);
        var paddingPyPx = (int)(theme.InputPaddingY * editText.Context.Resources.DisplayMetrics.Density);
        editText.SetPadding(paddingPx, paddingPyPx, paddingPx, paddingPyPx);
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(IEntryHandler handler, double cornerRadius, Color borderColor, 
        Color backgroundColor, Color textColor, BootstrapTheme theme)
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
        
        // Padding via left/right views
        var leftPadding = new UIView(new CGRect(0, 0, theme.InputPaddingX, 1));
        var rightPadding = new UIView(new CGRect(0, 0, theme.InputPaddingX, 1));
        textField.LeftView = leftPadding;
        textField.LeftViewMode = UITextFieldViewMode.Always;
        textField.RightView = rightPadding;
        textField.RightViewMode = UITextFieldViewMode.Always;
    }
#endif

#if WINDOWS
    private static void ApplyWindows(IEntryHandler handler, double cornerRadius, Color borderColor, 
        Color backgroundColor, Color textColor, BootstrapTheme theme)
    {
        var textBox = handler.PlatformView;
        if (textBox == null) return;

        textBox.BorderBrush = new SolidColorBrush(borderColor.ToWindowsColor());
        textBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(theme.BorderWidth);
        textBox.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(cornerRadius);
        textBox.Background = new SolidColorBrush(backgroundColor.ToWindowsColor());
        textBox.Foreground = new SolidColorBrush(textColor.ToWindowsColor());
        textBox.Padding = new Microsoft.UI.Xaml.Thickness(theme.InputPaddingX, theme.InputPaddingY, 
            theme.InputPaddingX, theme.InputPaddingY);
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
}
