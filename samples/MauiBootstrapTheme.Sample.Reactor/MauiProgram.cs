using MauiBootstrapTheme.Extensions;
using MauiReactor;
using Microsoft.Extensions.Logging;
using MauiDevFlow.Agent;

namespace MauiBootstrapTheme.Sample.Reactor;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiReactorApp<MainPage>()
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
        builder.Logging.AddDebug();
        builder.AddMauiDevFlowAgent();
#endif

        return builder.Build();
    }
}
