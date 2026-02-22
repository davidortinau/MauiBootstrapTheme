﻿﻿using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Reactor.Themes;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class VariantsPage : BasePage
{
    private static string TextBgClass(BootstrapVariant variant) => variant switch
    {
        BootstrapVariant.Primary => Bs.TextBgPrimary,
        BootstrapVariant.Secondary => Bs.TextBgSecondary,
        BootstrapVariant.Success => Bs.TextBgSuccess,
        BootstrapVariant.Danger => Bs.TextBgDanger,
        BootstrapVariant.Warning => Bs.TextBgWarning,
        BootstrapVariant.Info => Bs.TextBgInfo,
        _ => Bs.TextBgPrimary
    };

    private static string OnClass(BootstrapVariant variant) => variant switch
    {
        BootstrapVariant.Primary => Bs.OnPrimary,
        BootstrapVariant.Secondary => Bs.OnSecondary,
        BootstrapVariant.Success => Bs.OnSuccess,
        BootstrapVariant.Danger => Bs.OnDanger,
        BootstrapVariant.Warning => Bs.OnWarning,
        BootstrapVariant.Info => Bs.OnInfo,
        _ => Bs.OnPrimary
    };

    private static string BtnClass(BootstrapVariant variant) => variant switch
    {
        BootstrapVariant.Primary => Bs.BtnPrimary,
        BootstrapVariant.Secondary => Bs.BtnSecondary,
        BootstrapVariant.Success => Bs.BtnSuccess,
        BootstrapVariant.Danger => Bs.BtnDanger,
        BootstrapVariant.Warning => Bs.BtnWarning,
        BootstrapVariant.Info => Bs.BtnInfo,
        _ => Bs.BtnPrimary
    };

    private static string BtnOutlineClass(BootstrapVariant variant) => variant switch
    {
        BootstrapVariant.Primary => Bs.BtnOutlinePrimary,
        BootstrapVariant.Secondary => Bs.BtnOutlineSecondary,
        BootstrapVariant.Success => Bs.BtnOutlineSuccess,
        BootstrapVariant.Danger => Bs.BtnOutlineDanger,
        BootstrapVariant.Warning => Bs.BtnOutlineWarning,
        BootstrapVariant.Info => Bs.BtnOutlineInfo,
        _ => Bs.BtnOutlinePrimary
    };

    private static VisualNode RenderVariantSection(string title, BootstrapVariant variant)
    {
        return Border(
            VStack(spacing: Bs.Spacing3,
                Label(title).Class(Bs.H5).Class(OnClass(variant)),
                Entry().Placeholder($"{title} Entry").Class(Bs.FormControl),
                FlexLayout(
                    Button($"{title} Button").Class(BtnClass(variant)).Margin(0, 0, 8, 8),
                    Button($"{title} Outline").Class(BtnOutlineClass(variant)).Margin(0, 0, 8, 8)
                ).Wrap(FlexWrap.Wrap).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
            )
        ).Class(Bs.Card).Class(TextBgClass(variant));
    }

    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: Bs.Spacing4,
                // Header
                VStack(spacing: Bs.Spacing1,
                    Label("Color Variants").Class(Bs.H1),
                    Label("All Bootstrap color variants applied to controls for parity comparison.").Class(Bs.Lead).Class(Bs.TextMuted)
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
