namespace WebLab.RTR.Models;

public class Topic
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // HTML, CSS, JavaScript, Dizayn
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; } // O'rganish vaqti (daqiqa)
    public string Level { get; set; } = "Boshlang‘ich"; // Boshlang'ich, O'rta, Murakkab
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // --- RTR Nizomi talablari bo'yicha kengaytmalar ---
    
    // 1. Ma'ruza matni (Nazariy va amaliy qism)
    public string LectureContent { get; set; } = string.Empty;
    public string PracticalTasks { get; set; } = string.Empty;

    // 2. O'qitish metodlari (Interfaol metodlar: Keys-stadi, Aqliy hujum, FSMU va b.)
    public string TeachingMethods { get; set; } = string.Empty;

    // 3. Media va Video materiallar
    public string VideoUrl { get; set; } = string.Empty;
    public string VideoDuration { get; set; } = "15:00";
    public string VideoQuality { get; set; } = "1080p Full HD";

    // 4. Taqdimot va Ko'rgazmali materiallar
    public string PresentationFile { get; set; } = string.Empty;
    public int PresentationSlideCount { get; set; } = 15;

    // 5. Nazorat savollari (mavzuni mustahkamlash uchun)
    public string ReviewQuestions { get; set; } = string.Empty;

    // 6. Mavzuga oid Glossariy (atamalar va ularning izohlari)
    public string GlossaryTerms { get; set; } = string.Empty;
}
