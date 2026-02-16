# MauiBootstrapTheme

Use any [Bootstrap](https://getbootstrap.com/) or [Bootswatch](https://bootswatch.com/) CSS theme in your .NET MAUI app — no custom controls required.

Drop a Bootstrap CSS file into your project, call `.UseBootstrapTheme()`, and at build time the CSS is parsed into a native `ResourceDictionary` with styles for stock MAUI controls.

[![NuGet](https://img.shields.io/nuget/v/Plugin.Maui.BootstrapTheme.svg)](https://www.nuget.org/packages/Plugin.Maui.BootstrapTheme/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Build](https://github.com/davidortinau/MauiBootstrapTheme/actions/workflows/ci.yml/badge.svg)](https://github.com/davidortinau/MauiBootstrapTheme/actions/workflows/ci.yml)

## How It Works

1. **Build time** — The `MauiBootstrapTheme.Build` MSBuild task reads your Bootstrap CSS files and generates C# `ResourceDictionary` source files (`.g.cs`) via XAML Source Gen.
2. **Runtime** — The generated dictionaries are merged into `Application.Resources`, providing `StyleClass`-based styles for all stock MAUI controls.
3. **No wrappers** — Your `Button`, `Entry`, `Label`, `Border`, etc. remain standard MAUI controls. Styling is applied through `StyleClass` and `DynamicResource`.

## Quick Start

### 1. Install

```bash
dotnet add package Plugin.Maui.BootstrapTheme
```

### 2. Add a Bootstrap CSS file

Place any Bootstrap or Bootswatch CSS file in your project's `Resources/Themes/` folder (e.g., `Resources/Themes/bootstrap.min.css`). Mark it as a `BootstrapTheme` item in your `.csproj`:

```xml
<ItemGroup>
  <BootstrapTheme Include="Resources/Themes/bootstrap.min.css" />
  <BootstrapTheme Include="Resources/Themes/darkly.min.css" />
</ItemGroup>
```

### 3. Register in MauiProgram.cs

```csharp
using MauiBootstrapTheme.Extensions;

public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .UseBootstrapTheme();

    return builder.Build();
}
```

### 4. Apply a theme

```csharp
using MauiBootstrapTheme.Theming;

// Apply by name (matches the CSS filename without extension)
BootstrapTheme.Apply("bootstrap");   // default Bootstrap
BootstrapTheme.Apply("darkly");      // Bootswatch Darkly
BootstrapTheme.Apply("vapor");       // Bootswatch Vapor
```

### 5. Use StyleClass in XAML

```xml
<!-- Buttons -->
<Button Text="Primary" StyleClass="btn-primary" />
<Button Text="Outline" StyleClass="btn-outline-danger" />
<Button Text="Large Pill" StyleClass="btn-success,btn-pill-lg" />

<!-- Typography -->
<Label Text="Page Title" StyleClass="h1" />
<Label Text="Section heading" StyleClass="h4" />
<Label Text="Helper text" StyleClass="text-muted" />

<!-- Cards & Shadows -->
<Border StyleClass="card,shadow">
    <VerticalStackLayout>
        <Label Text="Card Title" StyleClass="h5" />
        <Label Text="Card content here" />
    </VerticalStackLayout>
</Border>

<!-- Colored backgrounds -->
<Border StyleClass="text-bg-primary">
    <Label Text="Primary background" StyleClass="on-primary" />
</Border>

<!-- Progress -->
<ProgressBar Progress="0.75" />
<ProgressBar Progress="0.5" StyleClass="progress-success" />

<!-- Badges -->
<Border StyleClass="badge,bg-danger">
    <Label Text="Alert" StyleClass="on-danger" />
</Border>
```

## Available Style Classes

### Buttons

| StyleClass | Description |
|------------|-------------|
| `btn-primary` .. `btn-dark` | Solid button variants (8 colors) |
| `btn-outline-primary` .. `btn-outline-dark` | Outline button variants |
| `btn-lg` | Large button sizing |
| `btn-sm` | Small button sizing |
| `btn-pill` | Pill-shaped (fully rounded) — default size |
| `btn-pill-lg` | Pill-shaped large button |
| `btn-pill-sm` | Pill-shaped small button |

### Typography (Label)

| StyleClass | Description |
|------------|-------------|
| `h1` .. `h6` | Heading levels with font size and weight |
| `text-muted` | Secondary/muted text color |
| `on-primary` .. `on-dark` | Contrast text color for use on colored backgrounds |

### Cards & Containers (Border)

| StyleClass | Description |
|------------|-------------|
| `card` | Card styling with border, corner radius, and padding |
| `shadow-sm` | Small shadow |
| `shadow` | Default shadow |
| `shadow-lg` | Large shadow |
| `text-bg-primary` .. `text-bg-dark` | Colored background containers |
| `bg-primary` .. `bg-warning` | Background color only |
| `badge` | Badge styling (small rounded container) |

### Progress (ProgressBar)

| StyleClass | Description |
|------------|-------------|
| *(implicit)* | Default primary progress color |
| `progress-primary` | Primary progress color |
| `progress-success` | Success progress color |
| `progress-danger` | Danger progress color |

## DynamicResource Keys

All theme values are available as `DynamicResource` keys for use in XAML bindings:

### Colors
`Primary`, `Secondary`, `Success`, `Danger`, `Warning`, `Info`, `Light`, `Dark`

### Spacing & Sizing
`CornerRadius`, `CornerRadiusSm`, `CornerRadiusLg`, `BorderWidth`, `ButtonHeight`, `ButtonHeightSm`, `ButtonHeightLg`

### Backgrounds
`BodyBackground`, `SurfaceColor`, `Gray100`

### Typography
`BodyFontSize`, `FontSizeH1` .. `FontSizeH6`

```xml
<!-- Use theme colors directly -->
<Border Stroke="{DynamicResource Primary}" StyleClass="card">
    <Label Text="Custom border color" />
</Border>

<CheckBox Color="{DynamicResource Primary}" />
<Switch OnColor="{DynamicResource Success}" />
<Slider MinimumTrackColor="{DynamicResource Primary}" ThumbColor="{DynamicResource Primary}" />
<ActivityIndicator Color="{DynamicResource Danger}" IsRunning="True" />
```

## Dark Mode

Themes automatically support light/dark mode. The generated `ResourceDictionary` responds to system theme changes.

```csharp
// Test dark mode programmatically
App.Current.UserAppTheme = AppTheme.Dark;

// Reset to system default
App.Current.UserAppTheme = AppTheme.Unspecified;
```

## Included Sample Themes

The sample app ships with these [Bootswatch](https://bootswatch.com/) themes:

| Theme | Description |
|-------|-------------|
| Default | Standard Bootstrap 5 |
| Darkly | Dark theme with muted blue tones |
| Slate | Dark gunmetal-gray theme |
| Flatly | Flat and modern light theme |
| Sketchy | Hand-drawn style (requires Neucha font) |
| Vapor | Cyberpunk neon with dark background |
| Brite | Vibrant neon-inspired light theme |

To use any Bootswatch theme, download the CSS from [bootswatch.com](https://bootswatch.com/) and add it to your project.

## Font Handling

Some Bootstrap themes require specific fonts (e.g., Lato, Neucha). If a theme references a non-system font, the build emits a **BT0001 warning** with the exact code to add:

```
warning BT0001: Theme 'darkly' requires font 'Lato' which is not available as a system font.
Download the font file and add it to your project:
// In MauiProgram.cs, add to ConfigureFonts:
.ConfigureFonts(fonts =>
{
    fonts.AddFont("lato.ttf", "Lato");
})
```

## Known Limitations

- **No text-shadow** — MAUI does not support text shadow on `Label`
- **No gradient backgrounds on buttons** — CSS `background-image: linear-gradient(...)` is parsed but MAUI `Button` does not support `LinearGradientBrush` on all platforms; solid `Background` color is used instead
- **No border-image** — CSS decorative borders (e.g., Sketchy theme hand-drawn borders) cannot be replicated with native controls
- **Shadow via StyleClass** — `StyleClass="card,shadow"` may not render on some platforms due to MAUI framework limitations with `Shadow` in Style Setters; use the `theme:Bootstrap.Shadow` attached property as a fallback
- **Font weight granularity** — MAUI only supports `Bold` and normal; CSS `font-weight: 500` is mapped to normal
- **CornerRadius is integer** — `Button.CornerRadius` is `int` in MAUI, so fractional CSS values are rounded
- **Percentage-based sizing** — CSS percentage values (e.g., `width: 50%`) have no MAUI equivalent
- **No CSS custom properties at runtime** — CSS variables are resolved at build time, not runtime
- **Platform rendering differences** — Some visual details (checkbox style, switch track shape) vary by platform

## Attached Properties (Legacy)

The library also includes attached properties for scenarios where `StyleClass` is not sufficient:

```xml
xmlns:theme="clr-namespace:MauiBootstrapTheme.Theming;assembly=MauiBootstrapTheme"

<!-- Shadow on any VisualElement -->
<Border theme:Bootstrap.Shadow="Default" StyleClass="card">
    <Label Text="Card with shadow" />
</Border>
```

## Requirements

- .NET 10.0+
- MAUI workload installed

## Building from Source

```bash
# Build the MSBuild task
dotnet build src/MauiBootstrapTheme.Build

# Build and run the sample app (macOS)
dotnet build samples/MauiBootstrapTheme.Sample -f net10.0-maccatalyst -t:Run

# Run tests
dotnet test tests/MauiBootstrapTheme.Build.Tests
```

## Contributing

Contributions welcome! Please open an issue first to discuss what you'd like to change.

## License

MIT License — see [LICENSE](LICENSE) for details.

## Acknowledgments

- [Bootswatch](https://bootswatch.com/) for the free Bootstrap themes
- [Bootstrap](https://getbootstrap.com/) for the CSS framework
- Inspired by [FlagstoneUI](https://github.com/matt-goldman/flagstone-ui)
