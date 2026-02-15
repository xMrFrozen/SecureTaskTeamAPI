# SecureTaskTeamAPI
A secure backend API for team-based task management, focusing on authentication, authorization, SQLite, and modern API development standards.

## Status
This project is under active development and is being continuously improved.

## Tech Stack
- Framework: ASP.NET Core (.NET 8.0)

- Database: SQLite

- ORM: Entity Framework Core

- Security: BCrypt.Net (password hashing)

## Features
- User registration and user login

- Bcrypt Hashing: Secure password storage.

- SQLite & EF Core: Local database and ORM integration.

- JWT Auth: Token-based security.

- Task CRUD: Full task management.

- Ownership Logic: User-specific data isolation.

- Smart Update: Partial data preservation on PUT.
  
- Task deadlines: Due dates and notifications.

- Categories: Task organization and labels.

- RBAC: Advanced role permissions.

## Planned features
- Team System: Multi-user collaboration.

## Getting Started
1. Clone the repository:
   git clone https://github.com/xMrFrozen/SecureTaskTeamAPI.git

2. Navigate to the project folder:
   cd SecureTaskTeamAPI

3. Run the application:
   dotnet run

The SQLite database is created automatically on first run.

## API Documentation
Once the application is running, open /swagger in your browser to explore the API. (for example: https://localhost:xxxx/swagger)
