using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;
using MauiDevFlow.Agent;

namespace MauiBootstrapTheme.Sample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseBootstrapTheme(options =>
            {
                options.AddTheme<Themes.DefaultTheme>("default");
                options.AddTheme<Themes.DarklyTheme>("darkly");
                options.AddTheme<Themes.SlateTheme>("slate");
                options.AddTheme<Themes.FlatlyTheme>("flatly");
                options.AddTheme<Themes.SketchyTheme>("sketchy");
                options.AddTheme<Themes.VaporTheme>("vapor");
                options.AddTheme<Themes.BriteTheme>("brite");
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.AddMauiDevFlowAgent();
#endif

        return builder.Build();
    }
}
