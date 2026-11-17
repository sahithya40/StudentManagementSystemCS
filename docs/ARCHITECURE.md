# Student Management System — Proposed Architecture

## 1. Purpose

This document proposes a beginner-friendly layered architecture for the .NET 10 Student Management System. It is based on the functional and non-functional requirements in [REQUIREMENTS.md](./REQUIREMENTS.md): student record CRUD, validation, unique IDs, readable console interaction, exception handling, separation of concerns, and persistent storage.

The design keeps the first version small: use one application project with clearly named folders, plus a separate test project. The folders represent architectural layers without requiring a multi-project solution from day one. The application can be split into separate projects later if it grows.

## 2. Layer Overview

```text
User
  |
  v
Presentation Layer
  |
  v
Business Logic Layer
  |
  v
Data Access Layer
  |
  v
Data Storage Layer
```

Dependencies should point inward toward the business rules. In particular, the Business Logic Layer defines the repository contract it needs; the Data Access Layer implements that contract. The Business Logic Layer should not depend on console APIs, file formats, or a specific database.

## 3. Responsibilities and Proposed Classes

### 3.1 Presentation Layer

**Responsibility:** Interact with the operator. Show the menu, read keyboard input, display student records and operation results, and provide understandable error messages. It should not implement student business rules or access files directly.

**Proposed classes**

- **`Program`** — Application entry point and composition root. Sets up dependencies and starts the menu.
- **`ConsoleMenu`** — Displays options and routes the selected action to the appropriate application service.
- **`StudentConsoleView`** — Prompts for student details and formats student, success, empty-result, and error output.

The view should handle basic interaction concerns such as an unrecognized menu option. Student field rules, such as required values and unique IDs, belong in the Business Logic Layer so they apply consistently to every caller.

### 3.2 Business Logic Layer

**Responsibility:** Enforce student-management rules and coordinate use cases. This layer should be testable without a console or real storage.

**Proposed classes and contract**

- **`Student`** — Represents a student and the fields agreed for the application. Keep field choices and validation constraints consistent with the open decisions in the requirements.
- **`StudentService`** — Implements the use cases: add, list, find by ID, update, and delete. Coordinates validation and repository operations.
- **`StudentValidator`** — Applies the agreed required-field, format, length, and range rules.
- **`IStudentRepository`** — Describes the student data operations needed by `StudentService`, without exposing how or where data is stored.
- **`StudentNotFoundException` / `DuplicateStudentIdException`** — Optional, specific outcomes if exceptions are chosen to communicate these expected failures. A clear result type is also reasonable; choose one approach and use it consistently.

`StudentService` should validate the complete proposed record before updating it, and only save a change after validation and ID uniqueness checks succeed. This supports the requirement that a failed update leaves the original record unchanged.

### 3.3 Data Access Layer

**Responsibility:** Implement the business layer's repository contract and translate student operations into storage operations. It should not contain console prompts or make user-interface decisions.

**Proposed class**

- **`StudentRepository`** — Implements `IStudentRepository`. Loads, finds, adds, updates, lists, and deletes records by using the storage component. It handles persistence-related mapping and reports storage failures rather than pretending an operation succeeded.

The repository is the boundary between business operations and the chosen storage implementation. It should preserve the repository contract if the storage technology changes.

### 3.4 Data Storage Layer

**Responsibility:** Read and write the durable data representation. This is the only layer that should know about file paths, serialization, or database-specific APIs.

**Proposed contract and class**

- **`IStudentStore`** — A small storage contract used by `StudentRepository` for loading and saving the collection of records.
- **`JsonStudentStore`** — A possible first implementation that reads and writes student records in a JSON file.

JSON is a simple starting option for a single-user console application, but the requirements leave the storage technology and data location open. Confirm those decisions before implementation. If another storage technology is selected, provide a different `IStudentStore` implementation without changing the console workflows or student rules.

The storage implementation should report read/write errors, avoid silently replacing unreadable existing data with an empty collection, and save changes in a way that minimizes the risk of leaving a partially written data file.

## 4. Communication Between Layers

Use a one-way flow for a typical operation:

1. `ConsoleMenu` receives an action from the operator and asks `StudentConsoleView` to collect the required input.
2. The menu calls the appropriate method on `StudentService`.
3. `StudentService` validates input and applies business rules, including the unique-ID rule. It uses `IStudentRepository` to retrieve or change records.
4. `StudentRepository` implements that contract and uses `IStudentStore` to load or save data.
5. The result flows back through the same boundaries. The presentation layer formats it for the console.

