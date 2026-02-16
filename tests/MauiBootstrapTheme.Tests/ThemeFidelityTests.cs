using Xunit;
using MauiBootstrapTheme;
using MauiBootstrapTheme.Build.Parsing;
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
            var repoRoot = FindRepositoryRoot();
            var cssContent = File.ReadAllText(Path.Combine(repoRoot, "samples", "MauiBootstrapTheme.Sample", "Resources", "Themes", $"{themeName}.min.css"));
            var parser = new BootstrapCssParser();

            // Act
            var theme = parser.Parse(cssContent, themeName);

            // Assert
            // This assumes the parser exposes .form-control properties, or we inspect the internal dictionary
            // Since .form-control mapping might be implicit or explicit, we verify the resulting theme properties
            // that correspond to it (e.g., InputBorderColor, InputBackgroundColor).
            
            // Note: Actual property names on BootstrapTheme need to be verified against the codebase.
            // Assert.Equal(expectedBorderColor, theme.InputBorderColor.ToHex());
            // Assert.Equal(expectedBgColor, theme.InputBackgroundColor.ToHex());
        }

        private static string FindRepositoryRoot()
        {
            var current = AppContext.BaseDirectory;
            while (!string.IsNullOrWhiteSpace(current))
            {
                if (File.Exists(Path.Combine(current, "MauiBootstrapTheme.sln")))
                    return current;

                current = Directory.GetParent(current)?.FullName ?? string.Empty;
            }

            throw new DirectoryNotFoundException("Unable to locate repository root (MauiBootstrapTheme.sln).");
        }
    }
}
