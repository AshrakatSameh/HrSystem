# Angular Frontend for Tamweely HR - Implementation Plan

Build a complete Angular frontend aligned with the backend API, featuring JWT authentication, role-based access, CRUD operations, advanced search/filter/pagination, Excel export, and a responsive UI supporting all business requirements.

## Implementation Steps

### 1. Setup Project Structure
- Create feature modules: Auth, Employees, Departments, Jobs, Shared
- Establish services layer, guards, interceptors, models, and components
- Implement lazy loading for feature modules
- Use clean, consistent naming conventions (components, services, models)
- Folder structure:
  ```
  src/
    app/
      core/
        guards/
        interceptors/
        services/
      shared/
        components/
        models/
        pipes/
      features/
        auth/
        employees/
        departments/
        jobs/
      layout/
  ```

### 2. Implement Authentication & Security
- Build login page with username/password form
- Implement JWT token storage (localStorage/sessionStorage)
- Create HTTP interceptor to attach Bearer token to all requests
- Build auth guard to protect routes and redirect unauthenticated users
- Implement role-based access control (Admin vs. User)
- Add logout functionality and token expiration handling
- LoginResponseDto integration: store token, username, fullname, roles

### 3. Build Core Services
- **AuthService**: login(), logout(), getCurrentUser(), isAuthenticated(), hasRole()
- **EmployeeService**: CRUD operations, filtering, pagination, Excel export
- **DepartmentService**: CRUD operations, filtering, pagination
- **JobService**: CRUD operations, filtering, pagination
- Implement error handling and HTTP interceptors
- Use BehaviorSubjects for state management where needed

### 4. Create Dashboard & Navigation
- Main layout component with navbar/sidebar (responsive toggle for mobile)
- Role-based menu items (hide admin features from regular users)
- Breadcrumb navigation
- Dashboard landing page with quick stats or recent activity
- Responsive design for mobile/tablet/desktop
- User profile menu with logout option

### 5. Build Employee Management Module
- **Employee List Page**:
  - Paginated table/list view (10 items per page, configurable)
  - Search by name, email, or phone
  - Filter by department, job, date-of-birth range
  - Filter by hire date range
  - Sort by firstName, lastName, email, dateOfBirth, hireDate, department, job
  - Pagination controls (previous/next, page selector)
  - Action buttons: View, Edit, Delete (soft-delete with confirmation)
  - Excel export button (filtered results)
  - Loading indicators and empty state messages

- **Employee Add/Edit Form**:
  - First Name (required, max 100)
  - Last Name (required, max 100)
  - Email (required, valid format, unique validation)
  - Phone Number (required, Egyptian format: 01[012,5]XXXXXXXX)
  - Date of Birth (required, past date, age 18-65)
  - Hire Date (required, not future)
  - Department dropdown (required)
  - Job dropdown (required)
  - Form validation with clear error messages
  - Submit and cancel buttons
  - API error handling with user-friendly messages

- **Employee View/Detail Page**:
  - Read-only display of all employee information
  - Edit and Delete buttons
  - Back to list link

### 6. Build Admin Lookup Modules
- **Department Management**:
  - List page with pagination, search, sorting
  - Add/Edit form (Name required, max 100; Description optional, max 500)
  - Soft-delete with confirmation
  - Form validation
  - Access restricted to Admin role

- **Job Management**:
  - List page with pagination, search, sorting
  - Add/Edit form (Title required, max 100; Description optional, max 500)
  - Soft-delete with confirmation
  - Form validation
  - Access restricted to Admin role

### 7. Add Form Validation & Error Handling
- Use Reactive Forms (FormBuilder, FormGroup, FormControl)
- Validators:
  - Required fields validation
  - Email format validation
  - Egyptian phone format: `01[012,5]\d{8}` (11 digits total)
  - Date validation (DateOfBirth: past, age 18-65; HireDate: not future)
  - Max length validators
  - Custom async validator for email uniqueness (optional, backend returns 409 if duplicate)
