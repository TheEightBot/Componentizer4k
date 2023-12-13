using CommunityToolkit.Mvvm.ComponentModel;
using Componentizer;

namespace SampleApp;

public class SampleAViewModel : ObservableObject, IComponentNavigationAware
{
    private readonly IComponentNavigation _componentNavigation;

    public SampleAViewModel(IComponentNavigation componentNavigation)
    {
        _componentNavigation = componentNavigation;
    }

    public Task NavigatedFromAsync()
    {
        Console.WriteLine("Navigated From");
        return Task.CompletedTask;
    }

    public Task NavigatedToAsync()
    {
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
