"# WMS-Project

A comprehensive Warehouse Management System (WMS) built with a .NET Core backend and a modern React frontend. This application facilitates efficient management of inventory, warehouses, users, and transactions in a warehouse environment.

Demo : http://wms-frontend.runasp.net/

username : binary
password : 12345678


## Features

- **Inventory Management**: Track items, categories, units, and stock levels across multiple warehouses.
- **Warehouse Operations**: Manage warehouse transfers, stock adjustments, and audit trails.
- **User Management**: Role-based access control for users and personnel.
- **Reporting**: Generate reports on transactions, stock levels, and system activities.
- **Real-time Updates**: Seamless integration between frontend and backend for live data synchronization.
- **Responsive UI**: Modern, mobile-friendly interface built with React and Tailwind CSS.

## Installation

### Prerequisites
- .NET 8.0 
- SQL Server or another supported database

### Backend Setup
1. Navigate to the `WMS-Project/WMS` directory.
2. Restore dependencies:
   ```
   dotnet restore
   ```
3. Update the connection string in `WMS.Presentation/appsettings.json`.
4. Run database migrations:
   ```
   dotnet ef database update
   ```
5. Start the backend server:
   ```
   dotnet run --project WMS.Presentation
   ```

### Frontend Setup
1. Navigate to the `WMS-YDC` directory.
2. Install dependencies:
   ```
   npm install
   ```
3. Start the development server:
   ```
   npm run dev
   ```

The application will be available at `http://localhost:5173` for the frontend and typically `https://localhost:5001` for the backend API.

## Technologies Used

### Backend
- **C#**: Primary programming language
- **.NET Core**: Framework for building the API
- **Entity Framework Core**: ORM for database operations
- **ASP.NET Core**: Web framework for API development
- **SQL Server**: Database management system

### Frontend
- **React**: JavaScript library for building user interfaces
- **TypeScript**: Typed superset of JavaScript
- **Vite**: Build tool and development server
- **Tailwind CSS**: Utility-first CSS framework
- **Axios**: HTTP client for API requests

## Architecture

This project follows the **Clean Architecture** principles to ensure maintainability, testability, and separation of concerns. Clean Architecture emphasizes the independence of the business logic from external frameworks and details.

### Backend Architecture Layers

- **Domain Layer (WMS.Domain)**: Contains the core business entities, value objects, and domain interfaces. This layer is independent of any external frameworks and defines the business rules and logic.
  
- **Application Layer (WMS.Application)**: Implements application-specific logic, such as services, use cases, and DTOs. It orchestrates the domain objects and handles cross-cutting concerns like validation and mapping.

- **Infrastructure Layer (WMS.Infrastructure)**: Provides implementations for external concerns, including data persistence (using Entity Framework Core), external APIs, and other infrastructure services. This layer depends on the Application and Domain layers.

- **Presentation Layer (WMS.Presentation)**: Contains the API controllers, middleware, and configuration for the ASP.NET Core web application. It acts as the entry point for external requests and translates them into application commands.

### Key Principles Applied

- **Dependency Inversion**: Higher-level modules (Domain and Application) do not depend on lower-level modules (Infrastructure and Presentation). Instead, they depend on abstractions defined in the Domain layer.
- **Separation of Concerns**: Each layer has a specific responsibility, making the codebase easier to maintain and test.
- **Testability**: The architecture allows for easy unit testing of business logic without external dependencies.
- **Framework Independence**: The core business logic is not tied to any specific framework, allowing for easier migrations or changes in the future.

The frontend uses a component-based architecture with React, organized into reusable components, pages, and layouts for a modular and scalable UI.

## Folder Structure

```
WMS-Project/
├── WMS/
│   ├── WMS.Application/          # Application layer with services and DTOs
│   ├── WMS.Domain/               # Domain entities and interfaces
│   ├── WMS.Infrastructure/       # Data access and configurations
│   └── WMS.Presentation/         # API controllers and presentation logic
└── WMS-YDC/                      # Frontend React application
    ├── src/
    │   ├── components/           # Reusable UI components
    │   ├── pages/                # Application pages
    │   ├── core/                 # Core utilities and services
    │   └── layouts/              # Layout components
    ├── public/                   # Static assets
    └── ...                       # Configuration files
```

## Future Improvements

- Implement real-time notifications using WebSockets or SignalR.
- Add barcode scanning functionality for inventory management.
- Integrate with third-party logistics APIs.
- Enhance reporting with advanced analytics and dashboards.
- Implement multi-language support (i18n).
- Add automated testing suites for both backend and frontend.
- Optimize performance for large-scale warehouse operations.
