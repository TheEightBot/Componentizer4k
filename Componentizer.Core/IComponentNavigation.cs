namespace Componentizer;

public interface IComponentNavigation
{
    void RegisterView<TViewModel, TView>();

    void RegisterNavigationComponent(IComponentNavigator componentNavigator);

    void UnregisterNavigationComponent(IComponentNavigator componentNavigator);

    Task NavigateToAsync<T>(string componentName, IDictionary<string, object>? query = null, bool animated = true);

    Task NavigatePopAsync(string componentName, bool animated = true);

    Task NavigatePopToAsync<T>(string componentName, bool animated = true);

    Task NavigatePopToRootAsync(string componentName, bool animated = true);

    Task<(bool CanNavigate, bool Prompt, string? PromptTitle, string? PromptMessage)> CanBackNavigateAsync(string componentName);

    Type? GetTypeOfCurrentContent(string componentName);
}
