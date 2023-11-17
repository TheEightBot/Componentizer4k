using CommunityToolkit.Mvvm.ComponentModel;
using Componentizer;

namespace SampleApp;

public class SampleAViewModel : ObservableObject
{
    private readonly IComponentNavigation _componentNavigation;

    public SampleAViewModel(IComponentNavigation componentNavigation)
    {
        _componentNavigation = componentNavigation;
    }
}