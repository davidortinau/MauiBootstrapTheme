using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Themes.Default;

/// <summary>
/// Bootswatch Darkly theme - a dark Bootstrap theme.
/// </summary>
public class DarklyTheme : IBootstrapThemeProvider
{
    public BootstrapTheme GetTheme() => new()
    {
        // Darkly Colors
        Primary = Color.FromArgb("#375a7f"),
        Secondary = Color.FromArgb("#444444"),
        Success = Color.FromArgb("#00bc8c"),
        Danger = Color.FromArgb("#e74c3c"),
        Warning = Color.FromArgb("#f39c12"),
        Info = Color.FromArgb("#3498db"),
        Light = Color.FromArgb("#adb5bd"),
        Dark = Color.FromArgb("#303030"),
        
        // Dark Mode by default
        Background = Color.FromArgb("#222222"),
        OnBackground = Color.FromArgb("#ffffff"),
        Surface = Color.FromArgb("#303030"),
        OnSurface = Color.FromArgb("#ffffff"),
        Outline = Color.FromArgb("#444444"),
        OutlineVariant = Color.FromArgb("#555555"),
        
        OnPrimary = Colors.White,
        OnSecondary = Colors.White,
        OnSuccess = Colors.White,
        OnDanger = Colors.White,
        OnWarning = Color.FromArgb("#222222"),
        OnInfo = Colors.White,
        
        DarkBackground = Color.FromArgb("#222222"),
        DarkOnBackground = Color.FromArgb("#ffffff"),
        DarkSurface = Color.FromArgb("#303030"),
        DarkOnSurface = Color.FromArgb("#ffffff"),
        DarkOutline = Color.FromArgb("#444444"),
        
        CornerRadius = 6.0,       // Bootstrap default is 6px (was 4)
        CornerRadiusSm = 4.0,     // Small is 4px (was 3)
        CornerRadiusLg = 8.0,
        CornerRadiusPill = 50.0,
        
        BorderWidth = 1.0,
        BorderWidthLg = 2.0,
        
        Spacer = 16.0,
        SpacerSm = 8.0,
        SpacerLg = 24.0,
        
        FontSizeBase = 16.0,
        FontSizeSm = 14.0,
        FontSizeLg = 20.0,
        
        ButtonPaddingX = 12.0,
        ButtonPaddingY = 6.0,
        ButtonPaddingXLg = 16.0,
        ButtonPaddingYLg = 8.0,
        ButtonPaddingXSm = 8.0,
        ButtonPaddingYSm = 4.0,
        
        InputPaddingX = 12.0,
        InputPaddingY = 6.0,
        InputMinHeight = 38.0,
    };
}

