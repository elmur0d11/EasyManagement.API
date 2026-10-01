# EasyManagement.API

This repository contains the backend API for EasyManagement, a project management application designed to streamline task and team collaboration. It is built with .NET 10, providing a robust and scalable RESTful service for managing users, rooms (workspaces), and tasks.

The corresponding frontend application can be found at: [https://easymanageapp.netlify.app/](https://easymanageapp.netlify.app/)

## Features

- **User Authentication**: Secure user registration and login using JWT with refresh token support.
- **Room Management**: Create collaborative rooms, invite members using a unique auto-generated code, and manage room settings.
- **Task Management**: Create, view, and update tasks within rooms. Tasks can be assigned priorities (`Low`, `Medium`, `High`, `Critical`) and statuses (`Todo`, `InProgress`, `Completed`, `Canceled`).
- **Role-Based Access Control (RBAC)**: Distinguishes between standard users and Project Managers (`PM`), granting `PM`s administrative privileges over rooms and tasks.
- **Account Management**: Endpoints for users to update their profile information and password.
- **Global Error Handling**: Centralized middleware for consistent and descriptive error responses.
- **Containerization**: Full Docker and Docker Compose support for a simplified development and deployment workflow.

## Technology Stack

- **Framework**: .NET 10 / ASP.NET Core
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core 10
- **Authentication**: JWT (JSON Web Tokens)
- **API Documentation**: Scalar
- **Containerization**: Docker / Docker Compose
- **Logging**: Serilog
- **Object Mapping**: AutoMapper

## Getting Started

The recommended way to run this project is by using Docker and Docker Compose.

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (for local development without Docker)

### Running with Docker

1.  **Clone the repository:**
    ```sh
    git clone https://github.com/elmur0d11/easymanagement.api.git
    cd easymanagement.api
    ```

2.  **Create a `.env` file:**
    In the root directory of the project, create a file named `.env` and add the database connection string. The `docker-compose.yml` is configured to use this file.

    ```env
    DefaultConnection="Host=db;Database=easy_manage_db;Username=postgres;Password=7655"
    ```

3.  **Build and run the containers:**
    ```sh
    docker-compose up --build
    ```

The API will be running and accessible at `http://localhost:8081`. The PostgreSQL database will be running on port `5432`.

## API Documentation

When the application is running in the development environment, interactive API documentation is available through Scalar.

-   **API Base URL**: `http://localhost:8081`
-   **Scalar UI**: `http://localhost:8081/scalar`

### API Endpoints

Here is a summary of the available endpoints:

#### Authentication (`/api/v1/auth`)

-   `POST /register`: Register a new user.
-   `POST /login`: Authenticate a user and receive an access token and refresh token.
-   `POST /refreshToken`: Obtain a new access token using a valid refresh token.

#### Account (`/api/v1/account`)
*Authentication required*

-   `PUT /`: Update the authenticated user's account details (username, full name, email, role).
-   `PUT /password`: Update the authenticated user's password.

#### Rooms (`/api/v1/room`)
*Authentication required*

-   `POST /create`: Create a new room. The creator becomes the owner (`PM`).
-   `POST /join`: Join an existing room using its unique code.
-   `GET /rooms`: Retrieve a list of all rooms the user is a member of.
-   `PUT /rename`: Rename a room (owner only).
-   `DELETE /delete`: Delete a room and all its associated tasks and members (owner only).

#### Tasks (`/api/v1/task`)
*Authentication required*

-   `POST /create`: Create a new task in a room (`PM` / `ProjectManager` role required).
-   `GET /tasks`: Get all tasks for a given room.
-   `PUT /updatePriority`: Update the priority of a task (`PM` / `ProjectManager` role required).
-   `PUT /updateStatus`: Update the status of a task (`PM` / `ProjectManager` role required).
-   `PUT /updateTask`: Update the title and description of a task (`PM` / `ProjectManager` role required).
-   `DELETE /delete`: Delete a task from a room (`PM` / `ProjectManager` role required).
