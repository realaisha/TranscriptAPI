namespace TranscriptAPI.Models;

// Models/Student.cs
public class Student
{
    public string Surname { get; set; } = "";
    public string Firstname { get; set; } = "";
    public string Othername { get; set; } = "";
    public string Gender { get; set; } = "";
    public string MatricNumber { get; set; } = "";
    public string Programme { get; set; } = "";
    public string ModeOfEntry { get; set; } = "";
    public string YearOfEntry { get; set; } = "";
    public List<CourseResult> Courses { get; set; } = new();
}