/// <summary>
/// Bootswatch Cyborg theme - a cyberpunk-styled dark theme.
/// </summary>
public class CyborgTheme : IBootstrapThemeProvider
{
    public BootstrapTheme GetTheme() => new()
    {
        // Cyborg Colors - neon accents on dark background
        Primary = Color.FromArgb("#2a9fd6"),
        Secondary = Color.FromArgb("#555555"),
        Success = Color.FromArgb("#77b300"),
        Danger = Color.FromArgb("#cc0000"),
        Warning = Color.FromArgb("#ff8800"),
        Info = Color.FromArgb("#9933cc"),
        Light = Color.FromArgb("#222222"),
        Dark = Color.FromArgb("#adafae"),
        
        Background = Color.FromArgb("#060606"),
        OnBackground = Color.FromArgb("#888888"),
        Surface = Color.FromArgb("#111111"),
        OnSurface = Color.FromArgb("#888888"),
        Outline = Color.FromArgb("#282828"),
        OutlineVariant = Color.FromArgb("#333333"),
        
        OnPrimary = Colors.White,
        OnSecondary = Colors.White,
        OnSuccess = Colors.White,
        OnDanger = Colors.White,
        OnWarning = Colors.White,
        OnInfo = Colors.White,
        
        DarkBackground = Color.FromArgb("#060606"),
        DarkOnBackground = Color.FromArgb("#888888"),
        DarkSurface = Color.FromArgb("#111111"),
        DarkOnSurface = Color.FromArgb("#888888"),
        DarkOutline = Color.FromArgb("#282828"),
        
        CornerRadius = 0.0,  // Sharp edges for cyberpunk look
        CornerRadiusSm = 0.0,
        CornerRadiusLg = 0.0,
        CornerRadiusPill = 50.0,
        
        BorderWidth = 1.0,
        BorderWidthLg = 2.0,
        
        Spacer = 16.0,
        SpacerSm = 8.0,
        SpacerLg = 24.0,
        
        FontSizeBase = 14.0,
        FontSizeSm = 12.0,
        FontSizeLg = 18.0,
        
        ButtonPaddingX = 12.0,
        ButtonPaddingY = 6.0,
        ButtonPaddingXLg = 16.0,
        ButtonPaddingYLg = 8.0,
        ButtonPaddingXSm = 8.0,
        ButtonPaddingYSm = 4.0,
        
        InputPaddingX = 12.0,
        InputPaddingY = 6.0,
        InputMinHeight = 38.0,
    };
}

/// <summary>
/// Bootswatch Minty theme - a fresh, minty light theme.
/// </summary>
public class MintyTheme : IBootstrapThemeProvider
{
    public BootstrapTheme GetTheme() => new()
    {
        // Minty Colors - fresh pastels
        Primary = Color.FromArgb("#78c2ad"),
        Secondary = Color.FromArgb("#f3969a"),
        Success = Color.FromArgb("#56cc9d"),
        Danger = Color.FromArgb("#ff7851"),
        Warning = Color.FromArgb("#ffce67"),
        Info = Color.FromArgb("#6cc3d5"),
        Light = Color.FromArgb("#f8f9fa"),
        Dark = Color.FromArgb("#343a40"),
        
        Background = Color.FromArgb("#ffffff"),
        OnBackground = Color.FromArgb("#5a5a5a"),
        Surface = Color.FromArgb("#ffffff"),
        OnSurface = Color.FromArgb("#5a5a5a"),
        Outline = Color.FromArgb("#dee2e6"),
        OutlineVariant = Color.FromArgb("#ced4da"),
        
        OnPrimary = Colors.White,
        OnSecondary = Colors.White,
        OnSuccess = Colors.White,
        OnDanger = Colors.White,
        OnWarning = Color.FromArgb("#5a5a5a"),
        OnInfo = Colors.White,
        
        DarkBackground = Color.FromArgb("#343a40"),
        DarkOnBackground = Color.FromArgb("#f8f9fa"),
        DarkSurface = Color.FromArgb("#495057"),
        DarkOnSurface = Color.FromArgb("#f8f9fa"),
        DarkOutline = Color.FromArgb("#6c757d"),
        
        CornerRadius = 20.0,  // Very rounded for friendly look
        CornerRadiusSm = 15.0,
        CornerRadiusLg = 25.0,
        CornerRadiusPill = 50.0,
        
        BorderWidth = 0.0,  // No borders - softer look
        BorderWidthLg = 0.0,
        
        Spacer = 16.0,
        SpacerSm = 8.0,
        SpacerLg = 24.0,
        
        FontSizeBase = 16.0,
        FontSizeSm = 14.0,
        FontSizeLg = 20.0,
        
        ButtonPaddingX = 16.0,
        ButtonPaddingY = 10.0,
        ButtonPaddingXLg = 20.0,
        ButtonPaddingYLg = 12.0,
        ButtonPaddingXSm = 12.0,
        ButtonPaddingYSm = 6.0,
        
        InputPaddingX = 16.0,
        InputPaddingY = 10.0,
        InputMinHeight = 44.0,
    };
}

