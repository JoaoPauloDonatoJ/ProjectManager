# TaskFlow - Advanced Task Manager API

A robust, enterprise-grade RESTful Web API designed to mimic core Trello and Jira workflows. This project is built using **ASP.NET Core**, **Entity Framework Core**, and modern decoupled architecture standards, focusing heavily on stateless security, data integrity, and scalable backend design patterns.

## 🚀 Technologies and Libraries

* **Framework:** ASP.NET Core Web API (REST Architecture)
* **Language Runtime:** .NET 10.0 (LTS)
* **ORM:** Entity Framework Core
* **Database:** SQL Server
* **Security:** JWT (JSON Web Tokens) & Cryptographic Hashing

## ✨ Key Features (Sprint 1 Foundations)

* **Stateless Authentication:** Secure user authentication powered by **JWT (JSON Web Tokens)** containing industry-standard claims (`sub`, `email`, `role`).
* **Data Integrity Rules:** Strict Fluent API constraints enforcing maximum character boundaries (`nvarchar(150)`) and data uniqueness indexes directly at the database layer (e.g., Duplicate Email Prevention).
* **Decoupled Architecture Foundations:** Native Cross-Origin Resource Sharing (CORS) security configuration implemented to isolate and securely authorize incoming HTTP traffic from frontend Single Page Applications (SPA).
* **Cryptographic Security:** Designed to isolate raw user credentials by processing and storing passwords exclusively as secure, cryptographic hashes rather than cleartext.

## 🏗️ Architecture & Design Patterns

The backend repository follows a clean, decoupled service architecture tailored for high-performance API interactions:
* **Controller Layer:** High-level endpoint routing using specialized API Controllers to handle incoming JSON payloads, returning semantic REST HTTP responses (`200 OK`, `401 Unauthorized`, `400 Bad Request`).
* **Data Layer:** Centralized database orchestration utilizing an isolated `AppDbContext` to map strongly-typed entity configurations via Fluent API code mappings.

---

## ⚙️ How to run the project

1. **Clone the repository**
  
    ```bash
    git clone [https://github.com/JoaoPauloDonatoJ/ProjectManager.git](https://github.com/JoaoPauloDonatoJ/ProjectManager.git)
    ```

2. **Navigate to the project folder:**
   
   Example:
   ```bash
   cd ProjectManager
    
3. **Configure the database connection (User Secrets)**
   
   This project utilizes User Secrets to safely isolate sensitive configuration parameters from the source code.
   
   Initialize and set your SQL Server connection string:
   ```bash
   dotnet user-secrets init
   ```
   Now set your connection string:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING"
   ```
   Example:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER;Database=YOUR_DB;Trusted_Connection=True;TrustServerCertificate=True"
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```
   This will create the database and tables automatically.
   
5. **Run the application**
   ```bash
   dotnet run
   ```
   The application bootstrapper will compile the source code and host the local HTTP/HTTPS development servers.

   You can interact with and test the endpoints directly via the generated OpenAPI/Swagger UI 
   through the default local URL displayed in your command prompt terminal (e.g., https://localhost:7001/swagger).

   Example Default URL:
   ```bash
   https://localhost:7001/swagger
   ```
