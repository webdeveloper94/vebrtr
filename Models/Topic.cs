namespace WebLab.RTR.Models;

public class Topic
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // masalan: HTML, CSS, JavaScript, Dizayn
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; } // O'rganish vaqti (daqiqa)
    public string Level { get; set; } = "Boshlang‘ich"; // Boshlang'ich, O'rta, Murakkab
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
