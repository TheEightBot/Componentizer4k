using Componentizer;

namespace SampleApp;

public partial class MainPage : ContentPage
{
    MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel, IComponentNavigation componentNavigation)
	{
        BindingContext = _viewModel = viewModel;
		InitializeComponent();

        componentNavigation.RegisterNavigationComponent(ComponentNav);
	}
}


