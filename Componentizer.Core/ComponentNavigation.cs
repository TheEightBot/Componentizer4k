﻿using System.Threading.RateLimiting;

namespace Componentizer;

public class ComponentNavigation : IComponentNavigation, IDisposable
{
    private readonly IServiceProvider _serviceProvider;

    private readonly object _registeredViewLock = new();
    private readonly Dictionary<Type, Type> _registeredViews = new();

    private readonly object _componentNavigationLock = new();
    private readonly Dictionary<string, IComponentNavigator> _componentNavigators = new();

    private readonly RateLimiter _navigationLimiter =
        new ConcurrencyLimiter(
            new ConcurrencyLimiterOptions
            {
                PermitLimit = 1,
                QueueLimit = int.MaxValue,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            });

    private bool _disposedValue;

    public ComponentNavigation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PushAsync<T>(string componentName, IDictionary<string, object>? query = null, bool animated = true)
    {
        using var navigationLease = await _navigationLimiter.AcquireAsync();

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

        if (query is not null)
        {
            navigator.ApplyQueryParameters(view, query);
        }

        await navigator.NavigateToAsync(view, typeof(T), animated);
    }

    public async Task PopAsync(string componentName, bool animated = true)
    {
        using var navigationLease = await _navigationLimiter.AcquireAsync();

        if (!_componentNavigators.TryGetValue(componentName, out var navigator))
        {
            return;
        }

        await navigator.NavigatePopAsync(animated);
    }

    public async Task PopToAsync<T>(string componentName, bool animated = true)
    {
        using var navigationLease = await _navigationLimiter.AcquireAsync();

        if (!_componentNavigators.TryGetValue(componentName, out var navigator))
        {
            return;
        }

        await navigator.NavigatePopToAsync<T>(animated);
    }

    public async Task PopToRootAsync(string componentName, bool animated = true)
    {
        using var navigationLease = await _navigationLimiter.AcquireAsync();

        if (!_componentNavigators.TryGetValue(componentName, out var navigator))
        {
            return;
        }

        await navigator.NavigatePopToRootAsync(animated);
    }

    public void RegisterNavigationComponent(IComponentNavigator componentNavigator)
    {
        lock (_componentNavigationLock)
        {
            _componentNavigators[componentNavigator.ComponentName] = componentNavigator;
        }
    }

    public void UnregisterNavigationComponent(IComponentNavigator componentNavigator)
    {
        lock (_componentNavigationLock)
        {
            if (!_componentNavigators.ContainsKey(componentNavigator.ComponentName))
            {
                return;
            }

            _componentNavigators.Remove(componentNavigator.ComponentName);
        }
    }

    public void RegisterView<TViewModel, TView>()
    {
        var viewModelType = typeof(TViewModel);

        lock (_registeredViewLock)
        {
            if (_registeredViews.ContainsKey(viewModelType))
            {
                return;
            }

            _registeredViews.Add(viewModelType, typeof(TView));
        }
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

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _navigationLimiter.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
