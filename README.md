# TaskFlow - Advanced Task Manager API

A robust, enterprise-grade RESTful Web API designed to mimic core Trello and Jira workflows. This project is built using **ASP.NET Core**, **Entity Framework Core**, and modern decoupled architecture standards, focusing heavily on stateless security, data integrity, and scalable backend design patterns.

## 🚀 Technologies and Libraries

- **Framework:** ASP.NET Core Web API (REST Architecture)
- **Language Runtime:** .NET 10.0 (LTS)
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Security:** JWT (JSON Web Tokens) & Cryptographic Hashing
- **API Documentation:** Scalar / OpenAPI (Swagger)

## ✨ Key Features

### 🔐 Authentication & Security
- **Stateless Authentication:** Secure user authentication powered by custom **JWT (JSON Web Tokens)** containing standard claims (`sub`, `email`, `role`).
- **Custom Security Architecture:** Built without ASP.NET Core Identity to maintain a lightweight, fully decoupled, and stateless JWT mechanism tailored for microservices/APIs.
- **Cryptographic Hashing:** Processes and stores passwords securely using salted cryptographic hashing.

### 📋 Columns Management & Reordering Algorithm
- **1-Indexed Position Management:** Strict ordinal position enforcement ensuring columns maintain sequential index structures ($1, 2, \dots, N$).
- **Atomic Position Shifting:** Built-in shift algorithm during `PUT` updates that automatically increments or decrements peer column positions to eliminate gaps or duplicates.
- **Workspace Security & Scope:** Strict ownership validation ensuring users can only read, create, or reorder columns belonging to boards within their own workspace.

### 🛡️ Data Integrity & Architecture
- **ServiceResult<T> Pattern:** Standardized application response wrapper decoupling HTTP status handling from core business logic.
- **Explicit DTO Validation:** Strong schema constraints using C# Records with explicit `{ get; init; }` properties and DataAnnotations (`[Required]`, `[StringLength]`).
- **Entity Framework Core Change Tracker:** Leverages EF Core tracking mechanism for atomic, single-transaction batch updates.

---

## 🏗️ Architecture & Design Patterns

The project follows a clean, layered architecture separating HTTP concerns from domain logic:

```text
[ Controller Layer ] -> Handles HTTP Routing, Model Binding & Status Codes
         │
[ Service Layer ]    -> Encapsulates Business Logic, Reordering Algorithms & Authorization
         │
[ Repository Layer ] -> Manages Entity Framework Data Context & Persistence Operations

````
> 📖 **Deep Dive Documentation:** For a detailed breakdown on why ASP.NET Core Identity was omitted, how the position shift algorithm works, and DTO serialization decisions, check out our [Architectural & Design Decisions Document](./docs/architecture-and-design-decisions.md).

## ⚙️ How to Run the Project

1. Clone the repository:

```Bash
git clone [https://github.com/JoaoPauloDonatoJ/ProjectManager.git](https://github.com/JoaoPauloDonatoJ/ProjectManager.git)
```

2. Navigate to the project folder:

```Bash
cd ProjectManager
```

3. Configure the database connection (User Secrets):
  
This project utilizes User Secrets to safely isolate sensitive configuration parameters from the source code.

Initialize User Secrets:

```Bash
dotnet user-secrets init
```

Set your SQL Server connection string:

```Bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER;Database=YOUR_DB;Trusted_Connection=True;TrustServerCertificate=True"
```
4. Apply database migrations:

```Bash
dotnet ef database update
```

5. Run the application:

```Bash
dotnet run
```

Explore the API:

Once running, access the interactive OpenAPI/Scalar documentation terminal in your browser (e.g., https://localhost:7099/scalar/v1).
