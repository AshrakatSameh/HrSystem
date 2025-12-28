import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../../services/employee.service';
import { DepartmentService } from '../../services/department.service';
import { JobService } from '../../services/job.service';
import { EmployeeDto } from '../../models/employee.model';
import { DepartmentDto } from '../../models/department.model';

interface DepartmentWithCount extends DepartmentDto {
  employeeCount?: number;
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  totalEmployees = 0;
  totalDepartments = 0;
  totalJobs = 0;
  activeUsers = 0;
  recentEmployees: EmployeeDto[] = [];
  departments: DepartmentWithCount[] = [];

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private jobService: JobService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  private loadDashboardData(): void {
    // Load employees
    this.employeeService.getEmployees({ pageSize: 5 }).subscribe(result => {
      this.totalEmployees = result.totalCount;
      this.recentEmployees = result.data.slice(0, 5);
      this.activeUsers = result.data.filter(e => e.isActive).length;
    });

    // Load departments
    this.departmentService.getDepartments({ pageSize: 100 }).subscribe(result => {
      this.totalDepartments = result.totalCount;
      this.departments = result.data.map(dept => ({ ...dept, employeeCount: 0 }));
    });

    // Load jobs
    this.jobService.getJobs({ pageSize: 100 }).subscribe(result => {
      this.totalJobs = result.totalCount;
    });
  }
}
