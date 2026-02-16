using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class CardsPage : BasePage
{
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
                    Grid("*", "*,*",
                        Border(
                            VStack(
                                Label("No Shadow").H5(),
                                Label("Default card without shadow").Muted()
                            )
                        ).Class("card").GridColumn(0),
                        Border(
                            VStack(
                                Label("Small Shadow").H5(),
                                Label("shadow-sm class").Muted()
                            )
                        ).Class("card").Class("shadow-sm").GridColumn(1)
                    ).ColumnSpacing(16).RowSpacing(16),
                    Grid("*", "*,*",
                        Border(
                            VStack(
                                Label("Default Shadow").H5(),
                                Label("shadow class").Muted()
                            )
                        ).Class("card").Class("shadow").GridColumn(0),
                        Border(
                            VStack(
                                Label("Large Shadow").H5(),
                                Label("shadow-lg class").Muted()
                            )
                        ).Class("card").Class("shadow-lg").GridColumn(1)
                    ).ColumnSpacing(16)
                ),

                // Border Color Variants
                VStack(spacing: 12,
                    Label("Border Color Variants").H4(),
                    FlexLayout(
                        Border(Label("Primary").HCenter())
                            .Class("card").Stroke(BootstrapTheme.Current.Primary).WidthRequest(150).Margin(0, 0, 12, 12),
                        Border(Label("Success").HCenter())
                            .Class("card").Stroke(BootstrapTheme.Current.Success).WidthRequest(150).Margin(0, 0, 12, 12),
                        Border(Label("Danger").HCenter())
                            .Class("card").Stroke(BootstrapTheme.Current.Danger).WidthRequest(150).Margin(0, 0, 12, 12),
                        Border(Label("Warning").HCenter())
                            .Class("card").Stroke(BootstrapTheme.Current.Warning).WidthRequest(150).Margin(0, 0, 12, 12),
                        Border(Label("Info").HCenter())
                            .Class("card").Stroke(BootstrapTheme.Current.Info).WidthRequest(150).Margin(0, 0, 12, 12)
                    ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                        .JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start)
                ),

                // Background Variants
                VStack(spacing: 12,
                    Label("Background Variants").H4(),
                    Grid("*", "*,*,*",
                        Border(
                            VStack(
                                Label("Primary Background").H5().Class("on-primary"),
                                Label("text-bg-primary").Class("on-primary")
                            )
                        ).Class("text-bg-primary").Class("shadow-sm").GridColumn(0),
                        Border(
                            VStack(
                                Label("Success Background").H5().Class("on-success"),
                                Label("text-bg-success").Class("on-success")
                            )
                        ).Class("text-bg-success").Class("shadow-sm").GridColumn(1),
                        Border(
                            VStack(
                                Label("Dark Background").H5().Class("on-dark"),
                                Label("text-bg-dark").Class("on-dark")
                            )
                        ).Class("text-bg-dark").Class("shadow-sm").GridColumn(2)
                    ).ColumnSpacing(12).RowSpacing(12)
                ),

                // Card with Header & Footer
                VStack(spacing: 12,
                    Label("Card with Header & Footer").H4(),
                    Border(
                        VStack(
                            Label("Featured").Padding(12, 8).Background(GetResource<Color>("Gray100")),
                            BoxView().HeightRequest(1).Color(BootstrapTheme.Current.Outline),
                            VStack(spacing: 8,
                                Label("Special Title").H5(),
                                Label("With supporting text below as a natural lead-in to additional content."),
                                Button("Go somewhere").Primary().HorizontalOptions(LayoutOptions.Start)
                            ).Padding(16),
                            BoxView().HeightRequest(1).Color(BootstrapTheme.Current.Outline),
                            Label("2 days ago").Muted().Padding(12, 8).Background(GetResource<Color>("Gray100"))
                        )
                    ).Class("card").Class("shadow").Padding(0)
                )
            ).Padding(20)
        );
}
