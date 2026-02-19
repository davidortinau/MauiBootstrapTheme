# .NET MAUI VisualStateManager & Pointer Research Report

## Executive Summary

This report details the capabilities of .NET MAUI's VisualStateManager (VSM) and pointer input system for implementing Bootstrap-style visual states. The research confirms that while MAUI provides a robust VSM, advanced customizations (like cursor changes and complex state transitions) often require platform-specific handling or workarounds.

## 1. VisualStateManager (VSM) Capabilities

The `VisualStateManager` allows you to define visual states for controls in XAML or C#. The `CommonStates` group is the standard contract for most controls.

### Built-in States
*   **Normal**: The default state.
*   **Disabled**: When `IsEnabled="False"`.
*   **Focused**: When the control has input focus.
*   **Selected**: For selectable controls (e.g., CollectionView items).
*   **PointerOver**: When a pointer (mouse/trackpad) hovers over the control.

### Property Targeting
VSM `Setters` can target any **BindableProperty** of the control.
*   **Supported**: `BackgroundColor`, `TextColor`, `Opacity`, `Scale`, `TranslationX/Y`, `Rotation`, `Stroke`, `StrokeThickness`.
*   **Complex Types**: Properties like `Shadow` (see Section 4) can be targeted if the type is supported in XAML.

**Documentation**: [Visual States in .NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/visual-states)

## 2. PointerOver State & Platform Support

The `PointerOver` state is part of the `CommonStates` group.

*   **Availability**: It is supported on all `VisualElement` derivatives (Views, Pages).
*   **iPad Support**: **Yes.** It works on iPadOS 13.4+ when using a trackpad or mouse (indirect pointer). It does *not* trigger for touch input.
*   **Android**: Works with mouse input.
*   **Windows/macOS**: Native mouse hover support.

**Documentation**: [Common States](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/visual-states#common-states)

## 3. Cursor Support (Hand/Pointer)

**Current Status**: .NET MAUI (as of .NET 8/9) does **not** have a cross-platform API to change the cursor (e.g., to a `Hand` cursor on hover).

### Workarounds
You must implement this via **Platform Behaviors** or **Custom Handlers**.

**Windows (Handler/Mapper approach):**
```csharp
#if WINDOWS
Microsoft.UI.Xaml.Input.InputSystemCursor.Create(Microsoft.UI.Xaml.Input.InputSystemCursorShape.Hand);
// Apply to FrameworkElement.ProtectedCursor
#endif
```

**Mac Catalyst (Handler/Mapper approach):**
Uses `NSCursor` (AppKit) or `UIPointerInteraction` (UIKit) for button-like effects.

**Pointer Events**:
The `PointerGestureRecognizer` provides the necessary events to drive these changes:
*   `PointerEntered`
*   `PointerExited`
*   `PointerMoved`

## 4. Targeting the Shadow Property

**Verdict**: **Yes**, but with syntax caveats.
You can target the `Shadow` property in VSM because it is a BindableProperty. However, you must replace the entire `Shadow` object; you cannot animate individual properties (like `Shadow.Opacity`) easily without a custom animation or behavior.

**Example XAML**:
```xml
<VisualState x:Name="PointerOver">
    <VisualState.Setters>
        <Setter Property="Shadow">
            <Setter.Value>
                <!-- Replaces the entire Shadow object -->
                <Shadow Brush="Black" Offset="0,4" Radius="8" Opacity="0.15"/>
            </Setter.Value>
        </Setter>
    </VisualState.Setters>
</VisualState>
```

**Warning**: Replacing the Shadow object can cause a "flicker" as the old shadow is removed and the new one added.

## 5. BackgroundColor vs. Native Handlers

**Critical Finding**: **Native Handlers often override VSM.**

If a Custom Handler or Mapper sets the native background directly (e.g., `view.Background = new GradientDrawable(...)` on Android), the MAUI VSM `Setter` for `BackgroundColor` usually updates the MAUI `View.BackgroundColor` property, **but the native view may not reflect it** if the handler doesn't listen for the property change or if the native background drawable masks the color.

**Best Practice for Custom Backgrounds**:
*   Do **not** use `BackgroundColor` for complex states if you are using a custom drawable.
*   Instead, use a **StateListDrawable** (Android) or **UIButton.SetBackgroundImage** (iOS) in the handler to manage states natively. This ensures instant feedback and proper transitioning.

## 6. Button Pressed State & Custom Backgrounds

**Issue**: MAUI's default "Pressed" visual (often a ripple or lightness change) is part of the default background drawable.
**Result**: When you replace the native background (e.g., `view.SetBackground(myDrawable)`), you **lose the default ripple/pressed effect**.

**Solution**: You must re-implement the ripple/pressed state in your custom drawable.

**Android Example (RippleDrawable):**
```csharp
// In Android Handler
var contentDrawable = new GradientDrawable(); // Your custom background
contentDrawable.SetColor(myColor);
contentDrawable.SetCornerRadius(myRadius);

// Wrap in Ripple for pressed state
var rippleColor = ColorStateList.ValueOf(Android.Graphics.Color.White.WithAlpha(0.3f));
var rippleDrawable = new RippleDrawable(rippleColor, contentDrawable, null);

nativeView.Background = rippleDrawable;
```

## 7. PointerGestureRecognizer Capabilities

**Availability**: .NET 7+
**Events**: `PointerEntered`, `PointerExited`, `PointerMoved`.

**Usage on Border**:
Yes, you can add a `PointerGestureRecognizer` to a `Border` to detect hover.

```xml
<Border>
    <Border.GestureRecognizers>
        <PointerGestureRecognizer PointerEntered="OnPointerEntered" PointerExited="OnPointerExited"/>
    </Border.GestureRecognizers>
</Border>
```

**Platform Support**:
*   **Windows/macOS**: Full mouse support.
*   **iPad**: Supported with **Trackpad/Mouse** (indirect pointer). Does NOT trigger on touch.
*   **Android**: Supported with mouse.

**Note**: On iPad, `PointerEntered` corresponds to the pointer hovering over the view. It is robust for trackpad users.
