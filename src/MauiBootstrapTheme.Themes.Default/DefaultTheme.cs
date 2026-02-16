using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Themes.Default;

/// <summary>
/// Provides the default Bootstrap 5 theme.
/// </summary>
public class DefaultTheme : IBootstrapThemeProvider
{
    /// <inheritdoc />
    public BootstrapTheme GetTheme() => new()
    {
        Name = "bootstrap",
        
        // Bootstrap 5 Default Colors
        Primary = Color.FromArgb("#0d6efd"),
        Secondary = Color.FromArgb("#6c757d"),
        Success = Color.FromArgb("#198754"),
        Danger = Color.FromArgb("#dc3545"),
        Warning = Color.FromArgb("#ffc107"),
        Info = Color.FromArgb("#0dcaf0"),
        Light = Color.FromArgb("#f8f9fa"),
        Dark = Color.FromArgb("#212529"),
        
        // Light Mode
        Background = Color.FromArgb("#ffffff"),
        OnBackground = Color.FromArgb("#212529"),
        Surface = Color.FromArgb("#ffffff"),
        OnSurface = Color.FromArgb("#212529"),
        Outline = Color.FromArgb("#dee2e6"),
        OutlineVariant = Color.FromArgb("#ced4da"),
        
        OnPrimary = Colors.White,
        OnSecondary = Colors.White,
        OnSuccess = Colors.White,
        OnDanger = Colors.White,
        OnWarning = Color.FromArgb("#212529"),
        OnInfo = Color.FromArgb("#212529"),
        
        // Dark Mode
        DarkBackground = Color.FromArgb("#212529"),
        DarkOnBackground = Color.FromArgb("#dee2e6"),
        DarkSurface = Color.FromArgb("#343a40"),
        DarkOnSurface = Color.FromArgb("#f8f9fa"),
        DarkOutline = Color.FromArgb("#495057"),
        
        // Bootstrap 5 Border Radius
        CornerRadius = 6.0,      // 0.375rem
        CornerRadiusSm = 4.0,    // 0.25rem
        CornerRadiusLg = 8.0,    // 0.5rem
        CornerRadiusPill = 50.0, // 50rem
        
        // Border
        BorderWidth = 1.0,
        BorderWidthLg = 2.0,
        
        // Spacing (1rem = 16px base)
        Spacer = 16.0,
        SpacerSm = 8.0,
        SpacerLg = 24.0,
        
        // Typography
        FontSizeBase = 16.0,  // 1rem
        FontSizeSm = 14.0,    // 0.875rem
        FontSizeLg = 20.0,    // 1.25rem
        FontSizeH1 = 40.0,    // 2.5rem
        FontSizeH2 = 32.0,    // 2rem
        FontSizeH3 = 28.0,    // 1.75rem
        FontSizeH4 = 24.0,    // 1.5rem
        FontSizeH5 = 20.0,    // 1.25rem
        FontSizeH6 = 16.0,    // 1rem
        
        // Button Padding
        ButtonPaddingX = 16.0,   // Increased to improve touch target and parity
        ButtonPaddingY = 8.0,    // Increased from 6.0
        ButtonPaddingXLg = 20.0,
        ButtonPaddingYLg = 10.0,
        ButtonPaddingXSm = 12.0,
        ButtonPaddingYSm = 6.0,
        
        // Input Padding
        InputPaddingX = 12.0,
        InputPaddingY = 8.0,     // Increased from 6.0
        InputPaddingXLg = 16.0,
        InputPaddingYLg = 10.0,
        InputPaddingXSm = 8.0,
        InputPaddingYSm = 6.0,
        InputMinHeight = 38.0,
        InputMinHeightLg = 48.0,
        InputMinHeightSm = 31.0,
    };
}
