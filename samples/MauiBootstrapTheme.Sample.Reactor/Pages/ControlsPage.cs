using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class ControlsPage : BasePage
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                // Page Header
                VStack(spacing: 4,
                    Label("Bootstrap Controls").H1(),
                    Label("Stock MAUI controls styled with Bootstrap 5 theme values.").Lead().Muted()
                ),

                BoxView().HeightRequest(1).Color(BootstrapTheme.Current.OutlineVariant).Margin(0, 4),

                // Buttons Section
                VStack(spacing: 8,
                    Label("Buttons").H4(),
                    FlexLayout(
                        Button("Primary").Primary().Margin(0, 0, 8, 8),
                        Button("Secondary").Secondary().Margin(0, 0, 8, 8),
                        Button("Success").Success().Margin(0, 0, 8, 8),
                        Button("Danger").Danger().Margin(0, 0, 8, 8),
                        Button("Warning").Warning().Margin(0, 0, 8, 8),
                        Button("Info").Info().Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                ),

                // Outline Buttons Section
                VStack(spacing: 8,
                    Label("Outline Buttons").H5(),
                    FlexLayout(
                        Button("Primary").Primary().Outlined().Margin(0, 0, 8, 8),
                        Button("Secondary").Secondary().Outlined().Margin(0, 0, 8, 8),
                        Button("Success").Success().Outlined().Margin(0, 0, 8, 8),
                        Button("Danger").Danger().Outlined().Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                ),

                // Button Sizes Section
                VStack(spacing: 8,
                    Label("Button Sizes").H5(),
                    FlexLayout(
                        Button("Large").Class("btn-primary").Class("btn-lg").Margin(0, 0, 8, 8),
                        Button("Default").Class("btn-primary").Margin(0, 0, 8, 8),
                        Button("Small").Class("btn-primary").Class("btn-sm").Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                ),

                // Pill Buttons Section
                VStack(spacing: 8,
                    Label("Pill Buttons").H5(),
                    FlexLayout(
                        Button("Primary Pill").Primary().Pill().Margin(0, 0, 8, 8),
                        Button("Success Pill").Success().Pill().Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                ),

                // Progress Section
                VStack(spacing: 12,
                    Label("Progress").H4(),
                    VStack(spacing: 4,
                        Label("Primary (75%)"),
                        ProgressBar().Progress(0.75).Primary().BootstrapHeight()
                    ),
                    VStack(spacing: 4,
                        Label("Success (50%)"),
                        ProgressBar().Progress(0.5).Success().BootstrapHeight()
                    ),
                    VStack(spacing: 4,
                        Label("Danger (25%)"),
                        ProgressBar().Progress(0.25).Danger().BootstrapHeight()
                    )
                ),

                // Spinners Section
                VStack(spacing: 8,
                    Label("Spinners").H4(),
                    HStack(spacing: 16,
                        ActivityIndicator().IsRunning(true).Primary(),
                        ActivityIndicator().IsRunning(true).Success(),
                        ActivityIndicator().IsRunning(true).Danger()
                    )
                )
            ).Padding(20)
        );
}
