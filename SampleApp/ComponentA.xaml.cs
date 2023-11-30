using Componentizer;

namespace SampleApp;

public partial class ComponentA : ContentView, IComponentNavigatorAware
{
    public ComponentA(SampleBViewModel viewModel)
    {
        BindingContext = viewModel;

        InitializeComponent();
    }

    public Task NavigatedFromAsync()
    {
        return Task.CompletedTask;
    }

    public Task NavigatedToAsync()
    {
        return Task.CompletedTask;
    }

    public Task PoppedAsync()
    {
        return Task.CompletedTask;
    }

    public Task PoppedBackToAsync()
    {
        return Task.CompletedTask;
    }
}
