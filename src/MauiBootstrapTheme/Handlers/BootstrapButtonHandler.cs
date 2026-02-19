using MauiBootstrapTheme.Theming;
using Microsoft.Maui.Handlers;
using System.Collections.Concurrent;
#if ANDROID
using Android.Graphics.Drawables;
using Android.Content.Res;
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
/// Handler extensions for Button control to support Bootstrap styling.
/// Enhances stock Button with consistent Bootstrap-style borders and states.
/// </summary>
public static class BootstrapButtonHandler
{
#if ANDROID
    private static readonly ConcurrentDictionary<string, Android.Graphics.Typeface> TypefaceCache = new(StringComparer.OrdinalIgnoreCase);
#endif

    /// <summary>
    /// Registers the Bootstrap Button handler customizations.
    /// </summary>
    public static void Register()
    {
        ButtonHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
        ButtonHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
    }

    private static void ApplyBootstrapStyle(IButtonHandler handler, IButton button)
    {
        try
        {
            var theme = BootstrapTheme.Current;
            var view = button as Button;

            var variant = view != null ? Bootstrap.GetVariant(view) : BootstrapVariant.Default;
            // Don't infer variant from StyleClass — let MAUI style system handle class-based
            // styles (btn-primary, etc.) via DynamicResource. Handler inference causes native-layer
            // overrides that persist across theme switches.
            var size = view != null ? Bootstrap.GetSize(view) : BootstrapSize.Default;
            var isPill = view != null && Bootstrap.GetIsPill(view);
            var isOutlined = IsOutlineVariant(variant) || (view != null && Bootstrap.GetIsOutlined(view));

            // Only apply platform-level color overrides when an explicit variant is set.
            // When variant is Default, let MAUI-level styles (StyleClass, DynamicResource) handle colors.
            // Also skip when no size or pill override is set.
            if (variant == BootstrapVariant.Default && !isPill && size == BootstrapSize.Default)
            {
#if IOS || MACCATALYST
                // MAUI shadows are drawn outside the button bounds; clipping hides them.
                // Ensure default/class-styled buttons (e.g., theme switcher row) can render shadows.
                if (handler.PlatformView != null)
                    handler.PlatformView.Layer.MasksToBounds = false;
#endif
                return;
            }

            var cornerRadius = isPill ? theme.CornerRadiusPill : GetCornerRadiusForSize(size, theme);
            var (backgroundColor, textColor) = Bootstrap.GetVariantColors(variant, theme);

            // For outline buttons, use transparent background and variant color for text
            if (isOutlined && !IsOutlineVariant(variant))
            {
                backgroundColor = Colors.Transparent;
                textColor = Bootstrap.GetOutlineBorderColor(variant, theme);
            }

            var borderColor = isOutlined ? Bootstrap.GetOutlineBorderColor(variant, theme) : backgroundColor;
            var borderWidth = isOutlined ? Math.Max(theme.BorderWidth, 1.0) : 0;
            var (paddingX, paddingY) = GetPaddingForSize(size, theme);
            var fontSize = GetFontSizeForSize(size, theme);
            var minHeight = GetMinHeightForSize(size, theme);
            var fontFamily = theme.FontFamily;

            // Compute hover/pressed state colors
            var baseVariant = GetBaseVariant(variant);
            Color hoverBgColor, pressedBgColor, hoverBorderColor, pressedBorderColor, hoverTextColor, pressedTextColor;

            if (isOutlined)
            {
                // Outline: hover/pressed fill with variant color, text becomes OnVariant
                var (solidBg, solidFg) = Bootstrap.GetVariantColors(baseVariant, theme);
                hoverBgColor = solidBg;
                pressedBgColor = solidBg;
                hoverTextColor = solidFg;
                pressedTextColor = solidFg;
                hoverBorderColor = solidBg;
                pressedBorderColor = BootstrapTheme.Shade(solidBg, theme.PressedBorderShadeAmount);
            }
            else
            {
                hoverBgColor = theme.GetHoverBackground(baseVariant);
                pressedBgColor = theme.GetPressedBackground(baseVariant);
                hoverBorderColor = theme.GetHoverBorder(baseVariant);
                pressedBorderColor = theme.GetPressedBorder(baseVariant);
                hoverTextColor = textColor;
                pressedTextColor = textColor;
            }

#if ANDROID
            ApplyAndroid(handler, cornerRadius, borderColor, backgroundColor, textColor, borderWidth, paddingX, paddingY, fontSize, minHeight, fontFamily,
                hoverBgColor, pressedBgColor, hoverBorderColor, pressedBorderColor, hoverTextColor, pressedTextColor);
#elif IOS || MACCATALYST
            ApplyiOS(handler, cornerRadius, borderColor, backgroundColor, textColor, borderWidth, paddingX, paddingY, fontSize, minHeight, isPill, fontFamily,
                hoverBgColor, pressedBgColor, hoverBorderColor, pressedBorderColor, hoverTextColor, pressedTextColor);
#elif WINDOWS
            ApplyWindows(handler, cornerRadius, borderColor, backgroundColor, textColor, borderWidth, paddingX, paddingY, fontSize, minHeight, fontFamily,
                hoverBgColor, pressedBgColor, hoverBorderColor, pressedBorderColor, hoverTextColor, pressedTextColor);
#endif
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BootstrapTheme: Handler error: {ex.Message}");
        }
    }

