# MauiBootstrapTheme Deep Comparison Analysis

## Measurement-Based Comparison

This report provides exact measurements comparing Blazor (pure Bootstrap CSS) to native MAUI implementations.

---

## 1. Button Sizing & Shape

### Measurements

| Property | Blazor (Bootstrap) | MAUI (Native) | Difference |
|----------|-------------------|---------------|------------|
| **Default Height** | 38px | 31px | ❌ -7px shorter |
| **Default Width (Primary)** | 81px | 76px | ⚠️ -5px narrower |
| **Font Size** | 16px | 14px | ❌ 2px smaller |
| **Padding** | 6px 12px | ~6px 12px | ✅ Match |
| **Border Radius** | 6px | 4px | ⚠️ 2px smaller |
| **Line Height** | 24px (1.5) | N/A | N/A |

### Pill Buttons

| Property | Blazor (Bootstrap) | MAUI (Native) | Difference |
|----------|-------------------|---------------|------------|
| **Height** | 38px | 31px | ❌ -7px shorter |
| **Border Radius** | 800px (fully round) | 50px | ⚠️ Different approach |

### Issues to Investigate

1. **IB-001: Button height too short**
   - Buttons render 7px shorter than Bootstrap
   - Root cause: Font size difference + line height not applied
   - Fix: Increase `FontSizeBase` from 14 to 16, ensure minimum height of 38px

2. **IB-002: Font size mismatch**
   - MAUI buttons use 14px, Bootstrap uses 16px
   - Root cause: Theme uses smaller `FontSizeBase`
   - Fix: Update theme `FontSizeBase` to 16 and `FontSizeSm` to 14

3. **IB-003: Border radius too small**
   - MAUI uses 4px, Bootstrap uses 6px for default buttons
   - Root cause: `CornerRadius` theme value set to 4 instead of 6
   - Fix: Some themes have `CornerRadius = 4` but Bootstrap default is 6

4. **IB-004: Pill button text rendering bug**
   - Visual: Text appears to render twice with offset/shadow
   - Root cause: Unknown - needs investigation in handler
   - Priority: HIGH - visually broken

---

## 2. Entry/Input Sizing & Shape

### Measurements

| Property | Blazor (Bootstrap) | MAUI (Native) | Difference |
|----------|-------------------|---------------|------------|
| **Default Height** | 38px | 22px | ❌ -16px shorter |
| **Large Height** | 48px | N/A | ❌ Not implemented |
| **Small Height** | 31px | N/A | ❌ Not implemented |
| **Font Size** | 16px | 14px | ❌ 2px smaller |
| **Padding** | 6px 12px | Unknown | ⚠️ Needs verification |
| **Border Radius** | 6px | Unknown | ⚠️ Needs verification |
| **Border Color** | #222 (Darkly) | Custom | ⚠️ Differs |

### Issues to Investigate

5. **IB-005: Entry height too short**
   - MAUI Entry is 22px, Bootstrap is 38px
   - Root cause: No MinHeight applied to Entry controls
   - Fix: Apply `InputMinHeight` (38px) in Entry handler

6. **IB-006: Missing Large/Small Entry variants**
   - Bootstrap has `.form-control-lg` (48px) and `.form-control-sm` (31px)
   - Fix: Implement `Bootstrap.Size` attached property for Entry

7. **IB-007: Entry font size mismatch**
   - Same as button - needs FontSizeBase = 16

---

## 3. Typography Sizing

### Measurements

| Element | Blazor (Bootstrap) | MAUI (Theme) | Match? |
|---------|-------------------|--------------|--------|
| **H1** | 48px, weight 500, line-height 1.2 | 40px, Bold | ❌ Smaller |
| **H2** | 40px, weight 500 | 32px, Bold | ❌ Smaller |
| **H3** | 32px, weight 500 | 28px, Bold | ❌ Smaller |
| **H4** | 24px, weight 500 | 24px, Bold | ✅ Match (weight differs) |
| **H5** | 20px, weight 500 | 20px, Bold | ✅ Match (weight differs) |
| **H6** | 16px, weight 500 | 16px, Bold | ✅ Match |

