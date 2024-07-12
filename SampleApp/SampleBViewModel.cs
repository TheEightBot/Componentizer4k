using CommunityToolkit.Mvvm.ComponentModel;
using Componentizer;

namespace SampleApp;

public partial class SampleBViewModel : ObservableObject, IQueryAttributable
{
    private readonly IComponentNavigation _componentNavigation;

    [ObservableProperty]
    private string _queryParameter;

    public SampleBViewModel(IComponentNavigation componentNavigation) =>
        this._componentNavigation = componentNavigation;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        QueryParameter = string.Join(',', query.Select(kvp => $"{kvp.Key} - {kvp.Value}"));
    }
}
