# Finance Management System

A robust console-based application for personal finance management, built with C# and .NET. This project demonstrates the implementation of a clean, N-tier architecture (Domain, DAL, BLL, PL) using modern software engineering patterns.

## Features
* **Accounts Management:** Create and update financial accounts, track balances.
* **Categories Management:** Manage transaction categories, distinctly separated into `Income` and `Expense`.
* **Transaction Tracking:** Record financial operations seamlessly. 
* **Financial Analytics:** View detailed statistics, including total income, total expenses, net balance, and breakdowns by specific categories.

##  Architecture & Technologies
The application is strictly separated into 4 layers to ensure scalability, maintainability, and loose coupling:

1. **Domain Layer:** Contains core Entities (`Account`, `Category`, `Transaction`) and domain interfaces (`IRepository`, `IUnitOfWork`).
2. **Data Access Layer (DAL):** Implements `GenericRepository` and `UnitOfWork` using **Entity Framework Core** for database operations.
3. **Business Logic Layer (BLL):** Contains `FinanceService` which handles core logic, validations, and maps entities to **DTOs** (Data Transfer Objects) using **AutoMapper**.
4. **Presentation Layer (PL):** A user-friendly Console UI that interacts exclusively with the BLL via interfaces (Dependency Injection).
