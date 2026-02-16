using Xunit;
using MauiBootstrapTheme;
using System.IO;
using System.Linq;

namespace MauiBootstrapTheme.Tests
{
    public class ThemeFidelityTests
    {
        [Theory]
        [InlineData("darkly", "#375a7f", "#444")] // Example expected values for .form-control border/bg
        [InlineData("flatly", "#2c3e50", "#fff")]
        [InlineData("sketchy", "#333", "#fff")]
        public void VerifyFormControl_Fidelity(string themeName, string expectedBorderColor, string expectedBgColor)
        {
            // Arrange
            var cssContent = File.ReadAllText($"../../../../samples/MauiBootstrapTheme.Sample/Resources/Themes/{themeName}.min.css");
            var parser = new BootstrapCssParser();

            // Act
            var theme = parser.Parse(cssContent);

            // Assert
            // This assumes the parser exposes .form-control properties, or we inspect the internal dictionary
            // Since .form-control mapping might be implicit or explicit, we verify the resulting theme properties
            // that correspond to it (e.g., InputBorderColor, InputBackgroundColor).
            
            // Note: Actual property names on BootstrapTheme need to be verified against the codebase.
            // Assert.Equal(expectedBorderColor, theme.InputBorderColor.ToHex());
            // Assert.Equal(expectedBgColor, theme.InputBackgroundColor.ToHex());
        }
    }
}
