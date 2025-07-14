# LaSophpy

This is a multi-project ASP.NET Core solution that includes:

- `UserApi`: Handles user registration and authentication.
- `UploadApi`: Manages PDF upload and storage.
- `CommentsApi`: Users can comment on books.
- `NotificationsApi`: Handles user notifications via Azure Service Bus.
- `SharedContract`: Common models and interfaces shared across services.
- `BookCommentApi.Tests`: Unit tests for comment features.

## Features

- JWT-based authentication
- Azure Blob Storage integration
- Service Bus messaging
- CI/CD via GitHub Actions

## Getting Started

1. Clone this repo
2. Run the solution in Visual Studio
3. Setup your Azure settings in `appsettings.json`