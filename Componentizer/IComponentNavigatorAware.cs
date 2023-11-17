namespace Componentizer;

public interface IComponentNavigatorAware
{
    /// <summary>
    /// Fires when originally 'pushed' onto the component navigator.
    /// </summary>
    /// <returns></returns>
    Task NavigatedToAsync();

    /// <summary>
    /// Fires when another component is pushed on top of the component via the component navigator and the component is no longer visible.
    /// </summary>
    /// <returns></returns>
    Task NavigatedFromAsync();

    /// <summary>
    /// Fires when all components on top are removed and the component reappears.
    /// </summary>
    /// <returns></returns>
    Task PoppedBackToAsync();

    /// <summary>
    /// Fires when component is 'removed' from the stack via the component navigator.
    /// </summary>
    /// <returns></returns>
    Task PoppedAsync();
}