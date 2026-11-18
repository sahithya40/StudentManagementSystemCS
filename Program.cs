using Microsoft.Extensions.DependencyInjection;
using StudentManagement.ConsoleApp.Business.Abstractions;
using StudentManagement.ConsoleApp.DataAccess;
using StudentManagement.ConsoleApp.DataAccess.Abstractions;
using StudentManagement.ConsoleApp.Models;
using StudentManagement.ConsoleApp.Services;
using StudentManagement.ConsoleApp.Storage;

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Student Management Console Application");
    Console.WriteLine("1. Display Application Information");
    Console.WriteLine("2. Run CRUD Scenario");
    Console.WriteLine("3. Exit");
    Console.Write("Select an option: ");

    switch (Console.ReadLine())
    {
        case null:
            return;
        case "1":
            DisplayApplicationInformation();
            break;
        case "2":
            RunCrudScenario();
            break;
        case "3":
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}

static void DisplayApplicationInformation()
{
    var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

    Console.WriteLine("Application Name: Student Management Console Application");
    Console.WriteLine($"Application Version: {version}");
    Console.WriteLine("Description: This is a console application for managing student records.");
}

static void RunCrudScenario()
{
    var temporaryDirectory = Directory.CreateTempSubdirectory("student-management-");
    var dataPath = Path.Combine(temporaryDirectory.FullName, "students.json");

    try
    {
        var services = new ServiceCollection();
        services.AddSingleton<IStudentStore>(new JsonStudentStore(dataPath));
        services.AddSingleton<IStudentRepository, StudentRepository>();
        services.AddSingleton<IStudentService, StudentService>();

        using var serviceProvider = services.BuildServiceProvider();
        var studentService = serviceProvider.GetRequiredService<IStudentService>();
        Console.WriteLine("Student service resolved through dependency injection.");
        Console.WriteLine("Running CRUD scenario with temporary student data.");

        var firstStudent = new Student
        {
            StudentId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Age = 20,
            Course = "Computer Science"
        };
        var secondStudent = new Student
        {
            StudentId = 2,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Age = 22,
            Course = "Mathematics"
        };

        Ensure(studentService.AddStudent(firstStudent), "First student registration failed.");
        Ensure(studentService.AddStudent(secondStudent), "Second student registration failed.");
        Console.WriteLine("Registered students 1 and 2.");

        var students = studentService.GetAllStudents();
        Ensure(students.Count == 2, "Expected two registered students.");
        Console.WriteLine("All students:");
        foreach (var student in students)
        {
            Console.WriteLine($"  {student.StudentId}: {student.FirstName} {student.LastName}, " +
                              $"{student.Email}, age {student.Age}, course: {student.Course}");
        }

        var found = studentService.GetStudentById(firstStudent.StudentId)
            ?? throw new InvalidOperationException("Student 1 was not found.");
        Console.WriteLine($"Found student {found.StudentId}: {found.FirstName} {found.LastName}.");

        found.Course = "Software Engineering";
        Ensure(studentService.UpdateStudent(found), "Student 1 update failed.");
        Ensure(studentService.GetStudentById(found.StudentId)?.Course == "Software Engineering",
            "The updated course was not saved.");
        Console.WriteLine($"Updated student {found.StudentId}'s course to {found.Course}.");

        Ensure(studentService.DeleteStudent(firstStudent.StudentId), "Student 1 deletion failed.");
        Console.WriteLine("Deleted student 1.");

        Ensure(studentService.GetStudentById(firstStudent.StudentId) is null,
            "Deleted student 1 was still found.");
        Console.WriteLine("Searching for deleted student 1 returned no result.");

        Ensure(!studentService.AddStudent(secondStudent), "Duplicate student ID was accepted.");
        Console.WriteLine("Duplicate student ID 2 was rejected.");
        Console.WriteLine("CRUD scenario completed successfully.");
    }
    finally
    {
        File.Delete(dataPath);
        temporaryDirectory.Delete();
    }
}

static void Ensure(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}