namespace TranscriptAPI.Models;

public class Transcript
{
    public Student Student { get; set; } = new();

    public List<AcademicSession> Sessions { get; set; } = new();

    public double CGPA { get; set; }

    public string DegreeAwarded { get; set; } = "";

    public string ClassOfDegree { get; set; } = "";

    public string RefNo { get; set; } = "";

    public string Date { get; set; } = "";

    public string Registrar { get; set; } = "";

    public int TotalCreditsOffered { get; set; }

    public int TotalCreditsPassed { get; set; }

    public double TotalWeightedGradePoints { get; set; }
}