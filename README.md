# HotelApp 🏨

A simple hotel booking web application built with ASP.NET Core Razor Pages and SQL Server. This project demonstrates the fundamentals of building a multi-layered web application, including data access, business logic, and a user-friendly web interface.

## Project Structure

- **HotelApp.Web**: The main ASP.NET Core web application (Razor Pages)
- **HotelAppLibrary**: Business logic and data access layer
- **HotelAppDB**: SQL Server database project with tables and stored procedures

## Core Learning Concepts

- ASP.NET Core Razor Pages structure and routing
- Dependency Injection in .NET
- Data access with Dapper and SQL Server stored procedures
- Separation of concerns (Web, Business Logic, Data Access)
- Basic front-end integration with Bootstrap and jQuery
- Using Entity Framework for database migrations (if applicable)

## Getting Started

1. Clone the repository
2. Set up the SQL Server database using the scripts in `HotelAppDB`
3. Update connection strings in `HotelApp.Web/appsettings.json`
4. Build and run the web project (`HotelApp.Web`)

## Screenshot

![Screenshot](screenshot.jpg)

## License

This project is for educational purposes.
