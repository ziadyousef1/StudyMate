# StudyMate

StudyMate is a study assistant application designed to help users organize their learning materials, summarize documents, and track their study progress.

## Features

### Implemented

#### Authentication & User Management
- ✅ User registration and login
- ✅ Email verification with confirmation codes
- ✅ Password reset functionality
- ✅ JWT token authentication with refresh tokens
- ✅ Role-based access control (Admin & User roles)
- ✅ User profile management
- ✅ Profile picture upload (using Azure Blob Storage)

#### Document Management
- ✅ AI-powered document summarization
- ✅ Extract text from PDF files
- ✅ Generate summary PDFs with formatting
- ✅ File uploads to Azure Blob Storage
#### Notes Management
- ✅ CRUD operations for user-specific notes

#### Notification System
- ✅ Create notifications for users
- ✅ Track notification read status
- ✅ Real-time notifications using SignalR

### Planned Features

#### Collaboration
- 📝 Study groups creation and management
- 📝 Shared notes and documents
- 📝 Discussion forums for topics

## Technologies Used

- **Framework**: ASP.NET Core Web API
- **ORM**: Entity Framework Core
- **Authentication**: Identity with JWT tokens
- **Storage**: SQL Server, Azure Blob Storage
- **Real-time Communication**: SignalR for notifications
- **AI Integration**: Azure OpenAI for document summarization
- **Email**: SMTP integration for verification emails
- **CI/CD**: Continuous Integration and Deployment pipelines
