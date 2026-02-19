using MauiReactor;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Reactor.Pages;

namespace MauiBootstrapTheme.Sample.Reactor;

class MainPage : Component
{
    public override VisualNode Render()
        => Shell(
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
            FlyoutItem("Themes",
                ShellContent().RenderContent(() => new ThemesPage()).Route("themes")
            ).Route("themes"),
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
        .FlyoutBehavior(FlyoutBehavior.Flyout);
}
