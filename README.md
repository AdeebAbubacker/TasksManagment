
# 📝 Task Management System – Backend API

A clean and scalable **Task Management System API** built with **ASP.NET Core Web API** using **Clean Architecture**, **JWT Authentication**, and **role-based access control**.

---

## 🚀 Project Overview

This API allows authenticated users to:

- Create and manage tasks
- View **only their own tasks**
- Apply pagination and filtering
- Secure endpoints with JWT

Designed with **maintainability**, **testability**, and **scalability** in mind.

---

## 🏗️ Clean Architecture Structure

```
TaskManagementSystem/
├── TaskManagement.API/
│   ├── Controllers/
│   │   └── TasksController.cs
│   ├── Middleware/
│   │   └── ErrorHandlingMiddleware.cs
│   ├── Program.cs
│   └── appsettings.json
│
├── TaskManagement.Application/
│   ├── Features/
│   │   └── Tasks/
│   │       ├── Commands/
│   │       ├── Queries/
│   │       └── DTOs/
│   ├── Contracts/
│   │   ├── Repositories/
│   │   └── Persistence/
│   ├── Utilities/
│   │   └── SimpleMediator.cs
│
├── TaskManagement.Domain/
│   ├── Entities/
│   │   └── TaskItem.cs
│   └── Common/
│
├── TaskManagement.Infrastructure/
│   ├── Data/
│   │   └── TaskManagementDbContext.cs
│   ├── Repositories/
│   │   └── TasksRepository.cs
│   └── Migrations/
│
└── TaskManagement.Tests/
    └── CreateTasksCommandHandlerTests.cs
```

---

## 🛠️ Tech Stack

- ASP.NET Core 8 Web API
- C#
- Entity Framework Core
- SQL Server
- JWT Authentication
- Clean Architecture
- Repository Pattern
- Unit of Work
- MSTest + NSubstitute

---

## 🔐 Authentication & Security

- JWT-based Authentication
- Custom JWT Claim: `userId`
- User ID is **automatically extracted from token**
- Users can access **only their own tasks**

```csharp
var userId = httpContextAccessor.HttpContext?
    .User.FindFirst("userId")?.Value;
```

---

## 📦 Features

### ✅ Task Management
- Create Task
- Get All Tasks
- Get My Tasks (User-specific)
- Pagination & Filtering
- Secure ownership validation

### ✅ Infrastructure
- Global Exception Handling
- DTO Mapping
- Repository Pattern
- Unit of Work
- Custom Mediator Pattern

### ✅ Testing
- Unit Tests for Command Handlers
- Mocking with NSubstitute
- Transaction rollback validation

---

## 📄 API Endpoints

### ➕ Create Task
```
POST /api/tasks
Authorization: Bearer <token>
```

```json
{
  "name": "Complete API",
  "description": "Finish Task Management API"
}
```

---

### 👤 Get My Tasks
```
GET /api/tasks/my?page=1&recordsPerPage=10
Authorization: Bearer <token>
```

---

### 📋 Get All Tasks
```
GET /api/tasks?page=1&recordsPerPage=10&title=test
Authorization: Bearer <token>
```

---

## 🗄️ Database Migrations

### Add Migration
```
dotnet ef migrations add InitialCreate \
--project TaskManagement.Infrastructure \
--startup-project TaskManagement.API
```

### Update Database
```
dotnet ef database update \
--project TaskManagement.Infrastructure \
--startup-project TaskManagement.API
```

---

## 🧪 Unit Test Example

```csharp
[TestMethod]
public async Task Handle_ValidCommand_ReturnsTaskId()
{
    var command = new CreateTasksCommand { Name = "Test Task" };
    var task = new TaskItem("Test Task");

    repository.Add(Arg.Any<TaskItem>()).Returns(task);

    var result = await handler.Handle(command);

    await repository.Received(1).Add(Arg.Any<TaskItem>());
    await unitOfWork.Received(1).Commit();

    Assert.AreEqual(task.Id, result);
}
```

---

## 📌 Best Practices

- Clean Architecture
- SOLID Principles
- Dependency Injection
- Secure JWT Handling
- Unit Testing

---

## 👨‍💻 Author

**Developer:** Adeeb Abubacker
**GitHub:** [https://github.com/YourUsername  ](https://github.com/AdeebAbubacker/TasksManagment)

---


