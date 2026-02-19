using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class CardsPage : BasePage
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                // Page Header
                VStack(spacing: 4,
                    Label("Cards & Containers").Class("h1"),
                    Label("Bootstrap card components with shadows").Class("lead").Class("text-muted")
                ),

                BoxView().HeightRequest(1).Color(BootstrapTheme.Current.OutlineVariant).Margin(0, 4),

                // Shadow Variants
                VStack(spacing: 12,
                    Label("Shadow Variants").Class("h4"),
                    Grid("*", "*,*",
                        Border(
                            VStack(
                                Label("No Shadow").Class("h5"),
                                Label("Default card without shadow").Class("text-muted")
                            )
                        ).Class("card").GridColumn(0),
                        Border(
                            VStack(
                                Label("Small Shadow").Class("h5"),
                                Label("shadow-sm class").Class("text-muted")
                            )
                        ).Class("card").Class("shadow-sm").GridColumn(1)
                    ).ColumnSpacing(16).RowSpacing(16),
                    Grid("*", "*,*",
                        Border(
                            VStack(
                                Label("Default Shadow").Class("h5"),
                                Label("shadow class").Class("text-muted")
                            )
                        ).Class("card").Class("shadow").GridColumn(0),
                        Border(
                            VStack(
                                Label("Large Shadow").Class("h5"),
                                Label("shadow-lg class").Class("text-muted")
                            )
                        ).Class("card").Class("shadow-lg").GridColumn(1)
                    ).ColumnSpacing(16)
                ),

                // Border Color Variants
                VStack(spacing: 12,
                    Label("Border Color Variants").Class("h4"),
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
                    Label("Background Variants").Class("h4"),
                    Grid("*", "*,*,*",
                        Border(
                            VStack(
                                Label("Primary Background").Class("h5").Class("on-primary"),
                                Label("text-bg-primary").Class("on-primary")
                            )
                        ).Class("text-bg-primary").Class("shadow-sm").GridColumn(0),
                        Border(
                            VStack(
                                Label("Success Background").Class("h5").Class("on-success"),
                                Label("text-bg-success").Class("on-success")
                            )
                        ).Class("text-bg-success").Class("shadow-sm").GridColumn(1),
                        Border(
                            VStack(
                                Label("Dark Background").Class("h5").Class("on-dark"),
                                Label("text-bg-dark").Class("on-dark")
                            )
                        ).Class("text-bg-dark").Class("shadow-sm").GridColumn(2)
                    ).ColumnSpacing(12).RowSpacing(12)
                ),

                // Card with Header & Footer
                VStack(spacing: 12,
                    Label("Card with Header & Footer").Class("h4"),
                    Border(
                        VStack(
                            Label("Featured").Padding(12, 8).Background(GetResource<Color>("Gray100")),
                            BoxView().HeightRequest(1).Color(BootstrapTheme.Current.Outline),
                            VStack(spacing: 8,
                                Label("Special Title").Class("h5"),
                                Label("With supporting text below as a natural lead-in to additional content."),
                                Button("Go somewhere").Class("btn-primary").HorizontalOptions(LayoutOptions.Start)
                            ).Padding(16),
                            BoxView().HeightRequest(1).Color(BootstrapTheme.Current.Outline),
                            Label("3 days ago").Class("text-muted").Padding(12, 8).Background(GetResource<Color>("Gray100"))
                        ).Padding(0)
                    ).Class("card").Class("shadow").Padding(0)
                )
            ).Padding(20)
        );
}
