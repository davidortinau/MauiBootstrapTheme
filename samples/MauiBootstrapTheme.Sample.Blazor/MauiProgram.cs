using Microsoft.Extensions.Logging;
using MauiDevFlow.Agent;
using MauiDevFlow.Blazor;

namespace MauiBootstrapTheme.Sample.Blazor;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
        builder.AddMauiDevFlowAgent();
        builder.AddMauiBlazorDevFlowTools();
#endif

        return builder.Build();
    }
}
