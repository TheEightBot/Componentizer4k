namespace Componentizer;

public static class MauiBuilderExtensions
{
    public static MauiAppBuilder RegisterComponents(this MauiAppBuilder builder, Action<IComponentNavigation> registerAction)
	{
        builder.Services.AddSingleton<IComponentNavigation>(
            serviceProvider =>
            {
                var componentNavigation = new ComponentNavigation(serviceProvider);

                registerAction.Invoke(componentNavigation);

                return componentNavigation;
            });

        return builder;
    }
}

