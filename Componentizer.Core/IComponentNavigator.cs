namespace Componentizer;

public interface IComponentNavigator
{
    string ComponentName { get; }

    List<Type> ViewModelTypes { get; }

    Task NavigateToAsync<T>(T view, Type viewModelType, bool animated = true)
         where T : class;

    Task NavigatePopAsync(bool animated = true);

    Task NavigatePopToAsync<TViewModel>(bool animated = true);

    Task NavigatePopToRootAsync(bool animated = true);

    void ApplyQueryParameters<T>(T view, IDictionary<string, object> query)
        where T : class;

    Task<(bool CanNavigate, bool Prompt, string? PromptTitle, string? PromptMessage)> CanBackNavigateAsync();
}