    private static BootstrapVariant InferVariantFromStyleClass(IList<string> styleClasses)
    {
        static IEnumerable<string> Tokens(IList<string> classes)
        {
            foreach (var item in classes)
            {
                foreach (var token in item.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    yield return token;
            }
        }

        foreach (var token in Tokens(styleClasses))
        {
            switch (token)
            {
                case "btn-outline-primary": return BootstrapVariant.OutlinePrimary;
                case "btn-outline-secondary": return BootstrapVariant.OutlineSecondary;
                case "btn-outline-success": return BootstrapVariant.OutlineSuccess;
                case "btn-outline-danger": return BootstrapVariant.OutlineDanger;
                case "btn-outline-warning": return BootstrapVariant.OutlineWarning;
                case "btn-outline-info": return BootstrapVariant.OutlineInfo;
                case "btn-outline-light": return BootstrapVariant.OutlineLight;
                case "btn-outline-dark": return BootstrapVariant.OutlineDark;
                case "btn-primary": return BootstrapVariant.Primary;
                case "btn-secondary": return BootstrapVariant.Secondary;
                case "btn-success": return BootstrapVariant.Success;
                case "btn-danger": return BootstrapVariant.Danger;
                case "btn-warning": return BootstrapVariant.Warning;
                case "btn-info": return BootstrapVariant.Info;
                case "btn-light": return BootstrapVariant.Light;
                case "btn-dark": return BootstrapVariant.Dark;
            }
        }

        return BootstrapVariant.Default;
    }

#if ANDROID
    private static void ApplyAndroid(IButtonHandler handler, double cornerRadius, Color borderColor, 
        Color backgroundColor, Color textColor, double borderWidth, double paddingX, double paddingY,
        double fontSize, double minHeight, string? fontFamily,
        Color hoverBgColor, Color pressedBgColor, Color hoverBorderColor, Color pressedBorderColor,
        Color hoverTextColor, Color pressedTextColor)
    {
        var button = handler.PlatformView;
        if (button == null) return;

        var density = button.Context?.Resources?.DisplayMetrics?.Density ?? 1;
        var cornerRadiusPx = (float)(cornerRadius * density);
        var borderWidthPx = (int)(borderWidth * density);

        // Normal state drawable
        var normalDrawable = new GradientDrawable();
        normalDrawable.SetShape(ShapeType.Rectangle);
        normalDrawable.SetCornerRadius(cornerRadiusPx);
        if (borderWidthPx > 0)
            normalDrawable.SetStroke(borderWidthPx, borderColor.ToPlatform());
        normalDrawable.SetColor(backgroundColor.ToPlatform());

        // Pressed state drawable
        var pressedDrawable = new GradientDrawable();
        pressedDrawable.SetShape(ShapeType.Rectangle);
        pressedDrawable.SetCornerRadius(cornerRadiusPx);
        if (borderWidthPx > 0)
            pressedDrawable.SetStroke(borderWidthPx, pressedBorderColor.ToPlatform());
        pressedDrawable.SetColor(pressedBgColor.ToPlatform());

        // Build StateListDrawable
        var stateList = new StateListDrawable();
        stateList.AddState(new[] { Android.Resource.Attribute.StatePressed }, pressedDrawable);
        stateList.AddState(new int[] { }, normalDrawable);

        // Wrap in RippleDrawable for animated feedback
        var rippleColor = ColorStateList.ValueOf(backgroundColor.WithAlpha(0.25f).ToPlatform());
        var ripple = new RippleDrawable(rippleColor, stateList, null);
        button.Background = ripple;

        button.SetTextColor(textColor.ToPlatform());
        
        // Apply font size
        button.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)fontSize);
        