/// <summary>
/// Bootswatch Slate theme - a professional dark gray theme.
/// </summary>
public class SlateTheme : IBootstrapThemeProvider
{
    public BootstrapTheme GetTheme() => new()
    {
        // Slate Colors - professional grays
        Primary = Color.FromArgb("#3a3f44"),
        Secondary = Color.FromArgb("#7a8288"),
        Success = Color.FromArgb("#62c462"),
        Danger = Color.FromArgb("#ee5f5b"),
        Warning = Color.FromArgb("#f89406"),
        Info = Color.FromArgb("#5bc0de"),
        Light = Color.FromArgb("#f8f9fa"),
        Dark = Color.FromArgb("#272b30"),
        
        Background = Color.FromArgb("#272b30"),
        OnBackground = Color.FromArgb("#c8c8c8"),
        Surface = Color.FromArgb("#3a3f44"),
        OnSurface = Color.FromArgb("#c8c8c8"),
        Outline = Color.FromArgb("#52575c"),
        OutlineVariant = Color.FromArgb("#62676c"),
        
        OnPrimary = Colors.White,
        OnSecondary = Colors.White,
        OnSuccess = Colors.White,
        OnDanger = Colors.White,
        OnWarning = Colors.White,
        OnInfo = Colors.White,
        
        DarkBackground = Color.FromArgb("#272b30"),
        DarkOnBackground = Color.FromArgb("#c8c8c8"),
        DarkSurface = Color.FromArgb("#3a3f44"),
        DarkOnSurface = Color.FromArgb("#c8c8c8"),
        DarkOutline = Color.FromArgb("#52575c"),
        
        CornerRadius = 6.0,       // Bootstrap default
        CornerRadiusSm = 4.0,
        CornerRadiusLg = 8.0,
        CornerRadiusPill = 50.0,
        
        BorderWidth = 1.0,
        BorderWidthLg = 2.0,
        
        Spacer = 16.0,
        SpacerSm = 8.0,
        SpacerLg = 24.0,
        
        FontSizeBase = 16.0,      // Bootstrap default (was 14)
        FontSizeSm = 14.0,        // (was 12)
        FontSizeLg = 20.0,        // (was 18)
        
        ButtonPaddingX = 12.0,
        ButtonPaddingY = 6.0,
        ButtonPaddingXLg = 16.0,
        ButtonPaddingYLg = 8.0,
        ButtonPaddingXSm = 8.0,
        ButtonPaddingYSm = 4.0,
        
        InputPaddingX = 12.0,
        InputPaddingY = 6.0,
        InputMinHeight = 38.0,
    };
}

/// <summary>
/// Bootswatch Flatly theme - flat design with clean lines.
/// </summary>
public class FlatlyTheme : IBootstrapThemeProvider
{
    public BootstrapTheme GetTheme() => new()
    {
        Primary = Color.FromArgb("#2c3e50"),
        Secondary = Color.FromArgb("#95a5a6"),
        Success = Color.FromArgb("#18bc9c"),
        Danger = Color.FromArgb("#e74c3c"),
        Warning = Color.FromArgb("#f39c12"),
        Info = Color.FromArgb("#3498db"),
        Light = Color.FromArgb("#ecf0f1"),
        Dark = Color.FromArgb("#7b8a8b"),
        
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
        OnWarning = Colors.White,
        OnInfo = Colors.White,
        
        DarkBackground = Color.FromArgb("#2c3e50"),
        DarkOnBackground = Color.FromArgb("#ecf0f1"),
        DarkSurface = Color.FromArgb("#34495e"),
        DarkOnSurface = Color.FromArgb("#ecf0f1"),
        DarkOutline = Color.FromArgb("#4a6278"),
        
        CornerRadius = 6.0,       // Bootstrap default (was 4)
        CornerRadiusSm = 4.0,     // (was 3)
        CornerRadiusLg = 8.0,     // (was 6)
        CornerRadiusPill = 50.0,
        BorderWidth = 0.0,
        BorderWidthLg = 0.0,
        FontSizeBase = 16.0,
        FontSizeSm = 14.0,
        FontSizeLg = 20.0,
        ButtonPaddingX = 16.0,
        ButtonPaddingY = 8.0,
        ButtonPaddingXLg = 20.0,
        ButtonPaddingYLg = 10.0,
        ButtonPaddingXSm = 12.0,
        ButtonPaddingYSm = 6.0,
        InputPaddingX = 12.0,
        InputPaddingY = 8.0,
        InputMinHeight = 40.0,
    };
}

