namespace keasocial.Models;

public class Calendar
{
    public int CalendarId { get; set; }
    public int UserId { get; set; }
    public bool IsPublic { get; set; }

    // Navigation properties
    public User User { get; set; }
    public ICollection<Lecture> Lectures { get; set; }
}