using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Themes.Default;
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
            .UseBootstrapTheme<DefaultTheme>()
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
