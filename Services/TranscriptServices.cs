// Services/TranscriptService.cs
using ClosedXML.Excel;
using TranscriptAPI.Models;


public class TranscriptService
{
    private readonly Dictionary<string, Student> _students = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Transcript> _transcripts = new(StringComparer.OrdinalIgnoreCase);
    private readonly string _filePath;

    public TranscriptService(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(env.ContentRootPath, "Data", "healthSciences.xlsx");
        Load();
    }

    private void Load()
    {
        using var workbook = new XLWorkbook(_filePath);
        var worksheet = workbook.Worksheet("Record");
        var lastUsedRow = worksheet.LastRowUsed();

        if (lastUsedRow == null)
        {
            return;
        }

        int lastRow = lastUsedRow.RowNumber();

        Student? currentStudent = null;

        for (int row = 3; row <= lastRow; row++)
        {
            string matricNo = worksheet.Cell(row, 6).Value.ToString().Trim();

            if (!string.IsNullOrWhiteSpace(matricNo))
            {
                currentStudent = new Student
                {
                    Surname = worksheet.Cell(row, 2).Value.ToString(),
                    Firstname = worksheet.Cell(row, 3).Value.ToString(),
                    Othername = worksheet.Cell(row, 4).Value.ToString(),
                    Gender = worksheet.Cell(row, 5).Value.ToString(),
                    MatricNumber = matricNo,
                    Programme = worksheet.Cell(row, 7).Value.ToString(),
                    ModeOfEntry = worksheet.Cell(row, 8).Value.ToString(),
                    YearOfEntry = worksheet.Cell(row, 9).Value.ToString(),
                };
                _students[matricNo] = currentStudent;
            }

            if (currentStudent == null) continue;

            string courseCode = worksheet.Cell(row, 10).Value.ToString();
            if (string.IsNullOrWhiteSpace(courseCode)) continue;
            currentStudent.Courses.Add(new CourseResult
            {
                CourseCode = courseCode,
                CourseDescription = worksheet.Cell(row, 11).Value.ToString(),
                Status = worksheet.Cell(row, 12).Value.ToString(),
                CreditUnit = worksheet.Cell(row, 13).Value.ToString(),
                Score = worksheet.Cell(row, 14).Value.ToString(),
                Grade = worksheet.Cell(row, 15).Value.ToString(),
                Level = worksheet.Cell(row, 16).Value.ToString(),
                Session = worksheet.Cell(row, 17).Value.ToString()
            });
        }

        var degreeworksheet = workbook.Worksheet("Class of Degree");
        var degreelastUsedRow = degreeworksheet.LastRowUsed();

        if (degreelastUsedRow == null)
        {
            return;
        }

        int degreelastRow = degreelastUsedRow.RowNumber();

        for (int row = 3; row <= degreelastRow; row++)
        {
            string matricNo = degreeworksheet.Cell(row, 2).Value.ToString().Trim();

            if (string.IsNullOrWhiteSpace(matricNo))
            {
                continue;
            }

            var transcript = new Transcript
                {
                    TotalCreditsOffered = degreeworksheet.Cell(row, 3).GetValue<int>(),
                    TotalCreditsPassed = degreeworksheet.Cell(row, 4).GetValue<int>(),
                    TotalWeightedGradePoints = degreeworksheet.Cell(row, 5).GetValue<double>(),
                    CGPA = degreeworksheet.Cell(row, 6).GetValue<double>(),
                    DegreeAwarded = degreeworksheet.Cell(row, 7).Value.ToString(),
                    ClassOfDegree = degreeworksheet.Cell(row, 8).Value.ToString(),
                
                };
                _transcripts[matricNo] = transcript;
            }


    }

    public Student? GetByMatricNumber(string matricNumber) =>
        _students.TryGetValue(matricNumber.Trim(), out var student) ? student : null;

    public Transcript? GetTranscriptByMatricNumber(string matricNumber)
{
    matricNumber = matricNumber.Trim();

    if (!_students.TryGetValue(matricNumber, out var student))
    {
        return null;
    }

    if (!_transcripts.TryGetValue(matricNumber, out var transcript))
    {
        return null;
    }

    transcript.Student = student;

    return transcript;
}

public IReadOnlyCollection<Student> GetAll() => _students.Values;

public void Reload()
{
    _students.Clear();
    _transcripts.Clear();
    Load();
}
}