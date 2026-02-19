using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class ControlsPage : BasePage
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                // Page Header
                VStack(spacing: 4,
                    Label("Bootstrap Controls").Class("h1"),
                    Label("Stock MAUI controls styled with Bootstrap 5 theme values.").Class("lead").Class("text-muted")
                ),

                BoxView().HeightRequest(1).Color(BootstrapTheme.Current.OutlineVariant).Margin(0, 4),

                // Buttons Section
                VStack(spacing: 8,
                    Label("Buttons").Class("h4"),
                    FlexLayout(
                        Button("Primary").Class("btn-primary").Margin(0, 0, 8, 8),
                        Button("Secondary").Class("btn-secondary").Margin(0, 0, 8, 8),
                        Button("Success").Class("btn-success").Margin(0, 0, 8, 8),
                        Button("Danger").Class("btn-danger").Margin(0, 0, 8, 8),
                        Button("Warning").Class("btn-warning").Margin(0, 0, 8, 8),
                        Button("Info").Class("btn-info").Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                ),

                // Outline Buttons Section
                VStack(spacing: 8,
                    Label("Outline Buttons").Class("h5"),
                    FlexLayout(
                        Button("Primary").Class("btn-outline-primary").Margin(0, 0, 8, 8),
                        Button("Secondary").Class("btn-outline-secondary").Margin(0, 0, 8, 8),
                        Button("Success").Class("btn-outline-success").Margin(0, 0, 8, 8),
                        Button("Danger").Class("btn-outline-danger").Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                ),

                // Button Sizes Section
                VStack(spacing: 8,
                    Label("Button Sizes").Class("h5"),
                    FlexLayout(
                        Button("Large").Class("btn-primary").Class("btn-lg").Margin(0, 0, 8, 8),
                        Button("Default").Class("btn-primary").Margin(0, 0, 8, 8),
                        Button("Small").Class("btn-primary").Class("btn-sm").Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                ),

                // Pill Buttons Section
                VStack(spacing: 8,
                    Label("Pill Buttons").Class("h5"),
                    FlexLayout(
                        Button("Primary Pill").Class("btn-primary").Class("btn-pill").Margin(0, 0, 8, 8),
                        Button("Success Pill").Class("btn-success").Class("btn-pill").Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                ),

                // Progress Section
                VStack(spacing: 12,
                    Label("Progress").Class("h4"),
                    VStack(spacing: 4,
                        Label("Primary (75%)"),
                        ProgressBar().Progress(0.75)
                    ),
                    VStack(spacing: 4,
                        Label("Success (50%)"),
                        ProgressBar().Progress(0.5).Class("progress-success")
                    ),
                    VStack(spacing: 4,
                        Label("Danger (25%)"),
                        ProgressBar().Progress(0.25).Class("progress-danger")
                    )
                ),

                // Spinners Section
                VStack(spacing: 8,
                    Label("Spinners").Class("h4"),
                    HStack(spacing: 16,
                        ActivityIndicator().IsRunning(true).Color(GetResource<Color>("Primary")),
                        ActivityIndicator().IsRunning(true).Color(GetResource<Color>("Success")),
                        ActivityIndicator().IsRunning(true).Color(GetResource<Color>("Danger"))
                    )
                )
            ).Padding(20)
        );
}
