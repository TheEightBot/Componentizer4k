using Componentizer;

namespace SampleApp;

public partial class ComponentA : ContentView, IComponentNavigationAware
{
    public ComponentA(SampleAViewModel viewModel)
    {
        this.BindingContext = viewModel;

        this.InitializeComponent();
    }

    public Task NavigatedFromAsync() => Task.CompletedTask;

    public Task NavigatedToAsync() => Task.CompletedTask;

    public Task PoppedAsync() => Task.CompletedTask;

    public Task PoppedBackToAsync() => Task.CompletedTask;
}
