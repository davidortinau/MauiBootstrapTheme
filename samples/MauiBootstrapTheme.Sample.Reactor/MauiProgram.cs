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
            // Register Bootstrap platform handlers (theme is set in App.xaml via ResourceDictionary)
            .UseBootstrapTheme()
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
