using CommunityToolkit.Mvvm.ComponentModel;
using Componentizer;

namespace SampleApp;

public partial class SampleAViewModel : ObservableObject, IComponentNavigationAware
{
    private readonly IComponentNavigation _componentNavigation;

    [ObservableProperty]
    private string _currentTime;

    public SampleAViewModel(IComponentNavigation componentNavigation) =>
        this._componentNavigation = componentNavigation;

    public Task NavigatedFromAsync()
    {
        Console.WriteLine("Navigated From");
        return Task.CompletedTask;
    }

    public Task NavigatedToAsync()
    {
        CurrentTime = $"Navigated to @ {DateTime.Now:F}";
        Console.WriteLine("Navigated To");
        return Task.CompletedTask;
    }

    public Task PoppedAsync()
    {
        Console.WriteLine("Popped");
        return Task.CompletedTask;
    }

    public Task PoppedBackToAsync()
    {
        Console.WriteLine("Popped Back To");
        return Task.CompletedTask;
    }
}
