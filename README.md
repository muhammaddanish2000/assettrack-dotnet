# AssetTrack

AssetTrack is a complete IT asset management REST API built using C#, ASP.NET Core, Entity Framework Core and SQLite.

The system helps organizations track laptops, monitors, projectors and other equipment, including assignment information, physical locations, purchase details, maintenance status and asset analytics.

---

## Project Overview

AssetTrack is designed as a backend portfolio project that demonstrates practical enterprise-style development using the .NET ecosystem.

The application provides a REST API for managing company assets and includes searching, filtering, analytics, CSV exports, validation and automated tests.

The database is stored locally using SQLite, making the project easy to run without requiring a separate database server.

---

## Main Features

- ASP.NET Core REST API
- C# backend development
- Entity Framework Core
- SQLite persistent database
- Complete CRUD operations
- Asset tracking
- Asset assignment tracking
- Asset status management
- Search functionality
- Category filtering
- Status filtering
- Asset analytics
- Total asset value calculation
- Asset utilisation calculation
- Category breakdown
- CSV export
- Swagger API documentation
- Input validation
- Unique asset tag validation
- Sample seed data
- xUnit automated tests
- Responsive frontend-ready REST architecture

---

## Technology Stack

```text
C#
.NET 8
ASP.NET Core
Entity Framework Core
SQLite
Swagger / OpenAPI
xUnit
REST API
```

---

## Project Structure

```text
assettrack-dotnet/
│
├── .gitignore
├── README.md
│
├── src/
│   └── AssetTrack.Api/
│       ├── AssetTrack.Api.csproj
│       ├── Program.cs
│       ├── appsettings.json
│       │
│       ├── Controllers/
│       │   ├── AssetsController.cs
│       │   └── AnalyticsController.cs
│       │
│       ├── Data/
│       │   └── AssetDbContext.cs
│       │
│       ├── DTOs/
│       │   ├── AssetCreateRequest.cs
│       │   └── AssetUpdateRequest.cs
│       │
│       ├── Models/
│       │   ├── Asset.cs
│       │   └── AssetStatus.cs
│       │
│       └── Services/
│           └── AssetService.cs
│
└── tests/
    └── AssetTrack.Tests/
        ├── AssetTrack.Tests.csproj
        └── AssetServiceTests.cs
```

---

# Asset Information

Each asset stores:

```text
ID
Asset Name
Asset Tag
Category
Assigned Person / Team
Location
Purchase Date
Purchase Price
Status
Notes
Created Date
Updated Date
```

---

# Asset Statuses

The application supports four statuses:

```text
Available
Assigned
Maintenance
Retired
```

---

# API Endpoints

## Health Check

```http
GET /api/health
```

Example:

```text
http://localhost:5000/api/health
```

---

# Assets

## Get All Assets

```http
GET /api/assets
```

---

## Get Asset by ID

```http
GET /api/assets/{id}
```

Example:

```http
GET /api/assets/1
```

---

## Search Assets

```http
GET /api/assets?search=laptop
```

Search checks:

- Asset name
- Asset tag
- Category
- Assigned person
- Location

---

## Filter by Category

```http
GET /api/assets?category=Laptop
```

---

## Filter by Status

```http
GET /api/assets?status=Assigned
```

---

## Combine Search and Filters

```http
GET /api/assets?search=office&category=Laptop&status=Assigned
```

---

# Create Asset

```http
POST /api/assets
```

Example body:

```json
{
  "name": "Dell Latitude 7450",
  "assetTag": "IT-0100",
  "category": "Laptop",
  "assignedTo": "Development Team",
  "location": "Main Office",
  "purchaseDate": "2026-01-15",
  "purchasePrice": 5200,
  "status": "Assigned",
  "notes": "Primary development laptop."
}
```

---

# Update Asset

```http
PUT /api/assets/{id}
```

Example:

```http
PUT /api/assets/1
```

Example body:

```json
{
  "name": "Dell Latitude 7450",
  "assetTag": "IT-0100",
  "category": "Laptop",
  "assignedTo": "Development Team",
  "location": "Level 3 Office",
  "purchaseDate": "2026-01-15",
  "purchasePrice": 5200,
  "status": "Assigned",
  "notes": "Assigned to new development workstation."
}
```

---

# Update Status Only

```http
PATCH /api/assets/{id}/status?status=Maintenance
```

Example:

```text
http://localhost:5000/api/assets/1/status?status=Maintenance
```

---

# Delete Asset

```http
DELETE /api/assets/{id}
```

---

# Export Assets to CSV

```http
GET /api/assets/export
```

This endpoint generates a downloadable CSV file containing all asset records.

Example output:

```text
Id,AssetTag,Name,Category,Status,AssignedTo,Location,PurchaseDate,PurchasePrice
1,"IT-0001","MacBook Pro","Laptop","Assigned","Web Development","Main Office","2025-01-15",6999.00
```

---

# Analytics

## Dashboard Summary

```http
GET /api/analytics/summary
```

Example response:

