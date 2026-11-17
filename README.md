# Student Management System

A .NET 10 console application intended to help manage student records and common academic information. The project is currently at its initial starter stage; the features below describe the proposed application scope and are not yet implemented.

## Project Overview

The Student Management System is designed as a straightforward, menu-driven application for maintaining student information. Its console interface is intended to make common record-management tasks accessible without requiring a graphical interface.

## Business Objectives

- Keep student information organized in one consistent system.
- Make it easier to add, find, review, and maintain student records.
- Reduce errors associated with repetitive manual record handling.
- Provide a foundation that can grow to support broader academic workflows.

## Technology Stack

- **Language:** C#
- **Platform:** .NET 10
- **Application type:** Console application
- **Project format:** .NET SDK-style project

## Application Features

The following features are planned for the application:

- Menu-driven navigation for common operations.
- Create, view, update, and remove student records.
- Search for students by identifying information.
- Validate user input and provide clear feedback.
- Organize student details such as name, contact information, and enrollment data.

## Project Structure

```text
StudentManagementConsoleApplication/
├── docs/
│   ├── API-DESIGN.md
│   ├── ARCHITECURE.md
│   ├── DATABASE.md
│   └── REQUIREMENTS.md
├── Program.cs
├── StudentManagementConsoleApplication.csproj
└── README.md
```

The `docs/` directory is reserved for project design and requirements documentation. Build output and intermediate files are generated in `bin/` and `obj/` when building the project.

## Prerequisites

- Windows, macOS, or Linux.
- The **.NET 10 SDK** installed and available on your `PATH`.
- A terminal or command prompt.

Verify the SDK installation with:

```bash
dotnet --version
```

The reported SDK version should be `10.0` or later.

## How to Build the Application

From the project directory, run:

```bash
dotnet build StudentManagementConsoleApplication.csproj
```

To build in Release configuration:

```bash
dotnet build StudentManagementConsoleApplication.csproj --configuration Release
```

## How to Run the Application

From the project directory, run:

```bash
dotnet run --project StudentManagementConsoleApplication.csproj
```

The current starter program prints `Hello, World!`. The student-management functionality described above remains planned work.

## Future Enhancements

- Implement the planned student record workflows and input validation.
- Persist records using a database or another durable storage option.
- Add course, enrollment, and academic progress management.
- Provide reporting and data export capabilities.
- Introduce role-based access and authentication if required.
- Add automated tests for core application behavior.