### Issues to Investigate

8. **IB-008: Heading sizes don't match Bootstrap**
   - H1 should be 48px (2.5rem × 16), currently 40px
   - H2 should be 40px (2rem × 16), currently 32px  
   - H3 should be 32px (1.75rem × 16), currently 28px
   - Fix: Update `BootstrapTheme.cs` heading font sizes:
     ```csharp
     FontSizeH1 = 48.0,  // was 40
     FontSizeH2 = 40.0,  // was 32
     FontSizeH3 = 32.0,  // was 28
     ```

9. **IB-009: Heading font weight differs**
   - Bootstrap uses `font-weight: 500` (medium)
   - MAUI uses `FontAttributes.Bold` (700)
   - Fix: Need custom font or accept difference

---

## 4. Progress Bar

### Measurements

| Property | Blazor (Bootstrap) | MAUI (Native) | Difference |
|----------|-------------------|---------------|------------|
| **Height** | 16px | ~4px (platform) | ❌ Much shorter |
| **Track Background** | #444 (Darkly) | None | ❌ Missing |
| **Border Radius** | 6px | 0 | ❌ Missing |

### Issues to Investigate

10. **IB-010: Progress bar has no visible track**
    - Bootstrap shows gray background track
    - MAUI ProgressBar only shows colored progress
    - Fix: Add track background in ProgressBar handler or wrap in styled container

11. **IB-011: Progress bar too thin**
    - Bootstrap progress is 16px tall
    - MAUI ProgressBar is platform-native (4px on macOS)
    - Fix: Apply `ProgressHeight` theme value via handler

---

## 5. Dark Theme Issues

### Measurements

| Element | Blazor (Darkly) | MAUI (Darkly) | Match? |
|---------|-----------------|---------------|--------|
| **Page Background** | #222 | White | ❌ Wrong |
| **Card Background** | #303030 | #303030 | ✅ Match |
| **Button Colors** | Correct | Correct | ✅ Match |
| **Text Color** | #fff | Varies | ⚠️ |

### Issues to Investigate

12. **IB-012: Page background doesn't change with theme**
    - Blazor body background switches to #222 in Darkly
    - MAUI page background stays white
    - Root cause: Theme changes control colors, not page/content background
    - Fix: Need to set `BackgroundColor` on ContentPage/Shell/ScrollView

13. **IB-013: Frame/Card background stays white**
    - When switching themes at runtime, containers don't update
    - Fix: Need dynamic resource binding or theme change notification

14. **IB-014: Text colors may not update**
    - Labels not bound to theme colors don't update
    - Fix: Use attached property styling or dynamic resources

---

## 6. Spacing & Alignment

### Measurements

| Property | Blazor (Bootstrap) | MAUI Sample | Match? |
|----------|-------------------|-------------|--------|
| **Button gap** | 8px (gap-2) | Variable | ⚠️ Inconsistent |
| **Section margin** | 32px (mb-4) | Variable | ⚠️ Inconsistent |
| **Container padding** | 24px | 20px | ⚠️ Close |

### Issues to Investigate

15. **IB-015: Inconsistent spacing between buttons**
    - Bootstrap uses consistent 8px gap
    - MAUI samples have varied Spacing values
    - Fix: Use consistent `SpacerSm` (8px) for HorizontalStackLayout.Spacing

16. **IB-016: Section spacing inconsistent**
    - Bootstrap uses mb-4 (24px) between sections consistently
    - MAUI samples use varied margins
    - Fix: Standardize on theme.SpacerLg between sections

---

## 7. Shape/Appearance Issues

### Issues to Investigate

17. **IB-017: Outline buttons background differs**
    - Blazor: `background-color: rgba(0,0,0,0)` (transparent)
    - MAUI: May have slight background tint on some platforms
    - Fix: Ensure truly transparent background in outline variant handler