```json
{
  "totalAssets": 12,
  "available": 3,
  "assigned": 6,
  "maintenance": 2,
  "retired": 1,
  "totalAssetValue": 38500.00,
  "utilisationRate": 54.55
}
```

---

## Category Analytics

```http
GET /api/analytics/categories
```

Example response:

```json
[
  {
    "category": "Laptop",
    "total": 6,
    "totalValue": 28000
  },
  {
    "category": "Monitor",
    "total": 4,
    "totalValue": 4200
  }
]
```

---

# How to Run

## Requirements

Install:

```text
.NET SDK 8 or above
```

Check your installation:

```bash
dotnet --version
```

---

# Clone Repository

```bash
git clone https://github.com/your-username/assettrack-dotnet.git
```

Enter the project folder:

```bash
cd assettrack-dotnet
```

---

# Restore Packages

```bash
dotnet restore src/AssetTrack.Api/AssetTrack.Api.csproj
```

---

# Run Application

```bash
dotnet run --project src/AssetTrack.Api
```

The terminal will display the application URL.

Usually:

```text
http://localhost:5000
```

or:

```text
https://localhost:7000
```

---

# Swagger API Documentation

Run the application in Development mode and open:

```text
/swagger
```

Example:

```text
http://localhost:5000/swagger
```

Swagger allows you to test every API endpoint directly from your browser.

---

# Database

AssetTrack uses:

```text
SQLite
```

The database is automatically created as:

```text
assettrack.db
```

No MySQL or PostgreSQL server is required.

---

# Sample Data

When the database is empty, AssetTrack automatically creates sample records such as:

```text
MacBook Pro
Dell 24-inch Monitor
Lenovo ThinkPad
Epson Projector
```

This allows the application to be tested immediately.

---

# Running Tests

Run:

```bash
dotnet test tests/AssetTrack.Tests
```

The test suite checks functionality such as:

```text
Creating assets
Preventing duplicate asset tags
Deleting assets
Updating asset status
```

---

# Create Solution File

Optional but recommended:

```bash
dotnet new sln -n AssetTrack
```

Add the API project:

```bash
dotnet sln AssetTrack.sln add src/AssetTrack.Api/AssetTrack.Api.csproj
```

Add the test project:

```bash
dotnet sln AssetTrack.sln add tests/AssetTrack.Tests/AssetTrack.Tests.csproj
```

You can then build everything using:

```bash
dotnet build AssetTrack.sln
```

And test everything using:

```bash
dotnet test AssetTrack.sln
```

---

# Architecture

AssetTrack uses a layered backend architecture:

```text
HTTP Request
     ↓
Controller
     ↓
Service
     ↓
Entity Framework Core
     ↓
SQLite Database
```

### Controllers

Handle HTTP requests and responses.

### Services

Contain application business logic.

### DTOs

Validate data coming from API clients.

### Models

Represent database entities.

### DbContext

Manages database access using Entity Framework Core.

---

# Project Category

```text
C# Backend Application
ASP.NET Core REST API
Asset Management System
Enterprise CRUD Application
SQLite Database Project
```

---

# Repository Description

Use this in GitHub:

```text
A C# ASP.NET Core asset management REST API for tracking equipment, assignments, maintenance status, analytics and CSV exports using SQLite.
```

---

# GitHub Topics

```text
csharp
dotnet
aspnet-core
entity-framework-core
sqlite
rest-api
asset-management
backend
crud-api
xunit
swagger
inventory-management
portfolio
```

---

# Suggested First Commit

```text
feat(api): add complete ASP.NET Core asset management system
```

---

# Suggested Future Commits

```text
docs(readme): add complete asset management API documentation

feat(search): add asset search and filtering

feat(analytics): add asset utilisation analytics

feat(export): add CSV asset export

feat(status): add maintenance and retirement tracking

test(service): add asset service unit tests

feat(auth): add JWT authentication

feat(audit): add asset history tracking

feat(import): add CSV asset import

feat(pagination): add paginated asset results
```

---

# Future Improvements

Possible future improvements include:

- JWT authentication
- Administrator accounts
- User roles
- Department management
- Asset assignment history
- Asset return workflow
- Maintenance records
- Maintenance cost tracking
- Warranty expiry tracking
- Serial numbers
- Vendor management
- QR code generation
- Barcode scanning
- CSV import
- Excel export
- PDF reports
- Pagination
- Sorting
- Advanced search
- Notifications
- Email reminders
- PostgreSQL support
- Docker support
- React frontend
- Blazor dashboard
- Azure deployment
- GitHub Actions CI/CD

---

# Learning Outcomes

This repository demonstrates:

- C#
- Object-oriented programming
- ASP.NET Core
- REST API development
- Entity Framework Core
- SQLite
- LINQ
- Dependency injection
- CRUD operations
- Service architecture
- API validation
- Swagger/OpenAPI
- CSV generation
- Unit testing
- xUnit
- Database design
- Backend architecture

---

# Author

Muhammad Danish

Web Developer  
Computer Science Background

---

# License

This project is open-source and can be used for educational, portfolio and personal development purposes.
