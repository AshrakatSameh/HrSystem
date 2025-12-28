# Tamweely HR - Angular Frontend

A comprehensive Angular 16 application for the Tamweely HR system, providing a secure, well-structured address book for managing employee information, departments, and job titles.

## Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Technology Stack](#technology-stack)
- [Architecture & Design Patterns](#architecture--design-patterns)
- [API Configuration](#api-configuration)
- [Usage Guide](#usage-guide)
- [Key Components](#key-components)

## Features

### Authentication & Security
- **JWT-based Authentication**: Secure login with JWT tokens
- **Role-Based Access Control (RBAC)**: Different permissions for Admin and User roles
- **Protected Routes**: Guards ensure only authenticated users can access protected pages
- **Auto-logout**: Automatic logout on 401 responses

### Employee Management
- Create, read, update, and soft-delete employees
- Search employees by name, email, or phone number
- Filter by department and job title
- Advanced filtering by date of birth range for birthday lists
- Pagination with customizable page size
- Export filtered employee lists to Excel (.xlsx)
- Form validation for email format and phone numbers

### Department Management (Admin Only)
- Create, read, update, and soft-delete departments
- Search and filter departments
- Ensure unique department names
- Pagination support

### Job Title Management (Admin Only)
- Create, read, update, and soft-delete job titles
- Search and filter job titles
- Ensure unique job title names
- Pagination support

### Dashboard
- Real-time statistics (total employees, departments, jobs)
- Recent employees list
- Department distribution view
- Quick access to all features

### User Experience
- Responsive design supporting all screen sizes
- Toast notifications for success/error messages
- Intuitive modal-based forms
- Clean, modern UI with Bootstrap 5
- Loading indicators and spinners
- Breadcrumb navigation
- Sidebar navigation with role-based menu items

## Project Structure

```
src/app/
├── features/
│   ├── auth/
│   │   ├── login.component.ts
│   │   ├── login.component.html
│   │   └── login.component.css
│   ├── dashboard/
│   │   ├── dashboard.component.ts
│   │   ├── dashboard.component.html
│   │   └── dashboard.component.css
│   ├── employees/
│   │   ├── employee-list.component.ts
│   │   ├── employee-list.component.html
│   │   ├── employee-form.component.ts
│   │   ├── employee-form.component.html
│   │   └── employee-list.component.css
│   ├── departments/
│   │   ├── department-list.component.ts
│   │   ├── department-list.component.html
│   │   ├── department-form.component.ts
│   │   ├── department-form.component.html
│   │   └── department-list.component.css
│   └── jobs/
│       ├── job-list.component.ts
│       ├── job-list.component.html
│       ├── job-form.component.ts
│       ├── job-form.component.html
│       └── job-list.component.css
├── shared/
│   ├── components/
│   │   ├── header.component.ts
│   │   ├── header.component.html
│   │   ├── header.component.css
│   │   ├── sidebar.component.ts
│   │   ├── sidebar.component.html
│   │   └── sidebar.component.css
│   └── services/
│       └── notification.service.ts
├── layouts/
│   ├── main-layout.component.ts
│   ├── main-layout.component.html
│   └── main-layout.component.css
├── models/
│   ├── auth.model.ts
│   ├── employee.model.ts
│   ├── department.model.ts
│   ├── job.model.ts
│   └── common.model.ts
├── services/
│   ├── auth.service.ts
│   ├── employee.service.ts
│   ├── department.service.ts
│   └── job.service.ts
├── guards/
│   ├── auth.guard.ts
│   ├── role.guard.ts
│   └── no-auth.guard.ts
├── interceptors/
│   └── auth.interceptor.ts
├── app.module.ts
├── app-routing.module.ts
├── app.component.ts
└── app.component.html
```

## Getting Started

### Prerequisites

- **Node.js** 16+ and **npm** 8+
- **Angular CLI** 16+
- Backend API running on `http://localhost:5287`

### Installation

1. **Navigate to the project directory**:
```bash
cd TamweelyClient/tamweelyHr
```

2. **Install dependencies**:
```bash
npm install --legacy-peer-deps
```

3. **Update API configuration** (if needed):
   - Edit the API base URL in service files if your backend is running on a different port

### Running the Application

**Development server**:
```bash
ng serve -o
```

The application will automatically open at `http://localhost:4200`

**Production build**:
```bash
ng build --configuration production
```

## Technology Stack

### Frontend Framework
- **Angular 16**: Modern TypeScript-based web framework
- **TypeScript 5.1**: Strongly typed JavaScript

### UI & Styling
- **Bootstrap 5**: Responsive CSS framework
- **Bootstrap Icons**: Icon library
- **CSS3**: Modern styling with animations

### State Management & Services
- **RxJS**: Reactive programming library
- **Angular Forms**: Reactive and template-driven forms

### Components & Libraries
- **ng-bootstrap**: Bootstrap components for Angular
- **ngx-toastr**: Toast notification library

### HTTP & Authentication
- **HttpClient**: Angular HTTP client
- **JWT**: JSON Web Tokens for authentication
- **AuthInterceptor**: Automatic token attachment to requests

## Architecture & Design Patterns

### Design Patterns Implemented

1. **Service Layer Pattern**: Centralized business logic in services
2. **Component Container/Presentational Pattern**: Smart and dumb components
3. **Reactive Programming**: Observable-based data streams with RxJS
4. **Interceptor Pattern**: HTTP interceptors for authentication
5. **Guard Pattern**: Route guards for access control
6. **Modal Pattern**: Bootstrap modals for forms
7. **Singleton Pattern**: Services provided at root level
8. **Observer Pattern**: EventEmitters and Observables for component communication

### Layered Architecture

```
Presentation Layer (Components)
        ↓
Service Layer (API Services + Business Logic)
        ↓
HTTP Layer (HttpClient + Interceptors)
        ↓
Backend API
```

### Key Architectural Features

- **Separation of Concerns**: Components, services, and models are clearly separated
- **Lazy Loading Ready**: Feature modules can be easily made lazy-loadable
- **Type Safety**: Strong typing with TypeScript interfaces
- **Dependency Injection**: Angular DI for loose coupling
- **Error Handling**: Centralized error handling in services and interceptors
- **State Management**: AuthService manages authentication state
- **Form Validation**: Both template and reactive form validation

## API Configuration

The application connects to the backend API at `http://localhost:5287`. 

To change the API URL, update the `apiUrl` property in the service files:

- [auth.service.ts](src/app/services/auth.service.ts)
- [employee.service.ts](src/app/services/employee.service.ts)
- [department.service.ts](src/app/services/department.service.ts)
- [job.service.ts](src/app/services/job.service.ts)

### API Endpoints Used

#### Authentication
- `POST /api/auth/login` - User login

#### Employees
- `GET /api/employees` - Get paginated employees
- `GET /api/employees/{id}` - Get employee by ID
- `POST /api/employees` - Create new employee
- `PUT /api/employees/{id}` - Update employee
- `DELETE /api/employees/{id}` - Soft delete employee
- `GET /api/employees/export` - Export employees to Excel

#### Departments
- `GET /api/departments` - Get paginated departments
- `GET /api/departments/{id}` - Get department by ID
- `POST /api/departments` - Create new department
- `PUT /api/departments/{id}` - Update department
- `DELETE /api/departments/{id}` - Soft delete department

#### Jobs
- `GET /api/jobs` - Get paginated jobs
- `GET /api/jobs/{id}` - Get job by ID
- `POST /api/jobs` - Create new job
- `PUT /api/jobs/{id}` - Update job
- `DELETE /api/jobs/{id}` - Soft delete job

## Usage Guide

### Login

1. Navigate to `http://localhost:4200/login`
2. Enter credentials:
   - **Admin User**: username: `admin`, password: `password`
   - **Regular User**: username: `user`, password: `password`
3. Click "Sign In"

### Employee Management

#### Viewing Employees
1. Click "Employees" in the sidebar
2. View paginated employee list with search and filter options
3. Use search bar to filter by name, email, or phone
4. Use dropdowns to filter by department or job
5. Use pagination controls to navigate pages

#### Adding an Employee
1. Click "Add Employee" button
2. Fill in the form with required fields:
   - First Name
   - Last Name
   - Email (must be valid and unique)
   - Phone Number (11 digits)
   - Date of Birth
   - Hire Date
   - Department
   - Job Title
3. Click "Create"

#### Editing an Employee
1. Click the pencil icon next to the employee
2. Update desired fields
3. Click "Update"

#### Deleting an Employee
1. Click the trash icon next to the employee
2. Confirm deletion
3. Employee will be soft-deleted (marked inactive)

#### Exporting Employees
1. Apply desired filters (search, department, job)
2. Click "Export" button
3. Excel file will be downloaded

### Department Management (Admin Only)

#### Viewing Departments
1. Click "Departments" in the sidebar (Admin only)
2. View paginated department list
3. Use search to filter by name

#### Adding a Department
1. Click "Add Department" button
2. Enter department name (must be unique)
3. Optionally add description
4. Click "Create"

#### Editing/Deleting
Similar to employee management

### Job Title Management (Admin Only)

#### Viewing Jobs
1. Click "Jobs" in the sidebar (Admin only)
2. View paginated job list
3. Use search to filter by title

#### Adding a Job
1. Click "Add Job Title" button
2. Enter job title (must be unique)
3. Optionally add description
4. Click "Create"

#### Editing/Deleting
Similar to employee management

### Dashboard

1. Click "Dashboard" to view overview
2. See total count cards for employees, departments, and jobs
3. View recent employees list
4. See department distribution

## Key Components

### AuthService
Manages authentication state, login, logout, and token storage. Provides observable streams for reactive components.

**Key Methods**:
- `login(credentials)` - Authenticates user
- `logout()` - Clears session
- `getToken()` - Retrieves JWT token
- `isAuthenticated()` - Checks auth status
- `isAdmin()` - Checks admin role

### EmployeeService
Handles all employee-related API calls with search, filter, pagination, and export functionality.

**Key Methods**:
- `getEmployees(params)` - Get paginated employees
- `createEmployee(data)` - Create new employee
- `updateEmployee(data)` - Update employee
- `deleteEmployee(id)` - Delete employee
- `exportToExcel(params)` - Export to Excel

### AuthGuard & RoleGuard
Protects routes and ensures only authenticated and authorized users can access specific pages.

### AuthInterceptor
Automatically attaches JWT token to all HTTP requests and handles 401 responses.

### NotificationService
Provides toast notifications for user feedback using ngx-toastr.

**Key Methods**:
- `success(message)` - Show success notification
- `error(message)` - Show error notification
- `warning(message)` - Show warning notification
- `info(message)` - Show info notification

## Error Handling

The application implements comprehensive error handling:

1. **HTTP Errors**: Intercepted and logged
2. **Form Validation**: Real-time feedback on form fields
3. **API Errors**: User-friendly messages in notifications
4. **Authorization**: Automatic redirect to login on 401
5. **Network Errors**: Graceful error messages

## Responsive Design

The application is fully responsive and works seamlessly on:
- Desktop (1920px+)
- Tablet (768px - 1024px)
- Mobile (320px - 767px)

Media queries adjust layout, fonts, and component sizing for optimal viewing experience.

## Performance Optimizations

1. **Lazy Loading**: Components can be lazy-loaded
2. **OnPush Change Detection**: Can be enabled in components
3. **Tree Shaking**: Angular CLI automatically optimizes builds
4. **HTTP Caching**: Services can implement caching strategies
5. **Pagination**: Server-side pagination reduces data transfer

## Security Features

1. **JWT Authentication**: Secure token-based authentication
2. **HTTPS Ready**: Configured for SSL/TLS
3. **CSRF Protection**: Angular built-in CSRF token handling
4. **XSS Protection**: Angular automatically sanitizes content
5. **Authentication Guards**: Route-level access control
6. **Role-Based Access**: Admin/User role separation
7. **Secure Token Storage**: Tokens stored in localStorage (can be upgraded to secure cookies)
8. **HTTP Interceptor**: Automatic token attachment and refresh

## Testing

To add unit tests:

```bash
ng test
```

To add e2e tests:

```bash
ng e2e
```

## Deployment

### Build for Production
```bash
ng build --configuration production
```

### Deploy to Server
1. Build the application
2. Copy contents of `dist/tamweely-hr` to your web server
3. Configure server to serve `index.html` for all routes (SPA configuration)
4. Update API URL to production backend

### Environment Configuration
Create `src/environments/environment.prod.ts`:
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.yourdomain.com'
};
```

## Troubleshooting

### Port Already in Use
```bash
ng serve --port 4300
```

### Build Errors
```bash
# Clear cache and reinstall
rm -rf node_modules
npm install --legacy-peer-deps
ng build
```

### API Connection Issues
- Verify backend is running on `http://localhost:5287`
- Check CORS configuration on backend
- Verify API endpoints match service implementations

## Contributing

Guidelines for extending the application:

1. **New Features**: Create feature modules with lazy loading
2. **Components**: Keep components small and focused
3. **Services**: Centralize business logic in services
4. **Guards**: Create specific guards for different access levels
5. **Testing**: Write tests for critical services
6. **Documentation**: Document complex logic and APIs

## License

Copyright © 2025 Tamweely HR. All rights reserved.

## Support

For issues and questions, contact the development team or check the project documentation.

---

**Last Updated**: December 28, 2025
**Angular Version**: 16
**Bootstrap Version**: 5
**Node Version**: 16+
