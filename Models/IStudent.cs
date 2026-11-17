namespace StudentManagement.ConsoleApp.Models;

/// <summary>
/// Defines the properties of a student.
/// </summary>
public interface IStudent
{
    /// <summary>
    /// Gets or sets the student's unique identifier.
    /// </summary>
    int StudentId { get; set; }

    /// <summary>
    /// Gets or sets the student's first name.
    /// </summary>
    string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the student's last name.
    /// </summary>
    string LastName { get; set; }

    /// <summary>
    /// Gets or sets the student's email address.
    /// </summary>
    string Email { get; set; }

    /// <summary>
    /// Gets or sets the student's age.
    /// </summary>
    int Age { get; set; }

    /// <summary>
    /// Gets or sets the student's course.
    /// </summary>
    string Course { get; set; }
}
