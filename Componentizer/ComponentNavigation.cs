using System.Threading.RateLimiting;

namespace Componentizer;

public class ComponentNavigation : IComponentNavigation
{
    private readonly IServiceProvider _serviceProvider;

    private readonly Dictionary<Type, Type> _registeredViews = new();

    private readonly Dictionary<string, IComponentNavigator> _componentNavigators = new();

    private readonly RateLimiter _navigationLimiter =
        new ConcurrencyLimiter(
            new ConcurrencyLimiterOptions
            {
                PermitLimit = 1,
                QueueLimit = int.MaxValue,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            });

    public ComponentNavigation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task NavigateToAsync<T>(string componentName, IDictionary<string, object?>? navigationParameters = null, bool animated = true)
    {
        using var _ = await _navigationLimiter.AcquireAsync();

        if (!_componentNavigators.TryGetValue(componentName, out var navigator))
        {
            return;
        }

        if (!_registeredViews.TryGetValue(typeof(T), out var viewType))
        {
            return;
        }

        var view = _serviceProvider.GetService(viewType);

        if (view is null)
        {
            return;
        }

        if (navigationParameters is not null)
        {
            if (view is IQueryAttributable vqav)
            {
                vqav.ApplyQueryAttributes(navigationParameters);
            }

            if (view is BindableObject bo && bo.BindingContext is IQueryAttributable boqav)
            {
                boqav.ApplyQueryAttributes(navigationParameters);
            }
        }

        await navigator.NavigateToAsync(view, typeof(T), animated);
    }

    public async Task NavigatePopAsync(string componentName, bool animated = true)
    {
        using var _ = await _navigationLimiter.AcquireAsync();

        if (!_componentNavigators.TryGetValue(componentName, out var navigator))
        {
            return;
        }

        await navigator.NavigatePopAsync(animated);
    }

    public async Task NavigatePopToAsync<T>(string componentName, bool animated = true)
    {
        using var _ = await _navigationLimiter.AcquireAsync();

        if (!_componentNavigators.TryGetValue(componentName, out var navigator))
        {
            return;
        }

        await navigator.NavigatePopToAsync<T>(animated);
    }

    public async Task NavigatePopToRootAsync(string componentName, bool animated = true)
    {
        using var _ = await _navigationLimiter.AcquireAsync();

        if (!_componentNavigators.TryGetValue(componentName, out var navigator))
        {
            return;
        }

        await navigator.NavigatePopToRootAsync(animated);
    }

    public void RegisterNavigationComponent(IComponentNavigator componentNavigator)
    {
        _componentNavigators[componentNavigator.ComponentName] = componentNavigator;
    }

    public void UnregisterNavigationComponent(IComponentNavigator componentNavigator)
    {
        if (!_componentNavigators.ContainsKey(componentNavigator.ComponentName))
        {
            return;
        }

        _componentNavigators.Remove(componentNavigator.ComponentName);
    }

    public void RegisterView<TViewModel, TView>()
    {
        var viewModelType = typeof(TViewModel);

        if (_registeredViews.ContainsKey(viewModelType))
        {
            return;
        }

        _registeredViews.Add(viewModelType, typeof(TView));
    }

    public async Task<(bool CanNavigate, bool Prompt, string? PromptTitle, string? PromptMessage)> CanBackNavigateAsync(string componentName)
    {
        if (!_componentNavigators.TryGetValue(componentName, out var navigator))
        {
            return (false, false, null, null);
        }

        return await navigator.CanBackNavigateAsync();
    }

    public Type? GetTypeOfCurrentContent(string componentName)
    {
        if (!_componentNavigators.TryGetValue(componentName, out var navigator))
        {
            return default;
        }

        return navigator.ViewModelTypes.LastOrDefault();
    }
}