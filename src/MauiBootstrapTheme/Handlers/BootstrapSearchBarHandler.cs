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
/// Handler extensions for SearchBar control.
/// </summary>
public static class BootstrapSearchBarHandler
{
    public static void Register()
    {
        SearchBarHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    }

    private static void ApplyBootstrapStyle(ISearchBarHandler handler, ISearchBar searchBar)
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

#if ANDROID
    private static void ApplyAndroid(ISearchBarHandler handler, double cornerRadius, Color borderColor, BootstrapTheme theme)
    {
        var searchView = handler.PlatformView;
        if (searchView == null) return;

        var density = searchView.Context?.Resources?.DisplayMetrics?.Density ?? 1;
        
        var drawable = new GradientDrawable();
        drawable.SetShape(ShapeType.Rectangle);
        drawable.SetCornerRadius((float)(cornerRadius * density));
        drawable.SetStroke((int)(theme.BorderWidth * density), borderColor.ToPlatform());
        drawable.SetColor(theme.InputBackground.ToPlatform());
        
        searchView.Background = drawable;
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
            searchBar.SearchTextField.BackgroundColor = theme.InputBackground.ToPlatform();
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
}
