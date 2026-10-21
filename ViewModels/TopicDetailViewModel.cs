using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WebLab.RTR.Models;
using WebLab.RTR.Services;

namespace WebLab.RTR.ViewModels;

public enum TopicSubView
{
    Dashboard,    // 6 ta resurs kartalari ro'yxati
    Lecture,      // Ma'ruza matni to'liq ekranda
    Methods,      // O'qitish metodlari
    Video,        // Media va Video dars
    Presentation, // Taqdimot materiallari
    Assessment,   // Baholash va Nazorat testlari
    Glossary      // Mavzuga oid Glossariy (atamalar lug'ati)
}

public partial class TopicDetailViewModel : ObservableObject
{
    private readonly ITopicService _topicService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private Topic? _topic;

    [ObservableProperty]
    private TopicSubView _currentSubView = TopicSubView.Dashboard;

    [ObservableProperty]
    private ObservableCollection<QuizQuestion> _questions = new();

    [ObservableProperty]
    private bool _hasQuiz;

    // Reader Mode (Shrift o'lchami va fokus)
    [ObservableProperty]
    private double _lectureFontSize = 18.0; // Calibri 14pt taxminan 18-19px

    public TopicDetailViewModel() : this(new TopicService(), new NavigationService())
    {
    }

    public TopicDetailViewModel(ITopicService topicService, INavigationService navigationService)
    {
        _topicService = topicService;
        _navigationService = navigationService;
    }

    public async Task LoadTopicAsync(int topicId)
    {
        var item = await _topicService.GetTopicByIdAsync(topicId);
        if (item != null)
        {
            Topic = item;
            CurrentSubView = TopicSubView.Dashboard;
            LectureFontSize = 18.0;
            SetupQuizForTopic(item);
        }
    }

    private void SetupQuizForTopic(Topic topic)
    {
        Questions.Clear();
        Questions.Add(new QuizQuestion
        {
            TopicId = topic.Id,
            QuestionText = $"«{topic.Title}» mavzusining asosiy maqsadi nimadan iborat?",
            OptionA = "Nazariy qoidalarni amaliyotda to'g'ri qo'llash",
            OptionB = "Faqatgina ma'lumotlarni yodlab olish",
            OptionC = "Dizayn parametrlarini hisobga olmaslik",
            OptionD = "Boshqa texnologiyalardan butunlay voz kechish",
            CorrectOptionIndex = 0,
            Explanation = "Har bir modul mavzusi aniq amaliy o'quv natijalariga yo'naltirilgan bo'lishi lozim."
        });

        Questions.Add(new QuizQuestion
        {
            TopicId = topic.Id,
            QuestionText = "Mazkur mavzuni o'rganishda tavsiya etilgan asosiy o'qitish metodi qaysi?",
            OptionA = "Passiv ma'ruza eshitish",
            OptionB = string.IsNullOrWhiteSpace(topic.TeachingMethods) ? "Interfaol amaliy keys-stadi va muammoli ta'lim" : topic.TeachingMethods,
            OptionC = "Mustaqil nazoratsiz o'rganish",
            OptionD = "Faqat test yechish",
            CorrectOptionIndex = 1,
            Explanation = "Kasbiy ta'limda interfaol metodlar (muammoli ta'lim, loyiha ishi) eng yuqori samaradorlik beradi."
        });

        HasQuiz = Questions.Count > 0;
    }

    [RelayCommand]
    private void IncreaseFontSize()
    {
        if (LectureFontSize < 28)
            LectureFontSize += 2;
    }

    [RelayCommand]
    private void DecreaseFontSize()
    {
        if (LectureFontSize > 14)
            LectureFontSize -= 2;
    }

    [RelayCommand]
    private void ResetFontSize()
    {
        LectureFontSize = 18.0;
    }

    [RelayCommand]
    private void SelectQuizOption(string param)
    {
        // param: "questionIndex,optionIndex"
        var parts = param.Split(',');
        if (parts.Length == 2 && int.TryParse(parts[0], out int qIdx) && int.TryParse(parts[1], out int optIdx))
        {
            if (qIdx >= 0 && qIdx < Questions.Count)
            {
                var q = Questions[qIdx];
                q.SelectedOptionIndex = optIdx;
            }
        }
    }

    [RelayCommand]
    private void CheckQuizQuestion(QuizQuestion question)
    {
        if (question.SelectedOptionIndex.HasValue)
        {
            question.IsChecked = true;
            question.IsCorrect = (question.SelectedOptionIndex.Value == question.CorrectOptionIndex);
        }
    }

    [RelayCommand]
    private void ResetQuizQuestion(QuizQuestion question)
    {
        question.SelectedOptionIndex = null;
        question.IsChecked = false;
        question.IsCorrect = false;
    }

    [RelayCommand]
    private void OpenResource(string resourceName)
    {
        CurrentSubView = resourceName switch
        {
            "Lecture" => TopicSubView.Lecture,
            "Methods" => TopicSubView.Methods,
            "Video" => TopicSubView.Video,
            "Presentation" => TopicSubView.Presentation,
            "Assessment" => TopicSubView.Assessment,
            "Glossary" => TopicSubView.Glossary,
            _ => TopicSubView.Dashboard
        };
    }

    [RelayCommand]
    private void BackToDashboard()
    {
        CurrentSubView = TopicSubView.Dashboard;
    }

    [RelayCommand]
    private void BackToTopics()
    {
        _navigationService.NavigateTo(AppView.Topics);
    }
}
