import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EmployeeService } from '../../services/employee.service';
import { DepartmentService } from '../../services/department.service';
import { JobService } from '../../services/job.service';
import { NotificationService } from '../../shared/services/notification.service';
import { EmployeeDto, EmployeeQueryParameters } from '../../models/employee.model';
import { DepartmentDto } from '../../models/department.model';
import { JobDto } from '../../models/job.model';
import { EmployeeFormComponent } from './employee-form.component';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  employees: EmployeeDto[] = [];
  departments: DepartmentDto[] = [];
  jobs: JobDto[] = [];
  
  loading = false;
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  
  searchTerm = '';
  selectedDepartment = '';
  selectedJob = '';
  
  Math = Math;

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private jobService: JobService,
    private notificationService: NotificationService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
    this.loadJobs();
    this.loadEmployees();
  }

  private loadDepartments(): void {
    this.departmentService.getDepartments({ pageSize: 1000 }).subscribe(result => {
      this.departments = result.data;
    });
  }

  private loadJobs(): void {
    this.jobService.getJobs({ pageSize: 1000 }).subscribe(result => {
      this.jobs = result.data;
    });
  }

  loadEmployees(): void {
    this.loading = true;
    const params: EmployeeQueryParameters = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm || undefined,
      departmentId: this.selectedDepartment ? parseInt(this.selectedDepartment) : undefined,
      jobId: this.selectedJob ? parseInt(this.selectedJob) : undefined
    };

    this.employeeService.getEmployees(params).subscribe({
      next: (result) => {
        this.employees = result.data;
        this.totalCount = result.totalCount;
        this.totalPages = result.totalPages;
        this.loading = false;
      },
      error: (error) => {
        this.loading = false;
        this.notificationService.error('Failed to load employees');
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadEmployees();
  }

  onFilterChange(): void {
    this.pageNumber = 1;
    this.loadEmployees();
  }

  resetFilters(): void {
    this.searchTerm = '';
    this.selectedDepartment = '';
    this.selectedJob = '';
    this.pageNumber = 1;
    this.loadEmployees();
  }

  openAddDialog(): void {
    const modalRef = this.modalService.open(EmployeeFormComponent, {
      size: 'lg',
      backdrop: 'static'
    });
    
    modalRef.componentInstance.departments = this.departments;
    modalRef.componentInstance.jobs = this.jobs;
    
    modalRef.result.then(
      () => this.loadEmployees(),
      () => {}
    );
  }

  editEmployee(employee: EmployeeDto): void {
    const modalRef = this.modalService.open(EmployeeFormComponent, {
      size: 'lg',
      backdrop: 'static'
    });
    
    modalRef.componentInstance.employee = employee;
    modalRef.componentInstance.departments = this.departments;
    modalRef.componentInstance.jobs = this.jobs;
    
    modalRef.result.then(
      () => this.loadEmployees(),
      () => {}
    );
  }

  deleteEmployee(id: number): void {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.deleteEmployee(id).subscribe({
        next: () => {
          this.notificationService.success('Employee deleted successfully');
          this.loadEmployees();
        },
        error: () => this.notificationService.error('Failed to delete employee')
      });
    }
  }

  exportToExcel(): void {
    const params: EmployeeQueryParameters = {
      searchTerm: this.searchTerm || undefined,
      departmentId: this.selectedDepartment ? parseInt(this.selectedDepartment) : undefined,
      jobId: this.selectedJob ? parseInt(this.selectedJob) : undefined
    };

    this.employeeService.exportToExcel(params).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `employees_${new Date().getTime()}.xlsx`;
        link.click();
        window.URL.revokeObjectURL(url);
        this.notificationService.success('Employees exported successfully');
      },
      error: () => this.notificationService.error('Failed to export employees')
    });
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadEmployees();
    }
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadEmployees();
    }
  }

  goToPage(page: number): void {
    this.pageNumber = page;
    this.loadEmployees();
  }

  getPages(): number[] {
    const pages: number[] = [];
    const start = Math.max(1, this.pageNumber - 2);
    const end = Math.min(this.totalPages, this.pageNumber + 2);
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  }
}
