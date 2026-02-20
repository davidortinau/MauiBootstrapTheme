using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Reactor.Themes;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class TypographyPage : BasePage
{
    static Border Card(VisualNode content) =>
        Border(content)
            .Class(Bs.Card);

    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                // Page Header
                VStack(spacing: 4,
                    Label("Typography").ThemeKey(Bs.H1),
                    Label("Bootstrap typography utilities").ThemeKey(Bs.Lead)
                ),

                BoxView().HeightRequest(1).Color(BootstrapTheme.Current.OutlineVariant).Margin(0, 4),

                // Headings
                VStack(spacing: 12,
                    Label("Headings").ThemeKey(Bs.H4),
                    Card(
                        VStack(spacing: 8,
                            Label("Heading 1 (h1)").ThemeKey(Bs.H1),
                            Label("Heading 2 (h2)").ThemeKey(Bs.H2),
                            Label("Heading 3 (h3)").ThemeKey(Bs.H3),
                            Label("Heading 4 (h4)").ThemeKey(Bs.H4),
                            Label("Heading 5 (h5)").ThemeKey(Bs.H5),
                            Label("Heading 6 (h6)").ThemeKey(Bs.H6)
                        )
                    )
                ),

                // Text Styles
                VStack(spacing: 12,
                    Label("Text Styles").ThemeKey(Bs.H4),
                    Card(
                        VStack(spacing: 8,
                            Label("This is lead text - larger and lighter for introductory content.").ThemeKey(Bs.Lead),
                            Label("This is normal body text. Lorem ipsum dolor sit amet, consectetur adipiscing elit."),
                            Label("This is small text for fine print.").ThemeKey(Bs.Small),
                            Label("This is muted text for secondary content.").Class(Bs.TextMuted),
                            Label("This is marked/highlighted text.").Class(Bs.Mark)
                        )
                    )
                ),

                // Text Colors
                VStack(spacing: 12,
                    Label("Text Colors").ThemeKey(Bs.H4),
                    Card(
                        VStack(spacing: 4,
                            Label("Primary text color").Class(Bs.TextPrimary),
                            Label("Secondary text color").Class(Bs.TextSecondary),
                            Label("Success text color").Class(Bs.TextSuccess),
                            Label("Danger text color").Class(Bs.TextDanger),
                            Label("Warning text color").Class(Bs.TextWarning),
                            Label("Info text color").Class(Bs.TextInfo)
                        )
                    )
                ),

                // Badges
                VStack(spacing: 12,
                    Label("Badges").ThemeKey(Bs.H4),
                    Card(
                        VStack(spacing: 12,
                            FlexLayout(
                                Border(Label("Primary").Class(Bs.OnPrimary).ThemeKey(Bs.Small)).Class(Bs.Badge).Class(Bs.BgPrimary).Margin(0, 0, 8, 8),
                                Border(Label("Secondary").Class(Bs.OnSecondary).ThemeKey(Bs.Small)).Class(Bs.Badge).Class(Bs.BgSecondary).Margin(0, 0, 8, 8),
                                Border(Label("Success").Class(Bs.OnSuccess).ThemeKey(Bs.Small)).Class(Bs.Badge).Class(Bs.BgSuccess).Margin(0, 0, 8, 8),
                                Border(Label("Danger").Class(Bs.OnDanger).ThemeKey(Bs.Small)).Class(Bs.Badge).Class(Bs.BgDanger).Margin(0, 0, 8, 8),
                                Border(Label("Warning").Class(Bs.OnWarning).ThemeKey(Bs.Small)).Class(Bs.Badge).Class(Bs.BgWarning).Margin(0, 0, 8, 8),
                                Border(Label("Info").Class(Bs.OnInfo).ThemeKey(Bs.Small)).Class(Bs.Badge).Class(Bs.BgInfo).Margin(0, 0, 8, 8)
                            ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                                .AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                                .JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                            Label("Pill Badges").ThemeKey(Bs.H6),
                            FlexLayout(
                                Border(Label("Primary").Class(Bs.OnPrimary).ThemeKey(Bs.Small)).Class(Bs.Badge).Class(Bs.BgPrimary).Margin(0, 0, 8, 8),
                                Border(Label("Success").Class(Bs.OnSuccess).ThemeKey(Bs.Small)).Class(Bs.Badge).Class(Bs.BgSuccess).Margin(0, 0, 8, 8),
                                Border(Label("Danger").Class(Bs.OnDanger).ThemeKey(Bs.Small)).Class(Bs.Badge).Class(Bs.BgDanger).Margin(0, 0, 8, 8)
                            ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                                .AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                                .JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                            Label("Badge with Text").ThemeKey(Bs.H6),
                            HStack(spacing: 8,
                                Label("Messages").VCenter(),
                                Border(Label("4").Class(Bs.OnPrimary).ThemeKey(Bs.Small)).Class(Bs.Badge).Class(Bs.BgPrimary).VCenter(),
                                Label("Notifications").VCenter().Margin(12, 0, 0, 0),
                                Border(Label("99+").Class(Bs.OnDanger).ThemeKey(Bs.Small)).Class(Bs.Badge).Class(Bs.BgDanger).VCenter()
                            )
                        )
                    )
                )
            ).Padding(20)
        );
}
