using CommunityToolkit.Mvvm.ComponentModel;
using Componentizer;

namespace SampleApp;

public class SampleBViewModel : ObservableObject
{
    private readonly IComponentNavigation _componentNavigation;

    public SampleBViewModel(IComponentNavigation componentNavigation)
	{
        _componentNavigation = componentNavigation;
    }
}