using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class VariantsPage : BasePage
{
    private static VisualNode RenderVariantSection(string title, BootstrapVariant variant)
    {
        return Border(
            VStack(spacing: 12,
                Label(title).H5(),
                Entry().Placeholder($"{title} Entry").BootstrapHeight(),
                FlexLayout(
                    Button($"{title} Button").Variant(variant).Margin(0, 0, 8, 8),
                    Button($"{title} Outline").Variant(variant).Outlined().Margin(0, 0, 8, 8)
                ).Wrap(FlexWrap.Wrap).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
            ).Padding(16)
        ).Background(variant).ShadowSm();
    }

    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 20,
                // Header
                VStack(spacing: 4,
                    Label("Color Variants").H1(),
                    Label("All Bootstrap color variants applied to controls for parity comparison.").Lead().Muted()
                ),

                RenderVariantSection("Primary", BootstrapVariant.Primary),
                RenderVariantSection("Secondary", BootstrapVariant.Secondary),
                RenderVariantSection("Success", BootstrapVariant.Success),
                RenderVariantSection("Danger", BootstrapVariant.Danger),
                RenderVariantSection("Warning", BootstrapVariant.Warning),
                RenderVariantSection("Info", BootstrapVariant.Info)
            ).Padding(20)
        );
}
