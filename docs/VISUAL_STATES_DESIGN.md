# Visual States Design Proposal — MauiBootstrapTheme

## 1. Problem Statement

MauiBootstrapTheme renders Bootstrap-styled controls at rest, but controls feel lifeless: pressing a button shows no shade change, focusing an entry shows no ring, hovering a button on desktop shows no color shift, and disabling a control is indistinguishable from its enabled state. The visual-parity-report.md explicitly lists `:hover`/`:focus` states and `transition`/`animation` as "⚠️ Not generated".

Bootstrap CSS defines five interactive states for every interactive control. Implementing them will close the largest remaining visual-parity gap.

---

## 2. State Mapping — Bootstrap CSS → MAUI Visual States

| Bootstrap CSS Pseudo-class | MAUI VisualState | Platform Relevance | Priority |
|---|---|---|---|
| *(none)* / default | `Normal` | All | P0 — already implemented |
| `:hover` | `PointerOver` | Windows, Mac Catalyst, iPad+mouse/trackpad | P1 |
| `:focus` | `Focused` | All (keyboard on desktop, ring on mobile after tap for inputs) | P0 |
| `:active` / `:pressed` | `Pressed` | All (most impactful on mobile) | P0 |
| `:disabled` | `Disabled` | All | P0 |
| `:focus-visible` | `Focused` (subset) | Desktop keyboard nav | P2 — fold into Focused |

**Recommendation**: Collapse `:focus-visible` into `Focused`. MAUI doesn't distinguish mouse-focus from keyboard-focus. Accessibility-conscious apps can rely on MAUI's built-in focus visuals. Implementing four states (Disabled, Pressed, Focused, PointerOver) plus Normal gives full coverage.

### Platform relevance breakdown

| State | Android Phone | iOS Phone | iPad+Mouse | Mac Catalyst | Windows |
|---|---|---|---|---|---|
| **Pressed** | ✅ Critical | ✅ Critical | ✅ Yes | ✅ Yes | ✅ Yes |
| **Disabled** | ✅ Critical | ✅ Critical | ✅ Yes | ✅ Yes | ✅ Yes |
| **Focused** | ⚡ Inputs only | ⚡ Inputs only | ✅ Yes | ✅ Yes | ✅ Yes |
| **PointerOver** | ❌ No hover | ❌ No hover | ✅ iPadOS 13.4+ | ✅ Yes | ✅ Yes |

---

## 3. Architecture Recommendation: **Option (C) — Hybrid Approach**

### Options analyzed

#### (A) VisualStateManager in generated ResourceDictionaries (XAML-level)

**How it works**: The `ResourceDictionaryGenerator` emits `<VisualStateGroup>` blocks inside implicit styles. Each style has VisualState setters for `Pressed`, `PointerOver`, `Focused`, `Disabled`.

**Pros**:
- Pure MAUI, no platform-specific code needed
- Works with DynamicResource — theme switches "just work"
- VSM setters fire automatically from the MAUI state machine
- Transparent to devs — styles are declared, not imperative

**Cons**:
- **VSM setters set MAUI-level properties. Handlers overwrite them.** This is the critical flaw. The `BootstrapButtonHandler.ApplyAndroid()` creates a `GradientDrawable` and sets `button.Background = drawable`. This is a native-layer property. A VSM setter like `<Setter Property="BackgroundColor" Value="#0b5ed7"/>` writes to the MAUI property, but the handler's native drawable takes visual precedence. The result: VSM state changes are invisible.
- Only works for properties that have BindableProperty counterparts. `Entry.BorderColor`, `Entry.CornerRadius` are handler-only — VSM can't target them.
- Can't animate native-layer properties (Android GradientDrawable color, iOS CALayer borderColor).

**Verdict**: ❌ Not viable alone. The handler architecture fundamentally conflicts.

#### (B) Platform-native state handling in handlers

**How it works**: Handlers use platform APIs to register state changes: Android `StateListDrawable` / `ColorStateList`, iOS `UIControlState`, WinUI `VisualStateManager`.

**Pros**:
- Pixel-perfect: native controls handle state transitions with platform-correct animations
- Android: `StateListDrawable` with per-state `GradientDrawable` items gives pressed/focused/disabled for free with system-standard ripple/transitions
- iOS: `UIButton.SetBackgroundImage(_:for:)` or `UIButton.configurationUpdateHandler` for state colors
- WinUI: Native `VisualStateManager` in XAML is the canonical approach
- No conflict with existing handler code — it *is* the handler code

**Cons**:
- Every handler needs 3× more platform code (4 states × 3 platforms = 12 code paths per control)
- State colors must be passed into handlers — the handler needs the pressed/focus/hover colors, not just the rest color
- Theme switches require re-creating all native state drawables (already handled by `RefreshHandlers()`)
- More complex to debug — state is in native layer, not inspectable via MAUI devtools

**Verdict**: ✅ Most faithful implementation but highest code cost.

#### (C) Hybrid approach — **RECOMMENDED**

**How it works**:
1. **Handlers own state styling** for controls where they already apply native-level properties (Button, Entry, Editor, Picker, DatePicker, TimePicker, SearchBar, CheckBox, Switch, RadioButton, Slider, Stepper).
2. **VSM in ResourceDictionaries** for controls where handlers don't override visual properties or where MAUI-level properties suffice (Label, Border, ActivityIndicator, ProgressBar — these don't have complex native-layer styling).
3. **`BootstrapTheme` provides computed state colors** so handlers can build state drawables without re-implementing color math.

