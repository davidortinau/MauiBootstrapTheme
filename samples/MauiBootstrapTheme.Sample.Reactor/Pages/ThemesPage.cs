using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class ThemesPage : BasePage
{
    private void ApplyTheme(string themeName)
    {
        BootstrapTheme.Apply(themeName);
    }

    public override VisualNode RenderContent()
    {
        var theme = BootstrapTheme.Current;
        
        return ScrollView(
            VStack(spacing: 24,
                // Page Header
                VStack(spacing: 4,
                    Label("Theme Switcher").H1(),
                    Label("Switch between Bootstrap themes at runtime").Lead().Muted()
                ),

                BoxView().HeightRequest(1).Color(theme.OutlineVariant).Margin(0, 4),

                // Theme Selection
                VStack(spacing: 12,
                    Label("Select Theme").H4(),
                    FlexLayout(
                        Button("Default").Primary().OnClicked(() => ApplyTheme("default")).Margin(0, 0, 8, 8),
                        Button("Darkly").Dark().OnClicked(() => ApplyTheme("darkly")).Margin(0, 0, 8, 8),
                        Button("Slate").Secondary().OnClicked(() => ApplyTheme("slate")).Margin(0, 0, 8, 8),
                        Button("Flatly").Info().OnClicked(() => ApplyTheme("flatly")).Margin(0, 0, 8, 8),
                        Button("Sketchy").Warning().OnClicked(() => ApplyTheme("sketchy")).Margin(0, 0, 8, 8),
                        Button("Vapor").Danger().OnClicked(() => ApplyTheme("vapor")).Margin(0, 0, 8, 8),
                        Button("Brite").Success().OnClicked(() => ApplyTheme("brite")).Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),
                    Label($"Current: {theme.Name}").Muted()
                ),

                // Preview
                VStack(spacing: 12,
                    Label("Preview").H4(),
                    Border(
                        VStack(spacing: 12,
                            Label("Theme Preview").H5(),

                            Entry().Placeholder("Sample input"),

                            FlexLayout(
                                Button("Primary").Primary().Margin(0, 0, 8, 8),
                                Button("Success").Success().Margin(0, 0, 8, 8),
                                Button("Danger").Danger().Margin(0, 0, 8, 8)
                            ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                            FlexLayout(
                                Button("Outline").Primary().Outlined().Margin(0, 0, 8, 8),
                                Button("Pill").Info().Pill().Margin(0, 0, 8, 8)
                            ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                            ProgressBar().Progress(0.65),

                            FlexLayout(
                                Label("Badge").Badge(BootstrapVariant.Primary).Margin(0, 0, 8, 8),
                                Label("Success").Badge(BootstrapVariant.Success).Margin(0, 0, 8, 8),
                                Label("Alert").Badge(BootstrapVariant.Danger).Margin(0, 0, 8, 8)
                            ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start)
                        )
                    ).Stroke(theme.GetOutline())
                        .BackgroundColor(theme.GetSurface())
                        .StrokeThickness(theme.BorderWidth)
                        .Set(Microsoft.Maui.Controls.Border.StrokeShapeProperty, new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = (float)theme.CornerRadius })
                        .Padding(16)
                        .ShadowMd()
                ),

                // All Color Variants
                VStack(spacing: 12,
                    Label("All Color Variants").H4(),
                    Grid("Auto,Auto,Auto,Auto", "*,*",
                        Border(Label("Primary").HCenter()).Background(BootstrapVariant.Primary).GridRow(0).GridColumn(0),
                        Border(Label("Secondary").HCenter()).Background(BootstrapVariant.Secondary).GridRow(0).GridColumn(1),
                        Border(Label("Success").HCenter()).Background(BootstrapVariant.Success).GridRow(1).GridColumn(0),
                        Border(Label("Danger").HCenter()).Background(BootstrapVariant.Danger).GridRow(1).GridColumn(1),
                        Border(Label("Warning").HCenter()).Background(BootstrapVariant.Warning).GridRow(2).GridColumn(0),
                        Border(Label("Info").HCenter()).Background(BootstrapVariant.Info).GridRow(2).GridColumn(1),
                        Border(Label("Light").HCenter()).Background(BootstrapVariant.Light).GridRow(3).GridColumn(0),
                        Border(Label("Dark").HCenter()).Background(BootstrapVariant.Dark).GridRow(3).GridColumn(1)
                    ).RowSpacing(8).ColumnSpacing(8)
                )
            ).Padding(20)
        );
    }
}
