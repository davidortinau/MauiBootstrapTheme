using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class SpacingPage : BasePage
{
    static Border Card(VisualNode content) =>
        Border(content)
            .Class("card");

    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 20,
                // Header
                VStack(spacing: 4,
                    Label("Spacing Utilities").Class("h1"),
                    Label("Canonical Bootstrap 5.3 spacing utility patterns.").Class("lead").Class("text-muted")
                ),

                // Notation card
                Card(
                    VStack(spacing: 8,
                        Label("Notation").Class("h4"),
                        Label("m = margin, p = padding").Class("small"),
                        Label("t, b, s, e, x, y = side selectors").Class("small"),
                        Label("0-5 = spacing scale values").Class("small"),
                        Label("Format: {property}{side}-{size} (example: mt-3, px-2)").Class("small").Class("text-muted")
                    )
                ),

                // Examples card
                Card(
                    VStack(spacing: 10,
                        Label("Examples").Class("h4"),

                        // Padding examples
                        Border(
                            VStack(spacing: 6,
                                Border(Label("p-0 (PaddingLevel=0)").TextColor(Colors.White)).BackgroundColor(BootstrapTheme.Current.Primary).Set(Bootstrap.PaddingLevelProperty, 0),
                                Border(Label("p-1 (PaddingLevel=1)").TextColor(Colors.White)).BackgroundColor(BootstrapTheme.Current.Primary).Set(Bootstrap.PaddingLevelProperty, 1),
                                Border(Label("p-2 (PaddingLevel=2)").TextColor(Colors.White)).BackgroundColor(BootstrapTheme.Current.Primary).Set(Bootstrap.PaddingLevelProperty, 2),
                                Border(Label("p-3 (PaddingLevel=3)").TextColor(Colors.White)).BackgroundColor(BootstrapTheme.Current.Primary).Set(Bootstrap.PaddingLevelProperty, 3),
                                Border(Label("p-4 (PaddingLevel=4)").TextColor(Colors.White)).BackgroundColor(BootstrapTheme.Current.Primary).Set(Bootstrap.PaddingLevelProperty, 4),
                                Border(Label("p-5 (PaddingLevel=5)").TextColor(Colors.White)).BackgroundColor(BootstrapTheme.Current.Primary).Set(Bootstrap.PaddingLevelProperty, 5)
                            )
                        ).BackgroundColor(Color.FromArgb("#e9ecef")).Padding(8),

                        // Margin examples
                        Border(
                            VStack(spacing: 0,
                                Border(Label("mt-0 (MarginLevel=0)").Padding(8)).BackgroundColor(Colors.White),
                                Border(Label("mt-1 (MarginLevel=1)").Padding(8)).BackgroundColor(Colors.White).Set(Bootstrap.MarginLevelProperty, 1),
                                Border(Label("mt-2 (MarginLevel=2)").Padding(8)).BackgroundColor(Colors.White).Set(Bootstrap.MarginLevelProperty, 2),
                                Border(Label("mt-3 (MarginLevel=3)").Padding(8)).BackgroundColor(Colors.White).Set(Bootstrap.MarginLevelProperty, 3),
                                Border(Label("mt-4 (MarginLevel=4)").Padding(8)).BackgroundColor(Colors.White).Set(Bootstrap.MarginLevelProperty, 4),
                                Border(Label("mt-5 (MarginLevel=5)").Padding(8)).BackgroundColor(Colors.White).Set(Bootstrap.MarginLevelProperty, 5)
                            )
                        ).BackgroundColor(Color.FromArgb("#f8f9fa")).Padding(8)
                    )
                ),

                // Horizontal centering card
                Card(
                    VStack(spacing: 8,
                        Label("Horizontal centering").Class("h4"),
                        Border(
                            Label(".mx-auto with width: 200px").TextColor(Colors.White).HorizontalOptions(LayoutOptions.Center)
                        ).WidthRequest(200).HorizontalOptions(LayoutOptions.Center).BackgroundColor(BootstrapTheme.Current.Secondary).Padding(8)
                    )
                ),

                // Gap utilities card
                Card(
                    VStack(spacing: 8,
                        Label("Gap utilities").Class("h4"),
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
