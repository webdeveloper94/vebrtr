using CommunityToolkit.Mvvm.ComponentModel;

namespace WebLab.RTR.Models;

public partial class QuizQuestion : ObservableObject
{
    public int Id { get; set; }
    public int TopicId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;
    public int CorrectOptionIndex { get; set; } // 0: A, 1: B, 2: C, 3: D
    public string? Explanation { get; set; }

    // Interaktiv tekshirish holatlari
    [ObservableProperty]
    private int? _selectedOptionIndex; // null: hali tanlanmadi, 0, 1, 2, 3

    [ObservableProperty]
    private bool _isChecked;

    [ObservableProperty]
    private bool _isCorrect;
}
