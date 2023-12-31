﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Componentizer;

namespace SampleApp;

public partial class MainViewModel : ObservableObject
{
    private readonly IComponentNavigation _componentNavigation;

    private bool _goToA = false;

    public MainViewModel(IComponentNavigation componentNavigation)
    {
        _componentNavigation = componentNavigation;
    }

    [RelayCommand]
    private async Task NavigateForwardAsync()
    {
        _goToA = !_goToA;

        if (_goToA)
        {
            await _componentNavigation.NavigateToAsync<SampleAViewModel>(ComponentNames.MainComponent);
            return;
        }

        await _componentNavigation.NavigateToAsync<SampleBViewModel>(
            ComponentNames.MainComponent,
            new Dictionary<string, object>
            {
                { "Value", DateTimeOffset.Now.ToUnixTimeMilliseconds() },
            });
    }

    [RelayCommand]
    private async Task NavigateBackwardAsync()
    {
        await _componentNavigation.NavigatePopAsync(ComponentNames.MainComponent);
    }
}
