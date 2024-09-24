namespace keasocial.Models;
public class Lecture
{
    public int LectureId { get; set; }
    public string LectureTitle { get; set; }
    public string LectureDescription { get; set; }
    public DateTime LectureDate { get; set; }
    public TimeSpan LectureTime { get; set; }

    // Navigation property
    public ICollection<Calendar> Calendars { get; set; }
}