# CarWashManagement

CarWashManagement is a small ASP.NET Core project for managing a self-service car wash.

The application manages customers, vehicles, six wash stations and wash transactions. When a wash starts, the system assigns an available station, tracks the wash time and frees the station when the wash finishes.

A simple web dashboard is included for managing the system without having to interact directly with the API.

## Features

 Customer and vehicle management
 Six fixed wash stations
 Automatic station assignment
 Basic, Premium and Deluxe wash programs
 Wash countdowns
 Manual and automatic wash completion
 Station availability tracking
 Transaction history
 Customer and vehicle deletion
 SQLite database
 Swagger API documentation
 Simple browser dashboard

## Tech Stack

Backend:
C#, .NET 10, ASP.NET Core Web API, Entity Framework Core, SQLite, Swagger

Frontend:
HTML, CSS, JavaScript

Tools:
VS Code, Git, GitHub

## How the project works

The backend follows a simple structure:

Dashboard / Swagger
 Controllers
 Services
 CarWashDbContext
 Entity Framework Core
 SQLite

Controllers handle API requests.

Services contain the main business logic.

CarWashDbContext handles the database.

DTOs are used for API input and output so the API models stay separate from the database models.

## Main Models

### Customer

A customer has:

 Guid Id
 FirstName
 LastName
 Email
 PhoneNumber
 CreatedAt

A customer can have multiple vehicles.

### Vehicle

A vehicle has:

 LicensePlate
 CustomerId
 Make
 Model
 Year

The license plate is used as the primary key, so vehicles do not need an extra Guid.

### WashStation

The application has exactly six physical wash stations.

Each station has:

 StationNumber
 Status
 IsActive

Possible station statuses:

 Available
 Occupied
 OutOfService
 Maintenance

Stations 1 to 6 are created automatically when the database is initialized.

### Wash Programs

The available wash programs are:

 Basic
 Premium
 Deluxe

They are stored as an enum instead of a database table.

Current durations:

Basic: 5 minutes
Premium: 8 minutes
Deluxe: 12 minutes

### WashTransaction

A transaction stores:

 Guid Id
 CustomerId
 LicensePlate
 WashProgram
 StationNumber
 Status
 StartedAt
 CompletedAt

Transaction statuses are InProgress, Completed and Cancelled.

## Wash Flow

When a wash starts, the system checks that the vehicle belongs to the customer and finds the first available active station.

The transaction is created with InProgress status and the station becomes Occupied.

If all six stations are busy, the API returns 409 Conflict.

The wash can finish in two ways.

The user can press Complete Wash manually, or the background service completes it automatically after the program duration expires.

When the wash finishes:

 the transaction becomes Completed
 CompletedAt is saved
 the station becomes Available again

The frontend countdown is only for display. The backend is responsible for the actual completion of the wash.

## Dashboard

The dashboard can be used to:

 add customers
 add vehicles
 view customer details
 view customer Guid and phone number
 remove customers and vehicles
 view station status
 start a wash
 view active washes
 view countdown timers
 complete washes manually
 view recent transactions

## API

Main endpoints:

Customers

GET /api/Customers
GET /api/Customers/{id}
POST /api/Customers
DELETE /api/Customers/{id}

Vehicles

GET /api/Vehicle
GET /api/Vehicle/{licensePlate}
POST /api/Vehicle
DELETE /api/Vehicle/{licensePlate}

Wash Stations

GET /api/WashStation
GET /api/WashStation/{stationNumber}

Wash Transactions

GET /api/WashTransaction
GET /api/WashTransaction/{id}
POST /api/WashTransaction/start
POST /api/WashTransaction/{id}/complete

## Running the project

Requirements:

 .NET 10 SDK
 Git

Clone the repository and open the API folder.

Run:

dotnet restore

Then apply the database migrations:

dotnet ef database update

Start the application with:

dotnet run

The project normally runs at:

http://localhost:5144

Swagger is available at:

http://localhost:5144/swagger

The main dashboard is available from the root address.

## Project Structure

CarWashManagement.Api/

Controllers
Data
DTOs
Models
Services
Migrations
wwwroot
Program.cs
appsettings.json

Controllers contain API endpoints.

Services contain the application logic.

Models contain the main entities.

DTOs contain API request and response objects.

Data contains the DbContext and database initialization.

wwwroot contains the dashboard HTML, CSS and JavaScript.

## Design Choices

Vehicles use the license plate as their identifier because adding another generated ID would not give us much value.

Wash stations use StationNumber because they represent six real physical stations.

Wash programs use an enum because the available programs are fixed.

Transactions use Guids because every wash represents a unique historical event.

## Current Status

The main workflow is working:

 customer management
 vehicle management
 station management
 station assignment
 wash transactions
 manual completion
 automatic completion
 countdown timer
 station release
 transaction history
 dashboard
 Swagger
 SQLite persistence

## Planned Improvements

Later improvements may include:

 protecting historical transactions when deleting customers or vehicles
 better API validation
 better error messages
 automated tests
 logging
 more dashboard improvements

## Purpose

This project was created to practice backend development with ASP.NET Core, Entity Framework Core, REST APIs, dependency injection, database relationships and business logic but also because as of now, I am currently employed by a self-service car-wash company in Greece. It basically works on the same principles and was designed to simulate the car-wash functions.

I kept the architecture simple so the code is easy to understand and explain.
