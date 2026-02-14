using MauiBootstrapTheme.Extensions;
using MauiDevFlow.Agent;

namespace MauiBootstrapTheme.Sample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            // Register Bootstrap platform handlers (theme is set in App.xaml via ResourceDictionary)
            .UseBootstrapTheme()
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