/// <summary>
/// Bootswatch Sketchy theme - hand-drawn, playful look.
/// </summary>
public class SketchyTheme : IBootstrapThemeProvider
{
    public BootstrapTheme GetTheme() => new()
    {
        Primary = Color.FromArgb("#333333"),
        Secondary = Color.FromArgb("#868e96"),
        Success = Color.FromArgb("#28a745"),
        Danger = Color.FromArgb("#dc3545"),
        Warning = Color.FromArgb("#ffc107"),
        Info = Color.FromArgb("#17a2b8"),
        Light = Color.FromArgb("#f8f9fa"),
        Dark = Color.FromArgb("#343a40"),
        
        Background = Color.FromArgb("#ffffff"),
        OnBackground = Color.FromArgb("#333333"),
        Surface = Color.FromArgb("#ffffff"),
        OnSurface = Color.FromArgb("#333333"),
        Outline = Color.FromArgb("#333333"),
        OutlineVariant = Color.FromArgb("#666666"),
        
        OnPrimary = Colors.White,
        OnSecondary = Colors.White,
        OnSuccess = Colors.White,
        OnDanger = Colors.White,
        OnWarning = Color.FromArgb("#333333"),
        OnInfo = Colors.White,
        
        DarkBackground = Color.FromArgb("#333333"),
        DarkOnBackground = Color.FromArgb("#f8f9fa"),
        DarkSurface = Color.FromArgb("#444444"),
        DarkOnSurface = Color.FromArgb("#f8f9fa"),
        DarkOutline = Color.FromArgb("#666666"),
        
        CornerRadius = 15.0,
        CornerRadiusSm = 12.0,
        CornerRadiusLg = 20.0,
        CornerRadiusPill = 50.0,
        BorderWidth = 2.0,
        BorderWidthLg = 3.0,
        FontSizeBase = 16.0,
        FontSizeSm = 14.0,
        FontSizeLg = 20.0,
        ButtonPaddingX = 16.0,
        ButtonPaddingY = 10.0,
        ButtonPaddingXLg = 20.0,
        ButtonPaddingYLg = 12.0,
        ButtonPaddingXSm = 12.0,
        ButtonPaddingYSm = 6.0,
        InputPaddingX = 14.0,
        InputPaddingY = 10.0,
        InputMinHeight = 44.0,
    };
}

