using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.DataAccess.Abstractions;

/// <summary>
/// Defines the low-level operations for loading and saving student records.
/// </summary>
public interface IStudentStore
{
    /// <summary>
    /// Loads all persisted student records.
    /// </summary>
    IReadOnlyList<Student> LoadAll();

    /// <summary>
    /// Saves all student records.
    /// </summary>
    /// <param name="students">The records to persist.</param>
    void SaveAll(IReadOnlyCollection<Student> students);
}
