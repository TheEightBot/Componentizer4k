namespace Componentizer;

public class ComponentNavigator : Grid, IComponentNavigator
{
    public string ComponentName { get; set; } = string.Empty;

    private readonly List<View> _viewStack = new();
    private readonly List<Type> _viewModelTypes = new();

    public static readonly BindableProperty CurrentContentProperty =
        BindableProperty.Create(
            nameof(CurrentContent),
            typeof(View),
            typeof(ComponentNavigator),
            default,
            propertyChanged:
                (bindable, oldValue, newValue) =>
                {
                    if (bindable is not ComponentNavigator cn)
                    {
                        return;
                    }
                });

    public View CurrentContent
    {
        get => (View)GetValue(CurrentContentProperty);
        private set => SetValue(CurrentContentProperty, value);
    }

    public string CurrentComponentTitle
    {
        get => (string)GetValue(CurrentComponentTitleProperty);
        private set => SetValue(CurrentComponentTitleProperty, value);
    }

    public static readonly BindableProperty CurrentComponentTitleProperty =
        BindableProperty.Create(
            nameof(CurrentComponentTitle),
            typeof(string),
            typeof(ComponentNavigator),
            default);

    public List<View> ViewStack
    {
        get => _viewStack;
    }

    public List<Type> ViewModelTypes => _viewModelTypes;

    public Task NavigateToAsync<TView>(TView view, Type viewModelType, bool animated = true)
        where TView : class
    {
        if (view is not View mauiView)
        {
            return Task.CompletedTask;
        }

        _viewModelTypes.Add(viewModelType);

        return NavigateToAsync(mauiView, animated);
    }

    private async Task NavigateToAsync(View newView, bool animated = true)
    {
        var currentContent = CurrentContent;

        newView.TranslationX = this.Width;
        newView.ZIndex = 0;
        newView.Opacity = 0;
        this.Add(newView, 0, 0);

        if (animated)
        {
            var animations = new List<Task>();

            if (currentContent is not null)
            {
                currentContent.ZIndex = 100;

                animations.Add(currentContent.TranslateTo(-this.Width, 0, 400, Easing.CubicIn));
                animations.Add(currentContent.FadeTo(0, 400, Easing.CubicIn));
            }

            animations.Add(newView.TranslateTo(0, 0, 400, Easing.CubicInOut));
            animations.Add(newView.FadeTo(1, 400, Easing.CubicInOut));

            await Task.WhenAll(animations);
        }

        newView.TranslationX = 0d;
        newView.ZIndex = 100;
        newView.Opacity = 1d;

        if (currentContent is not null)
        {
            this.Remove(currentContent);
        }

        _viewStack.Add(newView);

        if (currentContent is not null)
        {
            if (currentContent is IComponentNavigatorAware cna)
            {
                await cna.NavigatedFromAsync();
            }
        }

        if (newView is IComponentNavigatorAware newCna)
        {
            await newCna.NavigatedToAsync();
        }

        CurrentContent = newView;
    }

    public Task NavigatePopAsync(bool animated = true)
    {
        var currentIndex = _viewModelTypes.Count - 1;
        return PopToIndex(currentIndex - 1, animated);
    }

    public async Task<(bool CanNavigate, bool Prompt, string? PromptTitle, string? PromptMessage)> CanBackNavigateAsync()
    {
        var currentContent = CurrentContent;

        var previousContent = _viewStack.ElementAtOrDefault(_viewStack.IndexOf(currentContent) - 1);

        if (currentContent is IPreventBackNavigation pbn)
        {
            var canNavigateResult = await pbn.CanBackNavigateAsync();

            return (canNavigateResult, true, pbn.NavigationStoppedTitle, pbn.NavigationStoppedMessage);
        }

        return (previousContent is not null, false, null, null);
    }

    public Task NavigatePopToAsync<TViewModel>(bool animated = true)
    {
        var currentIndex = _viewModelTypes.Count - 1;

        // one less to prevent matching current page
        for (int i = currentIndex - 1; i >= 0; i--)
        {
            if (_viewModelTypes[i].Equals(typeof(TViewModel)))
            {
                return PopToIndex(i, animated);
            }
        }

        return Task.CompletedTask;
    }

    public Task NavigatePopToRootAsync(bool animated = true)
    {
        return PopToIndex(0, animated);
    }

    private async Task PopToIndex(int indexToPopTo, bool animated = true)
    {
        if (CurrentContent is null)
        {
            return;
        }

        var currentContent = CurrentContent;

        var previousContent = _viewStack.ElementAtOrDefault(indexToPopTo);

        if (previousContent is null || currentContent == previousContent)
        {
            return;
        }

        await NavigatePopToAsync(currentContent, previousContent, animated);

        // difference of stack index and index to pop to
        var totalNumberOfPagesToRemove = (_viewStack.Count - 1) - indexToPopTo;
        var viewsToRemove = _viewStack.GetRange(indexToPopTo + 1, totalNumberOfPagesToRemove);
        viewsToRemove.Reverse();
        foreach (var view in viewsToRemove)
        {
            _viewStack.Remove(view);
            _viewModelTypes.RemoveAt(_viewModelTypes.Count - 1);
            if (currentContent is IComponentNavigatorAware cna)
            {
                await cna.PoppedAsync();
            }
        }

        if (previousContent is IComponentNavigatorAware newCna)
        {
            await newCna.PoppedBackToAsync();
        }

        CurrentContent = previousContent;
    }

    private async Task NavigatePopToAsync(View currentContent, View previousContent, bool animated = true)
    {
        currentContent.ZIndex = 100;

        previousContent.ZIndex = 0;
        previousContent.Opacity = 0;
        previousContent.TranslationX = -this.Width;

        this.Add(previousContent);

        if (animated)
        {
            var animations =
                new List<Task>
                {
                    currentContent.TranslateTo(this.Width, 0, 400, Easing.CubicIn),
                    currentContent.FadeTo(0, 400, Easing.CubicIn),

                    previousContent.TranslateTo(0, 0, 400, Easing.CubicInOut),
                    previousContent.FadeTo(1, 400, Easing.CubicInOut)
                };

            await Task.WhenAll(animations);
        }

        previousContent.ZIndex = 100;
        previousContent.Opacity = 1d;
        previousContent.TranslationX = 0d;

        this.Remove(currentContent);
    }
}