using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class CardsPage : BasePage
{
    static Border Card(VisualNode content) =>
        Border(content)
            .Stroke(BootstrapTheme.Current.GetOutline())
            .BackgroundColor(BootstrapTheme.Current.GetSurface())
            .StrokeThickness(BootstrapTheme.Current.BorderWidth)
            .Set(Microsoft.Maui.Controls.Border.StrokeShapeProperty, new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = (float)BootstrapTheme.Current.CornerRadius })
            .Padding(16);

    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                // Page Header
                VStack(spacing: 4,
                    Label("Cards & Containers").H1(),
                    Label("Bootstrap card components with shadows").Lead().Muted()
                ),

                BoxView().HeightRequest(1).Color(BootstrapTheme.Current.OutlineVariant).Margin(0, 4),

                // Shadow Variants
                VStack(spacing: 12,
                    Label("Shadow Variants").H4(),
                    Grid("*,*", "*",
                        Card(
                            VStack(
                                Label("No Shadow").H5(),
                                Label("Default card without shadow").Muted()
                            )
                        ).GridColumn(0),
                        Card(
                            VStack(
                                Label("Small Shadow").H5(),
                                Label("shadow-sm class").Muted()
                            )
                        ).ShadowSm().GridColumn(1)
                    ).ColumnSpacing(16).RowSpacing(16),
                    Grid("*,*", "*",
                        Card(
                            VStack(
                                Label("Default Shadow").H5(),
                                Label("shadow class").Muted()
                            )
                        ).ShadowMd().GridColumn(0),
                        Card(
                            VStack(
                                Label("Large Shadow").H5(),
                                Label("shadow-lg class").Muted()
                            )
                        ).ShadowLg().GridColumn(1)
                    ).ColumnSpacing(16)
                ),

                // Border Color Variants
                VStack(spacing: 12,
                    Label("Border Color Variants").H4(),
                    FlexLayout(
                        Card(Label("Primary").HCenter())
                            .Stroke(BootstrapTheme.Current.Primary).WidthRequest(150).Margin(0, 0, 12, 12),
                        Card(Label("Success").HCenter())
                            .Stroke(BootstrapTheme.Current.Success).WidthRequest(150).Margin(0, 0, 12, 12),
                        Card(Label("Danger").HCenter())
                            .Stroke(BootstrapTheme.Current.Danger).WidthRequest(150).Margin(0, 0, 12, 12),
                        Card(Label("Warning").HCenter())
                            .Stroke(BootstrapTheme.Current.Warning).WidthRequest(150).Margin(0, 0, 12, 12),
                        Card(Label("Info").HCenter())
                            .Stroke(BootstrapTheme.Current.Info).WidthRequest(150).Margin(0, 0, 12, 12)
                    ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                        .JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start)
                ),

                // Background Variants
                VStack(spacing: 12,
                    Label("Background Variants").H4(),
                    Grid("*,*,*", "*",
                        Border(
                            VStack(
                                Label("Primary Background").H5(),
                                Label("text-bg-primary")
                            )
                        ).Background(BootstrapVariant.Primary).ShadowSm().GridColumn(0),
                        Border(
                            VStack(
                                Label("Success Background").H5(),
                                Label("text-bg-success")
                            )
                        ).Background(BootstrapVariant.Success).ShadowSm().GridColumn(1),
                        Border(
                            VStack(
                                Label("Dark Background").H5(),
                                Label("text-bg-dark")
                            )
                        ).Background(BootstrapVariant.Dark).ShadowSm().GridColumn(2)
                    ).ColumnSpacing(12).RowSpacing(12)
                ),

                // Card with Header & Footer
                VStack(spacing: 12,
                    Label("Card with Header & Footer").H4(),
                    Card(
                        VStack(
                            Label("Featured").Padding(12, 8).BackgroundColor(BootstrapTheme.Current.Light),
                            BoxView().HeightRequest(1).Color(BootstrapTheme.Current.Outline),
                            VStack(spacing: 8,
                                Label("Special Title").H5(),
                                Label("With supporting text below as a natural lead-in to additional content."),
                                Button("Go somewhere").Primary().HorizontalOptions(LayoutOptions.Start)
                            ).Padding(16),
                            BoxView().HeightRequest(1).Color(BootstrapTheme.Current.Outline),
                            Label("2 days ago").Muted().Padding(12, 8).BackgroundColor(BootstrapTheme.Current.Light)
                        )
                    ).Padding(0).ShadowMd()
                )
            ).Padding(20)
        );
}
