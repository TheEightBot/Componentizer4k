using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Componentizer;

namespace SampleApp;

public partial class MainViewModel : ObservableObject
{
    private readonly IComponentNavigation _componentNavigation;

    private bool _goToA;

    public MainViewModel(IComponentNavigation componentNavigation) => this._componentNavigation = componentNavigation;

    [RelayCommand]
    private async Task NavigateForwardAsync()
    {
        this._goToA = !this._goToA;

        if (this._goToA)
        {
            await this._componentNavigation.PushAsync<SampleAViewModel>(ComponentNames.MainComponent);
            return;
        }

        await this._componentNavigation.PushAsync<SampleBViewModel>(
            ComponentNames.MainComponent,
            new Dictionary<string, object>
            {
                { "Value", DateTimeOffset.Now.ToUnixTimeMilliseconds() },
            });
    }

    [RelayCommand]
    private async Task NavigateBackwardAsync() =>
        await this._componentNavigation.PopAsync(ComponentNames.MainComponent);
}
