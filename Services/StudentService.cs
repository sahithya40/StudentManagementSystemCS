using System.ComponentModel.DataAnnotations;
using StudentManagement.ConsoleApp.Business.Abstractions;
using StudentManagement.ConsoleApp.Models;

namespace StudentManagement.ConsoleApp.Services;

/// <summary>
/// Validates and manages student records through a repository.
/// </summary>
public sealed class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    /// <summary>
    /// Creates a student service with the specified repository.
    /// </summary>
    /// <param name="repository">The repository used to manage students.</param>
    public StudentService(IStudentRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public bool AddStudent(Student student)
    {
        ValidateStudent(student);

        if (_repository.GetById(student.StudentId) is not null)
        {
            return false;
        }

        _repository.Add(student);
        return true;
    }

    /// <inheritdoc />
    public IReadOnlyList<Student> GetAllStudents()
    {
        return _repository.GetAll();
    }

    /// <inheritdoc />
    public Student? GetStudentById(int studentId)
    {
        return _repository.GetById(studentId);
    }

    /// <inheritdoc />
    public bool UpdateStudent(Student student)
    {
        ValidateStudent(student);
        return _repository.Update(student.StudentId, student);
    }

    /// <inheritdoc />
    public bool DeleteStudent(int studentId)
    {
        return _repository.Delete(studentId);
    }

    private static void ValidateStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(
                student,
                new ValidationContext(student),
                results,
                validateAllProperties: true))
        {
            throw new ValidationException(results[0], null, student);
        }
    }
}
