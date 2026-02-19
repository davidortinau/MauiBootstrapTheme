using MauiReactor;
using MauiBootstrapTheme.Extensions;
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
                    Label("Theme Switcher").Class("h1"),
                    Label("Switch between Bootstrap themes at runtime").Class("lead").Class("text-muted")
                ),

                BoxView().HeightRequest(1).Color(theme.OutlineVariant).Margin(0, 4),

                // Theme Selection
                VStack(spacing: 12,
                    Label("Select Theme").Class("h4"),
                    FlexLayout(
                        Button("Default").Class("btn-primary").OnClicked(() => ApplyTheme("default")).Margin(0, 0, 8, 8),
                        Button("Darkly").Class("btn-dark").OnClicked(() => ApplyTheme("darkly")).Margin(0, 0, 8, 8),
                        Button("Slate").Class("btn-secondary").OnClicked(() => ApplyTheme("slate")).Margin(0, 0, 8, 8),
                        Button("Flatly").Class("btn-info").OnClicked(() => ApplyTheme("flatly")).Margin(0, 0, 8, 8),
                        Button("Sketchy").Class("btn-warning").OnClicked(() => ApplyTheme("sketchy")).Margin(0, 0, 8, 8),
                        Button("Vapor").Class("btn-danger").OnClicked(() => ApplyTheme("vapor")).Margin(0, 0, 8, 8),
                        Button("Brite").Class("btn-success").OnClicked(() => ApplyTheme("brite")).Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),
                    Label($"Current: {theme.Name}").Class("text-muted")
                ),

                // Preview
                VStack(spacing: 12,
                    Label("Preview").Class("h4"),
                    Border(
                        VStack(spacing: 12,
                            Label("Theme Preview").Class("h5"),

                            Entry().Placeholder("Sample input").Class("form-control"),

                            FlexLayout(
                                Button("Primary").Class("btn-primary").Margin(0, 0, 8, 8),
                                Button("Success").Class("btn-success").Margin(0, 0, 8, 8),
                                Button("Danger").Class("btn-danger").Margin(0, 0, 8, 8)
                            ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                            FlexLayout(
                                Button("Outline").Class("btn-outline-primary").Margin(0, 0, 8, 8),
                                Button("Pill").Class("btn-info").Class("btn-pill").Margin(0, 0, 8, 8)
                            ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                            ProgressBar().Progress(0.65),

                            FlexLayout(
                                Border(Label("Badge").Class("on-primary").Class("small")).Class("badge").Class("bg-primary").Margin(0, 0, 8, 8),
                                Border(Label("Success").Class("on-success").Class("small")).Class("badge").Class("bg-success").Margin(0, 0, 8, 8),
                                Border(Label("Alert").Class("on-danger").Class("small")).Class("badge").Class("bg-danger").Margin(0, 0, 8, 8)
                            ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start)
                        )
                    ).Class("card").Class("shadow")
                ),

                // All Color Variants
                VStack(spacing: 12,
                    Label("All Color Variants").Class("h4"),
                    Grid("Auto,Auto,Auto,Auto", "*,*",
                        Border(Label("Primary").HCenter().Class("on-primary")).Class("text-bg-primary").GridRow(0).GridColumn(0),
                        Border(Label("Secondary").HCenter().Class("on-secondary")).Class("text-bg-secondary").GridRow(0).GridColumn(1),
                        Border(Label("Success").HCenter().Class("on-success")).Class("text-bg-success").GridRow(1).GridColumn(0),
                        Border(Label("Danger").HCenter().Class("on-danger")).Class("text-bg-danger").GridRow(1).GridColumn(1),
                        Border(Label("Warning").HCenter().Class("on-warning")).Class("text-bg-warning").GridRow(2).GridColumn(0),
                        Border(Label("Info").HCenter().Class("on-info")).Class("text-bg-info").GridRow(2).GridColumn(1),
                        Border(Label("Light").HCenter().Class("on-light")).Class("text-bg-light").GridRow(3).GridColumn(0),
                        Border(Label("Dark").HCenter().Class("on-dark")).Class("text-bg-dark").GridRow(3).GridColumn(1)
                    ).RowSpacing(8).ColumnSpacing(8)
                )
            ).Padding(20)
        );
    }
}
