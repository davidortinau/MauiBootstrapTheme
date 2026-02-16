using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class SpacingPage : BasePage
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
            VStack(spacing: 20,
                // Header
                VStack(spacing: 4,
                    Label("Spacing Utilities").H1(),
                    Label("Canonical Bootstrap 5.3 spacing utility patterns.").Lead().Muted()
                ),

                // Notation card
                Card(
                    VStack(spacing: 8,
                        Label("Notation").H4(),
                        Label("m = margin, p = padding").TextStyle(BootstrapTextStyle.Small),
                        Label("t, b, s, e, x, y = side selectors").TextStyle(BootstrapTextStyle.Small),
                        Label("0-5 = spacing scale values").TextStyle(BootstrapTextStyle.Small),
                        Label("Format: {property}{side}-{size} (example: mt-3, px-2)").TextStyle(BootstrapTextStyle.Small).Muted()
                    )
                ),

                // Examples card
                Card(
                    VStack(spacing: 10,
                        Label("Examples").H4(),

                        // Padding examples
                        Border(
                            VStack(spacing: 6,
                                Border(Label("p-0 (PaddingLevel=0)").TextColor(Colors.White)).BackgroundColor(BootstrapTheme.Current.Primary).PaddingLevel(0),
                                Border(Label("p-1 (PaddingLevel=1)").TextColor(Colors.White)).BackgroundColor(BootstrapTheme.Current.Primary).PaddingLevel(1),
                                Border(Label("p-2 (PaddingLevel=2)").TextColor(Colors.White)).BackgroundColor(BootstrapTheme.Current.Primary).PaddingLevel(2),
                                Border(Label("p-3 (PaddingLevel=3)").TextColor(Colors.White)).BackgroundColor(BootstrapTheme.Current.Primary).PaddingLevel(3),
                                Border(Label("p-4 (PaddingLevel=4)").TextColor(Colors.White)).BackgroundColor(BootstrapTheme.Current.Primary).PaddingLevel(4),
                                Border(Label("p-5 (PaddingLevel=5)").TextColor(Colors.White)).BackgroundColor(BootstrapTheme.Current.Primary).PaddingLevel(5)
                            )
                        ).BackgroundColor(Color.FromArgb("#e9ecef")).Padding(8),

                        // Margin examples
                        Border(
                            VStack(spacing: 0,
                                Border(Label("mt-0 (MarginLevel=0)").Padding(8)).BackgroundColor(Colors.White),
                                Border(Label("mt-1 (MarginLevel=1)").Padding(8)).BackgroundColor(Colors.White).MarginLevel(1),
                                Border(Label("mt-2 (MarginLevel=2)").Padding(8)).BackgroundColor(Colors.White).MarginLevel(2),
                                Border(Label("mt-3 (MarginLevel=3)").Padding(8)).BackgroundColor(Colors.White).MarginLevel(3),
                                Border(Label("mt-4 (MarginLevel=4)").Padding(8)).BackgroundColor(Colors.White).MarginLevel(4),
                                Border(Label("mt-5 (MarginLevel=5)").Padding(8)).BackgroundColor(Colors.White).MarginLevel(5)
                            )
                        ).BackgroundColor(Color.FromArgb("#f8f9fa")).Padding(8)
                    )
                ),

                // Horizontal centering card
                Card(
                    VStack(spacing: 8,
                        Label("Horizontal centering").H4(),
                        Border(
                            Label(".mx-auto with width: 200px").TextColor(Colors.White).HorizontalOptions(LayoutOptions.Center)
                        ).WidthRequest(200).HorizontalOptions(LayoutOptions.Center).BackgroundColor(BootstrapTheme.Current.Secondary).Padding(8)
                    )
                ),

                // Gap utilities card
                Card(
                    VStack(spacing: 8,
                        Label("Gap utilities").H4(),
                        HStack(spacing: 8,
                            Border(Label("gap-2 equivalent")).BackgroundColor(BootstrapTheme.Current.Info).Padding(8),
                            Border(Label("Item")).BackgroundColor(BootstrapTheme.Current.Info).Padding(8),
                            Border(Label("Item")).BackgroundColor(BootstrapTheme.Current.Info).Padding(8)
                        ),
                        VStack(spacing: 12,
                            Border(Label("d-grid gap-3 item 1")).BackgroundColor(Color.FromArgb("#f8f9fa")).Padding(8),
                            Border(Label("d-grid gap-3 item 2")).BackgroundColor(Color.FromArgb("#f8f9fa")).Padding(8)
                        )
                    )
                )
            ).Padding(20)
        );
}
