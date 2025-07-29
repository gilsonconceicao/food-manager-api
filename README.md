# 🍽️ Food Ordering API

RESTful API built with **ASP.NET Core (.NET 8)** for managing foods, orders and payments.

## ✨ Key Features

- CRUD for **Foods** and **Orders**
- **PostgreSQL** persistence via Entity Framework Core
- **Firebase Authentication** (email/password & social providers)
- **MercadoPago** integration for checkout and webhook processing
- **Hangfire** background jobs:
  - automatic order status workflow (Paid → InPreparation after 2 min, etc.)
  - recurring clean‑ups & email notifications
- Clean Architecture + CQRS pattern
- Docker‑first: run the whole stack with one command

## 🛠️ Tech Stack

| Layer        | Technology                         |
|--------------|----------------------------------- |
| API          | ASP.NET Core 9, Minimal APIs       |
| Data         | PostgreSQL 16, EF Core, Migrations |
| Auth         | Firebase Admin SDK                 |
| Background   | Hangfire + PostgreSQL storage      |
| Payments     | MercadoPago .NET SDK               |
| DevOps       | Docker, Docker Compose, GitHub Actions |

## 📂 Project Structure

```text
src/
 ├── FoodOrdering.Api          <-- Presentation layer
 ├── FoodOrdering.Application  <-- CQRS handlers, DTOs
 ├── FoodOrdering.Domain       <-- Entities & enums
 ├── FoodOrdering.Infrastructure <-- EF Core, Firebase, MercadoPago, Hangfire
 └── FoodOrdering.Integration <-- integration layer

docker/
 src/Api
 ├── dockerfile

```
project link: https://github.com/gilsonconceicao/food-maneger-front-end

<img width="1614" height="901" alt="image" src="https://github.com/user-attachments/assets/9d3318de-9b66-4163-835e-07c3cce1fe4e" />