/// <summary>
/// Bootswatch Vapor theme - cyberpunk neon with purple/pink.
/// </summary>
public class VaporTheme : IBootstrapThemeProvider
{
    public BootstrapTheme GetTheme() => new()
    {
        Primary = Color.FromArgb("#6f42c1"),
        Secondary = Color.FromArgb("#ea39b8"),
        Success = Color.FromArgb("#3cf281"),
        Danger = Color.FromArgb("#ff6f6f"),
        Warning = Color.FromArgb("#fff787"),
        Info = Color.FromArgb("#32fbe2"),
        Light = Color.FromArgb("#44d9e8"),
        Dark = Color.FromArgb("#1a1a2e"),
        
        Background = Color.FromArgb("#190831"),
        OnBackground = Color.FromArgb("#ffffff"),
        Surface = Color.FromArgb("#1a1a2e"),
        OnSurface = Color.FromArgb("#ffffff"),
        Outline = Color.FromArgb("#6f42c1"),
        OutlineVariant = Color.FromArgb("#ea39b8"),
        
        OnPrimary = Colors.White,
        OnSecondary = Colors.White,
        OnSuccess = Color.FromArgb("#190831"),
        OnDanger = Color.FromArgb("#190831"),
        OnWarning = Color.FromArgb("#190831"),
        OnInfo = Color.FromArgb("#190831"),
        
        DarkBackground = Color.FromArgb("#190831"),
        DarkOnBackground = Color.FromArgb("#ffffff"),
        DarkSurface = Color.FromArgb("#1a1a2e"),
        DarkOnSurface = Color.FromArgb("#ffffff"),
        DarkOutline = Color.FromArgb("#6f42c1"),
        
        CornerRadius = 0.0,
        CornerRadiusSm = 0.0,
        CornerRadiusLg = 0.0,
        CornerRadiusPill = 50.0,
        BorderWidth = 2.0,
        BorderWidthLg = 3.0,
        FontSizeBase = 16.0,
        FontSizeSm = 14.0,
        FontSizeLg = 20.0,
        ButtonPaddingX = 16.0,
        ButtonPaddingY = 8.0,
        ButtonPaddingXLg = 20.0,
        ButtonPaddingYLg = 10.0,
        ButtonPaddingXSm = 12.0,
        ButtonPaddingYSm = 6.0,
        InputPaddingX = 14.0,
        InputPaddingY = 8.0,
        InputMinHeight = 40.0,
    };
}

/// <summary>
/// Bootswatch Brite theme - bold and bright colors.
/// </summary>
public class BriteTheme : IBootstrapThemeProvider
{
    public BootstrapTheme GetTheme() => new()
    {
        Primary = Color.FromArgb("#a2e436"),
        Secondary = Color.FromArgb("#ff7518"),
        Success = Color.FromArgb("#32cd32"),
        Danger = Color.FromArgb("#ff4444"),
        Warning = Color.FromArgb("#ffff00"),
        Info = Color.FromArgb("#00bfff"),
        Light = Color.FromArgb("#f5f5f5"),
        Dark = Color.FromArgb("#333333"),
        
        Background = Color.FromArgb("#ffffff"),
        OnBackground = Color.FromArgb("#333333"),
        Surface = Color.FromArgb("#ffffff"),
        OnSurface = Color.FromArgb("#333333"),
        Outline = Color.FromArgb("#cccccc"),
        OutlineVariant = Color.FromArgb("#dddddd"),
        
        OnPrimary = Color.FromArgb("#333333"),
        OnSecondary = Colors.White,
        OnSuccess = Color.FromArgb("#333333"),
        OnDanger = Colors.White,
        OnWarning = Color.FromArgb("#333333"),
        OnInfo = Color.FromArgb("#333333"),
        
        DarkBackground = Color.FromArgb("#222222"),
        DarkOnBackground = Color.FromArgb("#f5f5f5"),
        DarkSurface = Color.FromArgb("#333333"),
        DarkOnSurface = Color.FromArgb("#f5f5f5"),
        DarkOutline = Color.FromArgb("#555555"),
        
        CornerRadius = 8.0,
        CornerRadiusSm = 6.0,
        CornerRadiusLg = 12.0,
        CornerRadiusPill = 50.0,
        BorderWidth = 2.0,
        BorderWidthLg = 3.0,
        FontSizeBase = 16.0,
        FontSizeSm = 14.0,
        FontSizeLg = 20.0,
        ButtonPaddingX = 16.0,
        ButtonPaddingY = 10.0,
        ButtonPaddingXLg = 20.0,
        ButtonPaddingYLg = 12.0,
        ButtonPaddingXSm = 12.0,
        ButtonPaddingYSm = 6.0,
        InputPaddingX = 14.0,
        InputPaddingY = 10.0,
        InputMinHeight = 44.0,
    };
}
