using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Reactor.Themes;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class CardsPage : BasePage
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                // Page Header
                VStack(spacing: 4,
                    Label("Cards & Containers").Class(Bs.H1),
                    Label("Bootstrap card components with shadows").Class(Bs.Lead).Class(Bs.TextMuted)
                ),

                BoxView().HeightRequest(1).Color(BootstrapTheme.Current.OutlineVariant).Margin(0, 4),

                // Shadow Variants
                VStack(spacing: 12,
                    Label("Shadow Variants").Class(Bs.H4),
                    Grid("*", "*,*",
                        Border(
                            VStack(
                                Label("No Shadow").Class(Bs.H5),
                                Label("Default card without shadow").Class(Bs.TextMuted)
                            )
                        ).Class(Bs.Card).GridColumn(0),
                        Border(
                            VStack(
                                Label("Small Shadow").Class(Bs.H5),
                                Label("shadow-sm class").Class(Bs.TextMuted)
                            )
                        ).Class(Bs.Card).Class(Bs.ShadowSm).GridColumn(1)
                    ).ColumnSpacing(16).RowSpacing(16),
                    Grid("*", "*,*",
                        Border(
                            VStack(
                                Label("Default Shadow").Class(Bs.H5),
                                Label("shadow class").Class(Bs.TextMuted)
                            )
                        ).Class(Bs.Card).Class(Bs.Shadow).GridColumn(0),
                        Border(
                            VStack(
                                Label("Large Shadow").Class(Bs.H5),
                                Label("shadow-lg class").Class(Bs.TextMuted)
                            )
                        ).Class(Bs.Card).Class(Bs.ShadowLg).GridColumn(1)
                    ).ColumnSpacing(16)
                ),

                // Border Color Variants
                VStack(spacing: 12,
                    Label("Border Color Variants").Class(Bs.H4),
                    FlexLayout(
                        Border(Label("Primary").HCenter())
                            .Class(Bs.Card).Stroke(BootstrapTheme.Current.Primary).WidthRequest(150).Margin(0, 0, 12, 12),
                        Border(Label("Success").HCenter())
                            .Class(Bs.Card).Stroke(BootstrapTheme.Current.Success).WidthRequest(150).Margin(0, 0, 12, 12),
                        Border(Label("Danger").HCenter())
                            .Class(Bs.Card).Stroke(BootstrapTheme.Current.Danger).WidthRequest(150).Margin(0, 0, 12, 12),
                        Border(Label("Warning").HCenter())
                            .Class(Bs.Card).Stroke(BootstrapTheme.Current.Warning).WidthRequest(150).Margin(0, 0, 12, 12),
                        Border(Label("Info").HCenter())
                            .Class(Bs.Card).Stroke(BootstrapTheme.Current.Info).WidthRequest(150).Margin(0, 0, 12, 12)
                    ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                        .JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start)
                ),

                // Background Variants
                VStack(spacing: 12,
                    Label("Background Variants").Class(Bs.H4),
                    Grid("*", "*,*,*",
                        Border(
                            VStack(
                                Label("Primary Background").Class(Bs.H5).Class(Bs.OnPrimary),
                                Label("text-bg-primary").Class(Bs.OnPrimary)
                            )
                        ).Class(Bs.TextBgPrimary).Class(Bs.ShadowSm).GridColumn(0),
                        Border(
                            VStack(
                                Label("Success Background").Class(Bs.H5).Class(Bs.OnSuccess),
                                Label("text-bg-success").Class(Bs.OnSuccess)
                            )
                        ).Class(Bs.TextBgSuccess).Class(Bs.ShadowSm).GridColumn(1),
                        Border(
                            VStack(
                                Label("Dark Background").Class(Bs.H5).Class(Bs.OnDark),
                                Label("text-bg-dark").Class(Bs.OnDark)
                            )
                        ).Class(Bs.TextBgDark).Class(Bs.ShadowSm).GridColumn(2)
                    ).ColumnSpacing(12).RowSpacing(12)
                ),

                // Card with Header & Footer
                VStack(spacing: 12,
                    Label("Card with Header & Footer").Class(Bs.H4),
                    Border(
                        VStack(
                            Label("Featured").Padding(12, 8).Background(GetResource<Color>("Gray100")),
                            BoxView().HeightRequest(1).Color(BootstrapTheme.Current.Outline),
                            VStack(spacing: 8,
                                Label("Special Title").Class(Bs.H5),
                                Label("With supporting text below as a natural lead-in to additional content."),
                                Button("Go somewhere").Class(Bs.BtnPrimary).HorizontalOptions(LayoutOptions.Start)
                            ).Padding(16),
                            BoxView().HeightRequest(1).Color(BootstrapTheme.Current.Outline),
                            Label("2 days ago").Class(Bs.TextMuted).Padding(12, 8).Background(GetResource<Color>("Gray100"))
                        ).Padding(0)
                    ).Class(Bs.Card).Class(Bs.Shadow).Padding(0)
                )
            ).Padding(20)
        );
}
