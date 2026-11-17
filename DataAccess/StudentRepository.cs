using System.ComponentModel.DataAnnotations;
using StudentManagement.ConsoleApp.Business.Abstractions;
using StudentManagement.ConsoleApp.DataAccess.Abstractions;
using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.DataAccess;

/// <summary>
/// Implements student repository operations using an <see cref="IStudentStore"/>.
/// </summary>
public sealed class StudentRepository : IStudentRepository
{
    private readonly IStudentStore _store;

    /// <summary>
    /// Creates a repository backed by the specified student store.
    /// </summary>
    public StudentRepository(IStudentStore store)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    /// <inheritdoc />
    public IReadOnlyList<Student> GetAll()
    {
        return _store.LoadAll();
    }

    /// <inheritdoc />
    public Student? GetById(int studentId)
    {
        return _store.LoadAll().FirstOrDefault(student => student.StudentId == studentId);
    }

    /// <inheritdoc />
    public void Add(Student student)
    {
        ValidateStudent(student);

        var students = _store.LoadAll().ToList();
        if (students.Any(existing => existing.StudentId == student.StudentId))
        {
            throw new InvalidOperationException(
                $"A student with ID {student.StudentId} already exists.");
        }

        students.Add(student);
        _store.SaveAll(students);
    }

    /// <inheritdoc />
    public bool Update(int currentStudentId, Student updatedStudent)
    {
        ValidateStudent(updatedStudent);

        var students = _store.LoadAll().ToList();
        var currentIndex = students.FindIndex(student => student.StudentId == currentStudentId);
        if (currentIndex < 0)
        {
            return false;
        }

        if (students.Any(student =>
                student.StudentId == updatedStudent.StudentId &&
                student.StudentId != currentStudentId))
        {
            throw new InvalidOperationException(
                $"A student with ID {updatedStudent.StudentId} already exists.");
        }

        students[currentIndex] = updatedStudent;
        _store.SaveAll(students);
        return true;
    }

    /// <inheritdoc />
    public bool Delete(int studentId)
    {
        var students = _store.LoadAll().ToList();
        var student = students.FirstOrDefault(existing => existing.StudentId == studentId);
        if (student is null)
        {
            return false;
        }

        students.Remove(student);
        _store.SaveAll(students);
        return true;
    }

    private static void ValidateStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        Validator.ValidateObject(
            student,
            new ValidationContext(student),
            validateAllProperties: true);
    }
}
