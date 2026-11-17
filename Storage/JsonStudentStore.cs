using System.Text.Json;
using StudentManagement.ConsoleApp.DataAccess.Abstractions;
using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Storage;

/// <summary>
/// Loads and saves student records in a JSON file.
/// </summary>
public sealed class JsonStudentStore : IStudentStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _filePath;

    /// <summary>
    /// Creates a JSON store at the supplied path, or in the current user's
    /// application data directory when no path is supplied.
    /// </summary>
    public JsonStudentStore(string? filePath = null)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            var applicationDataPath =
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            if (string.IsNullOrWhiteSpace(applicationDataPath))
            {
                throw new InvalidOperationException(
                    "The current user's application data directory could not be determined.");
            }

            filePath = Path.Combine(
                applicationDataPath,
                "StudentManagementConsoleApplication",
                "students.json");
        }

        _filePath = Path.GetFullPath(filePath);
    }

    /// <inheritdoc />
    public IReadOnlyList<Student> LoadAll()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Student>();
        }

        var json = File.ReadAllText(_filePath);
        var students = JsonSerializer.Deserialize<List<Student>>(json, SerializerOptions)
            ?? throw new InvalidDataException("The student data file does not contain a valid student list.");

        if (students.Any(student => student is null))
        {
            throw new InvalidDataException("The student data file contains a null student record.");
        }

        if (students.GroupBy(student => student.StudentId).Any(group => group.Count() > 1))
        {
            throw new InvalidDataException("The student data file contains duplicate student IDs.");
        }

        return students;
    }

    /// <inheritdoc />
    public void SaveAll(IReadOnlyCollection<Student> students)
    {
        ArgumentNullException.ThrowIfNull(students);

        var directory = Path.GetDirectoryName(_filePath)
            ?? throw new InvalidOperationException("The student data file path has no parent directory.");

        Directory.CreateDirectory(directory);

        var json = JsonSerializer.Serialize(students, SerializerOptions);
        var temporaryPath = $"{_filePath}.{Guid.NewGuid():N}.tmp";

        File.WriteAllText(temporaryPath, json);
        File.Move(temporaryPath, _filePath, overwrite: true);
    }
}
