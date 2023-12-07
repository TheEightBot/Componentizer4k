namespace Componentizer;

public interface IPreventBackNavigation
{
    Task<bool> CanBackNavigateAsync();

    string NavigationStoppedTitle { get; }

    string NavigationStoppedMessage { get; }
}