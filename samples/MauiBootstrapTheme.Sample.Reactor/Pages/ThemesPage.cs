﻿﻿using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Reactor.Themes;

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
                    Label("Theme Switcher").Class(Bs.H1),
                    Label("Switch between Bootstrap themes at runtime").Class(Bs.Lead).Class(Bs.TextMuted)
                ),

                BoxView().HeightRequest(1).Color(theme.OutlineVariant).Margin(0, 4),

                // Theme Selection
                VStack(spacing: 12,
                    Label("Select Theme").Class(Bs.H4),
                    FlexLayout(
                        Button("Default").Class(Bs.BtnPrimary).OnClicked(() => ApplyTheme("default")).Margin(0, 0, 8, 8),
                        Button("Darkly").Class(Bs.BtnDark).OnClicked(() => ApplyTheme("darkly")).Margin(0, 0, 8, 8),
                        Button("Slate").Class(Bs.BtnSecondary).OnClicked(() => ApplyTheme("slate")).Margin(0, 0, 8, 8),
                        Button("Flatly").Class(Bs.BtnInfo).OnClicked(() => ApplyTheme("flatly")).Margin(0, 0, 8, 8),
                        Button("Sketchy").Class(Bs.BtnWarning).OnClicked(() => ApplyTheme("sketchy")).Margin(0, 0, 8, 8),
                        Button("Vapor").Class(Bs.BtnDanger).OnClicked(() => ApplyTheme("vapor")).Margin(0, 0, 8, 8),
                        Button("Brite").Class(Bs.BtnSuccess).OnClicked(() => ApplyTheme("brite")).Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),
                    Label($"Current: {theme.Name}").Class(Bs.TextMuted)
                ),

                // Preview
                VStack(spacing: 12,
                    Label("Preview").Class(Bs.H4),
                    Border(
                        VStack(spacing: 12,
                            Label("Theme Preview").Class(Bs.H5),

                            Entry().Placeholder("Sample input").Class(Bs.FormControl),

                            FlexLayout(
                                Button("Primary").Class(Bs.BtnPrimary).Margin(0, 0, 8, 8),
                                Button("Success").Class(Bs.BtnSuccess).Margin(0, 0, 8, 8),
                                Button("Danger").Class(Bs.BtnDanger).Margin(0, 0, 8, 8)
                            ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                            FlexLayout(
                                Button("Outline").Class(Bs.BtnOutlinePrimary).Margin(0, 0, 8, 8),
                                Button("Pill").Class(Bs.BtnInfo).Class(Bs.BtnPill).Margin(0, 0, 8, 8)
                            ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                            ProgressBar().Progress(0.65),

                            FlexLayout(
                                Border(Label("Badge").Class(Bs.OnPrimary).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgPrimary).Margin(0, 0, 8, 8),
                                Border(Label("Success").Class(Bs.OnSuccess).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgSuccess).Margin(0, 0, 8, 8),
                                Border(Label("Alert").Class(Bs.OnDanger).Class(Bs.Small)).Class(Bs.Badge).Class(Bs.BgDanger).Margin(0, 0, 8, 8)
                            ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start)
                        )
                    ).Class(Bs.Card).Class(Bs.Shadow)
                ),

                // All Color Variants
                VStack(spacing: 12,
                    Label("All Color Variants").Class(Bs.H4),
                    Grid("Auto,Auto,Auto,Auto", "*,*",
                        Border(Label("Primary").Center().Class(Bs.OnPrimary)).Class(Bs.TextBgPrimary).GridRow(0).GridColumn(0),
                        Border(Label("Secondary").Center().Class(Bs.OnSecondary)).Class(Bs.TextBgSecondary).GridRow(0).GridColumn(1),
                        Border(Label("Success").Center().Class(Bs.OnSuccess)).Class(Bs.TextBgSuccess).GridRow(1).GridColumn(0),
                        Border(Label("Danger").Center().Class(Bs.OnDanger)).Class(Bs.TextBgDanger).GridRow(1).GridColumn(1),
                        Border(Label("Warning").Center().Class(Bs.OnWarning)).Class(Bs.TextBgWarning).GridRow(2).GridColumn(0),
                        Border(Label("Info").Center().Class(Bs.OnInfo)).Class(Bs.TextBgInfo).GridRow(2).GridColumn(1),
                        Border(Label("Light").Center().Class(Bs.OnLight)).Class(Bs.TextBgLight).GridRow(3).GridColumn(0),
                        Border(Label("Dark").Center().Class(Bs.OnDark)).Class(Bs.TextBgDark).GridRow(3).GridColumn(1)
                    ).RowSpacing(8).ColumnSpacing(8)
                )
            ).Padding(20)
        );
    }
}
