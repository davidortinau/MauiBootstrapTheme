---
name: maui-bootstrap-theme
description: >-
  Styling guide for MauiBootstrapTheme (Plugin.Maui.BootstrapTheme NuGet).
  Use when writing, editing, or reviewing .NET MAUI XAML, C#, or MauiReactor
  code that uses Bootstrap-style classes. Covers installation, setup,
  per-control StyleClass reference, DynamicResource keys, visual states,
  theme switching, and common mistakes. Triggers: "bootstrap", "btn-primary",
  "StyleClass", "form-control", "card", "bootstrap theme", "theme switching",
  "BootstrapTheme", or any Bootstrap CSS class name used in MAUI context.
---

# MauiBootstrapTheme Styling Guide

Bootstrap 5 styling for .NET MAUI via CSS-generated ResourceDictionaries.
NuGet: `Plugin.Maui.BootstrapTheme`.

## Setup

### 1. Install NuGet

```xml
<PackageReference Include="Plugin.Maui.BootstrapTheme" />
```

### 2. Add CSS themes to .csproj

```xml
<ItemGroup>
  <BootstrapCss Include="Resources\Themes\bootstrap.min.css" ThemeName="default" />
  <BootstrapCss Include="Resources\Themes\darkly.min.css" />
</ItemGroup>
```

