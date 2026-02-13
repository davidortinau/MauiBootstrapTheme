# MauiBootstrapTheme

Style stock .NET MAUI controls with Bootstrap themes — no custom control wrappers required.

[![NuGet](https://img.shields.io/nuget/v/MauiBootstrapTheme.svg)](https://www.nuget.org/packages/MauiBootstrapTheme/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Features

✅ **Stock MAUI controls** — Entry, Button, Editor, Picker, DatePicker, TimePicker, SearchBar, CheckBox, Switch, RadioButton, Slider, Stepper, ProgressBar, ActivityIndicator, Border, Label  
✅ **Bootstrap 5 themes** — Use any Bootstrap CSS theme  
✅ **Per-control variants** — Primary, Secondary, Success, Danger, Warning, Info, Light, Dark  
✅ **Size variants** — Small, Default, Large  
✅ **Typography** — Headings (H1-H6), Lead, Small, Muted, Mark text styles  
✅ **Badges** — Badge variants for Labels  
✅ **Cards & Shadows** — Border control with shadow levels (None, Small, Default, Large)  
✅ **Spacing** — Bootstrap spacing scale (0-5) for margin and padding  
✅ **Dark mode** — Respects system theme preference with opt-out  
✅ **Outline & Pill styles** — Full Bootstrap button arsenal  
✅ **Platform support** — iOS, Android, Mac Catalyst, Windows  
✅ **No custom wrappers** — Just your regular MAUI controls  

## Quick Start

### 1. Install the NuGet Package

```bash
dotnet add package MauiBootstrapTheme
dotnet add package MauiBootstrapTheme.Themes.Default
```

### 2. Register in MauiProgram.cs

```csharp
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Themes.Default;

public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .UseBootstrapTheme<DefaultTheme>();  // That's it!
    
    return builder.Build();
}
```

### 3. Use Bootstrap Styling in XAML

```xml
<ContentPage xmlns:theme="clr-namespace:MauiBootstrapTheme.Theming;assembly=MauiBootstrapTheme">

    <!-- Styled Entry controls -->
    <Entry Placeholder="Default styling" />
    <Entry Placeholder="Primary" theme:Bootstrap.Variant="Primary" />
    <Entry Placeholder="Danger" theme:Bootstrap.Variant="Danger" />
    
    <!-- Styled Buttons -->
    <Button Text="Primary" theme:Bootstrap.Variant="Primary" />
    <Button Text="Success" theme:Bootstrap.Variant="Success" />
    <Button Text="Outline" theme:Bootstrap.Variant="Secondary" theme:Bootstrap.IsOutlined="True" />
    <Button Text="Pill" theme:Bootstrap.Variant="Primary" theme:Bootstrap.IsPill="True" />
    
    <!-- Typography -->
    <Label Text="Heading 1" theme:Bootstrap.Heading="1" />
    <Label Text="This is lead text" theme:Bootstrap.TextStyle="Lead" />
    <Label Text="Muted helper text" theme:Bootstrap.TextStyle="Muted" />
    
    <!-- Badges -->
    <Label Text="New" theme:Bootstrap.Badge="Success" />
    <Label Text="4" theme:Bootstrap.Badge="Primary" />
    
    <!-- Cards with Shadows -->
    <Border theme:Bootstrap.Shadow="Default" Padding="16">
        <Label Text="Card content" />
    </Border>
    
    <!-- Spacing -->
    <StackLayout theme:Bootstrap.PaddingLevel="3" theme:Bootstrap.MarginLevel="2">
        <Label Text="Content with Bootstrap spacing" />
    </StackLayout>

</ContentPage>
```

## Supported Controls

| Control | Variants | Sizes | Features |
|---------|----------|-------|----------|
| Button | ✅ | ✅ | Outline, Pill styles |
| Entry | ✅ | ✅ | Border, corner radius |
| Editor | ✅ | ✅ | Multi-line text |
| SearchBar | ✅ | ✅ | Search icon tint |
| Picker | ✅ | ✅ | Dropdown styling |
| DatePicker | ✅ | — | Calendar styling |
| TimePicker | ✅ | — | Time selector |
| CheckBox | ✅ | — | Tint color |
| Switch | ✅ | — | Track/thumb colors |
| RadioButton | ✅ | — | Selection color |
| Slider | ✅ | — | Track/thumb colors |
| Stepper | ✅ | — | Button tint |
| ProgressBar | ✅ | — | Progress color |
| ActivityIndicator | ✅ | — | Spinner color |
| Border | ✅ | — | Shadows, backgrounds |
| Label | — | — | Headings, text styles, badges |

## Attached Properties

### Bootstrap.Variant
Color variant for controls.

```xml
<Button Text="Submit" theme:Bootstrap.Variant="Primary" />
<Entry theme:Bootstrap.Variant="Danger" />
<CheckBox theme:Bootstrap.Variant="Success" />
```

### Bootstrap.Size
Size variant (Small, Default, Large).

```xml
<Button Text="Large" theme:Bootstrap.Size="Large" />
<Entry theme:Bootstrap.Size="Small" />
```

### Bootstrap.IsOutlined
Outline-style button.

```xml
<Button Text="Outline" theme:Bootstrap.Variant="Primary" theme:Bootstrap.IsOutlined="True" />
```

### Bootstrap.IsPill
Pill-shaped button.

```xml
<Button Text="Pill" theme:Bootstrap.Variant="Success" theme:Bootstrap.IsPill="True" />
```

### Bootstrap.Heading
Heading level (1-6) for Labels.

```xml
<Label Text="Page Title" theme:Bootstrap.Heading="1" />
<Label Text="Section" theme:Bootstrap.Heading="3" />
```

### Bootstrap.TextStyle
Text style (Lead, Small, Muted, Mark).

```xml
<Label Text="Introduction" theme:Bootstrap.TextStyle="Lead" />
<Label Text="Fine print" theme:Bootstrap.TextStyle="Small" />
<Label Text="Secondary info" theme:Bootstrap.TextStyle="Muted" />
<Label Text="Highlighted" theme:Bootstrap.TextStyle="Mark" />
```

### Bootstrap.TextColorVariant
Text color variant.

```xml
<Label Text="Error message" theme:Bootstrap.TextColorVariant="Danger" />
<Label Text="Success!" theme:Bootstrap.TextColorVariant="Success" />
```

### Bootstrap.Badge
Badge variant (turns Label into a badge).

```xml
<Label Text="New" theme:Bootstrap.Badge="Success" />
<Label Text="99+" theme:Bootstrap.Badge="Danger" />
```

### Bootstrap.Shadow
Shadow level for Border (None, Small, Default, Large).

```xml
<Border theme:Bootstrap.Shadow="Small" Padding="16">
    <Label Text="Subtle elevation" />
</Border>

<Border theme:Bootstrap.Shadow="Large" Padding="16">
    <Label Text="Prominent card" />
</Border>
```

### Bootstrap.BackgroundVariant
Background color variant for containers.

```xml
<Border theme:Bootstrap.BackgroundVariant="Primary" Padding="16">
    <Label Text="Primary background" TextColor="White" />
</Border>
```

### Bootstrap.MarginLevel / Bootstrap.PaddingLevel
Bootstrap spacing scale (0-5).

| Level | Value |
|-------|-------|
| 0 | 0px |
| 1 | 4px (0.25rem) |
| 2 | 8px (0.5rem) |
| 3 | 16px (1rem) |
| 4 | 24px (1.5rem) |
| 5 | 48px (3rem) |

```xml
<StackLayout theme:Bootstrap.PaddingLevel="4" theme:Bootstrap.MarginLevel="3">
    <Label Text="Consistent spacing" />
</StackLayout>
```

## Available Variants

| Variant | Color |
|---------|-------|
| `Primary` | #0d6efd |
| `Secondary` | #6c757d |
| `Success` | #198754 |
| `Danger` | #dc3545 |
| `Warning` | #ffc107 |
| `Info` | #0dcaf0 |
| `Light` | #f8f9fa |
| `Dark` | #212529 |

## Included Themes

The `MauiBootstrapTheme.Themes.Default` package includes several Bootswatch-inspired themes:

| Theme | Description |
|-------|-------------|
| `DefaultTheme` | Standard Bootstrap 5 - blue primary, rounded corners |
| `DarklyTheme` | Dark mode with teal accents |
| `CyborgTheme` | Cyberpunk style - sharp edges, neon colors |
| `MintyTheme` | Fresh pastels with very rounded corners |
| `SlateTheme` | Professional dark gray theme |

```csharp
// Use any included theme
.UseBootstrapTheme<DarklyTheme>()
.UseBootstrapTheme<MintyTheme>()
.UseBootstrapTheme<CyborgTheme>()
.UseBootstrapTheme<SlateTheme>()
```

## Dark Mode Support

MauiBootstrapTheme respects the system dark/light mode preference by default. The theme automatically adjusts surface, background, and outline colors.

```csharp
// Opt out of system theme (force light mode)
BootstrapTheme.RespectSystemTheme = false;
```

## Adding Custom Themes

### Option 1: Use a Pre-built Theme

```bash
dotnet add package MauiBootstrapTheme.Themes.Default
```

```csharp
.UseBootstrapTheme<DefaultTheme>()
```

### Option 2: Create Your Own Theme

```csharp
using MauiBootstrapTheme.Theming;

var myTheme = new BootstrapTheme
{
    Primary = Color.FromArgb("#7c3aed"),    // Purple
    Secondary = Color.FromArgb("#64748b"),
    Success = Color.FromArgb("#22c55e"),
    Danger = Color.FromArgb("#ef4444"),
    Warning = Color.FromArgb("#f59e0b"),
    Info = Color.FromArgb("#06b6d4"),
    
    CornerRadius = 8.0,
    BorderWidth = 2.0,
    
    // Dark mode colors
    DarkBackground = Color.FromArgb("#1e1b4b"),
    DarkSurface = Color.FromArgb("#312e81"),
};

builder.UseBootstrapTheme(myTheme);
```

### Option 3: Load from CSS File (Runtime)

```csharp
builder.UseBootstrapTheme(options =>
{
    options.CssPath = "Resources/my-bootstrap.css";
});
```

## How It Works

MauiBootstrapTheme uses **handler extensions** to add Bootstrap styling to stock MAUI controls. This means:

1. **No inheritance** — Your controls remain standard MAUI controls
2. **No breaking changes** — Existing code works unchanged
3. **Full platform support** — Native styling on each platform

```csharp
// Under the hood, we use Mapper.AppendToMapping
EntryHandler.Mapper.AppendToMapping("BootstrapStyle", (handler, view) =>
{
#if IOS || MACCATALYST
    handler.PlatformView.Layer.BorderColor = theme.Outline.ToCGColor();
    handler.PlatformView.Layer.CornerRadius = theme.CornerRadius;
#elif ANDROID
    handler.PlatformView.Background = CreateBorderedDrawable(theme);
#endif
});
```

## Sample App

The repository includes a sample app demonstrating all features:

```bash
cd samples/MauiBootstrapTheme.Sample
dotnet build -f net10.0-maccatalyst -t:Run
```

The sample app includes pages for:
- **Controls** — Buttons, Entries, Editors
- **Inputs** — All input control types
- **Typography** — Headings, text styles, badges
- **Cards** — Border with shadows and backgrounds
- **Spacing** — Margin and padding utilities
- **Variants** — All color variants
- **Forms** — Complete form example
- **Themes** — Theme switcher

## Requirements

- .NET 10.0+
- MAUI workload installed

## Contributing

Contributions welcome! Please open an issue first to discuss what you'd like to change.

## License

MIT License - see [LICENSE](LICENSE) for details.

## Acknowledgments

- Inspired by [FlagstoneUI](https://github.com/matt-goldman/flagstone-ui)
- Bootstrap theme system by [Bootstrap](https://getbootstrap.com/)
