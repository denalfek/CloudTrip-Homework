# CloudTrip-Homework ✈️

CloudTrip-Homework is a C#/.NET-based REST API that aggregates flight data from multiple (mock) providers, offering search, filtering, sorting, and booking functionalities. Built with scalability in mind, it includes caching, logging, and a standardized response format.

Features:

✅ Aggregates flight data from multiple sources (mock providers)

✅ Supports filtering (date, price, stops, airline) and sorting

✅ Simulates flight booking requests

✅ Implements caching for performance optimization

✅ Logs all API requests for future analysis

✅ Designed following RESTful principles with OpenAPI documentation

This project is a home assignment, showcasing clean architecture and best practices in API development.

# Architecture & Progress Overview

## ✅ Implemented Features

- **Authentication API**  
  Endpoints for user registration and JWT-based authentication.

- **Client App (Angular)**  
  A basic client-side project scaffolded for future development and testing of authentication and data filtering features.

- **Mocked Flight Providers**  
  Two independent mock providers simulating integration with external flight data systems.  
  Each provider exposes its own models and endpoint structure, mimicking real-world inconsistency between APIs.

- **Result Caching**  
  In-memory caching using Redis for flight data from providers to improve response times and reduce provider load.  
  *Note:* No distributed lock is currently implemented to manage cache updates in horizontally scaled scenarios.

- **Adapter Pattern**  
  Adapters are used to normalize and integrate diverse provider response models without forcing a shared contract.

- **Layered Architecture**  
  The system is divided into multiple layers:
  - `Web` – API endpoints and client app.
  - `Application` – Shared DTOs and minimal contracts between layers.
  - `Domain` – Business logic layer.
  - `Infrastructure` – Data access (MongoDB, Redis), provider integrations.

- **Clean Code Practices**  
  Implementation classes are marked `internal` to encapsulate details and `sealed` to improve runtime performance where inheritance is not needed.

---

## 🔜 Planned Improvements

- **Booking API**  
  Add functionality to allow authenticated users to book flights.

- **Client-side Authentication Flow**  
  Implement login flow and ability to filter/search flights via UI.

- **Logging**  
  Integrate Serilog for structured, performant logging.

- **CI/CD Pipeline**  
  Add automated build and deployment processes.

- **Unit testing**  
  Implement project with tests that covered business logic.
---

## 🔎 Notes

- **Configuration Management**  
  Configuration values (e.g., secrets, connection strings) are currently hardcoded for development speed. In production, these should be externalized via configuration files or secret management.

- **Interface Placement**  
  Interfaces are currently located alongside their consumers (e.g., repositories, services) instead of a dedicated shared layer. This can be improved later for clearer separation of concerns.

- **Scalability Considerations**  
  Redis caching is in place, but distributed locking and cache invalidation are not yet implemented for scaled environments.
