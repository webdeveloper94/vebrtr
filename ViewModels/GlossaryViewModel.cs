using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using WebLab.RTR.Models;
using WebLab.RTR.Services;

namespace WebLab.RTR.ViewModels;

public partial class GlossaryViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private List<GlossaryItem> _allGlossary = new();

    [ObservableProperty]
    private ObservableCollection<GlossaryItem> _filteredGlossary = new();

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private string _selectedCategory = "Barchasi";

    [ObservableProperty]
    private bool _isLoading = false;

    public GlossaryViewModel() : this(new NavigationService())
    {
    }

    public GlossaryViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public async Task LoadGlossaryAsync()
    {
        IsLoading = true;
        try
        {
            await using var db = new AppDbContext();
            await db.Database.EnsureCreatedAsync();

            if (!await db.GlossaryItems.AnyAsync())
            {
                // Standart boshlang'ich RTR lug'ati (WebLab bo'yicha)
                var initialItems = new List<GlossaryItem>
                {
                    new() { Term = "HTML (HyperText Markup Language)", Category = "HTML", Definition = "Gipermatnli belgilash tili bo'lib, barcha veb-sahifalarning asosi, skeletini hosil qiladi.", Example = "<!DOCTYPE html><html>...</html>" },
                    new() { Term = "CSS (Cascading Style Sheets)", Category = "CSS", Definition = "Kaskadli uslublar jadvali — veb-sahifaning tashqi ko'rinishi, dizayni va ranglarini boshqaradi.", Example = "body { background-color: #f8fafc; }" },
                    new() { Term = "JavaScript (JS)", Category = "JavaScript", Definition = "Veb-sahifalarni interfaol, jonli va foydalanuvchi harakatlariga javob beradigan qiluvchi dasturlash tili.", Example = "document.querySelector('button').addEventListener('click', ...)" },
                    new() { Term = "Semantik HTML", Category = "HTML", Definition = "Har bir teg o'zining mazmuni va vazifasiga mos nomlanishi (<header>, <nav>, <article>, <footer>).", Example = "<main> asosiy sahifa kontentini bildiradi" },
                    new() { Term = "Box Model (CSS)", Category = "CSS", Definition = "Har bir HTML elementini o'rab turuvchi to'rtta qatlam: Content, Padding, Border va Margin.", Example = "box-sizing: border-box;" },
                    new() { Term = "Flexbox", Category = "CSS", Definition = "Bir o'lchamli (qator yoki ustun) qulay joylashuv modeli, elementlarni markazlashtirish va taqsimlashda ishlatiladi.", Example = "display: flex; justify-content: center;" },
                    new() { Term = "CSS Grid", Category = "CSS", Definition = "Murakkab ikki o'lchamli (satrlar va ustunlar kesishmasi) veb qoliplarini yaratish tizimi.", Example = "display: grid; grid-template-columns: repeat(3, 1fr);" },
                    new() { Term = "DOM (Document Object Model)", Category = "JavaScript", Definition = "HTML hujjatining daraxtsimon dasturiy ko'rinishi, JS orqali sahifa elementlarini o'zgartirish vositasi.", Example = "document.getElementById('app')" },
                    new() { Term = "API (Application Programming Interface)", Category = "JavaScript", Definition = "Ikki dastur yoki tizimning o'zaro ma'lumot almashishini ta'minlovchi interfeys.", Example = "fetch('https://api.example.com/data')" },
                    new() { Term = "Responsive Web Design", Category = "Umumiy", Definition = "Veb-saytning turli xil ekran o'lchamlari (mobil, planshet, noutbuk)ga moslashuvchan dizayni.", Example = "@media (max-width: 768px) { ... }" },
                    new() { Term = "UI (User Interface)", Category = "Dizayn", Definition = "Foydalanuvchi ko'radigan barcha vizual elementlar: tugmalar, shriftlar, ranglar, piktogrammalar.", Example = "Tugmaning shakli va rangi" },
                    new() { Term = "UX (User Experience)", Category = "Dizayn", Definition = "Foydalanuvchining mahsulotdan foydalanish davomidagi qulaylik va umumiy tajribasi.", Example = "Navigatsiyaning tez va tushunarli bo'lishi" }
                };

                await db.GlossaryItems.AddRangeAsync(initialItems);
                await db.SaveChangesAsync();
            }

            _allGlossary = await db.GlossaryItems.OrderBy(g => g.Term).AsNoTracking().ToListAsync();
            ApplyFilter();
        }
        finally
        {
            IsLoading = false;
        }
    }

    partial void OnSearchQueryChanged(string value) => ApplyFilter();
    partial void OnSelectedCategoryChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        var query = _allGlossary.AsEnumerable();

        if (SelectedCategory != "Barchasi")
        {
            query = query.Where(g => g.Category.Equals(SelectedCategory, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            var search = SearchQuery.Trim().ToLower();
            query = query.Where(g => g.Term.ToLower().Contains(search) || g.Definition.ToLower().Contains(search));
        }

        FilteredGlossary = new ObservableCollection<GlossaryItem>(query.ToList());
    }

    [RelayCommand]
    private void FilterByCategory(string category)
    {
        SelectedCategory = category;
    }

    [RelayCommand]
    private void BackToHome()
    {
        _navigationService.NavigateTo(AppView.Home);
    }
}
