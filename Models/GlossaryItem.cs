namespace WebLab.RTR.Models;

public class GlossaryItem
{
    public int Id { get; set; }
    public string Term { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // HTML, CSS, JS, Umumiy
    public string Definition { get; set; } = string.Empty;
    public string? Example { get; set; }
}
