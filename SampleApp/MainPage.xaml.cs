using Componentizer;

namespace SampleApp;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel, IComponentNavigation componentNavigation)
    {
        this.BindingContext = this._viewModel = viewModel;
        this.InitializeComponent();

        componentNavigation.RegisterNavigationComponent(this.ComponentNav);
    }

    protected override async void OnAppearing()
    {
        await _viewModel.NavigateForwardCommand.ExecuteAsync(null);
    }
}
