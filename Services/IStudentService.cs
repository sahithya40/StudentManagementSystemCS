using System.ComponentModel.DataAnnotations;
using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Services;

/// <summary>
/// Defines operations for managing students.
/// </summary>
public interface IStudentService
{
    /// <summary>
    /// Adds a validated student.
    /// </summary>
    /// <param name="student">The student to add.</param>
    /// <returns>False if the student ID already exists; otherwise, true.</returns>
    /// <exception cref="ValidationException">The student information is invalid.</exception>
    bool AddStudent(Student student);

    /// <summary>
    /// Gets all students.
    /// </summary>
    /// <returns>All stored students.</returns>
    IReadOnlyList<Student> GetAllStudents();

    /// <summary>
    /// Finds a student by ID.
    /// </summary>
    /// <param name="studentId">The student ID to search for.</param>
    /// <returns>The student, or null if not found.</returns>
    Student? GetStudentById(int studentId);

    /// <summary>
    /// Updates the student identified by the supplied student's ID.
    /// </summary>
    /// <param name="student">The validated replacement student.</param>
    /// <returns>True if the student was updated; otherwise, false.</returns>
    /// <exception cref="ValidationException">The student information is invalid.</exception>
    bool UpdateStudent(Student student);

    /// <summary>
    /// Deletes a student by ID.
    /// </summary>
    /// <param name="studentId">The student ID to delete.</param>
    /// <returns>True if the student was deleted; otherwise, false.</returns>
    bool DeleteStudent(int studentId);
}