        // Apply font family if specified
        if (!string.IsNullOrEmpty(fontFamily))
        {
            var typeface = TypefaceCache.GetOrAdd(fontFamily,
                static family => Android.Graphics.Typeface.Create(family, Android.Graphics.TypefaceStyle.Normal));
            button.SetTypeface(typeface, Android.Graphics.TypefaceStyle.Normal);
        }
        
        // Apply min height
        button.SetMinHeight((int)(minHeight * density));
        
        // Apply padding
        var px = (int)(paddingX * density);
        var py = (int)(paddingY * density);
        button.SetPadding(px, py, px, py);
        
        // Remove all caps default
        button.SetAllCaps(false);
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(IButtonHandler handler, double cornerRadius, Color borderColor, 
        Color backgroundColor, Color textColor, double borderWidth, double paddingX, double paddingY,
        double fontSize, double minHeight, bool isPill, string? fontFamily,
        Color hoverBgColor, Color pressedBgColor, Color hoverBorderColor, Color pressedBorderColor,
        Color hoverTextColor, Color pressedTextColor)
    {
        var button = handler.PlatformView;
        if (button == null) return;

        // For pill buttons, calculate corner radius based on actual height
        var effectiveCornerRadius = isPill ? minHeight / 2.0 : cornerRadius;
        
        button.Layer.CornerRadius = (nfloat)effectiveCornerRadius;
        button.ClipsToBounds = true;

        if (borderWidth > 0)
        {
            button.Layer.BorderColor = borderColor.ToCGColor();
            button.Layer.BorderWidth = (nfloat)borderWidth;
        }

        // Set title colors per state
        button.SetTitleColor(textColor.ToPlatform(), UIControlState.Normal);
        button.SetTitleColor(pressedTextColor.ToPlatform(), UIControlState.Highlighted);

        // Set background images per state (1x1 colored images)
        button.SetBackgroundImage(CreateColorImage(backgroundColor.ToPlatform()), UIControlState.Normal);
        button.SetBackgroundImage(CreateColorImage(pressedBgColor.ToPlatform()), UIControlState.Highlighted);

        // Apply font
        if (button.TitleLabel != null)
        {
            UIFont font;
            if (!string.IsNullOrEmpty(fontFamily))
            {
                font = UIFont.FromName(fontFamily, (nfloat)fontSize) ?? UIFont.SystemFontOfSize((nfloat)fontSize);
            }
            else
            {
                font = UIFont.SystemFontOfSize((nfloat)fontSize);
            }
            button.TitleLabel.Font = font;
        }
        
        // Set minimum height constraint via intrinsic content size won't work directly
        // Instead, ensure frame height through content insets calculation
        var verticalPadding = Math.Max(paddingY, (minHeight - fontSize - 4) / 2.0);
        
        // Content insets for padding - use Configuration on newer iOS
        button.ContentEdgeInsets = new UIEdgeInsets(
            (nfloat)verticalPadding, (nfloat)paddingX, 
            (nfloat)verticalPadding, (nfloat)paddingX);

#if MACCATALYST
        // Add hover gesture for pointer-over state
        var hoverBgPlatform = hoverBgColor.ToPlatform();
        var hoverGesture = new UIHoverGestureRecognizer((recognizer) =>
        {
            switch (recognizer.State)
            {
                case UIGestureRecognizerState.Began:
                    button.Layer.BackgroundColor = hoverBgPlatform.CGColor;
                    break;
                case UIGestureRecognizerState.Ended:
                case UIGestureRecognizerState.Cancelled:
                    // Reset - the SetBackgroundImage handles normal state
                    button.Layer.BackgroundColor = null;
                    break;
            }
        });
        button.AddGestureRecognizer(hoverGesture);
#endif
    }

