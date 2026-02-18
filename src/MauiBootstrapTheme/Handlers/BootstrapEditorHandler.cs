using MauiBootstrapTheme.Theming;
using Microsoft.Maui.Handlers;
#if ANDROID
using Android.Graphics.Drawables;
using Microsoft.Maui.Platform;
#elif IOS || MACCATALYST
using UIKit;
using CoreGraphics;
using Microsoft.Maui.Platform;
#elif WINDOWS
using Microsoft.UI.Xaml.Media;
using Microsoft.Maui.Platform;
#endif

using System.Runtime.CompilerServices;

namespace MauiBootstrapTheme.Handlers;

/// <summary>
/// Handler extensions for Editor control to support Bootstrap styling.
/// Provides consistent styling with Entry for multi-line text input.
/// </summary>
public static class BootstrapEditorHandler
{
    /// <summary>
    /// Registers the Bootstrap Editor handler customizations.
    /// </summary>
    public static void Register()
    {
        EditorHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
        EditorHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
    }

    private static void ApplyBootstrapStyle(IEditorHandler handler, IEditor editor)
    {
        var theme = BootstrapTheme.Current;
        var view = editor as Editor;
        
        var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
        var size = view != null ? Bootstrap.GetSize(view) : BootstrapSize.Default;
        
        var cornerRadius = GetCornerRadiusForSize(size, theme);
        var borderColor = GetBorderColorForVariant(variant, theme);
        var backgroundColor = theme.InputBackground;
        var textColor = theme.InputText;

#if ANDROID
        ApplyAndroid(handler, cornerRadius, borderColor, backgroundColor, textColor, theme);
#elif IOS || MACCATALYST
        ApplyiOS(handler, cornerRadius, borderColor, backgroundColor, textColor, theme);
#elif WINDOWS
        ApplyWindows(handler, cornerRadius, borderColor, backgroundColor, textColor, theme);
#endif
    }

#if ANDROID
    private static void ApplyAndroid(IEditorHandler handler, double cornerRadius, Color borderColor, 
        Color backgroundColor, Color textColor, BootstrapTheme theme)
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
        normalDrawable.SetColor(backgroundColor.ToPlatform());

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
        
        // Padding
        var px = (int)(theme.InputPaddingX * density);
        var py = (int)(theme.InputPaddingY * density);
        editText.SetPadding(px, py, px, py);
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(IEditorHandler handler, double cornerRadius, Color borderColor, 
        Color backgroundColor, Color textColor, BootstrapTheme theme)
    {
        var textView = handler.PlatformView;
        if (textView == null) return;

        textView.Layer.BorderColor = borderColor.ToCGColor();
        textView.Layer.BorderWidth = (nfloat)theme.BorderWidth;
        textView.Layer.CornerRadius = (nfloat)cornerRadius;
        textView.Layer.MasksToBounds = true;
        textView.BackgroundColor = backgroundColor.ToPlatform();
        textView.TextColor = textColor.ToPlatform();
        
        // Content inset for padding
        textView.TextContainerInset = new UIEdgeInsets(
            (nfloat)theme.InputPaddingY, (nfloat)theme.InputPaddingX,
            (nfloat)theme.InputPaddingY, (nfloat)theme.InputPaddingX);
    }
#endif

#if WINDOWS
    private static void ApplyWindows(IEditorHandler handler, double cornerRadius, Color borderColor, 
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

    private static readonly ConditionalWeakTable<object, StrongBox<double>> _originalOpacity = new();

    private static void ApplyDisabledState(IEditorHandler handler, IEditor control)
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
}
