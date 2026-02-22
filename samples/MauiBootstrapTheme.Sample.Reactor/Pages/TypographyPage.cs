﻿using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Reactor.Themes;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class TypographyPage : BasePage
{
    static Border Card(VisualNode content) =>
        Border(content)
            .Class(Bs.Card).Class(Bs.Shadow);

    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: Bs.Spacing4,
                // Page Header
                VStack(spacing: Bs.Spacing0,
                    Label("Typography").Class(Bs.H1),
                    Label("Bootstrap typography utilities").Class(Bs.Lead)
                ),

                BoxView().HeightRequest(1).Color(BootstrapTheme.Current.Outline),

                // Headings
                VStack(spacing: Bs.Spacing3,
                    Label("Headings").Class(Bs.H4),
                    Border(
                        VStack(spacing: Bs.Spacing3,
                            Label("Heading 1 (h1)").Class(Bs.H1),
                            Label("Heading 2 (h2)").Class(Bs.H2),
                            Label("Heading 3 (h3)").Class(Bs.H3),
                            Label("Heading 4 (h4)").Class(Bs.H4),
                            Label("Heading 5 (h5)").Class(Bs.H5),
                            Label("Heading 6 (h6)").Class(Bs.H6)
                        )
                    ).Class(Bs.Card).Class(Bs.Shadow)
                ),

                // Text Styles
                VStack(spacing: Bs.Spacing3,
                    Label("Text Styles").Class(Bs.H4),
                    Border(
                        VStack(spacing: Bs.Spacing3,
                            Label("This is lead text - larger and lighter for introductory content.").Class(Bs.Lead),
                            Label("This is normal body text. Lorem ipsum dolor sit amet, consectetur adipiscing elit."),
                            Label("This is small text for fine print.").Class(Bs.Small),
                            Label("This is muted text for secondary content.").Class(Bs.TextMuted),
                            Label("This is marked/highlighted text.").Class(Bs.Mark)
                        )
                    ).Class(Bs.Card).Class(Bs.Shadow)
                ),

                // Text Colors
                VStack(spacing: Bs.Spacing3,
                    Label("Text Colors").Class(Bs.H4),
                    Border(
                        VStack(spacing: Bs.Spacing1,
                            Label("Primary text color").Class(Bs.TextPrimary),
                            Label("Secondary text color").Class(Bs.TextSecondary),
                            Label("Success text color").Class(Bs.TextSuccess),
                            Label("Danger text color").Class(Bs.TextDanger),
                            Label("Warning text color").Class(Bs.TextWarning),
                            Label("Info text color").Class(Bs.TextInfo)
                        )
                    ).Class(Bs.Card).Class(Bs.Shadow)
                ),

                // Badges
                VStack(spacing: Bs.Spacing3,
                    Label("Badges").Class(Bs.H4),
                    Border(
                        VStack(spacing: Bs.Spacing3,
                            FlexLayout(
                                Border(Label("Primary").Class(Bs.OnPrimary).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgPrimary).Margin(0, 0, 8, 8),
                                Border(Label("Secondary").Class(Bs.OnSecondary).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgSecondary).Margin(0, 0, 8, 8),
                                Border(Label("Success").Class(Bs.OnSuccess).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgSuccess).Margin(0, 0, 8, 8),
                                Border(Label("Danger").Class(Bs.OnDanger).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgDanger).Margin(0, 0, 8, 8),
                                Border(Label("Warning").Class(Bs.OnWarning).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgWarning).Margin(0, 0, 8, 8),
                                Border(Label("Info").Class(Bs.OnInfo).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgInfo).Margin(0, 0, 8, 8)
                            ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                                .AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                                .JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                            Label("Pill Badges").Class(Bs.H6),
                            FlexLayout(
                                Border(Label("Primary").Class(Bs.OnPrimary).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgPrimary).Margin(0, 0, 8, 8),
                                Border(Label("Success").Class(Bs.OnSuccess).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgSuccess).Margin(0, 0, 8, 8),
                                Border(Label("Danger").Class(Bs.OnDanger).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgDanger).Margin(0, 0, 8, 8)
                            ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                                .AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                                .JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                            Label("Badge with Text").Class(Bs.H6),
                            HStack(spacing: Bs.Spacing2,
                                Label("Messages").VCenter(),
                                Border(Label("4").Class(Bs.OnPrimary).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgPrimary).VCenter(),
                                Label("Notifications").VCenter().Margin(12, 0, 0, 0),
                                Border(Label("99+").Class(Bs.OnDanger).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgDanger).VCenter()
                            )
                        )
                    ).Class(Bs.Card).Class(Bs.Shadow)
                )
            ).Padding(20)
        );
}
