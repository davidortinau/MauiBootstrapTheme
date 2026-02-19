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

using System.Runtime.CompilerServices;

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
        EntryHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
    }

    private static void ApplyBootstrapStyle(IEntryHandler handler, IEntry entry)
    {
        try
        {
            var theme = BootstrapTheme.Current;
            var view = entry as Entry;

            var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
            var size = view != null ? Bootstrap.GetSize(view) : BootstrapSize.Default;
            var isPill = view != null && Bootstrap.GetIsPill(view);

            var cornerRadius = isPill ? theme.CornerRadiusPill : GetCornerRadiusForSize(size, theme);
            var borderColor = GetBorderColorForVariant(variant, theme);
            // Use theme-defined InputBackground/InputText (matches Bootstrap form-control colors)
            var backgroundColor = theme.GetInputBackground();
            var textColor = theme.GetInputText();
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
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BootstrapTheme: Handler error: {ex.Message}");
        }
    }

#if ANDROID
    private static void ApplyAndroid(IEntryHandler handler, double cornerRadius, Color borderColor, 
        Color backgroundColor, Color textColor, BootstrapTheme theme, double minHeight, double fontSize,
        double paddingX, double paddingY)
    {
        var editText = handler.PlatformView;
        if (editText == null) return;

        var density = editText.Context?.Resources?.DisplayMetrics?.Density ?? 1;
        var cornerRadiusPx = (float)(cornerRadius * density);
        var borderWidthPx = (int)(theme.BorderWidth * density);

        // Compute focused border color (Bootstrap: tinted primary)
        var focusBorderColor = BootstrapTheme.Tint(theme.Primary, 0.50f);

        // Normal state drawable
        var normalDrawable = new GradientDrawable();
        normalDrawable.SetShape(ShapeType.Rectangle);
        normalDrawable.SetCornerRadius(cornerRadiusPx);
        normalDrawable.SetStroke(borderWidthPx, borderColor.ToPlatform());
        normalDrawable.SetColor(backgroundColor.ToPlatform());

        // Focused state drawable
        var focusedDrawable = new GradientDrawable();
        focusedDrawable.SetShape(ShapeType.Rectangle);
        focusedDrawable.SetCornerRadius(cornerRadiusPx);
        focusedDrawable.SetStroke(borderWidthPx, focusBorderColor.ToPlatform());
        focusedDrawable.SetColor(backgroundColor.ToPlatform());

        var stateList = new StateListDrawable();
        stateList.AddState(new[] { Android.Resource.Attribute.StateFocused }, focusedDrawable);
        stateList.AddState(new int[] { }, normalDrawable);

        editText.Background = stateList;
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

        // Override WinUI TextBox visual state resources for hover/focus
        var focusBorderColor = BootstrapTheme.Tint(theme.Primary, 0.50f);
        var hoverBorderColor = BootstrapTheme.Shade(borderColor, theme.HoverShadeAmount);

        textBox.Resources["TextControlBorderBrushPointerOver"] = new SolidColorBrush(hoverBorderColor.ToWindowsColor());
        textBox.Resources["TextControlBorderBrushFocused"] = new SolidColorBrush(focusBorderColor.ToWindowsColor());
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

    private static readonly ConditionalWeakTable<object, StrongBox<double>> _originalOpacity = new();

    private static void ApplyDisabledState(IEntryHandler handler, IEntry control)
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
