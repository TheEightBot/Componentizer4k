namespace Componentizer;

public interface IComponentNavigation
{
    void RegisterView<TViewModel, TView>();

    void RegisterNavigationComponent(IComponentNavigator componentNavigator);

    void UnregisterNavigationComponent(IComponentNavigator componentNavigator);

    Task PushAsync<T>(string componentName, IDictionary<string, object>? query = null, bool animated = true);

    Task PopAsync(string componentName, bool animated = true);

    Task PopToAsync<T>(string componentName, bool animated = true);

    Task PopToRootAsync(string componentName, bool animated = true);

    Task<(bool CanNavigate, bool Prompt, string? PromptTitle, string? PromptMessage)> CanBackNavigateAsync(string componentName);

    Type? GetTypeOfCurrentContent(string componentName);
}
