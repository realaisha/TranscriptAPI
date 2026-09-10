namespace TranscriptAPI.Models;

public class AcademicSession
{
    public string Session { get; set; } = "";
    public string Level { get; set; } = "";
    public List<CourseResult> Courses { get; set; } = new();
}