    private static UIImage CreateColorImage(UIColor color)
    {
        var rect = new CGRect(0, 0, 1, 1);
        var renderer = new UIGraphicsImageRenderer(rect.Size);
        return renderer.CreateImage(ctx =>
        {
            color.SetFill();
            ctx.FillRect(rect);
        });
    }
#endif

#if WINDOWS
    private static void ApplyWindows(IButtonHandler handler, double cornerRadius, Color borderColor, 
        Color backgroundColor, Color textColor, double borderWidth, double paddingX, double paddingY,
        double fontSize, double minHeight, string? fontFamily,
        Color hoverBgColor, Color pressedBgColor, Color hoverBorderColor, Color pressedBorderColor,
        Color hoverTextColor, Color pressedTextColor)
    {
        var button = handler.PlatformView;
        if (button == null) return;

        button.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(cornerRadius);
        button.Background = new SolidColorBrush(backgroundColor.ToWindowsColor());
        button.Foreground = new SolidColorBrush(textColor.ToWindowsColor());
        
        if (borderWidth > 0)
        {
            button.BorderBrush = new SolidColorBrush(borderColor.ToWindowsColor());
            button.BorderThickness = new Microsoft.UI.Xaml.Thickness(borderWidth);
        }
        
        button.Padding = new Microsoft.UI.Xaml.Thickness(paddingX, paddingY, paddingX, paddingY);
        button.MinHeight = minHeight;
        button.FontSize = fontSize;
        
        if (!string.IsNullOrEmpty(fontFamily))
        {
            button.FontFamily = new Microsoft.UI.Xaml.Media.FontFamily(fontFamily);
        }

        // Override WinUI Button visual state resources for hover/pressed
        button.Resources["ButtonBackgroundPointerOver"] = new SolidColorBrush(hoverBgColor.ToWindowsColor());
        button.Resources["ButtonForegroundPointerOver"] = new SolidColorBrush(hoverTextColor.ToWindowsColor());
        button.Resources["ButtonBorderBrushPointerOver"] = new SolidColorBrush(hoverBorderColor.ToWindowsColor());

        button.Resources["ButtonBackgroundPressed"] = new SolidColorBrush(pressedBgColor.ToWindowsColor());
        button.Resources["ButtonForegroundPressed"] = new SolidColorBrush(pressedTextColor.ToWindowsColor());
        button.Resources["ButtonBorderBrushPressed"] = new SolidColorBrush(pressedBorderColor.ToWindowsColor());
    }
#endif

    /// <summary>
    /// Maps an outline variant to its base variant for state color computation.
    /// </summary>
    private static BootstrapVariant GetBaseVariant(BootstrapVariant variant) => variant switch
    {
        BootstrapVariant.OutlinePrimary => BootstrapVariant.Primary,
        BootstrapVariant.OutlineSecondary => BootstrapVariant.Secondary,
        BootstrapVariant.OutlineSuccess => BootstrapVariant.Success,
        BootstrapVariant.OutlineDanger => BootstrapVariant.Danger,
        BootstrapVariant.OutlineWarning => BootstrapVariant.Warning,
        BootstrapVariant.OutlineInfo => BootstrapVariant.Info,
        BootstrapVariant.OutlineLight => BootstrapVariant.Light,
        BootstrapVariant.OutlineDark => BootstrapVariant.Dark,
        _ => variant
    };

    private static bool IsOutlineVariant(BootstrapVariant variant) => variant switch
    {
        BootstrapVariant.OutlinePrimary or
        BootstrapVariant.OutlineSecondary or
        BootstrapVariant.OutlineSuccess or
        BootstrapVariant.OutlineDanger or
        BootstrapVariant.OutlineWarning or
        BootstrapVariant.OutlineInfo or
        BootstrapVariant.OutlineLight or
        BootstrapVariant.OutlineDark => true,
        _ => false
    };

    private static double GetCornerRadiusForSize(BootstrapSize size, BootstrapTheme theme) => size switch
    {
        BootstrapSize.Small => theme.CornerRadiusSm,
        BootstrapSize.Large => theme.CornerRadiusLg,
        _ => theme.CornerRadius
    };

    private static (double X, double Y) GetPaddingForSize(BootstrapSize size, BootstrapTheme theme) => size switch
    {
        BootstrapSize.Small => (theme.ButtonPaddingXSm, theme.ButtonPaddingYSm),
        BootstrapSize.Large => (theme.ButtonPaddingXLg, theme.ButtonPaddingYLg),
        _ => (theme.ButtonPaddingX, theme.ButtonPaddingY)
    };

    private static double GetFontSizeForSize(BootstrapSize size, BootstrapTheme theme) => size switch
    {
        BootstrapSize.Small => theme.FontSizeSm,
        BootstrapSize.Large => theme.FontSizeLg,
        _ => theme.FontSizeBase
    };

    private static double GetMinHeightForSize(BootstrapSize size, BootstrapTheme theme) => size switch
    {
        BootstrapSize.Small => theme.ButtonMinHeightSm,
        BootstrapSize.Large => theme.ButtonMinHeightLg,
        _ => theme.ButtonMinHeight
    };

    private static readonly ConditionalWeakTable<object, StrongBox<double>> _originalOpacity = new();

    private static void ApplyDisabledState(IButtonHandler handler, IButton control)
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
