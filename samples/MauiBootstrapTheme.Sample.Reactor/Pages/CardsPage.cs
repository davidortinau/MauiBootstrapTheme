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

                // Shadow Variants
                VStack(spacing: 12,
                    Label("Shadow Variants").H4(),
                    Grid("*,*", "*",
                        Border(
                            VStack(
                                Label("No Shadow").FontAttributes(FontAttributes.Bold),
                                Label("Default card without shadow").TextColor(Colors.Gray)
                            ).Padding(16)
                        ).GridColumn(0),
                        Border(
                            VStack(
                                Label("Small Shadow").FontAttributes(FontAttributes.Bold),
                                Label("shadow-sm class").TextColor(Colors.Gray)
                            ).Padding(16)
                        ).ShadowSm().GridColumn(1)
                    ).ColumnSpacing(16),
                    Grid("*,*", "*",
                        Border(
                            VStack(
                                Label("Default Shadow").FontAttributes(FontAttributes.Bold),
                                Label("shadow class").TextColor(Colors.Gray)
                            ).Padding(16)
                        ).ShadowMd().GridColumn(0),
                        Border(
                            VStack(
                                Label("Large Shadow").FontAttributes(FontAttributes.Bold),
                                Label("shadow-lg class").TextColor(Colors.Gray)
                            ).Padding(16)
                        ).ShadowLg().GridColumn(1)
                    ).ColumnSpacing(16)
                ),

                // Border Color Variants
                VStack(spacing: 12,
                    Label("Border Color Variants").H4(),
                    FlexLayout(
                        Border(Label("Primary").HCenter().VCenter()).Primary().Padding(16).WidthRequest(100).HeightRequest(60).Margin(0, 0, 8, 8),
                        Border(Label("Success").HCenter().VCenter()).Success().Padding(16).WidthRequest(100).HeightRequest(60).Margin(0, 0, 8, 8),
                        Border(Label("Danger").HCenter().VCenter()).Danger().Padding(16).WidthRequest(100).HeightRequest(60).Margin(0, 0, 8, 8),
                        Border(Label("Warning").HCenter().VCenter()).Warning().Padding(16).WidthRequest(100).HeightRequest(60).Margin(0, 0, 8, 8),
                        Border(Label("Info").HCenter().VCenter()).Info().Padding(16).WidthRequest(100).HeightRequest(60).Margin(0, 0, 8, 8)
                    ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                ),

                // Background Variants
                VStack(spacing: 12,
                    Label("Background Variants").H4(),
                    Border(
                        VStack(
                            Label("Primary Background").FontAttributes(FontAttributes.Bold).TextColor(Colors.White),
                            Label("text-bg-primary").TextColor(Colors.White)
                        ).Padding(16)
                    ).Background(BootstrapVariant.Primary).ShadowSm(),
                    Border(
                        VStack(
                            Label("Success Background").FontAttributes(FontAttributes.Bold).TextColor(Colors.White),
                            Label("text-bg-success").TextColor(Colors.White)
                        ).Padding(16)
                    ).Background(BootstrapVariant.Success).ShadowSm(),
                    Border(
                        VStack(
                            Label("Dark Background").FontAttributes(FontAttributes.Bold).TextColor(Colors.White),
                            Label("text-bg-dark").TextColor(Colors.White)
                        ).Padding(16)
                    ).Background(BootstrapVariant.Dark).ShadowSm()
                ),

                // Card with Header & Footer
                VStack(spacing: 12,
                    Label("Card with Header & Footer").H4(),
                    Border(
                        VStack(spacing: 0,
                            Label("Featured").Padding(12, 8).BackgroundColor(Color.FromArgb("#f8f9fa")),
                            BoxView().HeightRequest(1).Color(Colors.LightGray),
                            VStack(spacing: 8,
                                Label("Special Title").FontSize(18).FontAttributes(FontAttributes.Bold),
                                Label("With supporting text below as a natural lead-in to additional content."),
                                Button("Go somewhere").Primary().HeightRequest(38)
                            ).Padding(16),
                            BoxView().HeightRequest(1).Color(Colors.LightGray),
                            Label("2 days ago").TextColor(Colors.Gray).Padding(12, 8).BackgroundColor(Color.FromArgb("#f8f9fa"))
                        )
                    ).ShadowMd()
                )
            ).Padding(20)
        );
}
