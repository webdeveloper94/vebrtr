using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WebLab.RTR.Services;

namespace WebLab.RTR.ViewModels;

public partial class PassportViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    // Resurs pasporti ma'lumotlari (Nizom SLIDE 2 va SLIDE 4 talablari asosida)
    [ObservableProperty]
    private string _courseName = "WEB dasturlash va dizayn";

    [ObservableProperty]
    private string _professionName = "30610101 — Raqamli axborotlarni qayta ishlash ustasi";

    [ObservableProperty]
    private string _moduleCode = "RTK-WEB-30610101";

    [ObservableProperty]
    private int _totalHours = 80;

    [ObservableProperty]
    private int _lectureHours = 34;

    [ObservableProperty]
    private int _practiceHours = 46;

    [ObservableProperty]
    private int _independentHours = 0;

    [ObservableProperty]
    private string _learningOutcomes = 
        "1. Zamonaviy semantik HTML5 standartlari asosida to'g'ri veb-sahifalar yaratish ko'nikmasiga ega bo'lish.\n" +
        "2. CSS (Flexbox, Grid) yordamida har qanday murakkablikdagi adaptiv (responsive) maketlar tuzish.\n" +
        "3. JavaScript yordamida interaktivlik, DOM bilan ishlash va asinxron REST API ulanishlarini amalga oshirish.\n" +
        "4. UI/UX ergonomika tamoyillari va dizayn tizimlari asosida foydalanuvchiga qulay interfeys loyihalash.\n" +
        "5. Git/GitHub versiya nazorati va zamonaviy xosting platformalarida resurslarni joylashtirishni bilish.";

    [ObservableProperty]
    private string _authorsInfo = 
        "Muallif: WebLab RTR Ishchi guruhi\n" +
        "Ta'lim tashkiloti: Kasb-hunar maktabi / Kollej / Texnikum\n" +
        "Murojaat uchun: support@weblab.uz | +998 (71) 200-00-00\n" +
        "Respublika ko'rik-tanlovi: 2026-yil";

    public PassportViewModel() : this(new NavigationService())
    {
    }

    public PassportViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void BackToHome()
    {
        _navigationService.NavigateTo(AppView.Home);
    }
}