For example, adding a student should validate the entered fields, check that the ID is unused, request the repository to add the record, and show success only after persistence succeeds. A validation, duplicate-ID, or storage failure should return a clear failure to the presentation layer; it must not be reported as a successful save.

Expected outcomes such as “student not found” or “duplicate ID” should be represented consistently (for example, with explicit result types or specific exceptions). Unexpected storage or programming failures should be handled at an appropriate boundary, reported without sensitive implementation details, and logged if diagnostics are configured. Avoid catching and ignoring errors.

## 5. Dependency Injection

Use constructor injection: each class receives the services or contracts it needs rather than creating its own dependencies or using global state.

- `StudentService` receives `IStudentRepository` and `StudentValidator`.
- `StudentRepository` receives `IStudentStore`.
- `ConsoleMenu` receives `StudentService` and its view.
- The composition root in `Program` creates and connects the implementations: `IStudentRepository` to `StudentRepository`, and `IStudentStore` to the chosen store implementation.

For a small first version, these dependencies can be created explicitly in the composition root. If using the .NET dependency-injection container, add the appropriate `Microsoft.Extensions.DependencyInjection` package (or the Generic Host package if its other hosting features are needed), register the interfaces and implementations at startup, and resolve the top-level menu there. The container should be configured in one place; business classes should not access the container directly.

The container is a wiring tool, not a layer. It does not change the direction of dependencies: business logic still depends on abstractions, while startup code selects concrete implementations.

## 6. Unit Testing Strategy

Keep the test project separate from the application project. Test the Business Logic Layer first because it contains the core rules and can run without a terminal or persistent store.

- **`StudentValidator` tests:** Check required and invalid field values using the agreed field rules.
- **`StudentService` tests:** Supply a fake or in-memory `IStudentRepository` and test adding valid students, rejecting invalid data, rejecting duplicate IDs, finding existing and missing IDs, updating while preserving the original on failure, and deleting according to the requested operation.
- **Data Access / storage tests:** Exercise `StudentRepository` and the selected store using a temporary data location. Verify that records survive reload, updates persist, deletions persist, and malformed or inaccessible data produces a visible failure rather than silent data loss.
- **Presentation checks:** Keep most menu formatting simple. Where useful, test interaction by substituting input/output streams or manually verify representative menu flows; do not make business-rule tests depend on console text.
- **End-to-end check:** Run the application with a temporary data location and verify a record can be added, found, displayed, updated, deleted, and remains consistent after restarting.

Because dependencies are injected, tests can substitute a fake repository for the real one. This makes business tests fast and predictable and avoids requiring file access for every test.

## 7. Suggested Folder Structure

Start with folders in the existing console application project:

```text
StudentManagementConsoleApplication/
├── Presentation/
│   ├── ConsoleMenu
│   └── StudentConsoleView
├── Business/
│   ├── Student
│   ├── StudentService
│   ├── StudentValidator
│   └── IStudentRepository
├── DataAccess/
│   └── StudentRepository
├── Storage/
│   ├── IStudentStore
│   └── JsonStudentStore
├── Program
└── StudentManagementConsoleApplication.csproj

StudentManagementConsoleApplication.Tests/
```

File extensions are omitted in the diagram for readability. When the application becomes larger, these folders can be moved into separate projects while retaining the same dependency direction.

## 8. Requirements Traceability

| Requirement area | Main architecture responsibility |
|---|---|
| FR-001 Add a student | `StudentService`, `StudentValidator`, repository, and store |
| FR-002 Display all students | `StudentService`, repository, and presentation formatting |
| FR-003 Search by ID | `StudentService`, repository, and presentation result handling |
| FR-004 Update details | `StudentService` validation and repository update |
| FR-005 Delete a student | Presentation confirmation, service use case, repository and store |
| FR-006 Validate information | `StudentValidator`, coordinated by `StudentService` |
| FR-007 Prevent duplicate IDs | `StudentService` and repository operations |
| NFR-001 Maintainability / NFR-004 separation | Clear layer boundaries and dependency direction |
| NFR-002 Exception handling | Each layer reports failures; presentation provides safe messages |
| NFR-003 Input validation | Presentation checks input shape; business layer validates domain data |
| NFR-005 Readable output | `StudentConsoleView` |
| NFR-006 Persistent storage | Data Access and Data Storage layers |

## 9. Decisions to Confirm Before Implementation

- Which student fields are supported and which are required.
- Student ID format and comparison rules, including case and whitespace handling.
- Storage technology, data file/database location, and backup/recovery expectations.
- Whether the application needs a logging/diagnostics package or a minimal diagnostic mechanism.