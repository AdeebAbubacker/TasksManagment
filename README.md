<h1>📝 Task Management System – Backend API</h1>

<p>
  A clean and scalable <b>Task Management System Backend API</b> built using
  <b>ASP.NET Core Web API</b>, following <b>Clean Architecture</b>,
  <b>JWT Authentication</b>, and <b>Role-Based Access Control</b>.
</p>

<p>
  This backend serves an <b>Angular Frontend Task Management Application</b>
  and focuses on security, maintainability, and scalability.
</p>

<hr/>

<h2>🚀 Project Overview</h2>

<ul>
  <li>Create and manage tasks</li>
  <li>Users can view <b>only their own tasks</b></li>
  <li>Pagination and filtering support</li>
  <li>JWT-secured endpoints</li>
</ul>

<hr/>

<h2>🖥️ Frontend Integration</h2>

<p>
  The frontend for this system is built using <b>Angular</b>.
</p>

<p>
  🔗 <b>Frontend Repository:</b><br/>
  <a href="https://github.com/AdeebAbubacker/GtaskManagement" target="_blank">
    https://github.com/AdeebAbubacker/GtaskManagement
  </a>
</p>

<hr/>

<h2>🏗️ Clean Architecture Structure</h2>

<pre>
TaskManagementSystem/
├── TaskManagement.API/
│   ├── Controllers/
│   ├── Middleware/
│   ├── Program.cs
│   └── appsettings.json
│
├── TaskManagement.Application/
│   ├── Features/
│   ├── Contracts/
│   └── Utilities/
│
├── TaskManagement.Domain/
│   ├── Entities/
│   └── Common/
│
├── TaskManagement.Infrastructure/
│   ├── Data/
│   ├── Repositories/
│   └── Migrations/
│
└── TaskManagement.Tests/
</pre>

<hr/>

<h2>🛠️ Tech Stack</h2>

<h4>Backend</h4>
<ul>
  <li>ASP.NET Core 8 Web API</li>
  <li>C#</li>
  <li>Entity Framework Core</li>
  <li><b>In-Memory Database (Current)</b></li>
  <li>JWT Authentication</li>
  <li>Clean Architecture</li>
  <li>Repository + Unit of Work Pattern</li>
</ul>

<h4>Testing</h4>
<ul>
  <li>MSTest</li>
  <li>NSubstitute</li>
</ul>

<hr/>

<h2>🗄️ Database Configuration</h2>

<p>
  <b>Important Update:</b>
</p>

<ul>
  <li>Previously used <b>SQL Server</b></li>
  <li>Currently switched to <b>In-Memory Database</b></li>
</ul>

<p>
  <b>Reason:</b> Faster setup, no external dependency, ideal for demos and interviews.
</p>

<pre>
builder.Services.AddDbContext&lt;TaskManagementDbContext&gt;(options =>
    options.UseInMemoryDatabase("TaskManagementDb"));
</pre>

<hr/>

<h2>🔐 Authentication & Security</h2>

<ul>
  <li>JWT-based Authentication</li>
  <li>Custom JWT claim: <code>userId</code></li>
  <li>User ID extracted automatically from token</li>
  <li>Users can access <b>only their own tasks</b></li>
</ul>

<pre>
var userId = httpContextAccessor.HttpContext?
    .User.FindFirst("userId")?.Value;
</pre>

<hr/>

<h2>📦 Features</h2>

<h4>Task Management</h4>
<ul>
  <li>Create Task</li>
  <li>Get All Tasks (Admin)</li>
  <li>Get My Tasks (User)</li>
  <li>Pagination & Filtering</li>
  <li>Ownership validation</li>
</ul>

<h4>Infrastructure</h4>
<ul>
  <li>Global Exception Handling Middleware</li>
  <li>DTO Mapping</li>
  <li>Custom Mediator Pattern</li>
</ul>

<h4>Testing</h4>
<ul>
  <li>Unit tests for command handlers</li>
  <li>Mocking with NSubstitute</li>
</ul>

<hr/>

<h2>🔑 Sample User Credentials</h2>

<table border="1" cellpadding="8" cellspacing="0">
  <tr>
    <th>Username</th>
    <th>Password</th>
    <th>Role</th>
    <th>User ID</th>
  </tr>
  <tr>
    <td>admins</td>
    <td>admin1234</td>
    <td>Admin</td>
    <td>admin-001</td>
  </tr>
  <tr>
    <td>users</td>
    <td>user1234</td>
    <td>User</td>
    <td>user-001</td>
  </tr>
</table>

<hr/>

<h2>📄 API Endpoints</h2>

<h4>➕ Create Task</h4>
<pre>
POST /api/tasks
Authorization: Bearer &lt;token&gt;
</pre>

<pre>
{
  "name": "Complete API",
  "description": "Finish Task Management API"
}
</pre>

<h4>👤 Get My Tasks</h4>
<pre>
GET /api/tasks/my?page=1&recordsPerPage=10
Authorization: Bearer &lt;token&gt;
</pre>

<h4>📋 Get All Tasks (Admin)</h4>
<pre>
GET /api/tasks?page=1&recordsPerPage=10&title=test
Authorization: Bearer &lt;token&gt;
</pre>

<hr/>

<h2>🧪 Unit Test Example</h2>

<pre>
[TestMethod]
public async Task Handle_ValidCommand_ReturnsTaskId()
{
    var command = new CreateTasksCommand { Name = "Test Task" };
    var task = new TaskItem("Test Task");

    repository.Add(Arg.Any&lt;TaskItem&gt;()).Returns(task);

    var result = await handler.Handle(command);

    await repository.Received(1).Add(Arg.Any&lt;TaskItem&gt;());
    await unitOfWork.Received(1).Commit();

    Assert.AreEqual(task.Id, result);
}
</pre>

<hr/>

<h2>📌 Best Practices Followed</h2>

<ul>
  <li>Clean Architecture</li>
  <li>SOLID Principles</li>
  <li>Dependency Injection</li>
  <li>Secure JWT Handling</li>
  <li>Unit Testing</li>
</ul>

<hr/>
## ⚙️ How to Run the Project

### Prerequisites
- Dotnet 8

```bash
npm install -g @angular/cli

git clone https://github.com/AdeebAbubacker/TasksManagment

cd TasksManagment

dotnet restore

```
Then set TaskManagement.API as startup project and run

ng serve --open
<h2>👨‍💻 Author</h2>

<p>
  <b>Adeeb Abubacker</b><br/>
  GitHub:
  <a href="https://github.com/AdeebAbubacker" target="_blank">
    https://github.com/AdeebAbubacker
  </a>
</p>

<hr/>

<h2>✅ Conclusion</h2>

<p>
  This backend demonstrates a <b>Task Management API</b>
  using modern <b>ASP.NET Core</b>, secure JWT authentication,
  and clean architecture principles, fully integrated with an Angular frontend.
</p>
