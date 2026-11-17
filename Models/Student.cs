using System.ComponentModel.DataAnnotations;

namespace StudentManagement.ConsoleApp.Models;

/// <summary>
/// Represents a student in the Student Management System.
/// </summary>
public class Student : IStudent
{
    /// <summary>
    /// Gets or sets the student's unique identifier.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int StudentId { get; set; }

    /// <summary>
    /// Gets or sets the student's first name.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the student's last name.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the student's email address.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the student's age.
    /// </summary>
    [Range(3, 100)]
    public int Age { get; set; }

    /// <summary>
    /// Gets or sets the student's course.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Course { get; set; } = string.Empty;
}
