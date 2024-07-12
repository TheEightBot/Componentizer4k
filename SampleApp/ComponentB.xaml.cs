using Componentizer;

namespace SampleApp;

public partial class ComponentB : ContentView, IComponentNavigationAware, IQueryAttributable
{
    public ComponentB(SampleBViewModel viewModel)
    {
        this.BindingContext = viewModel;

        this.InitializeComponent();
    }

    public Task NavigatedFromAsync() => Task.CompletedTask;

    public Task NavigatedToAsync() => Task.CompletedTask;

    public Task PoppedAsync() => Task.CompletedTask;

    public Task PoppedBackToAsync() => Task.CompletedTask;

    public void ApplyQueryAttributes(IDictionary<string, object> query) => Console.WriteLine($"Component B: {query}");
}
