using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Business.Abstractions;

/// <summary>
/// Defines operations for managing persisted student records.
/// </summary>
public interface IStudentRepository
{
    /// <summary>
    /// Gets all students.
    /// </summary>
    IReadOnlyList<Student> GetAll();

    /// <summary>
    /// Finds a student by ID.
    /// </summary>
    /// <param name="studentId">The student ID to search for.</param>
    /// <returns>The matching student, or <see langword="null"/> if no student is found.</returns>
    Student? GetById(int studentId);

    /// <summary>
    /// Adds a new student.
    /// </summary>
    /// <param name="student">The student to add.</param>
    /// <exception cref="InvalidOperationException">The student ID is already in use.</exception>
    void Add(Student student);

    /// <summary>
    /// Updates a student, optionally changing the student's ID.
    /// </summary>
    /// <param name="currentStudentId">The ID of the student to update.</param>
    /// <param name="updatedStudent">The updated student values.</param>
    /// <returns><see langword="true"/> if a student was updated; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="InvalidOperationException">The updated ID belongs to another student.</exception>
    bool Update(int currentStudentId, Student updatedStudent);

    /// <summary>
    /// Deletes a student by ID.
    /// </summary>
    /// <param name="studentId">The ID of the student to delete.</param>
    /// <returns><see langword="true"/> if a student was deleted; otherwise, <see langword="false"/>.</returns>
    bool Delete(int studentId);
}