18. **IB-018: Activity indicators don't match Bootstrap spinners**
    - Bootstrap uses animated circular spinner (border-spinner)
    - MAUI uses platform-native ActivityIndicator (asterisk on Mac)
    - Fix: Low priority - would need custom drawing or Lottie animation

19. **IB-019: Checkboxes/Switches are platform-native**
    - Bootstrap has custom styled checkboxes and switches
    - MAUI uses platform-native controls
    - Fix: Accept difference or implement custom handlers

---

## Issues Summary

### Critical (P0) - Visual Bugs
| ID | Issue | Impact |
|----|-------|--------|
| IB-004 | Pill button text rendering bug | Visually broken |

### High (P1) - Size/Shape Mismatches
| ID | Issue | Impact |
|----|-------|--------|
| IB-001 | Button height 31px vs 38px | Controls look small |
| IB-005 | Entry height 22px vs 38px | Inputs look cramped |
| IB-010 | Progress bar no track | Missing visual element |
| IB-012 | Page background doesn't change | Dark themes incomplete |

### Medium (P2) - Typography/Spacing
| ID | Issue | Impact |
|----|-------|--------|
| IB-002 | Font size 14px vs 16px | Text looks small |
| IB-003 | Border radius 4px vs 6px | Slight shape diff |
| IB-008 | Headings smaller than Bootstrap | H1-H3 undersized |
| IB-011 | Progress bar too thin | Different appearance |
| IB-015 | Inconsistent button spacing | Layout variance |

### Low (P3) - Enhancement Opportunities
| ID | Issue | Impact |
|----|-------|--------|
| IB-006 | Missing Large/Small Entry variants | Feature gap |
| IB-009 | Heading font weight 700 vs 500 | Slightly bolder |
| IB-013 | Runtime theme card updates | Theme switching |
| IB-016 | Section spacing inconsistent | Layout variance |
| IB-017 | Outline button background | Subtle difference |
| IB-018 | Activity indicator style | Platform native |
| IB-019 | Checkbox/Switch style | Platform native |

---

## Recommended Fixes

### Immediate (Critical Path)

1. **Fix button/input minimum heights**
   ```csharp
   // In BootstrapTheme.cs - ensure heights match Bootstrap
   public double InputMinHeight { get; set; } = 38.0;  // was correct
   // Apply this in EntryHandler and ButtonHandler
   ```

2. **Fix font sizes**
   ```csharp
   public double FontSizeBase { get; set; } = 16.0;  // verify this is used
   ```

3. **Fix heading sizes**
   ```csharp
   public double FontSizeH1 { get; set; } = 48.0;  // 2.5rem × 16
   public double FontSizeH2 { get; set; } = 40.0;  // 2.5rem × 16
   public double FontSizeH3 { get; set; } = 32.0;  // 2rem × 16
   ```

4. **Investigate pill button rendering bug**
   - Debug ButtonHandler on Mac Catalyst
   - Check if ContentEdgeInsets is causing text offset

5. **Add progress bar track**
   - Modify ProgressBarHandler to add background track
   - Or use a wrapper container approach

### Short-Term

6. **Apply theme background to pages**
   - Add method to set page background from theme
   - Consider Shell background theming

7. **Standardize sample spacing**
   - Use consistent 8px button gaps
   - Use consistent 24px section margins

### Long-Term

8. **Size variants for inputs**
   - Add Bootstrap.Size support to Entry handler

9. **Custom spinner drawing**
   - Consider Bootstrap-style spinner as custom GraphicsView

---

## Screenshots Reference

All screenshots in `docs/screenshots/`:
- `blazor-controls-default.png` - Gold standard baseline
- `xaml-controls-default.png` - Current MAUI implementation
- `reactor-controls-default.png` - MauiReactor implementation
- `blazor-controls-darkly.png` - Dark theme reference
- `xaml-controls-darkly.png` - Dark theme MAUI issues visible
