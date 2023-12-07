using CommunityToolkit.Mvvm.ComponentModel;
using Componentizer;

namespace SampleApp;

public class SampleBViewModel : ObservableObject, IComponentQueryAttributable
{
    private readonly IComponentNavigation _componentNavigation;

    public SampleBViewModel(IComponentNavigation componentNavigation)
    {
        _componentNavigation = componentNavigation;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        System.Console.WriteLine($"SampleBViewModel: {query}");
    }
}