**Why this is the right answer**:
- The handler/VSM conflict only exists for controls where handlers set native properties. For Label (which the handler only uses for font/size) or Border (which uses MAUI-level `Stroke`/`StrokeThickness`), VSM setters work fine.
- The color computation can be centralized in `BootstrapTheme` — handlers and VSM both read from the same source of truth.
- Controls like Button need 4 native state colors; pushing that into `StateListDrawable` (Android) / `UIButton.configurationUpdateHandler` (iOS) / WinUI VSM (Windows) gives platform-native press animations and hover effects for free.
- This matches how real apps do it: handlers for custom native behavior, VSM for MAUI-level property swaps.

---

## 4. Color Computation Strategy

### How Bootstrap computes state colors

Bootstrap's Sass uses these functions:
```scss
// From bootstrap/scss/_functions.scss and _variables.scss
$btn-hover-bg-shade-amount:     15%   // darken by 15%
$btn-hover-border-shade-amount: 20%
$btn-active-bg-shade-amount:    20%   // darken by 20%
$btn-active-border-shade-amount: 25%
$btn-disabled-opacity:          0.65
$input-focus-border-color:      tint-color($primary, 50%)  // lighten primary by 50%
$input-focus-box-shadow:        0 0 0 0.25rem rgba($primary, 0.25)
```

For light-background variants (Warning, Light):
```scss
$btn-hover-bg-tint-amount:      15%   // lighten by 15% (instead of darken)
```

### Recommendation: Compute at runtime from base colors

**Why not at theme-generation time (CSS parser)?**

The CSS parser already extracts `:focus` border-color and `:disabled` colors for inputs. However:
- Bootstrap's compiled CSS doesn't always contain the intermediate hover/pressed colors as named CSS variables. They're computed at Sass compile time and baked into the `.btn-primary:hover { background-color: #0b5ed7 }` rules. Parsing them requires matching pseudo-class rules per variant — possible but fragile across Bootswatch themes that override these rules differently.
- Runtime computation keeps the theme data model clean. A theme with 8 variants × 4 states = 32 extra colors clutters `BootstrapTheme`.
- Runtime darken/lighten is a trivial operation — it's a single HSL manipulation.

**Why not at theme-generation time (code generator)?**

- Could emit the colors into the ResourceDictionary as `PrimaryHover`, `PrimaryPressed`, etc. This works for VSM-based states but handlers don't read ResourceDictionaries — they read `BootstrapTheme.Current`. Adding 32 colors to both systems doubles the sync burden.

