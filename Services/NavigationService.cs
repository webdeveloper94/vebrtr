namespace WebLab.RTR.Services;

public enum AppView
{
    Home,
    Topics,
    TopicDetail,
    Glossary,
    Quiz,
    Passport,
    Author
}

public interface INavigationService
{
    AppView CurrentView { get; }
    int? SelectedTopicId { get; }
    void NavigateTo(AppView view);
    void NavigateToTopicDetail(int topicId);
    void ShowUpcomingFeatureNotice(string sectionTitle);
}

public class NavigationService : INavigationService
{
    public AppView CurrentView { get; private set; } = AppView.Home;
    public int? SelectedTopicId { get; private set; }

    public Action<AppView>? OnViewChanged { get; set; }
    public Action<int>? OnTopicSelected { get; set; }
    public Action<string>? OnNotificationRequested { get; set; }

    public void NavigateTo(AppView view)
    {
        CurrentView = view;
        OnViewChanged?.Invoke(view);
    }

    public void NavigateToTopicDetail(int topicId)
    {
        SelectedTopicId = topicId;
        CurrentView = AppView.TopicDetail;
        OnTopicSelected?.Invoke(topicId);
        OnViewChanged?.Invoke(CurrentView);
    }

    public void ShowUpcomingFeatureNotice(string sectionTitle)
    {
        OnNotificationRequested?.Invoke(sectionTitle);
    }
}
