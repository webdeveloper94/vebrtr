using Microsoft.EntityFrameworkCore;
using WebLab.RTR.Models;

namespace WebLab.RTR.Services;

public interface ITopicRepository
{
    Task<List<Topic>> GetAllTopicsAsync();
    Task<Topic?> GetTopicByIdAsync(int id);
    Task SeedInitialTopicsAsync();
}

public class TopicRepository : ITopicRepository
{
    public async Task<List<Topic>> GetAllTopicsAsync()
    {
        await using var db = new AppDbContext();
        return await db.Topics
            .Where(t => t.IsPublished)
            .OrderBy(t => t.OrderNumber)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Topic?> GetTopicByIdAsync(int id)
    {
        await using var db = new AppDbContext();
        return await db.Topics.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task SeedInitialTopicsAsync()
    {
        await using var db = new AppDbContext();
        await db.Database.EnsureCreatedAsync();

        if (await db.Topics.AnyAsync())
        {
            return; // 30 ta mavzu allaqachon mavjud, takroran kiritilmaydi
        }

        var initialTopics = new List<Topic>
        {
            // HTML & Web Asoslari (1 - 7)
            new() { OrderNumber = 1, Category = "HTML", Title = "Veb dasturlashga kirish va Internet arxitekturasi", DurationMinutes = 45, Level = "Boshlang‘ich", Description = "Internet qanday ishlaydi, mijoz-server (Client-Server) modeli, HTTP/HTTPS protokollari va veb-saytning umumiy tuzilishi." },
            new() { OrderNumber = 2, Category = "HTML", Title = "HTML asoslari va hujjatning bazaviy strukturasi", DurationMinutes = 50, Level = "Boshlang‘ich", Description = "<!DOCTYPE>, <html>, <head>, <body> teglari, meta-ma’lumotlar va qidiruv tizimlari uchun tavsiyalar." },
            new() { OrderNumber = 3, Category = "HTML", Title = "Semantik HTML5 teglari va ularning afzalliklari", DurationMinutes = 40, Level = "Boshlang‘ich", Description = "<header>, <nav>, <main>, <section>, <article>, <aside>, <footer> elementlaridan to‘g‘ri foydalanish." },
            new() { OrderNumber = 4, Category = "HTML", Title = "Matnlar, havolalar va ro‘yxatlar bilan ishlash", DurationMinutes = 35, Level = "Boshlang‘ich", Description = "Sarlavhalar (h1-h6), paragraflar, formatlash teglari, tartibli (ol) va tartibsiz (ul) ro‘yxatlar." },
            new() { OrderNumber = 5, Category = "HTML", Title = "Multimedia: Rasmlar, audio va video integratsiyasi", DurationMinutes = 45, Level = "Boshlang‘ich", Description = "<img>, <picture>, <audio>, <video> teglari, zamonaviy veb formatlari (WebP, SVG) va optimallashtirish." },
            new() { OrderNumber = 6, Category = "HTML", Title = "Jadvallar va ma’lumotlarni tizimli ifodalash", DurationMinutes = 40, Level = "Boshlang‘ich", Description = "<table>, <tr>, <th>, <td>, <thead>, <tbody>, <tfoot> teglari va zamonaviy jadvallar arxitekturasi." },
            new() { OrderNumber = 7, Category = "HTML", Title = "HTML Shakllari (Forms) va ma’lumot kiritish maydonlari", DurationMinutes = 55, Level = "O‘rta", Description = "<form>, <input>, <textarea>, <select>, validatsiya atributlari va foydalanuvchidan ma’lumot qabul qilish." },

            // CSS & Stilistika (8 - 15)
            new() { OrderNumber = 8, Category = "CSS", Title = "CSS asoslari, selektorlar va sintaksis", DurationMinutes = 50, Level = "Boshlang‘ich", Description = "CSS qoidalari, Inline, Internal, External uslublar, sinf va ID selektorlari, selektor ustunligi (specificity)." },
            new() { OrderNumber = 9, Category = "CSS", Title = "CSS Box Model: Margin, Border, Padding va Content", DurationMinutes = 45, Level = "Boshlang‘ich", Description = "Elementlarning o‘lchamlari, box-sizing qoidasi, chekka masofalar va elementlar joylashuvi sirlari." },
            new() { OrderNumber = 10, Category = "CSS", Title = "Ranglar, gradientlar va zamonaviy tipografika", DurationMinutes = 40, Level = "Boshlang‘ich", Description = "HEX, RGB, HSL rang modellari, chiziqli va radial gradientlar, Google Fonts va shrift parametrlar." },
            new() { OrderNumber = 11, Category = "CSS", Title = "Flexbox: Bir o‘lchamli moslashuvchan joylashuv", DurationMinutes = 60, Level = "O‘rta", Description = "display: flex, justify-content, align-items, flex-direction, flex-wrap va zamonaviy navigatsiya barlari." },
            new() { OrderNumber = 12, Category = "CSS", Title = "CSS Grid: Ikki o‘lchamli qudratli tartib tizimi", DurationMinutes = 65, Level = "O‘rta", Description = "grid-template-columns, grid-template-rows, gap, minmax() va murakkab portal qoliplari yaratish." },
            new() { OrderNumber = 13, Category = "CSS", Title = "Media Queries va Responsive (Moslashuvchan) Dizayn", DurationMinutes = 55, Level = "O‘rta", Description = "Mobil qurilmalar, planshet va noutbuklar uchun ekran o‘lchamiga moslashuvchan veb sahifalar yasash." },
            new() { OrderNumber = 14, Category = "CSS", Title = "CSS Transitions va Keyframe Animatsiyalari", DurationMinutes = 50, Level = "O‘rta", Description = "Tugmalardagi silliq hover effektlar, @keyframes orqali suzuvchi va aylanuvchi professional animatsiyalar." },
            new() { OrderNumber = 15, Category = "CSS", Title = "CSS o‘zgaruvchilari (Variables) va Dark Mode arxitekturasi", DurationMinutes = 45, Level = "O‘rta", Description = ":root yordamida dizayn tizimi yaratish, bir tugma bilan kunduzgi va tungi rejimlarni boshqarish." },

            // JavaScript & Dasturlash Mantig'i (16 - 24)
            new() { OrderNumber = 16, Category = "JavaScript", Title = "JavaScript asoslari va skriptlarni ulash", DurationMinutes = 50, Level = "Boshlang‘ich", Description = "<script> tegi, console.log, o‘zgaruvchilar (let, const, var) va ma’lumot turlari." },
            new() { OrderNumber = 17, Category = "JavaScript", Title = "Operatorlar, shartli operatorlar va sikllar", DurationMinutes = 55, Level = "Boshlang‘ich", Description = "if/else, switch-case, mantiqiy operatorlar, for, while va for...of sikllarida amallar bajarish." },
            new() { OrderNumber = 18, Category = "JavaScript", Title = "Funksiyalar, Arrow Functions va Scope", DurationMinutes = 60, Level = "O‘rta", Description = "Funksiya parametrlari, qaytarish qiymati, o‘qsimon funksiyalar (=>), lokal va global o‘zgaruvchilar doirasi." },
            new() { OrderNumber = 19, Category = "JavaScript", Title = "Massivlar (Arrays) va obyektlar (Objects)", DurationMinutes = 60, Level = "O‘rta", Description = "Massiv metodlari (map, filter, reduce, find), kalit-qiymat obyektlari va murakkab ma’lumotlar bilan ishlash." },
            new() { OrderNumber = 20, Category = "JavaScript", Title = "DOM (Document Object Model) bilan ishlash", DurationMinutes = 65, Level = "O‘rta", Description = "querySelector, getElementById, elementlarni yaratish, tahrirlash, o‘chirish va sahifani jonlantirish." },
            new() { OrderNumber = 21, Category = "JavaScript", Title = "Hodisalar (Events) va Event Listeners", DurationMinutes = 55, Level = "O‘rta", Description = "click, submit, keydown, mouseover hodisalari, event bubbling va interaktiv elementlar yaratish." },
            new() { OrderNumber = 22, Category = "JavaScript", Title = "Asinxron JavaScript, Promises va Fetch API", DurationMinutes = 70, Level = "Murakkab", Description = "REST API'lardan ma’lumot olish, async/await sintaksisi, JSON formatini o‘qish va server bilan aloqa." },
            new() { OrderNumber = 23, Category = "JavaScript", Title = "Lokal xotira (LocalStorage va SessionStorage)", DurationMinutes = 45, Level = "O‘rta", Description = "Foydalanuvchi sozlamalari, savatcha ma’lumotlari va tanlangan mavzularni brauzerda saqlab qolish." },
            new() { OrderNumber = 24, Category = "JavaScript", Title = "Xatolarni ushlash va tozalash (Debugging & DevTools)", DurationMinutes = 40, Level = "Boshlang‘ich", Description = "try...catch bloklari, Chrome DevTools yordamida dastur qadamlarini tekshirish va xatolarni tuzatish." },

            // UI/UX Dizayn & Amaliyot (25 - 30)
            new() { OrderNumber = 25, Category = "Dizayn", Title = "UI/UX dizayn asoslari va ranglar nazariyasi", DurationMinutes = 50, Level = "Boshlang‘ich", Description = "Foydalanuvchi tajribasi (UX), interfeys qulayligi (UI), 60-30-10 qoidasi va rang psixologiyasi." },
            new() { OrderNumber = 26, Category = "Dizayn", Title = "Vizual ierarxiya, bo‘shliqlar va tipografika qoidalari", DurationMinutes = 45, Level = "Boshlang‘ich", Description = "Ko‘rish traektoriyasi (F va Z pattern), bo‘shliqlar (whitespace) orqali e’tiborni boshqarish sirlari." },
            new() { OrderNumber = 27, Category = "Dizayn", Title = "Figma dasturida veb interfeys maketini yaratish", DurationMinutes = 65, Level = "O‘rta", Description = "Figma ramkalari, komponentlar, Auto-Layout va dasturchiga dizaynni topshirish jarayoni." },
            new() { OrderNumber = 28, Category = "Dizayn", Title = "Veb sahifaning tezligi va optimallashtirish usullari", DurationMinutes = 55, Level = "Murakkab", Description = "Rasmlarni siqish, kodni minifikatsiya qilish, kesh va Core Web Vitals ko‘rsatkichlarini oshirish." },
            new() { OrderNumber = 29, Category = "Dizayn", Title = "Git va GitHub yordamida kodni boshqarish", DurationMinutes = 60, Level = "O‘rta", Description = "Versiyalarni nazorat qilish, repository yaratish, commit, push, pull va loyihani GitHub'da yuritish." },
            new() { OrderNumber = 30, Category = "Dizayn", Title = "Veb saytni bepul xostingga yuklash (Deploy & Hosting)", DurationMinutes = 50, Level = "O‘rta", Description = "Vercel, Netlify va GitHub Pages orqali tayyor saytni internet foydalanuvchilariga taqdim etish." }
        };

        await db.Topics.AddRangeAsync(initialTopics);
        await db.SaveChangesAsync();
    }
}
