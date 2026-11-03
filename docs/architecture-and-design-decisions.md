# Architectural & Design Decisions

This document outlines the core architectural patterns, design decisions, and technical rationales behind the **TaskFlow (ProjectManager)** API.

---

## 1. Architectural Patterns

### Service-Repository Pattern & `ServiceResult<T>`
To ensure separation of concerns and maintainability, the project adopts a layered **Service-Repository** architecture:
- **Controllers:** Strictly handle HTTP routing, model binding, authorization attributes, and translating service outcomes into semantic HTTP status codes (`200 OK`, `400 Bad Request`, `404 Not Found`, etc.).
- **Services:** Encapsulate core business logic, domain rules, validation orchestration, and workspace security enforcement.
- **Repositories:** Act as an abstraction layer over Entity Framework Core, isolating database operations.

To prevent controllers from dealing with raw exceptions or ambiguous boolean flags, business methods return a standardized generic wrapper: **`ServiceResult<T>`**. This wrapper explicitly encapsulates success states, payloads, and standardized error messages.

---

## 2. Column Management & Reordering Algorithm

### 1-Indexed Position Management
Columns within a board maintain sequential ordinal positions starting at `1` ($1, 2, \dots, N$). 

### Atomic Position Shifting (Shift Algorithm)
When a column's position is updated via `PUT /api/column/{columnId}`, a position shift algorithm handles peer reordering to prevent gaps or duplicate positions:
1. **Target Identification:** The system fetches the target column and compares its `positionBefore` with the requested `positionAfter`.
2. **Neighbor Filtering:** All peer columns belonging to the same board are retrieved, **excluding the column being updated** (`c.Id != columnId`) to avoid tracking conflicts.
3. **Shift Logic:**
   - **Moving Up (e.g., Position 4 to 2):** Neighbor columns whose positions fall between `positionAfter` and `positionBefore - 1` have their positions incremented (`position++`).
   - **Moving Down (e.g., Position 2 to 4):** Neighbor columns whose positions fall between `positionBefore + 1` and `positionAfter` have their positions decremented (`position--`).
4. **Persistence:** EF Core tracks these modifications within a single transaction scope, guaranteeing data integrity upon `SaveChangesAsync()`.

---

## 3. Authentication & Security: JWT vs. ASP.NET Core Identity

### Why We Omitted ASP.NET Core Identity
While ASP.NET Core Identity is a robust framework for monolithic MVC web applications, it was intentionally omitted in favor of a lightweight, custom security layer for the following reasons:
- **Statelessness & Microservices Readiness:** Identity relies heavily on cookie-based sessions, role tables, and heavy state tracking out-of-the-box. For a pure stateless RESTful API powered by JWT, Identity introduces unnecessary database overhead and complex schema footprints.
- **Full Control over Claims & Payloads:** Implementing manual token generation gives us complete authority over custom claim mapping (`sub`, `email`, `role`) without wrestling with Identity's abstract user managers.
- **Reduced Coupling:** Decoupling user authentication from a rigid framework allows direct manipulation of User entities, cryptographic hashing, and repository queries, keeping the domain model clean and streamlined.

### Stateless JWT Mechanism
- Users authenticate via credentials, and upon validation, the API issues a signed JSON Web Token containing cryptographically verified claims.
- Endpoints are protected via standard `[Authorize]` attributes, validating bearer tokens statelessly on each HTTP request.
- Passwords are never stored in cleartext; they are strictly secured using salted cryptographic hashing.

---

## 4. API Documentation & Schema Standards

- **OpenAPI / Scalar Integration:** Interactive API documentation is served via Scalar, providing an automated UI for testing routes.
- **DTO Rigor:** Data Transfer Objects are implemented using C# Records with explicit `{ get; init; }` properties and DataAnnotations (`[Required]`, `[Range]`, `[StringLength]`), ensuring precise JSON model binding and robust OpenAPI schema generation.