**Recommendation**: Add color-computation helper methods to `BootstrapTheme`. These compute state colors on-demand from the base variant color. Cache them if profiling shows it matters (it won't — color math is <1μs).

### Proposed API on BootstrapTheme

```csharp
public class BootstrapTheme
{
    // ── State Color Computation ──

    /// <summary>
    /// Computes the hover background color for a variant (Bootstrap: darken 15% or lighten 15% for light variants).
    /// </summary>
    public Color GetHoverBackground(BootstrapVariant variant)
    {
        var baseColor = GetVariantColor(variant);
        return IsLightColor(baseColor) ? Tint(baseColor, 0.15f) : Shade(baseColor, 0.15f);
    }

    /// <summary>
    /// Computes the pressed/active background color (Bootstrap: darken 20% or lighten 20%).
    /// </summary>
    public Color GetPressedBackground(BootstrapVariant variant)
    {
        var baseColor = GetVariantColor(variant);
        return IsLightColor(baseColor) ? Tint(baseColor, 0.20f) : Shade(baseColor, 0.20f);
    }

    /// <summary>
    /// Computes the hover border color (Bootstrap: darken 20%).
    /// </summary>
    public Color GetHoverBorder(BootstrapVariant variant)
    {
        var baseColor = GetVariantColor(variant);
        return Shade(baseColor, 0.20f);
    }

    /// <summary>
    /// Computes the disabled variant colors (same base color at 0.65 opacity).
    /// </summary>
    public (Color Background, Color Foreground) GetDisabledColors(BootstrapVariant variant)
    {
        var (bg, fg) = Bootstrap.GetVariantColors(variant, this);
        return (bg.WithAlpha(0.65f), fg.WithAlpha(0.65f));
    }

    /// <summary>
    /// Returns the focus ring color for a variant (Bootstrap: primary at 25% opacity).
    /// </summary>
    public Color GetFocusRingColor(BootstrapVariant variant)
    {
        var baseColor = GetVariantColor(variant);
        return baseColor.WithAlpha(0.25f);
    }

    /// <summary>
    /// Returns the input focus border color (Bootstrap: tint primary 50%).
    /// </summary>
    public Color GetInputFocusBorderColor()
    {
        return Tint(Primary, 0.50f);
    }

    // ── Color Math Helpers ──

    /// <summary>
    /// Darkens a color by mixing with black. Amount 0.0-1.0.
    /// Equivalent to Bootstrap's shade-color($color, $amount).
    /// </summary>
    public static Color Shade(Color color, float amount)
    {
        return new Color(
            color.Red * (1 - amount),
            color.Green * (1 - amount),
            color.Blue * (1 - amount),
            color.Alpha);
    }

    /// <summary>
    /// Lightens a color by mixing with white. Amount 0.0-1.0.
    /// Equivalent to Bootstrap's tint-color($color, $amount).
    /// </summary>
    public static Color Tint(Color color, float amount)
    {
        return new Color(
            color.Red + (1 - color.Red) * amount,
            color.Green + (1 - color.Green) * amount,
            color.Blue + (1 - color.Blue) * amount,
            color.Alpha);
    }

    /// <summary>
    /// Returns true if a color is "light" (Bootstrap: contrast ratio check).
    /// Light colors get tinted (lightened) on hover; dark colors get shaded (darkened).
    /// </summary>
    public static bool IsLightColor(Color color)
    {
        // Bootstrap uses WCAG relative luminance: (0.2126*R + 0.7152*G + 0.0722*B)
        var luminance = 0.2126f * color.Red + 0.7152f * color.Green + 0.0722f * color.Blue;
        return luminance > 0.5f;
    }
}
```

### For outline buttons

Outline buttons have special hover behavior in Bootstrap: hover fills the button with the variant color and switches text to the on-variant color. This is not a darken/lighten — it's a complete state flip:

```
Normal:  bg=transparent, text=variant, border=variant
Hover:   bg=variant,     text=onVariant, border=variant
Pressed: bg=variant,     text=onVariant, border=darken(variant, 25%)
```

The handler needs to know both the outline-normal and solid-variant colors for state transitions. `Bootstrap.GetVariantColors()` already provides both — the handler just needs to use the outline colors for normal state and solid colors for hover/pressed.

---

## 5. Focus Ring Implementation

### The problem

Bootstrap's focus ring is `box-shadow: 0 0 0 0.25rem rgba(primary, 0.25)`. MAUI has no `box-shadow` on controls. MAUI's `Shadow` property creates a drop shadow (offset shadow) beneath the control — it cannot create the zero-offset, spread-only outline that Bootstrap uses.

### Platform-specific approaches

#### Android

Android has `android:state_focused` in `StateListDrawable` and can use a `LayerList` with a second `GradientDrawable` that has a larger corner radius and semi-transparent fill behind the main drawable. However, this is complex and fragile.

**Better approach**: Use a `RippleDrawable` wrapping the `GradientDrawable`. Android's ripple already provides a focus/pressed visual. For the focus ring specifically, set the `outline` or use `View.setOutlineProvider()` + `ViewOutlineProvider` to draw a ring. But the simplest and most Bootstrap-faithful approach:

```csharp
// Android focus ring via padding + background layering
var layers = new LayerDrawable(new Drawable[] { focusRingDrawable, mainDrawable });
layers.SetLayerInset(1, ringWidth, ringWidth, ringWidth, ringWidth);
```

Where `focusRingDrawable` is a `GradientDrawable` with `SetColor(focusRingColor)` and same corner radius, and `mainDrawable` is the existing button drawable. This creates a ring effect by placing a colored rectangle behind the main drawable with insets.

**Simpler alternative**: Change the border to the focus color and increase border width by the ring size (4dp). This approximates the ring without layered drawables.

#### iOS / Mac Catalyst

iOS has no built-in focus ring for `UIButton`. Mac Catalyst respects `UIFocusSystem` but it's limited to tvOS-style focus.

**Approach**: Use `CALayer` to add a second border-like effect:
```csharp
// Add focus ring as shadow on iOS
button.Layer.ShadowColor = focusRingColor.ToCGColor();
button.Layer.ShadowOffset = new CGSize(0, 0);
button.Layer.ShadowRadius = 4;  // 0.25rem = 4px
button.Layer.ShadowOpacity = 1;
button.Layer.MasksToBounds = false;  // Must disable to show shadow
```

This is actually a perfect match for Bootstrap's `box-shadow: 0 0 0 0.25rem rgba(color, 0.25)` because iOS `CALayer.shadowRadius` with zero offset creates the exact same visual as CSS box-shadow with zero offset and a spread. The only difference is CSS spread vs shadow blur — set `ShadowRadius` to the spread value (4px) for a close match.

**Tradeoff**: The button handler currently sets `MasksToBounds = true` for pill buttons. The focus ring requires `MasksToBounds = false`. Solution: remove `MasksToBounds = true` — it was only needed to clip content to rounded corners, but `UIButton` already handles corner clipping via `layer.cornerRadius` without needing masking.

#### Windows (WinUI)

WinUI has built-in `FocusVisualPrimaryBrush` and `FocusVisualSecondaryBrush` on `Control`:

```csharp
button.FocusVisualPrimaryBrush = new SolidColorBrush(focusRingColor.ToWindowsColor());
button.FocusVisualPrimaryThickness = new Thickness(4);
button.FocusVisualSecondaryBrush = new SolidColorBrush(Colors.Transparent.ToWindowsColor());
button.UseSystemFocusVisuals = true;
```

This gives a native focus ring that matches Bootstrap's intent perfectly. WinUI's focus visual is keyboard-triggered by default, matching `:focus-visible`.

### Recommendation

| Platform | Focus Ring Mechanism | Fidelity |
|---|---|---|
| Android | Border color change + increased border width on focused state, or `LayerDrawable` ring | Good |
| iOS / Mac Catalyst | `CALayer.shadow*` with zero offset, `MasksToBounds = false` | Excellent |
| Windows | `FocusVisualPrimaryBrush` + `FocusVisualPrimaryThickness` | Excellent |

**For inputs (Entry, Editor, SearchBar, Picker, DatePicker, TimePicker)**: Focus ring is more critical here because users type into these. Use the same mechanism as buttons but the focus is triggered by `IsFocused` state. The handler should subscribe to `IsFocused` changes or use the mapper for the `Focus` property.

### Implementation pattern for Entry handlers

```csharp
// Register both BootstrapStyle and Focus-state updates
EntryHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
EntryHandler.Mapper.AppendToMapping("IsFocused", OnFocusChanged);

private static void OnFocusChanged(IEntryHandler handler, IEntry entry)
{
    if (entry is not Entry view) return;
    var theme = BootstrapTheme.Current;
    var isFocused = view.IsFocused;

    #if ANDROID
    var drawable = handler.PlatformView?.Background as GradientDrawable;
    if (drawable != null)
    {
        var borderColor = isFocused ? theme.GetInputFocusBorderColor() : theme.Outline;
        drawable.SetStroke((int)(theme.BorderWidth * density), borderColor.ToPlatform());
    }
    #elif IOS || MACCATALYST
    var textField = handler.PlatformView;
    if (textField != null)
    {
        textField.Layer.BorderColor = isFocused
            ? theme.GetInputFocusBorderColor().ToCGColor()
            : theme.Outline.ToCGColor();
        textField.Layer.ShadowColor = isFocused
            ? theme.GetFocusRingColor(BootstrapVariant.Primary).ToCGColor()
            : UIColor.Clear.CGColor;
        textField.Layer.ShadowRadius = isFocused ? 4 : 0;
        textField.Layer.ShadowOpacity = isFocused ? 1f : 0f;
        textField.Layer.ShadowOffset = new CGSize(0, 0);
    }
    #endif
}
```

---

## 6. Disabled State

### Current behavior

When `IsEnabled = false` on a Bootstrap-styled control, nothing visual changes. The control is functionally disabled (can't interact) but looks identical to its enabled state.

### What Bootstrap does

```css
.btn:disabled, .btn.disabled {
    opacity: 0.65;
    pointer-events: none;
}
```

The entire button becomes 65% opaque. Text, background, border — everything dims uniformly.

### Recommendation: Use MAUI's built-in Opacity + handler-level alpha

**Approach 1 (Simplest — RECOMMENDED for v1)**: Set `Opacity` on the MAUI control when disabled.

Handlers can check `IsEnabled` and apply opacity:

```csharp
private static void ApplyBootstrapStyle(IButtonHandler handler, IButton button)
{
    // ... existing code ...

    // Disabled state
    if (button is VisualElement ve)
    {
        ve.Opacity = ve.IsEnabled ? 1.0 : 0.65;
    }
}
```

This is the simplest approach and matches Bootstrap exactly. However, it has a subtle issue: if the developer has already set a custom `Opacity`, this overwrites it. To avoid this, only set opacity when the control is disabled and restore the original value when re-enabled. Store the original opacity in an attached property or `ConditionalWeakTable`.

**Better approach**: Handle it through the mapper for `IsEnabled`:

```csharp
public static void Register()
{
    ButtonHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    ButtonHandler.Mapper.AppendToMapping("IsEnabled", OnIsEnabledChanged);
}

private static readonly ConditionalWeakTable<IButton, object> OriginalOpacity = new();

private static void OnIsEnabledChanged(IButtonHandler handler, IButton button)
{
    if (button is not VisualElement ve) return;
    var variant = (button as Button) != null ? Bootstrap.GetVariant((Button)(object)button) : BootstrapVariant.Default;
    if (variant == BootstrapVariant.Default) return; // Only apply to Bootstrap-styled buttons

    if (!ve.IsEnabled)
    {
        OriginalOpacity.AddOrUpdate(button, ve.Opacity);
        ve.Opacity = 0.65;
    }
    else if (OriginalOpacity.TryGetValue(button, out var orig))
    {
        ve.Opacity = (double)orig;
        OriginalOpacity.Remove(button);
    }
}
```

**For inputs**: Same pattern. Disabled inputs in Bootstrap also get `opacity: 0.65` and a slightly different background color. The handler can check `IsEnabled` and adjust both opacity and background.

### VSM-based disabled (for non-handler controls)

For Label, Border, and other controls where handlers don't override visual properties, emit VSM disabled states in the ResourceDictionary:

```xml
<Style TargetType="Button">
    <!-- existing setters -->
    <Setter Property="VisualStateManager.VisualStateGroups">
        <VisualStateGroupList>
            <VisualStateGroup x:Name="CommonStates">
                <VisualState x:Name="Normal"/>
                <VisualState x:Name="Disabled">
                    <VisualState.Setters>
                        <Setter Property="Opacity" Value="0.65"/>
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateGroupList>
    </Setter>
</Style>
```

**Important**: For Button specifically, the handler approach is better than VSM because the handler already runs for every Bootstrap-styled button. VSM opacity and handler opacity can coexist if the handler defers to VSM for opacity — but since `Opacity` is a MAUI-level property (not a native override), VSM setters for `Opacity` will actually work even when handlers set native backgrounds. **This means we can use VSM for Disabled/Opacity on all controls, including Button.**

### Final recommendation for Disabled

Use VSM `Opacity` setters in generated styles for **all controls**. This is the cleanest approach:
- It matches Bootstrap (opacity: 0.65)
- `Opacity` is always a MAUI-level property — no handler conflict
- The ResourceDictionary generator already emits implicit styles — add `VisualStateGroup` to them
- No handler changes needed for disabled state

---

## 7. Per-Control Implementation Plan

### Tier 1: Button (highest impact)

Buttons are the primary interactive control. Users expect visual feedback on press.

**States to implement**:

| State | Background | Text | Border |
|---|---|---|---|
| Normal | variant color | onVariant | variant or transparent |
| PointerOver | shade/tint 15% | onVariant | shade 20% |
| Pressed | shade/tint 20% | onVariant | shade 25% |
| Focused | variant + focus ring | onVariant | variant |
| Disabled | variant (opacity 0.65) | onVariant (opacity 0.65) | variant (opacity 0.65) |

**Handler changes (Android)**:

Replace the single `GradientDrawable` with a `StateListDrawable`:

```csharp
private static void ApplyAndroid(IButtonHandler handler, BootstrapVariant variant, BootstrapTheme theme, ...)
{
    var button = handler.PlatformView;
    var density = button.Context?.Resources?.DisplayMetrics?.Density ?? 1;

    var (bgNormal, fgNormal) = Bootstrap.GetVariantColors(variant, theme);
    var bgHover = theme.GetHoverBackground(variant);
    var bgPressed = theme.GetPressedBackground(variant);

    var normalDrawable = CreateRoundedDrawable(bgNormal, cornerRadius, borderColor, borderWidth, density);
    var pressedDrawable = CreateRoundedDrawable(bgPressed, cornerRadius, pressedBorderColor, borderWidth, density);

    // Android: RippleDrawable gives animated press feedback
    var rippleColor = Android.Content.Res.ColorStateList.ValueOf(
        theme.GetFocusRingColor(variant).ToPlatform());
    var ripple = new RippleDrawable(rippleColor, normalDrawable, null);

    button.Background = ripple;
}
```

Using `RippleDrawable` gives free material-design press animation that's superior to a flat `StateListDrawable`. The ripple color = focus ring color (primary at 25% alpha).

**Handler changes (iOS)**:

```csharp
private static void ApplyiOS(IButtonHandler handler, BootstrapVariant variant, BootstrapTheme theme, ...)
{
    var button = handler.PlatformView;

    // iOS 15+ configuration-based button
    if (OperatingSystem.IsIOSVersionAtLeast(15))
    {
        button.ConfigurationUpdateHandler = (btn) =>
        {
            var config = UIButtonConfiguration.FilledButtonConfiguration;
            var state = btn.State;

            if (state.HasFlag(UIControlState.Disabled))
            {
                config.BaseBackgroundColor = bgNormal.ToPlatform().ColorWithAlpha(0.65f);
                config.BaseForegroundColor = fgNormal.ToPlatform().ColorWithAlpha(0.65f);
            }
            else if (state.HasFlag(UIControlState.Highlighted)) // pressed
            {
                config.BaseBackgroundColor = bgPressed.ToPlatform();
                config.BaseForegroundColor = fgNormal.ToPlatform();
            }
            else
            {
                config.BaseBackgroundColor = bgNormal.ToPlatform();
                config.BaseForegroundColor = fgNormal.ToPlatform();
            }

            config.CornerStyle = UIButtonConfiguration.CornerStyle.Fixed;
            config.Background.CornerRadius = cornerRadius;
            btn.Configuration = config;
        };
    }
    else
    {
        // Fallback: manual state handling via SetTitleColor per state
        button.SetTitleColor(fgNormal.ToPlatform(), UIControlState.Normal);
        button.SetTitleColor(fgNormal.ToPlatform().ColorWithAlpha(0.65f), UIControlState.Disabled);
        // Background color change on press via event subscription or CALayer animation
    }
}
```

However, `UIButtonConfiguration` is a heavy rewrite of the iOS handler. A lighter approach for v1:

```csharp
// Simpler iOS approach: use SetBackgroundImage for states
button.SetTitleColor(fgNormal.ToPlatform(), UIControlState.Normal);
button.SetTitleColor(fgNormal.ToPlatform(), UIControlState.Highlighted);
button.SetTitleColor(fgNormal.ToPlatform().ColorWithAlpha(0.65f), UIControlState.Disabled);

// For background, iOS UIButton doesn't support per-state BackgroundColor directly.
// Use UIGraphicsImageRenderer to create 1x1 colored images per state:
button.SetBackgroundImage(ColorToImage(bgNormal), UIControlState.Normal);
button.SetBackgroundImage(ColorToImage(bgPressed), UIControlState.Highlighted);
button.SetBackgroundImage(ColorToImage(bgNormal.WithAlpha(0.65f)), UIControlState.Disabled);

button.Layer.CornerRadius = cornerRadius;
button.ClipsToBounds = true;
```

Where `ColorToImage` creates a 1×1 pixel `UIImage` from a color. This is a well-known iOS pattern for per-state button colors without `UIButtonConfiguration`.

**Handler changes (Windows)**:

WinUI's `Button` has its own `VisualStateManager` in the default `ControlTemplate`. We can override the visual states:

```csharp
// Set lightweight styling resources on the button
button.Resources["ButtonBackgroundPointerOver"] = new SolidColorBrush(bgHover.ToWindowsColor());
button.Resources["ButtonBackgroundPressed"] = new SolidColorBrush(bgPressed.ToWindowsColor());
button.Resources["ButtonBackgroundDisabled"] = new SolidColorBrush(bgNormal.ToWindowsColor());
button.Resources["ButtonForegroundPointerOver"] = new SolidColorBrush(fgNormal.ToWindowsColor());
button.Resources["ButtonForegroundPressed"] = new SolidColorBrush(fgNormal.ToWindowsColor());
button.Resources["ButtonForegroundDisabled"] = new SolidColorBrush(fgNormal.ToWindowsColor());
button.Resources["ButtonBorderBrushPointerOver"] = new SolidColorBrush(hoverBorderColor.ToWindowsColor());
button.Resources["ButtonBorderBrushPressed"] = new SolidColorBrush(pressedBorderColor.ToWindowsColor());
// Opacity for disabled
button.Resources["ButtonBackgroundDisabled"] = new SolidColorBrush(bgNormal.ToWindowsColor()) { Opacity = 0.65 };
```

WinUI's `Button` control template references these themed resources. By setting them on the button's local `Resources`, they override the theme defaults. **This is the cleanest WinUI approach** — no custom templates, no VSM rewriting, just resource overrides.

### Tier 2: Input Controls (Entry, Editor, SearchBar, Picker, DatePicker, TimePicker)

**States to implement**:

| State | Border | Background | Focus Ring |
|---|---|---|---|
| Normal | theme.Outline | inputBg | none |
| Focused | tint(primary, 50%) | inputBg | rgba(primary, 0.25) |
| Disabled | theme.Outline | darken(inputBg, 5%) | none, opacity 0.65 |

**Focus is the most important state for inputs.** Users need to know which field they're editing.

The existing handlers already set border color via native layer. Add an `IsFocused` mapper callback to swap border color and add/remove the focus ring shadow.

### Tier 3: CheckBox, Switch, RadioButton, Slider

These controls have limited Bootstrap-defined states. The handlers already use `ColorStateList` (Android) for checked/unchecked. Extend to include disabled state opacity. Focus ring is less critical for these toggle controls.

### Tier 4: Label, Border, ProgressBar, ActivityIndicator

These display-only controls primarily need disabled state (opacity 0.65). Handle entirely via VSM in generated ResourceDictionaries.

---

## 8. Impact on Existing API

### Changes to `BootstrapTheme`

```diff
  public class BootstrapTheme
  {
      // ... existing properties ...

+     // ── State Color Constants ──
+     public float HoverShadeAmount { get; set; } = 0.15f;
+     public float PressedShadeAmount { get; set; } = 0.20f;
+     public float HoverBorderShadeAmount { get; set; } = 0.20f;
+     public float PressedBorderShadeAmount { get; set; } = 0.25f;
+     public float DisabledOpacity { get; set; } = 0.65f;
+     public float FocusRingOpacity { get; set; } = 0.25f;
+     public float FocusRingWidth { get; set; } = 4f;  // 0.25rem
+
+     // ── State Color Methods ──
+     public Color GetHoverBackground(BootstrapVariant variant) { ... }
+     public Color GetPressedBackground(BootstrapVariant variant) { ... }
+     public Color GetHoverBorder(BootstrapVariant variant) { ... }
+     public Color GetPressedBorder(BootstrapVariant variant) { ... }
+     public Color GetFocusRingColor(BootstrapVariant variant) { ... }
+     public Color GetInputFocusBorderColor() { ... }
+
+     // ── Color Math ──
+     public static Color Shade(Color color, float amount) { ... }
+     public static Color Tint(Color color, float amount) { ... }
+     public static bool IsLightColor(Color color) { ... }
  }
```

### Changes to `BootstrapAttachedProperties`

No new attached properties needed. The states are intrinsic to the controls (IsEnabled, IsFocused, IsPressed, IsPointerOver) — they don't need developer configuration.

### Changes to Handlers

Every handler gains:
1. An `IsEnabled` mapper registration for disabled state styling
2. An `IsFocused` mapper registration for focus ring (input controls)
3. The main `ApplyBootstrapStyle` method extended with state-aware native drawables/layers

Example registration:

```csharp
public static void Register()
{
    ButtonHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    ButtonHandler.Mapper.AppendToMapping("IsEnabled", OnIsEnabledChanged);
}
```

For focus, handlers need to watch `IsFocused`:

```csharp
public static void Register()
{
    EntryHandler.Mapper.AppendToMapping("BootstrapStyle", ApplyBootstrapStyle);
    EntryHandler.Mapper.AppendToMapping("IsFocused", OnFocusChanged);
    EntryHandler.Mapper.AppendToMapping("IsEnabled", OnIsEnabledChanged);
}
```

### Changes to ResourceDictionaryGenerator

Add `VisualStateGroup` to all implicit styles with `Disabled` state for opacity:

```xml
<Style TargetType="Button">
    <!-- existing setters -->
    <Setter Property="VisualStateManager.VisualStateGroups">
        <VisualStateGroupList>
            <VisualStateGroup x:Name="CommonStates">
                <VisualState x:Name="Normal"/>
                <VisualState x:Name="Disabled">
                    <VisualState.Setters>
                        <Setter Property="Opacity" Value="0.65"/>
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateGroupList>
    </Setter>
</Style>
```

The C# generator (`GenerateCSharpResourceDictionary`) needs the same — emitting `VisualStateManager.SetVisualStateGroups()` calls.

### Changes to CSS Parser

The parser already extracts `FocusBorderColor`, `FocusShadow`, `DisabledBackground`, and `DisabledColor` for inputs. Extend it to also extract:
- Button hover/active colors from `.btn-primary:hover { background-color: ... }` rules
- Button hover/active border colors
- Store them on `ButtonRule` for per-theme override support (Bootswatch themes may compute different hover amounts)

```diff
  public class ButtonRule
  {
      // ... existing properties ...
+     public string? HoverBackground { get; set; }
+     public string? HoverBorderColor { get; set; }
+     public string? HoverColor { get; set; }
+     public string? ActiveBackground { get; set; }
+     public string? ActiveBorderColor { get; set; }
+     public string? ActiveColor { get; set; }
+     public string? DisabledBackground { get; set; }
+     public string? DisabledBorderColor { get; set; }
+     public string? DisabledColor { get; set; }
+     public string? FocusShadow { get; set; }
  }
```

If the CSS-parsed values exist, use them. If not, fall back to the runtime `Shade`/`Tint` computation. This gives exact values for Bootswatch themes that diverge from the standard darken formula.

### Changes to `SyncFromResources`

Add sync entries for new theme properties if they're emitted into ResourceDictionaries:

```diff
  if (resources.TryGetValue("DisabledOpacity", out var dop) && dop is double dopd)
      theme.DisabledOpacity = (float)dopd;
  if (resources.TryGetValue("FocusRingWidth", out var frw) && frw is double frwd)
      theme.FocusRingWidth = (float)frwd;
```

---

## 9. Platform-Specific Considerations

### Android

- **Ripple feedback**: Use `RippleDrawable` wrapping the `GradientDrawable`. This gives material-style animated press feedback. The ripple color should be the focus ring color (variant at 25% alpha). This is better than `StateListDrawable` because it provides animated transitions.
- **No hover events on touch**: Android phones never trigger hover. Android tablets with mouse do. `RippleDrawable` handles this automatically.
- **Disabled state**: Android natively respects `setEnabled(false)` but doesn't change visual appearance. Our handler must apply opacity.
- **Focus on Android**: Only matters for D-pad / keyboard navigation (accessibility, Android TV). `StateListDrawable` with `state_focused` handles this. Low priority for mobile.

### iOS

- **No native hover on iPhone**: `UIHoverGestureRecognizer` exists on iOS 13+ but only triggers with indirect pointer (iPad + mouse/trackpad, or Apple Pencil hover on M-series iPads).
- **Press feedback**: iOS buttons dim their title color to 0.3 alpha on highlight by default. We override this by setting per-state title colors via `SetTitleColor(_:for:)` and per-state backgrounds via `SetBackgroundImage(_:for:)`.
- **Focus ring on iOS**: Only meaningful for full keyboard access (Settings → Accessibility → Keyboards → Full Keyboard Access). `UIFocusSystem` handles this at the system level. We should not fight it — let the system draw its own focus ring for accessibility. Our custom focus ring is for visual fidelity with Bootstrap, not for accessibility compliance.
- **MasksToBounds conflict**: The current handler sets `MasksToBounds = true` for pill buttons. The focus ring shadow requires `MasksToBounds = false`. **Resolution**: Use `clipsToBounds` (which clips subview content) instead of `masksToBounds` (which clips layer content including shadows). Actually they're the same property on iOS. **Better resolution**: Set a shadow path explicitly — `CALayer` can show shadows outside bounds if `shadowPath` is set even when `masksToBounds = true`. But actually `masksToBounds = true` prevents shadow rendering. **Final resolution**: Don't clip — use `layer.cornerRadius` which rounds corners without needing `masksToBounds`. Remove the `MasksToBounds = true` line. If content overflows (e.g., long text), it's already handled by the button's intrinsic layout.

### Mac Catalyst

- **Hover works**: Pointer-over triggers just like desktop macOS. iOS handlers with `UIHoverGestureRecognizer` work on Mac Catalyst automatically.
- **Focus ring**: macOS draws its own focus ring around focused controls. Let the system handle this for native feel. Our Bootstrap focus ring is additive — it changes border color, which augments (not replaces) the system ring.
- **Cursor changes**: Consider setting `UIPointerStyle` on buttons for hover cursor change. Not in v1 scope but a nice-to-have.

### Windows

- **Full state support**: WinUI's control templates have built-in PointerOver, Pressed, Disabled, Focused visual states. The recommended approach (setting themed resources on the control's `Resources` dictionary) integrates perfectly with the existing template.
- **Focus visuals**: WinUI has `UseSystemFocusVisuals` which draws a dotted rectangle. Setting `FocusVisualPrimaryBrush` and `FocusVisualPrimaryThickness` gives a Bootstrap-style ring. Set `FocusVisualMargin = new Thickness(-3)` to position the ring outside the button bounds.
- **High contrast**: WinUI automatically switches to high-contrast colors. Our themed resources should use `ThemeResource` awareness. For v1, our overrides may break high contrast — document as known limitation.

### iPad with External Mouse/Trackpad

- iPadOS 13.4+ supports pointer hover via `UIPointerInteraction`. MAUI maps this to `PointerOver` visual state.
- The iOS handler can subscribe to `UIHoverGestureRecognizer` to detect hover:

```csharp
var hoverGesture = new UIHoverGestureRecognizer((recognizer) =>
{
    switch (recognizer.State)
    {
        case UIGestureRecognizerState.Began:
            // Apply hover colors
            break;
        case UIGestureRecognizerState.Ended:
        case UIGestureRecognizerState.Cancelled:
            // Restore normal colors
            break;
    }
});
button.AddGestureRecognizer(hoverGesture);
```

However, for v1, PointerOver is P1 (not P0). The visual difference is subtle and only affects desktop/tablet-with-mouse users. Implement Pressed and Disabled first.

---

## 10. Implementation Phases

### Phase 1: Disabled State (P0, Low Risk)

**Scope**: All controls
**Approach**: VSM in generated ResourceDictionaries + handler `IsEnabled` mapper for controls with native opacity issues
**Changes**:
- `ResourceDictionaryGenerator`: Add `VisualStateGroup` with `Disabled` state (`Opacity = 0.65`) to all implicit styles
- `BootstrapTheme`: Add `DisabledOpacity` property (default 0.65)
- Handlers: Add `IsEnabled` mapper to apply/remove opacity for handler-styled controls
**Risk**: Low — `Opacity` is a MAUI-level property, no platform conflicts
**Estimated effort**: 1-2 days

### Phase 2: Pressed State (P0, Medium Risk)

**Scope**: Button (primary), then input controls
**Approach**: Platform-native in handlers
**Changes**:
- `BootstrapTheme`: Add `Shade()`, `Tint()`, `IsLightColor()`, `GetPressedBackground()`, `GetPressedBorder()`
- `BootstrapButtonHandler`: Android — `RippleDrawable`; iOS — per-state background images; Windows — themed resource overrides
- CSS parser: Extract `:active`/pressed colors from CSS
**Risk**: Medium — native drawable code is platform-specific and needs testing
**Estimated effort**: 3-5 days

### Phase 3: Focus Ring (P0, Medium Risk)

**Scope**: Input controls (Entry, Editor, SearchBar, Picker, DatePicker, TimePicker), then Button
**Approach**: Platform-native in handlers via `IsFocused` mapper
**Changes**:
- `BootstrapTheme`: Add `GetInputFocusBorderColor()`, `GetFocusRingColor()`, `FocusRingWidth`
- Input handlers: Add `IsFocused` mapper — change border color, add/remove shadow ring
- `BootstrapButtonHandler`: Add focus ring on focus (less critical for buttons)
**Risk**: Medium — iOS `MasksToBounds` interaction needs care
**Estimated effort**: 3-4 days

### Phase 4: PointerOver / Hover (P1, Low Risk)

**Scope**: Button, input controls
**Approach**: Platform-native in handlers (Windows resource overrides, iOS UIHoverGestureRecognizer, Android no-op on phones)
**Changes**:
- `BootstrapTheme`: Add `GetHoverBackground()`, `GetHoverBorder()`
- Handlers: Add hover state colors for desktop platforms
**Risk**: Low — hover is additive; no regression risk on mobile (hover is never triggered)
**Estimated effort**: 2-3 days

### Total estimated effort: 9-14 days

---

## 11. Open Questions

1. **~~Should outline button hover fill the button?~~** ✅ **DECIDED: Match Bootstrap.** Outline buttons fill completely on hover (bg goes from transparent to variant color, text flips to onVariant).

2. **~~Should focus ring be Bootstrap-style or platform-native?~~** ✅ **DECIDED: Deferred.** Focus ring implementation is deferred to a later phase. Skip Phase 3 for now.

3. **~~Transitions/animations~~**: ✅ **DECIDED: Let platforms handle transitions.** No custom animation code. Android `RippleDrawable` animates automatically, Windows has built-in transitions in control templates, iOS uses native state handling. Revisit only if platform defaults feel wrong.

4. **Opacity vs color change for disabled**: Bootstrap uses `opacity: 0.65` which dims everything uniformly. An alternative is to compute disabled-specific colors (lighter/more-muted versions of the variant). Opacity is simpler and matches Bootstrap exactly.

5. **Dark mode interaction**: State colors should respect dark mode. The `Shade`/`Tint` functions work on the current variant color, which is already dark-mode-aware (via `BootstrapTheme.Current` being synced from the active theme). No additional dark-mode logic needed.

---

## 12. Summary

| Decision | Recommendation |
|---|---|
| Architecture | Hybrid: handlers for native-styled controls, VSM for display controls |
| Color computation | Runtime via `Shade()`/`Tint()` methods on `BootstrapTheme`, CSS-parsed overrides when available |
| Focus ring | iOS: `CALayer.shadow*`; Android: `RippleDrawable` ripple + border; Windows: `FocusVisualPrimaryBrush` |
| Disabled state | VSM `Opacity=0.65` in generated styles (works for all controls) |
| Pressed state | Platform-native in handlers (RippleDrawable, SetBackgroundImage, themed resources) |
| Hover state | Platform-native in handlers, no-op on touch-only devices |
| Implementation order | Disabled → Pressed → Focus → Hover |
| Estimated effort | 9-14 days for all four states across all controls and platforms |
