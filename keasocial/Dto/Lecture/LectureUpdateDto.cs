namespace keasocial.Dto;

public class LectureUpdateDto
{
    public string LectureTitle { get; set; }
    public string LectureDescription { get; set; }
    public DateTime LectureDate { get; set; }
    public TimeSpan LectureTime { get; set; }
}