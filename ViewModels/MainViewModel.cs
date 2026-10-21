using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WebLab.RTR.Services;

namespace WebLab.RTR.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly ITopicService _topicService;
    private DispatcherTimer? _notificationTimer;

    [ObservableProperty]
    private AppView _currentView = AppView.Home;

    [ObservableProperty]
    private TopicsViewModel _topicsVM;

    [ObservableProperty]
    private TopicDetailViewModel _topicDetailVM;

    [ObservableProperty]
    private GlossaryViewModel _glossaryVM;

    [ObservableProperty]
    private PassportViewModel _passportVM;

    [ObservableProperty]
    private string _notificationMessage = string.Empty;

    [ObservableProperty]
    private bool _isNotificationVisible;

    [ObservableProperty]
    private string _notificationSectionTitle = string.Empty;

    public MainViewModel() : this(new NavigationService(), new TopicService())
    {
    }

    public MainViewModel(INavigationService navigationService, ITopicService topicService)
    {
        _navigationService = navigationService;
        _topicService = topicService;

        _topicsVM = new TopicsViewModel(_topicService, _navigationService);
        _topicDetailVM = new TopicDetailViewModel(_topicService, _navigationService);
        _glossaryVM = new GlossaryViewModel(_navigationService);
        _passportVM = new PassportViewModel(_navigationService);

        if (_navigationService is NavigationService navSvc)
        {
            navSvc.OnViewChanged = view =>
            {
                CurrentView = view;
                if (view == AppView.Topics)
                {
                    _ = _topicsVM.LoadTopicsAsync();
                }
                else if (view == AppView.Glossary)
                {
                    _ = _glossaryVM.LoadGlossaryAsync();
                }
            };

            navSvc.OnTopicSelected = topicId =>
            {
                _ = _topicDetailVM.LoadTopicAsync(topicId);
            };

            navSvc.OnNotificationRequested = HandleNotificationRequest;
        }

        // Initialize SQLite Database in background on app startup
        _ = _topicService.InitializeDatabaseAsync();
    }

    private void HandleNotificationRequest(string sectionTitle)
    {
        NotificationSectionTitle = sectionTitle;
        NotificationMessage = "Bu bo‘lim ma’lumotlari tez orada to‘ldiriladi.";
        IsNotificationVisible = true;

        _notificationTimer?.Stop();
        _notificationTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(3.5)
        };
        _notificationTimer.Tick += (s, e) =>
        {
            IsNotificationVisible = false;
            _notificationTimer.Stop();
        };
        _notificationTimer.Start();
    }

    [RelayCommand]
    private void NavigateToTopics()
    {
        _navigationService.NavigateTo(AppView.Topics);
    }

    [RelayCommand]
    private void NavigateToGlossary()
    {
        _navigationService.NavigateTo(AppView.Glossary);
    }

    [RelayCommand]
    private void NavigateToPassport()
    {
        _navigationService.NavigateTo(AppView.Passport);
    }

    [RelayCommand]
    private void NavigateToAuthor()
    {
        NavigateToPassport();
    }

    [RelayCommand]
    private void NavigateToHome()
    {
        _navigationService.NavigateTo(AppView.Home);
    }

    [RelayCommand]
    private void DismissNotification()
    {
        IsNotificationVisible = false;
        _notificationTimer?.Stop();
    }
}
