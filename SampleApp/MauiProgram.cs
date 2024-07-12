using Componentizer;
using Microsoft.Extensions.Logging;

namespace SampleApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
            .RegisterViews()
            .RegisterViewModels()
            .RegisterComponents(
                componentNavigation =>
                {
                    componentNavigation.RegisterView<SampleAViewModel, ComponentA>();
                    componentNavigation.RegisterView<SampleBViewModel, ComponentB>();
                });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<ComponentA>();
        builder.Services.AddTransient<ComponentB>();

        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<SampleAViewModel>();
        builder.Services.AddTransient<SampleBViewModel>();

        return builder;
    }
}
