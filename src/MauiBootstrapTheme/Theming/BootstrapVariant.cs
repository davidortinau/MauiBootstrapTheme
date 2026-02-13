namespace MauiBootstrapTheme.Theming;

/// <summary>
/// Bootstrap variant types for controls.
/// </summary>
public enum BootstrapVariant
{
    /// <summary>No specific variant, uses default styling.</summary>
    Default,
    
    /// <summary>Primary variant (typically blue).</summary>
    Primary,
    
    /// <summary>Secondary variant (typically gray).</summary>
    Secondary,
    
    /// <summary>Success variant (typically green).</summary>
    Success,
    
    /// <summary>Danger variant (typically red).</summary>
    Danger,
    
    /// <summary>Warning variant (typically yellow/amber).</summary>
    Warning,
    
    /// <summary>Info variant (typically cyan/light blue).</summary>
    Info,
    
    /// <summary>Light variant.</summary>
    Light,
    
    /// <summary>Dark variant.</summary>
    Dark,
    
    /// <summary>Outlined primary variant.</summary>
    OutlinePrimary,
    
    /// <summary>Outlined secondary variant.</summary>
    OutlineSecondary,
    
    /// <summary>Outlined success variant.</summary>
    OutlineSuccess,
    
    /// <summary>Outlined danger variant.</summary>
    OutlineDanger,
    
    /// <summary>Outlined warning variant.</summary>
    OutlineWarning,
    
    /// <summary>Outlined info variant.</summary>
    OutlineInfo,
    
    /// <summary>Outlined light variant.</summary>
    OutlineLight,
    
    /// <summary>Outlined dark variant.</summary>
    OutlineDark,
    
    /// <summary>Link style (text only, no background).</summary>
    Link
}

/// <summary>
/// Size variants for controls.
/// </summary>
public enum BootstrapSize
{
    /// <summary>Default size.</summary>
    Default,
    
    /// <summary>Small size.</summary>
    Small,
    
    /// <summary>Large size.</summary>
    Large
}
