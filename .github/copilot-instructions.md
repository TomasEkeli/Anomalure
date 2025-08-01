---
applyTo: "*"
---

# Development Instructions for Anomalure

## Project Overview
This is a .NET Razor application that uses HTMX for dynamic, interactive web components. The project avoids MVC in favor of Razor Pages, offering a balance between simplicity and functionality. The focus is on clean, maintainable code.

## Coding Guidelines

### General Principles
- **Clean Code**: Write code that is self-explanatory and avoids unnecessary comments.
- **Small Files**: Keep files small and focused on a single responsibility.
- **Short Lines**: Limit lines to a maximum of 80 characters for readability across multiple tools and devices.
- **Readable Names**: Use descriptive and meaningful names for variables, methods, and classes.
- **Consistent Formatting**: Follow consistent indentation and spacing. Format method parameters vertically for readability when needed.

### Razor Pages
- Use Razor Pages for structuring the application.
- Keep Razor Pages focused on a single feature or functionality.
- Use partial views and components to break down complex pages.

### HTMX Integration
- Use HTMX for dynamic interactions and partial updates.
- Keep HTMX requests lightweight and focused on specific actions.
- Use Razor Pages as endpoints for HTMX requests.
- HATEOAS as the guiding principle.

### Folder Structure
- Organize files logically by feature or functionality.
- Use feature-based folders, not functionality-based folders (i.e. keep all files related to a feature together).
- Pull out shared resources when needed.

### Code Style
- Follow .NET coding conventions.
- Use consistent indentation and spacing.
- Avoid magic numbers and hardcoded values; use constants or configuration files instead.
- Use `partial` classes for extensibility when appropriate.
- Integrate logging into methods to track operations.
- Prefer `async`/`await` for methods interacting with external resources.

### Dependency Injection
- Use dependency injection for services like `ILogger` and `IRsaKeyManager`.
- Implement appropriate service lifetimes (e.g., `ISingletonService`).

## Deployment
- Use Docker for containerized deployment.
- Configure CI/CD pipelines using GitHub Actions.
- Ensure environment-specific settings are managed via `appsettings.json` files.
