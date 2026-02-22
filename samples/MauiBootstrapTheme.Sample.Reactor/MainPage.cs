﻿using MauiReactor;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Reactor.Pages;

namespace MauiBootstrapTheme.Sample.Reactor;

class MainPage : Component
{
    public override VisualNode Render()
        => Shell(
            FlyoutItem("Themes",
                ShellContent().RenderContent(() => new ThemesPage()).Route("themes")
            ).Route("themes"),
            FlyoutItem("Controls",
                ShellContent().RenderContent(() => new ControlsPage()).Route("controls")
            ).Route("controls"),
            FlyoutItem("Inputs",
                ShellContent().RenderContent(() => new InputsPage()).Route("inputs")
            ).Route("inputs"),
            FlyoutItem("Typography",
                ShellContent().RenderContent(() => new TypographyPage()).Route("typography")
            ).Route("typography"),
            FlyoutItem("Cards",
                ShellContent().RenderContent(() => new CardsPage()).Route("cards")
            ).Route("cards"),
            FlyoutItem("Forms",
                ShellContent().RenderContent(() => new FormsPage()).Route("forms")
            ).Route("forms"),
            FlyoutItem("Variants",
                ShellContent().RenderContent(() => new VariantsPage()).Route("variants")
            ).Route("variants"),
            FlyoutItem("Spacing",
                ShellContent().RenderContent(() => new SpacingPage()).Route("spacing")
            ).Route("spacing")
        )
        .Title("Bootstrap Theme")
        .FlyoutBehavior(FlyoutBehavior.Flyout)
        .FlyoutBackgroundColor(BootstrapTheme.Current.GetBackground())
        .OnAppearing((s, e) =>
        {
            if (s is Microsoft.Maui.Controls.Shell shell)
            {
                var textColor = BootstrapTheme.Current.GetOnBackground();
                shell.ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Microsoft.Maui.Controls.Grid
                    {
                        Padding = new Thickness(16, 12),
                    };
                    var label = new Microsoft.Maui.Controls.Label
                    {
                        TextColor = textColor,
                        FontSize = 16,
                        VerticalOptions = LayoutOptions.Center
                    };
                    label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, "Title");
                    grid.Children.Add(label);
                    return grid;
                });
            }
        });
}
