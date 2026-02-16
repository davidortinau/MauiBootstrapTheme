using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class TypographyPage : BasePage
{
    static Border Card(VisualNode content) =>
        Border(content)
            .Class("card");

    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                // Page Header
                VStack(spacing: 4,
                    Label("Typography").H1(),
                    Label("Bootstrap typography utilities").Lead()
                ),

                BoxView().HeightRequest(1).Color(BootstrapTheme.Current.OutlineVariant).Margin(0, 4),

                // Headings
                VStack(spacing: 12,
                    Label("Headings").H4(),
                    Card(
                        VStack(spacing: 8,
                            Label("Heading 1 (h1)").H1(),
                            Label("Heading 2 (h2)").H2(),
                            Label("Heading 3 (h3)").H3(),
                            Label("Heading 4 (h4)").H4(),
                            Label("Heading 5 (h5)").H5(),
                            Label("Heading 6 (h6)").H6()
                        )
                    )
                ),

                // Text Styles
                VStack(spacing: 12,
                    Label("Text Styles").H4(),
                    Card(
                        VStack(spacing: 8,
                            Label("This is lead text - larger and lighter for introductory content.").Lead(),
                            Label("This is normal body text. Lorem ipsum dolor sit amet, consectetur adipiscing elit."),
                            Label("This is small text for fine print.").Class("small"),
                            Label("This is muted text for secondary content.").Class("text-muted"),
                            Label("This is marked/highlighted text.").Class("mark")
                        )
                    )
                ),

                // Text Colors
                VStack(spacing: 12,
                    Label("Text Colors").H4(),
                    Card(
                        VStack(spacing: 4,
                            Label("Primary text color").Class("text-primary"),
                            Label("Secondary text color").Class("text-secondary"),
                            Label("Success text color").Class("text-success"),
                            Label("Danger text color").Class("text-danger"),
                            Label("Warning text color").Class("text-warning"),
                            Label("Info text color").Class("text-info")
                        )
                    )
                ),

                // Badges
                VStack(spacing: 12,
                    Label("Badges").H4(),
                    Card(
                        VStack(spacing: 12,
                            FlexLayout(
                                Border(Label("Primary").Class("on-primary").Class("small")).Class("badge").Class("bg-primary").Margin(0, 0, 8, 8),
                                Border(Label("Secondary").Class("on-secondary").Class("small")).Class("badge").Class("bg-secondary").Margin(0, 0, 8, 8),
                                Border(Label("Success").Class("on-success").Class("small")).Class("badge").Class("bg-success").Margin(0, 0, 8, 8),
                                Border(Label("Danger").Class("on-danger").Class("small")).Class("badge").Class("bg-danger").Margin(0, 0, 8, 8),
                                Border(Label("Warning").Class("on-warning").Class("small")).Class("badge").Class("bg-warning").Margin(0, 0, 8, 8),
                                Border(Label("Info").Class("on-info").Class("small")).Class("badge").Class("bg-info").Margin(0, 0, 8, 8)
                            ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                                .AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                                .JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                            Label("Pill Badges").H6(),
                            FlexLayout(
                                Border(Label("Primary").Class("on-primary").Class("small")).Class("badge").Class("bg-primary").Margin(0, 0, 8, 8),
                                Border(Label("Success").Class("on-success").Class("small")).Class("badge").Class("bg-success").Margin(0, 0, 8, 8),
                                Border(Label("Danger").Class("on-danger").Class("small")).Class("badge").Class("bg-danger").Margin(0, 0, 8, 8)
                            ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                                .AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                                .JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                            Label("Badge with Text").H6(),
                            HStack(spacing: 8,
                                Label("Messages").VCenter(),
                                Border(Label("4").Class("on-primary").Class("small")).Class("badge").Class("bg-primary").VCenter(),
                                Label("Notifications").VCenter().Margin(12, 0, 0, 0),
                                Border(Label("99+").Class("on-danger").Class("small")).Class("badge").Class("bg-danger").VCenter()
                            )
                        )
                    )
                )
            ).Padding(20)
        );
}
