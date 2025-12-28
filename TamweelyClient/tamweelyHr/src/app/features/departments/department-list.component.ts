import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DepartmentService } from '../../services/department.service';
import { NotificationService } from '../../shared/services/notification.service';
import { DepartmentDto, DepartmentQueryParameters } from '../../models/department.model';
import { DepartmentFormComponent } from './department-form.component';

@Component({
  selector: 'app-department-list',
  templateUrl: './department-list.component.html',
  styleUrls: ['./department-list.component.css']
})
export class DepartmentListComponent implements OnInit {
  departments: DepartmentDto[] = [];
  
  loading = false;
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  
  searchTerm = '';
  
  Math = Math;

  constructor(
    private departmentService: DepartmentService,
    private notificationService: NotificationService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.loading = true;
    const params: DepartmentQueryParameters = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm || undefined
    };

    this.departmentService.getDepartments(params).subscribe({
      next: (result) => {
        this.departments = result.data;
        this.totalCount = result.totalCount;
        this.totalPages = result.totalPages;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.notificationService.error('Failed to load departments');
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadDepartments();
  }

  resetFilters(): void {
    this.searchTerm = '';
    this.pageNumber = 1;
    this.loadDepartments();
  }

  openAddDialog(): void {
    const modalRef = this.modalService.open(DepartmentFormComponent, {
      size: 'lg',
      backdrop: 'static'
    });
    
    modalRef.result.then(
      () => this.loadDepartments(),
      () => {}
    );
  }

  editDepartment(department: DepartmentDto): void {
    const modalRef = this.modalService.open(DepartmentFormComponent, {
      size: 'lg',
      backdrop: 'static'
    });
    
    modalRef.componentInstance.department = department;
    
    modalRef.result.then(
      () => this.loadDepartments(),
      () => {}
    );
  }

  deleteDepartment(id: number): void {
    if (confirm('Are you sure you want to delete this department?')) {
      this.departmentService.deleteDepartment(id).subscribe({
        next: () => {
          this.notificationService.success('Department deleted successfully');
          this.loadDepartments();
        },
        error: () => this.notificationService.error('Failed to delete department')
      });
    }
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadDepartments();
    }
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadDepartments();
    }
  }

  goToPage(page: number): void {
    this.pageNumber = page;
    this.loadDepartments();
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
