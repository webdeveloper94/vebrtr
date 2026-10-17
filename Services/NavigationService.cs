namespace WebLab.RTR.Services;

public enum AppView
{
    Home,
    Topics,
    Author
}

public interface INavigationService
{
    AppView CurrentView { get; }
    void NavigateTo(AppView view);
    void ShowUpcomingFeatureNotice(string sectionTitle);
}

public class NavigationService : INavigationService
{
    public AppView CurrentView { get; private set; } = AppView.Home;
    public Action<AppView>? OnViewChanged { get; set; }
    public Action<string>? OnNotificationRequested { get; set; }

    public void NavigateTo(AppView view)
    {
        CurrentView = view;
        OnViewChanged?.Invoke(view);
    }

    public void ShowUpcomingFeatureNotice(string sectionTitle)
    {
        OnNotificationRequested?.Invoke(sectionTitle);
    }
}