Download CSS from [Bootswatch](https://bootswatch.com) or use stock Bootstrap 5.

### 3. Register in MauiProgram.cs

```csharp
// XAML app:
builder.UseMauiApp<App>()
    .UseBootstrapTheme(options => {
        options.AddTheme<Themes.DefaultTheme>("default");
        options.AddTheme<Themes.DarklyTheme>("darkly");
    });

// MauiReactor app:
builder.UseMauiReactorApp<MainPage>()
    .UseBootstrapTheme(options => {
        options.AddTheme<Themes.DefaultTheme>("default");
    });
```

### 4. Load theme in App.xaml.cs

```csharp
Resources.MergedDictionaries.Add(new Themes.DefaultTheme());
BootstrapTheme.SyncFromResources(Resources);
```

Theme classes (e.g., `DefaultTheme`) are auto-generated at build time into `obj/BootstrapTheme/`.

## Critical Rules

**ALWAYS apply StyleClass to EVERY styled control.** A bare `<Button Text="Save"/>` gets no Bootstrap styling. Write `<Button Text="Save" StyleClass="btn-primary"/>`.

**ALWAYS use `DynamicResource` (not `StaticResource`)** for color/theme keys — enables runtime theme switching.

**ALWAYS set page background:**
```csharp
this.BackgroundColor = BootstrapTheme.Current.GetBackground();
```

## StyleClass Reference

### Buttons

| StyleClass | Effect |
|------------|--------|
| `btn-primary` `btn-secondary` `btn-success` `btn-danger` `btn-warning` `btn-info` `btn-light` `btn-dark` | Solid colored button |
| `btn-outline-primary` ... `btn-outline-dark` | Transparent bg, colored border+text; fills on hover |
| `btn-lg` | Large button (combine with variant) |
| `btn-sm` | Small button (combine with variant) |
| `btn-pill` | Pill/capsule shape (combine with variant) |

**Combine classes with comma:** `StyleClass="btn-primary,btn-lg"` or `StyleClass="btn-outline-danger,btn-sm"`

Buttons include visual states: Pressed darkens bg, PointerOver lightens bg (desktop), Disabled = 0.65 opacity.

#### XAML
```xml
<Button Text="Save" StyleClass="btn-primary"/>
<Button Text="Delete" StyleClass="btn-outline-danger,btn-sm"/>
<Button Text="Submit" StyleClass="btn-success,btn-lg"/>
<Button Text="Disabled" StyleClass="btn-primary" IsEnabled="False"/>
```

#### C# / MauiReactor
```csharp
// C#
new Button { Text = "Save", StyleClass = { "btn-primary" } };
// MauiReactor
Button("Save").Class("btn-primary")
Button("Delete").Class("btn-outline-danger").Class("btn-sm")
```

### Typography (Label)

| StyleClass | Effect |
|------------|--------|
| `h1` `h2` `h3` `h4` `h5` `h6` | Heading sizes |
| `lead` | Larger lead paragraph text |
| `small` | Smaller text |
| `text-muted` | Gray/secondary text color |
| `mark` | Highlighted/marked text |
| `form-label` | Label above a form field |
| `form-text` | Help text below a form field |
| `form-check-label` | Label next to checkbox/radio |

**Text color classes:**

| StyleClass | Effect |
|------------|--------|
| `text-primary` ... `text-dark` | Semantic text color |
| `on-primary` ... `on-dark` | Contrast text ON a colored background |

**Combine freely:** `StyleClass="h4,text-muted"` or `StyleClass="lead,text-primary"`

#### XAML
```xml
<Label Text="Page Title" StyleClass="h1"/>
<Label Text="Subtitle" StyleClass="lead,text-muted"/>
<Label Text="Field Label" StyleClass="form-label"/>
<Label Text="On blue card" StyleClass="h5,on-primary"/>
```

#### C# / MauiReactor
```csharp
// C#
new Label { Text = "Page Title", StyleClass = { "h1" } };
// MauiReactor
Label("Page Title").Class("h1")
Label("Subtitle").Class("lead").Class("text-muted")
Label("Field Label").Class("form-label")
```

### Form Inputs

| StyleClass | Controls | Effect |
|------------|----------|--------|
| `form-control` | Entry, Editor, DatePicker, TimePicker, SearchBar | Standard input styling |
| `form-control-lg` | Entry, Editor | Large input (combine with form-control) |
| `form-control-sm` | Entry, Editor | Small input (combine with form-control) |
| `form-select` | Picker | Dropdown select styling |
| `form-select-lg` | Picker | Large dropdown |
| `form-select-sm` | Picker | Small dropdown |
| `form-check-input` | CheckBox, RadioButton | Check/radio styling |
| `form-switch` | Switch | Toggle switch styling |
| `form-range` | Slider | Range slider styling |

**Every input control MUST have a StyleClass.** A bare `<Entry/>` is unstyled.

#### XAML
```xml
<Label Text="Email" StyleClass="form-label"/>
<Entry Placeholder="name@example.com" StyleClass="form-control"/>

<Label Text="Message" StyleClass="form-label"/>
<Editor Placeholder="Type here..." StyleClass="form-control" HeightRequest="100"/>

<Label Text="Country" StyleClass="form-label"/>
<Picker Title="Select..." StyleClass="form-select"/>

<SearchBar Placeholder="Search..." StyleClass="form-control"/>
<DatePicker StyleClass="form-control"/>
<TimePicker StyleClass="form-control"/>

<CheckBox StyleClass="form-check-input"/>
<Label Text="Remember me" StyleClass="form-check-label"/>

<Switch StyleClass="form-switch"/>
<Slider StyleClass="form-range"/>
```

#### C# / MauiReactor
```csharp
Label("Email").Class("form-label")
Entry().Placeholder("name@example.com").Class("form-control")
Picker().Title("Select...").Class("form-select")
CheckBox().Class("form-check-input")
Switch().Class("form-switch")
```

### Cards & Containers (Border)

| StyleClass | Effect |
|------------|--------|
| `card` | Card with border, radius, padding |
| `card-hoverable` | Adds shadow on pointer hover (desktop) |
| `shadow-sm` | Small drop shadow |
| `shadow` | Medium drop shadow |
| `shadow-lg` | Large drop shadow |
| `text-bg-primary` ... `text-bg-dark` | Colored card (bg + contrast text) |
| `bg-primary` ... `bg-dark` | Background color only |

#### XAML
```xml
<!-- Basic card -->
<Border StyleClass="card">
    <VerticalStackLayout>
        <Label Text="Card Title" StyleClass="h5"/>
        <Label Text="Card content" StyleClass="text-muted"/>
    </VerticalStackLayout>
</Border>

<!-- Card with shadow and hover effect -->
<Border StyleClass="card,shadow,card-hoverable">
    <VerticalStackLayout>
        <Label Text="Hover me" StyleClass="h5"/>
        <Button Text="Action" StyleClass="btn-primary,btn-sm" HorizontalOptions="Start"/>
    </VerticalStackLayout>
</Border>

<!-- Colored card -->
<Border StyleClass="card,text-bg-primary">
    <Label Text="Primary card" StyleClass="on-primary"/>
</Border>
```

#### C# / MauiReactor
```csharp
Border(
    VStack(
        Label("Card Title").Class("h5"),
        Label("Content").Class("text-muted")
    )
).Class("card").Class("shadow")
```

### Badges

Badges use a Border container with a Label inside:

```xml
<Border StyleClass="badge,bg-primary">
    <Label Text="New" StyleClass="on-primary,small"/>
</Border>
```

### Progress

```xml
<ProgressBar Progress="0.75"/>                              <!-- Primary (default) -->
<ProgressBar Progress="0.5" StyleClass="progress-success"/>
<ProgressBar Progress="0.25" StyleClass="progress-danger"/>
```

## DynamicResource Keys

### Colors
| Key | Usage |
|-----|-------|
| `Primary` `Secondary` `Success` `Danger` `Warning` `Info` `Light` `Dark` | Semantic colors |
| `OnPrimary` ... `OnDark` | Contrast text colors |
| `Background` | Page/app background |
| `OnBackground` | Text on background |
| `Surface` | Card/container background |
| `OnSurface` | Text on surface |
| `Outline` | Border color |
| `OutlineVariant` | Subtle border/divider color |
| `Gray100` ... `Gray900` | Gray scale colors |

```xml
<BoxView Color="{DynamicResource Primary}"/>
<Border Stroke="{DynamicResource Outline}"/>
<Label TextColor="{DynamicResource OnBackground}"/>
```

## Attached Properties

Alternative to StyleClass for handler-level styling (buttons, spacing, shadows). See `references/attached-properties.md` for details.

## Theme Switching

```csharp
// Switch theme at runtime:
BootstrapTheme.Apply("darkly");
```

Available Bootswatch themes: `default`, `darkly`, `slate`, `flatly`, `sketchy`, `vapor`, `brite` (or any Bootstrap 5 CSS).

### Handling Theme Changes

`BootstrapTheme.Apply()` swaps the `ResourceDictionary` and fires `BootstrapTheme.ThemeChanged`. Controls using `DynamicResource` update automatically, but **page backgrounds and any values read from `BootstrapTheme.Current` do not** — you must listen and refresh.

#### XAML Pages

Subscribe to `BootstrapTheme.ThemeChanged` to update the page background (and any other `BootstrapTheme.Current` values) on theme switch:

```csharp
public partial class MyPage : ContentPage
{
    public MyPage()
    {
        InitializeComponent();
        this.BackgroundColor = BootstrapTheme.Current.GetBackground();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BootstrapTheme.ThemeChanged += OnThemeChanged;
    }

    protected override void OnDisappearing()
    {
        BootstrapTheme.ThemeChanged -= OnThemeChanged;
        base.OnDisappearing();
    }

    private void OnThemeChanged(object? sender, EventArgs e)
    {
        this.BackgroundColor = BootstrapTheme.Current.GetBackground();
    }
}
```

#### MauiReactor Pages — BasePage Pattern

In MauiReactor, create a `BasePage` component that subscribes to `ThemeChanged` and calls `Invalidate()` to re-render with the new theme values:

```csharp
abstract class BasePage : Component
{
    protected override void OnMounted()
    {
        BootstrapTheme.ThemeChanged += OnThemeChanged;
        base.OnMounted();
    }

    protected override void OnWillUnmount()
    {
        BootstrapTheme.ThemeChanged -= OnThemeChanged;
        base.OnWillUnmount();
    }

    private void OnThemeChanged(object? sender, EventArgs e) => Invalidate();

    public abstract VisualNode RenderContent();

    public override VisualNode Render()
    {
        return ContentPage(
            RenderContent()
        ).BackgroundColor(BootstrapTheme.Current.GetBackground());
    }
}
```

Then every page extends `BasePage` (or `BasePage<TState>` for stateful pages — same pattern with `Component<TState>`):

```csharp
class ControlsPage : BasePage
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                Label("Controls").Class("h1"),
                Button("Primary").Class("btn-primary")
            ).Padding(20)
        );
}
```

## Common Mistakes

### ❌ DON'T: Bare controls without StyleClass
```xml
<Button Text="Save"/>           <!-- No styling! -->
<Entry Placeholder="Name"/>     <!-- No styling! -->
```

### ✅ DO: Always apply StyleClass
```xml
<Button Text="Save" StyleClass="btn-primary"/>
<Entry Placeholder="Name" StyleClass="form-control"/>
```

### ❌ DON'T: Use wrong class on wrong control
```xml
<Entry StyleClass="btn-primary"/>        <!-- btn-* is for Button only -->
<Button StyleClass="form-control"/>      <!-- form-control is for inputs -->
<Label StyleClass="card"/>               <!-- card is for Border only -->
```

### ✅ DO: Match classes to control types
```xml
<Button StyleClass="btn-primary"/>
<Entry StyleClass="form-control"/>
<Border StyleClass="card"/>
<Label StyleClass="h4"/>
```

### ❌ DON'T: Forget text contrast on colored backgrounds
```xml
<Border StyleClass="text-bg-primary">
    <Label Text="Invisible text"/>      <!-- Text inherits dark color, invisible on blue -->
</Border>
```

### ✅ DO: Use on-variant classes for contrast text
```xml
<Border StyleClass="text-bg-primary">
    <Label Text="Visible text" StyleClass="on-primary"/>
</Border>
```

### ❌ DON'T: Use StaticResource for theme colors
```xml
<Label TextColor="{StaticResource Primary}"/>  <!-- Won't update on theme switch -->
```

### ✅ DO: Use DynamicResource
```xml
<Label TextColor="{DynamicResource Primary}"/>
```

### ❌ DON'T: Forget page background and theme change handling
```csharp
public MyPage() { InitializeComponent(); }  // White bg, won't update on theme switch
```

### ✅ DO: Set background and subscribe to theme changes
See the **Handling Theme Changes** section above for the full XAML and MauiReactor patterns.

## Control → StyleClass Quick Reference

| Control | Required StyleClass | Size Variants | Notes |
|---------|-------------------|---------------|-------|
| Button | `btn-{variant}` or `btn-outline-{variant}` | `btn-lg` `btn-sm` `btn-pill` | Combine: `btn-primary,btn-lg` |
| Label | `h1`-`h6`, `lead`, `small`, `text-muted` | — | Use `on-{variant}` on colored bg |
| Entry | `form-control` | `form-control-lg` `form-control-sm` | |
| Editor | `form-control` | `form-control-lg` `form-control-sm` | Set HeightRequest too |
| Picker | `form-select` | `form-select-lg` `form-select-sm` | |
| DatePicker | `form-control` | — | |
| TimePicker | `form-control` | — | |
| SearchBar | `form-control` | — | |
| CheckBox | `form-check-input` | — | |
| RadioButton | `form-check-input` | — | |
| Switch | `form-switch` | — | |
| Slider | `form-range` | — | |
| Border | `card` | — | Add `shadow-sm`/`shadow`/`shadow-lg`, `card-hoverable` |
| ProgressBar | (default) | — | `progress-success` `progress-danger` for variants |
