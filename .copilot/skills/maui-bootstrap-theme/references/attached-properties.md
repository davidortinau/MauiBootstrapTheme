# Attached Properties (Alternative to StyleClass)

For buttons, you can use attached properties instead of StyleClass for native handler-level styling.

XAML namespace: `xmlns:theme="clr-namespace:MauiBootstrapTheme.Theming;assembly=MauiBootstrapTheme"`

## Button Variant & Size

```xml
<Button Text="Primary" theme:Bootstrap.Variant="Primary"/>
<Button Text="Outline" theme:Bootstrap.Variant="OutlinePrimary"/>
<Button Text="Large" theme:Bootstrap.Variant="Danger" theme:Bootstrap.Size="Large"/>
```

Variant values: `Primary`, `Secondary`, `Success`, `Danger`, `Warning`, `Info`, `Light`, `Dark`, `OutlinePrimary`...`OutlineDark`, `Link`

## Spacing

```xml
<Border theme:Bootstrap.PaddingLevel="3"/>  <!-- 0-5 scale: 0=0, 1=4, 2=8, 3=16, 4=24, 5=48 -->
<Border theme:Bootstrap.MarginLevel="2"/>
```

MauiReactor: `.Set(Bootstrap.PaddingLevelProperty, 3)` / `.Set(Bootstrap.MarginLevelProperty, 2)`

## Shadow

```xml
<Border theme:Bootstrap.Shadow="Default" StyleClass="card">
    <Label Text="Card with shadow" />
</Border>
```

Shadow values: `None`, `Small`, `Default`, `Large`
