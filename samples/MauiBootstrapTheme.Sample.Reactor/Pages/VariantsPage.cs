using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class VariantsPage : BasePage
{
    private static string TextBgClass(BootstrapVariant variant) => variant switch
    {
        BootstrapVariant.Primary => "text-bg-primary",
        BootstrapVariant.Secondary => "text-bg-secondary",
        BootstrapVariant.Success => "text-bg-success",
        BootstrapVariant.Danger => "text-bg-danger",
        BootstrapVariant.Warning => "text-bg-warning",
        BootstrapVariant.Info => "text-bg-info",
        _ => "text-bg-primary"
    };

    private static string OnClass(BootstrapVariant variant) => variant switch
    {
        BootstrapVariant.Primary => "on-primary",
        BootstrapVariant.Secondary => "on-secondary",
        BootstrapVariant.Success => "on-success",
        BootstrapVariant.Danger => "on-danger",
        BootstrapVariant.Warning => "on-warning",
        BootstrapVariant.Info => "on-info",
        _ => "on-primary"
    };

    private static string BtnClass(BootstrapVariant variant) => variant switch
    {
        BootstrapVariant.Primary => "btn-primary",
        BootstrapVariant.Secondary => "btn-secondary",
        BootstrapVariant.Success => "btn-success",
        BootstrapVariant.Danger => "btn-danger",
        BootstrapVariant.Warning => "btn-warning",
        BootstrapVariant.Info => "btn-info",
        _ => "btn-primary"
    };

    private static string BtnOutlineClass(BootstrapVariant variant) => variant switch
    {
        BootstrapVariant.Primary => "btn-outline-primary",
        BootstrapVariant.Secondary => "btn-outline-secondary",
        BootstrapVariant.Success => "btn-outline-success",
        BootstrapVariant.Danger => "btn-outline-danger",
        BootstrapVariant.Warning => "btn-outline-warning",
        BootstrapVariant.Info => "btn-outline-info",
        _ => "btn-outline-primary"
    };

    private static VisualNode RenderVariantSection(string title, BootstrapVariant variant)
    {
        return Border(
            VStack(spacing: 12,
                Label(title).Class("h5").Class(OnClass(variant)),
                Entry().Placeholder($"{title} Entry").Class("form-control"),
                FlexLayout(
                    Button($"{title} Button").Class(BtnClass(variant)).Margin(0, 0, 8, 8),
                    Button($"{title} Outline").Class(BtnOutlineClass(variant)).Margin(0, 0, 8, 8)
                ).Wrap(FlexWrap.Wrap).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
            )
        ).Class("card").Class(TextBgClass(variant));
    }

    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 20,
                // Header
                VStack(spacing: 4,
                    Label("Color Variants").Class("h1"),
                    Label("All Bootstrap color variants applied to controls for parity comparison.").Class("lead").Class("text-muted")
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
