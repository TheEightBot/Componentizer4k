using Componentizer;

namespace SampleApp;

public partial class ComponentB : ContentView, IComponentNavigatorAware, IComponentQueryAttributable
{
    public ComponentB(SampleBViewModel viewModel)
    {
        BindingContext = viewModel;

        InitializeComponent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        System.Console.WriteLine($"Component B: {query}");
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
