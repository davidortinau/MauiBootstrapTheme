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
/// Handler extensions for SearchBar control.
/// </summary>
public static class BootstrapSearchBarHandler
{
    public static void Register()
    {
        SearchBarHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
        SearchBarHandler.Mapper.AppendToMapping("IsEnabled", ApplyDisabledState);
    }

    private static void ApplyBootstrapStyle(ISearchBarHandler handler, ISearchBar searchBar)
    {
        try
        {
            var theme = BootstrapTheme.Current;
            var view = searchBar as SearchBar;

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
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BootstrapTheme: Handler error: {ex.Message}");
        }
    }

#if ANDROID
    private static void ApplyAndroid(ISearchBarHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme)
    {
        var searchView = handler.PlatformView;
        if (searchView == null) return;

        var density = searchView.Context?.Resources?.DisplayMetrics?.Density ?? 1;
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

        searchView.Background = stateList;
    }
#endif

#if IOS || MACCATALYST
    private static void ApplyiOS(ISearchBarHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme)
    {
        var searchBar = handler.PlatformView;
        if (searchBar == null) return;

        searchBar.SearchBarStyle = UISearchBarStyle.Minimal;
        
        if (searchBar.SearchTextField != null)
        {
            searchBar.SearchTextField.Layer.BorderColor = borderColor.ToCGColor();
            searchBar.SearchTextField.Layer.BorderWidth = (nfloat)theme.BorderWidth;
            searchBar.SearchTextField.Layer.CornerRadius = (nfloat)cornerRadius;
            searchBar.SearchTextField.Layer.MasksToBounds = true;
            searchBar.SearchTextField.BackgroundColor = theme.GetInputBackground().ToPlatform();
        }
    }
#endif

#if WINDOWS
    private static void ApplyWindows(ISearchBarHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme)
    {
        var autoSuggestBox = handler.PlatformView;
        if (autoSuggestBox == null) return;

        autoSuggestBox.BorderBrush = new SolidColorBrush(borderColor.ToWindowsColor());
        autoSuggestBox.BorderThickness = new Microsoft.UI.Xaml.Thickness(theme.BorderWidth);
        autoSuggestBox.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(cornerRadius);

        var focusBorderColor = BootstrapTheme.Tint(theme.Primary, 0.50f);
        var hoverBorderColor = BootstrapTheme.Shade(borderColor, theme.HoverShadeAmount);

        autoSuggestBox.Resources["TextControlBorderBrushPointerOver"] = new SolidColorBrush(hoverBorderColor.ToWindowsColor());
        autoSuggestBox.Resources["TextControlBorderBrushFocused"] = new SolidColorBrush(focusBorderColor.ToWindowsColor());
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

    private static readonly ConditionalWeakTable<object, StrongBox<double>> _originalOpacity = new();

    private static void ApplyDisabledState(ISearchBarHandler handler, ISearchBar control)
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
