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

using System.Runtime.CompilerServices;

namespace MauiBootstrapTheme.Handlers;

/// <summary>
/// Handler extensions for TimePicker control.
/// </summary>
public static class BootstrapTimePickerHandler
{
    public static void Register()
    {
        TimePickerHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
        TimePickerHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
    }

    private static void ApplyBootstrapStyle(ITimePickerHandler handler, ITimePicker timePicker)
    {
        try
        {
            var theme = BootstrapTheme.Current;
            var view = timePicker as TimePicker;

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
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BootstrapTheme: Handler error: {ex.Message}");
        }
    }

#if ANDROID
    private static void ApplyAndroid(ITimePickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme,
        double minHeight, double fontSize, double paddingX, double paddingY)
    {
        var editText = handler.PlatformView;
        if (editText == null) return;

        var density = editText.Context?.Resources?.DisplayMetrics?.Density ?? 1;
        var cornerRadiusPx = (float)(cornerRadius * density);
        var borderWidthPx = (int)(theme.BorderWidth * density);

        var focusBorderColor = BootstrapTheme.Tint(theme.Primary, 0.50f);

        var normalDrawable = new GradientDrawable();
        normalDrawable.SetShape(ShapeType.Rectangle);
        normalDrawable.SetCornerRadius(cornerRadiusPx);
        normalDrawable.SetStroke(borderWidthPx, borderColor.ToPlatform());
        normalDrawable.SetColor(theme.GetInputBackground().ToPlatform());

        var focusedDrawable = new GradientDrawable();
        focusedDrawable.SetShape(ShapeType.Rectangle);
        focusedDrawable.SetCornerRadius(cornerRadiusPx);
        focusedDrawable.SetStroke(borderWidthPx, focusBorderColor.ToPlatform());
        focusedDrawable.SetColor(theme.GetInputBackground().ToPlatform());

        var stateList = new StateListDrawable();
        stateList.AddState(new[] { Android.Resource.Attribute.StateFocused }, focusedDrawable);
        stateList.AddState(new int[] { }, normalDrawable);

        editText.Background = stateList;
        editText.SetTextColor(theme.GetInputText().ToPlatform());
        editText.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)fontSize);
        editText.SetMinHeight((int)(minHeight * density));
        
        var px = (int)(paddingX * density);
        var py = (int)(paddingY * density);
        editText.SetPadding(px, py, px, py);
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(ITimePickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme)
    {
        var datePicker = handler.PlatformView;
        if (datePicker == null) return;

        datePicker.TintColor = theme.Primary.ToPlatform();
        datePicker.Layer.BorderColor = borderColor.ToCGColor();
        datePicker.Layer.BorderWidth = (nfloat)theme.BorderWidth;
        datePicker.Layer.CornerRadius = (nfloat)cornerRadius;
        datePicker.Layer.MasksToBounds = true;
        datePicker.BackgroundColor = theme.GetInputBackground().ToPlatform();
    }
#endif

#if WINDOWS
    private static void ApplyWindows(ITimePickerHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme,
        double minHeight, double fontSize, double paddingX, double paddingY)
    {
        var timePicker = handler.PlatformView;
        if (timePicker == null) return;

        timePicker.BorderBrush = new SolidColorBrush(borderColor.ToWindowsColor());
        timePicker.BorderThickness = new Microsoft.UI.Xaml.Thickness(theme.BorderWidth);
        timePicker.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(cornerRadius);
        timePicker.Foreground = new SolidColorBrush(theme.GetInputText().ToWindowsColor());
        timePicker.Background = new SolidColorBrush(theme.GetInputBackground().ToWindowsColor());
        timePicker.FontSize = fontSize;
        timePicker.MinHeight = minHeight;
        timePicker.Padding = new Microsoft.UI.Xaml.Thickness(paddingX, paddingY, paddingX, paddingY);

        var focusBorderColor = BootstrapTheme.Tint(theme.Primary, 0.50f);
        var hoverBorderColor = BootstrapTheme.Shade(borderColor, theme.HoverShadeAmount);

        timePicker.Resources["TimePickerBorderBrushPointerOver"] = new SolidColorBrush(hoverBorderColor.ToWindowsColor());
        timePicker.Resources["TimePickerBorderBrushFocused"] = new SolidColorBrush(focusBorderColor.ToWindowsColor());
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

    private static void ApplyDisabledState(ITimePickerHandler handler, ITimePicker control)
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
