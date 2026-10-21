using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WebLab.RTR.Models;
using WebLab.RTR.Services;

namespace WebLab.RTR.ViewModels;

public enum TopicSortOption
{
    OrderNumber,
    Title,
    Duration
}

public partial class TopicsViewModel : ObservableObject
{
    private readonly ITopicService _topicService;
    private readonly INavigationService _navigationService;
    private List<Topic> _allTopics = new();

    [ObservableProperty]
    private ObservableCollection<Topic> _filteredTopics = new();

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private string _selectedCategory = "Barchasi";

    [ObservableProperty]
    private TopicSortOption _selectedSortOption = TopicSortOption.OrderNumber;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _hasError = false;

    [ObservableProperty]
    private bool _hasNoTopics = false;

    [ObservableProperty]
    private Topic? _selectedTopic;

    [ObservableProperty]
    private bool _isDetailModalOpen = false;

    // Category Counts
    [ObservableProperty]
    private int _allCount;

    [ObservableProperty]
    private int _htmlCount;

    [ObservableProperty]
    private int _cssCount;

    [ObservableProperty]
    private int _jsCount;

    [ObservableProperty]
    private int _designCount;

    public TopicsViewModel(ITopicService topicService, INavigationService navigationService)
    {
        _topicService = topicService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    public async Task LoadTopicsAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            HasError = false;

            await _topicService.InitializeDatabaseAsync();
            _allTopics = await _topicService.GetTopicsAsync();

            AllCount = _allTopics.Count;
            HtmlCount = _allTopics.Count(t => t.Category == "HTML");
            CssCount = _allTopics.Count(t => t.Category == "CSS");
            JsCount = _allTopics.Count(t => t.Category == "JavaScript");
            DesignCount = _allTopics.Count(t => t.Category == "Dizayn");

            ApplyFilterAndSort();
        }
        catch (Exception ex)
        {
            ErrorMessage = "Ma’lumotlar bazasi bilan bog‘lanishda xatolik yuz berdi. Iltimos, dasturni qayta ishga tushiring.";
            HasError = true;
            System.Diagnostics.Debug.WriteLine($"[WebLab Error] DB Load Exception: {ex}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    partial void OnSearchQueryChanged(string value) => ApplyFilterAndSort();
    partial void OnSelectedCategoryChanged(string value) => ApplyFilterAndSort();
    partial void OnSelectedSortOptionChanged(TopicSortOption value) => ApplyFilterAndSort();

    [RelayCommand]
    private void SelectCategory(string category)
    {
        SelectedCategory = category;
    }

    [RelayCommand]
    private void SetSort(string sortKey)
    {
        SelectedSortOption = sortKey switch
        {
            "Title" => TopicSortOption.Title,
            "Duration" => TopicSortOption.Duration,
            _ => TopicSortOption.OrderNumber
        };
    }

    private void ApplyFilterAndSort()
    {
        var query = _allTopics.AsEnumerable();

        // 1. Kategoriya filtri
        if (!string.Equals(SelectedCategory, "Barchasi", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Where(t => string.Equals(t.Category, SelectedCategory, StringComparison.OrdinalIgnoreCase));
        }

        // 2. Qidiruv so'zi bo'yicha filter
        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            var search = SearchQuery.Trim().ToLower();
            query = query.Where(t =>
                t.Title.ToLower().Contains(search) ||
                t.Description.ToLower().Contains(search) ||
                t.Category.ToLower().Contains(search) ||
                t.OrderNumber.ToString().Contains(search));
        }

        // 3. Saralash
        query = SelectedSortOption switch
        {
            TopicSortOption.Title => query.OrderBy(t => t.Title),
            TopicSortOption.Duration => query.OrderBy(t => t.DurationMinutes),
            _ => query.OrderBy(t => t.OrderNumber)
        };

        var list = query.ToList();
        FilteredTopics = new ObservableCollection<Topic>(list);
        HasNoTopics = list.Count == 0 && !IsLoading && !HasError;
    }

    [RelayCommand]
    private void OpenTopicDetail(Topic topic)
    {
        SelectedTopic = topic;
        _navigationService.NavigateToTopicDetail(topic.Id);
    }

    [RelayCommand]
    private void CloseTopicDetail()
    {
        IsDetailModalOpen = false;
        SelectedTopic = null;
    }

    [RelayCommand]
    private void NavigateBack()
    {
        _navigationService.NavigateTo(AppView.Home);
    }
}
