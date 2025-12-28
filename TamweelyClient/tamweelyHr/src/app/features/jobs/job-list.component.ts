import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { JobService } from '../../services/job.service';
import { NotificationService } from '../../shared/services/notification.service';
import { JobDto, JobQueryParameters } from '../../models/job.model';
import { JobFormComponent } from './job-form.component';

@Component({
  selector: 'app-job-list',
  templateUrl: './job-list.component.html',
  styleUrls: ['./job-list.component.css']
})
export class JobListComponent implements OnInit {
  jobs: JobDto[] = [];
  
  loading = false;
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  
  searchTerm = '';
  
  Math = Math;

  constructor(
    private jobService: JobService,
    private notificationService: NotificationService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.loadJobs();
  }

  loadJobs(): void {
    this.loading = true;
    const params: JobQueryParameters = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm || undefined
    };

    this.jobService.getJobs(params).subscribe({
      next: (result) => {
        this.jobs = result.data;
        this.totalCount = result.totalCount;
        this.totalPages = result.totalPages;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.notificationService.error('Failed to load job titles');
      }
    });
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadJobs();
  }

  resetFilters(): void {
    this.searchTerm = '';
    this.pageNumber = 1;
    this.loadJobs();
  }

  openAddDialog(): void {
    const modalRef = this.modalService.open(JobFormComponent, {
      size: 'lg',
      backdrop: 'static'
    });
    
    modalRef.result.then(
      () => this.loadJobs(),
      () => {}
    );
  }

  editJob(job: JobDto): void {
    const modalRef = this.modalService.open(JobFormComponent, {
      size: 'lg',
      backdrop: 'static'
    });
    
    modalRef.componentInstance.job = job;
    
    modalRef.result.then(
      () => this.loadJobs(),
      () => {}
    );
  }

  deleteJob(id: number): void {
    if (confirm('Are you sure you want to delete this job title?')) {
      this.jobService.deleteJob(id).subscribe({
        next: () => {
          this.notificationService.success('Job title deleted successfully');
          this.loadJobs();
        },
        error: () => this.notificationService.error('Failed to delete job title')
      });
    }
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadJobs();
    }
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadJobs();
    }
  }

  goToPage(page: number): void {
    this.pageNumber = page;
    this.loadJobs();
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
