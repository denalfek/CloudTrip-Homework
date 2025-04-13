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

### 🔐 Authentication
- Simple login/password-based authentication was implemented.
- Booking endpoints are secured; read-only endpoints are publicly accessible.
- This design helps separate public data browsing from user-specific actions.

### 📦 Data Retrieval from Providers
- Implemented a provider-level integration returning available flight data.
- All available data is preloaded and shown on the homepage upon client launch.

### 🔎 Filtered Data Retrieval
- A dedicated endpoint supports querying flight data using filters (e.g., date, origin, destination).

### 🧠 Caching & Warm-Up
- Redis-based caching is used to store provider data.
- On application startup, a warm-up routine fetches and populates cache to improve first-load user experience.
- This design shows awareness of domain specifics: a flight site without data is meaningless to users.

### 🌐 Provider Simulation
- Providers are mocked with artificial `Task.Delay()` to simulate slow responses.
- `CancellationToken` is used throughout the controllers to demonstrate how timeout-aware processing and retry handling would work in real-world scenarios.
- This models how clients might cancel long-running requests (e.g., in browser apps).

### 🔁 Retry Mechanism
- A simple retry mechanism is implemented manually.
- Although in real production HTTP calls we would use `DelegatingHandler` with `Polly`, this implementation was made to showcase understanding of retry mechanics from scratch.

### 🚫 Booking is Not Idempotent
- The `Book` endpoint is intentionally non-idempotent.
- No retry logic is applied to booking operations, which would otherwise risk duplicate reservations.

### 🧪 Unit Testing
- Basic unit tests cover key logic in the booking and data provider services.

### 🗃️ Logging
- Serilog is configured to write structured logs to a dedicated MongoDB database.
- A TTL index is configured to automatically purge old logs.
- Logging is fully asynchronous and records all major service events.

### 💾 Immutable Collections
- `ImmutableList<T>` is used on the provider level to ensure thread-safety under parallel requests.
- This demonstrates an understanding of safe concurrent access, even when data isn’t updated in runtime.

### 🧪 Mocked Flight Providers
  Two independent mock providers simulating integration with external flight data systems.  
  Each provider exposes its own models and endpoint structure, mimicking real-world inconsistency between APIs.

### ⚡ Result Caching
  In-memory caching using Redis for flight data from providers to improve response times and reduce provider load.  
  *Note:* No distributed lock is currently implemented to manage cache updates in horizontally scaled scenarios.

### 🧱 Adapter Pattern
  Adapters are used to normalize and integrate diverse provider response models without forcing a shared contract.
  
### 🧩 Layered Architecture
  The system is divided into multiple layers:
  - `Web` – API endpoints and client app.
  - `Application` – Shared DTOs and minimal contracts between layers.
  - `Domain` – Business logic layer.
  - `Infrastructure` – Data access (MongoDB, Redis), provider integrations.

### 🧼 Clean Code Practices
  Implementation classes are marked `internal` to encapsulate details and `sealed` to improve runtime performance.

> ⚠️ Although deployment infrastructure is ready, there was not enough time to finalize and publish the live version. This was a demotivating limitation due to time constraints.

---

## 🔧 Planned Improvements

- 🔄 **Deployment**: Finalize cloud deployment to expose demo endpoints. Two environments (staging & production) were prepared in advance.
- 🌐 **Frontend**: Build a simple web frontend for booking/search flow. Not my primary focus, but still valuable for demo completeness.
- 🧩 **Logging Admin Panel**: Add a basic UI to display logs from MongoDB, especially useful for admin/debugging purposes.
- ⚙️ **Mongo Config Separation**: Split logging and main databases into separate config sections for clarity and maintainability.
- 👤 **Admin User Roles**: Introduce a separate user type for accessing admin panels.

---

## 🧠 Notes

This implementation emphasizes:
- Domain awareness (cold starts with no flights = poor UX)
- Infrastructure readiness (cloud-hosted services, warm cache, logging)
- Reliability techniques (manual retry, cancellation handling)
- Clean code and testability
