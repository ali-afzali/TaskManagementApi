# Task Management API

Small task management API built with `.NET 8` and `ASP.NET Core`.

This repository contains the backend only.

## What this project does

- JWT-based login
- Authenticated task CRUD
- Task assignment tracking
- Swagger for local testing

## What is implemented

### Authentication

- `POST /Authentication/login`
- `POST /Authentication/logout`
- Passwords are hashed with `BCrypt`
- Task endpoints require a bearer token

### Tasks

- `GET /Tasks`
- `GET /Tasks/{id}`
- `POST /Tasks`
- `PUT /Tasks/{id}`
- `DELETE /Tasks/{id}`

Each task includes:

- `title`
- `description`
- `status`
- `assigneeUserId`
- `createdDate`
- `updatedDate`
- `createdByUserId`
- `updatedByUserId`

### Validation

- Task title is required
- Standard HTTP responses are returned for common failures

## Tech stack

- `.NET 8`
- `ASP.NET Core Web API`
- `Entity Framework Core`
- `EF Core InMemory`
- `JWT`
- `BCrypt.NET`
- `Swagger / OpenAPI`

## Project structure

- `TaskManagementApi` - API layer and app startup
- `TaskManagementApi.BL` - business logic
- `TaskManagementApi.DAL` - data access, models, and seed data

## Running locally

### Prerequisites

- `.NET 8 SDK`

### Start the API

```bash
dotnet restore
dotnet run --project TaskManagementApi
```

Once it starts, open:

- `https://localhost:7217/swagger`
- `http://localhost:5218/swagger`

## Default credentials

The app seeds one user on startup for local testing in Swagger or any UI connected to this API:

- Username: `admin`
- Password: `123456`

## Seed data

The app starts with:

- `1` user
- `5` sample tasks

## Task status values

- `0` = `NotStarted`
- `1` = `InProgress`
- `2` = `Completed`
- `3` = `Cancelled`

## Important notes

- The database is in-memory, so data is lost when the app stops.
- This repo is API-only. There is no frontend project here.
- CORS is configured for `http://localhost:3000`.
- The companion React UI expects this API at `https://localhost:7217`.
- The `logout` endpoint returns success, but it does not revoke JWTs server-side.

## What I built on purpose

- A working API that can be run locally with no extra setup
- Authentication around the task endpoints
- Full CRUD for tasks
- Audit fields for who created or updated a task
- Seed data so the project is usable right away
- Sample unit tests for the API, business logic, and data access layers

## Tests

The solution includes three test projects:

- `TaskManagementApi.Tests`
- `TaskManagementApi.BL.Tests`
- `TaskManagementApi.DAL.Tests`

They cover a few sample scenarios across controllers, services, repositories, and seeded data.

Run all tests with:

```bash
dotnet test TaskManagementApi.sln
```

## What I left out on purpose

- Persistent storage
- User registration and broader user management
- Advanced filtering, sorting, and pagination
- Production security hardening

## Known limitations

- Data resets on every restart because `EF Core InMemory` is used.
- Only one seeded user is available by default.
- Authentication is implemented, but tasks are not isolated per user account.
- The current task model is closer to a shared task board than a private per-user todo list.
- Validation is intentionally light and focused on the main flow.

## If I had another day

- Replace the in-memory database with SQL storage
- Make task visibility and permissions explicit and enforce them
- Add user registration and multi-user scenarios

## Troubleshooting

- If you get `401 Unauthorized`, log in again and send `Authorization: Bearer <token>`.
- If your changes disappear after restart, that is expected with the current database setup.

## Repositories

- Frontend: `https://github.com/ali-afzali/taskmanagement-ui`
- Backend: `https://github.com/ali-afzali/TaskManagementApi`