- Field-level error messages below each input
- Form-level error summary at the top
- API error handling (400, 404, 409, 500) with user-friendly messages
- Toast notifications for success/warning/error messages

### 8. Implement Responsive Design
- Use Bootstrap 5 or Angular Material
- Responsive layout:
  - Mobile: Single-column, collapsible navigation
  - Tablet: Two-column where applicable
  - Desktop: Full layout with sidebar
- Responsive tables (horizontal scroll on mobile, or card view alternative)
- Modal dialogs for add/edit forms (centered, scrollable on small screens)
- Mobile-friendly buttons and touch targets
- Responsive navigation bar with hamburger menu
- Ensure readability on all screen sizes

### 9. Add Lazy Loading & Optimization
- Configure module lazy loading in routing:
  - Auth module (eager-loaded or guard-redirected)
  - Employees module (lazy)
  - Departments module (lazy, Admin-only)
  - Jobs module (lazy, Admin-only)
- Implement virtual scrolling for large employee lists (if using 1000+ records)
- Use OnPush change detection strategy
- Implement trackBy in *ngFor directives
- Unsubscribe from observables to prevent memory leaks (use takeUntil pattern or async pipe)
- Bundle lazy-loaded modules efficiently

### 10. Testing & Refinement
- Verify all API endpoints are correctly integrated
- Test role-based access restrictions (User can't access admin pages)
- Test form validation: required fields, email format, phone format, date ranges
- Test soft-delete confirmation and list refresh after delete
- Test Excel export with various filters applied
- Test pagination, search, filtering, sorting combinations
- Test error scenarios (network errors, validation errors, 404/409/500 responses)
- Test responsive design on different screen sizes
- Test authentication flow: login → token storage → logout → redirect to login
- Verify token is attached to all API requests
- Test expired token handling (redirect to login)

## Key Architecture Decisions

### UI Framework
- **Bootstrap 5** for quick, consistent responsive layout with pre-built components
- Alternatively: Angular Material for more polished Material Design (requires more setup)
- Recommendation: Bootstrap 5 for faster implementation

### State Management
- Use **Services with BehaviorSubjects** for simple state management
- No NgRx needed for this scope (sufficient for CRUD + filtering)
- Pattern: Service exposes observables; components subscribe via async pipe

### Excel Export
- **Handled entirely by backend** (ClosedXML library)
- Frontend: GET request to `/api/employees/export?filters...`
- Download as blob and trigger browser download dialog
- No client-side Excel generation library needed

### Date/Time Handling
- Use **Angular Material DatePicker** (or ngx-bootstrap)
- Validation logic:
  - DateOfBirth: past date, age must be 18-65 years
  - HireDate: past or present date (not future)
- Display dates in consistent format (e.g., DD/MM/YYYY)

### Phone Number Input
- **Custom formatter and validator** for Egyptian format
- Pattern: `01[012,5]\d{8}` (11 digits)
- Display format: `01X XXXX XXXX` for readability
- Custom directive or component for input masking

### Error Handling Strategy
- HTTP Interceptor catches all responses
- Map backend error codes to user-friendly messages
- Toast notifications (ngx-toastr or custom service)
- Detailed error messages in form validation
- Clear messaging for soft-delete operations

### Authentication Token Management
- Store JWT in **localStorage** (or sessionStorage for increased security)
- Implement token refresh mechanism if backend supports it (currently 7-day expiration)
- HTTP Interceptor adds token to all requests automatically
- Auth Guard prevents access to protected routes
- Implement automatic logout on token expiration

### Routing & Guards
- Public routes: Login
- Protected routes: Protected by AuthGuard
- Admin-only routes: Protected by RoleGuard (check for 'Admin' role)
- Lazy-loaded feature modules for code splitting
- Wildcard route redirects to dashboard or 404 page

## Further Refinements Needed

1. **Confirmation Dialogs** — Use Angular Material Dialog or Bootstrap Modal for delete confirmations
2. **Loading States** — Show spinners during API calls; disable buttons to prevent double-submit
3. **Success Messages** — Toast notifications after CRUD operations (e.g., "Employee added successfully")
4. **Pagination Controls** — Display current page, total pages, option to change page size
5. **Search Debouncing** — Debounce search input (300ms) to reduce API calls
6. **Filter Persistence** — Save filter state in route params (optional: URL-based filter state)
7. **Accessibility** — Use semantic HTML, ARIA labels, keyboard navigation
8. **Dark Mode** (Optional) — Toggle dark/light theme if time permits
9. **Date Range Picker** — Use daterangepicker.com or ngx-daterangepicker-material for filter date ranges
10. **Breadcrumb Navigation** — Implement breadcrumbs for navigation context

## Technology Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| **Framework** | Angular | 17+ |
| **Language** | TypeScript | 5.2+ |
| **UI Framework** | Bootstrap | 5.3+ |
| **Forms** | Reactive Forms | Built-in |
| **HTTP** | HttpClient | Built-in |
| **Routing** | Angular Router | Built-in |
| **Styling** | SCSS/CSS | Built-in |
| **Notifications** | ngx-toastr | Latest |
| **Date Picker** | ng-bootstrap (ngx-bootstrap) | Latest |
| **Icons** | Bootstrap Icons / FontAwesome | Latest |
| **State** | RxJS BehaviorSubjects | Built-in |

## File Structure

```
src/
├── app/
│   ├── core/
│   │   ├── guards/
│   │   │   ├── auth.guard.ts
│   │   │   └── role.guard.ts
│   │   ├── interceptors/
│   │   │   ├── auth.interceptor.ts
│   │   │   └── error.interceptor.ts
│   │   ├── services/
│   │   │   ├── auth.service.ts
│   │   │   ├── employee.service.ts
│   │   │   ├── department.service.ts
│   │   │   ├── job.service.ts
│   │   │   └── notification.service.ts
│   │   └── core.module.ts
│   ├── shared/
│   │   ├── components/
│   │   │   ├── navbar/
│   │   │   ├── sidebar/
│   │   │   ├── breadcrumb/
│   │   │   └── confirmation-dialog/
│   │   ├── models/
│   │   │   ├── auth.model.ts
│   │   │   ├── employee.model.ts
│   │   │   ├── department.model.ts
│   │   │   ├── job.model.ts
│   │   │   └── pagination.model.ts
│   │   ├── pipes/
│   │   │   └── date-format.pipe.ts
│   │   ├── validators/
│   │   │   └── egyptian-phone.validator.ts
│   │   └── shared.module.ts
│   ├── features/
│   │   ├── auth/
│   │   │   ├── login/
│   │   │   ├── logout/
│   │   │   └── auth.module.ts
│   │   ├── employees/
│   │   │   ├── employee-list/
│   │   │   ├── employee-form/
│   │   │   ├── employee-detail/
│   │   │   └── employees.module.ts
│   │   ├── departments/
│   │   │   ├── department-list/
│   │   │   ├── department-form/
│   │   │   └── departments.module.ts
│   │   └── jobs/
│   │       ├── job-list/
│   │       ├── job-form/
│   │       └── jobs.module.ts
│   ├── layout/
│   │   └── main-layout/
│   ├── app.component.ts
│   ├── app.component.html
│   ├── app.routing.module.ts
│   └── app.module.ts
├── assets/
├── environments/
│── main.ts
└── styles.scss
```

## Notes

- Follow Angular style guide: https://angular.io/guide/styleguide
- Use BEM naming for CSS classes
- Keep components focused and reusable
- Separate smart (container) and dumb (presentational) components
- Use trackBy function in *ngFor for performance
- Always unsubscribe or use async pipe to prevent memory leaks
- Test each module independently before integration
