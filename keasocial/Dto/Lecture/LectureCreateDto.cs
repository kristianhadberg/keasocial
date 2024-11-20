namespace keasocial.Dto;

public class LectureCreateDto
{
    public string LectureTitle { get; set; }
    public string LectureDescription { get; set; }
    public DateTime LectureDate { get; set; }
    public TimeSpan LectureTime { get; set; }
}