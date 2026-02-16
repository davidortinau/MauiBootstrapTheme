using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class TypographyPage : BasePage
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                // Page Header
                VStack(spacing: 4,
                    Label("Typography").H1(),
                    Label("Bootstrap typography utilities").Lead().Muted()
                ),

                // Headings
                VStack(spacing: 12,
                    Label("Headings").H4(),
                    Border(
                        VStack(spacing: 8,
                            Label("Heading 1 (h1)").H1(),
                            Label("Heading 2 (h2)").H2(),
                            Label("Heading 3 (h3)").H3(),
                            Label("Heading 4 (h4)").H4(),
                            Label("Heading 5 (h5)").H5(),
                            Label("Heading 6 (h6)").H6()
                        ).Padding(16)
                    ).ShadowSm()
                ),

                // Text Styles
                VStack(spacing: 12,
                    Label("Text Styles").H4(),
                    Border(
                        VStack(spacing: 8,
                            Label("Lead text - larger and lighter for introductions.").Lead(),
                            Label("Normal body text. Lorem ipsum dolor sit amet."),
                            Label("Small text for fine print.").TextStyle(BootstrapTextStyle.Small),
                            Label("Muted text for secondary content.").Muted()
                        ).Padding(16)
                    ).ShadowSm()
                ),

                // Text Colors
                VStack(spacing: 12,
                    Label("Text Colors").H4(),
                    Border(
                        VStack(spacing: 4,
                            Label("Primary text color").TextColor(BootstrapVariant.Primary),
                            Label("Secondary text color").TextColor(BootstrapVariant.Secondary),
                            Label("Success text color").TextColor(BootstrapVariant.Success),
                            Label("Danger text color").TextColor(BootstrapVariant.Danger),
                            Label("Warning text color").TextColor(BootstrapVariant.Warning),
                            Label("Info text color").TextColor(BootstrapVariant.Info)
                        ).Padding(16)
                    ).ShadowSm()
                ),

                // Badges
                VStack(spacing: 12,
                    Label("Badges").H4(),
                    Border(
                        VStack(spacing: 12,
                            FlexLayout(
                                Label("Primary").Badge(BootstrapVariant.Primary).Margin(0, 0, 8, 8),
                                Label("Secondary").Badge(BootstrapVariant.Secondary).Margin(0, 0, 8, 8),
                                Label("Success").Badge(BootstrapVariant.Success).Margin(0, 0, 8, 8),
                                Label("Danger").Badge(BootstrapVariant.Danger).Margin(0, 0, 8, 8),
                                Label("Warning").Badge(BootstrapVariant.Warning).Margin(0, 0, 8, 8),
                                Label("Info").Badge(BootstrapVariant.Info).Margin(0, 0, 8, 8)
                            ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap),
                            Label("Pill Badges").FontSize(18),
                            FlexLayout(
                                Label("Primary").Badge(BootstrapVariant.Primary).Margin(0, 0, 8, 8),
                                Label("Success").Badge(BootstrapVariant.Success).Margin(0, 0, 8, 8),
                                Label("Danger").Badge(BootstrapVariant.Danger).Margin(0, 0, 8, 8)
                            ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                        ).Padding(16)
                    ).ShadowSm()
                )
            ).Padding(20)
        );
}
