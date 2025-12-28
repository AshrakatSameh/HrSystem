import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { EmployeeService } from '../../services/employee.service';
import { NotificationService } from '../../shared/services/notification.service';
import { EmployeeDto } from '../../models/employee.model';
import { DepartmentDto } from '../../models/department.model';
import { JobDto } from '../../models/job.model';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.css']
})
export class EmployeeFormComponent implements OnInit {
  @Input() employee?: EmployeeDto;
  @Input() departments: DepartmentDto[] = [];
  @Input() jobs: JobDto[] = [];

  form: FormGroup;
  loading = false;
  isEdit = false;

  constructor(
    private fb: FormBuilder,
    public activeModal: NgbActiveModal,
    private employeeService: EmployeeService,
    private notificationService: NotificationService
  ) {
    this.form = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{11}$/)]],
      dateOfBirth: ['', Validators.required],
      hireDate: ['', Validators.required],
      departmentId: ['', Validators.required],
      jobId: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    if (this.employee) {
      this.isEdit = true;
      this.form.patchValue({
        firstName: this.employee.firstName,
        lastName: this.employee.lastName,
        email: this.employee.email,
        phoneNumber: this.employee.phoneNumber,
        dateOfBirth: this.formatDateForInput(this.employee.dateOfBirth),
        hireDate: this.formatDateForInput(this.employee.hireDate),
        departmentId: this.employee.departmentId,
        jobId: this.employee.jobId
      });
    }
  }

  private formatDateForInput(date: string | Date): string {
    const d = new Date(date);
    return d.toISOString().split('T')[0];
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.form.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.loading = true;
    const data = this.form.value;

    if (this.isEdit && this.employee) {
      this.employeeService.updateEmployee({ ...data, id: this.employee.id }).subscribe({
        next: () => {
          this.notificationService.success('Employee updated successfully');
          this.activeModal.close();
        },
        error: () => {
          this.loading = false;
          this.notificationService.error('Failed to update employee');
        }
      });
    } else {
      this.employeeService.createEmployee(data).subscribe({
        next: () => {
          this.notificationService.success('Employee created successfully');
          this.activeModal.close();
        },
        error: () => {
          this.loading = false;
          this.notificationService.error('Failed to create employee');
        }
      });
    }
  }

  dismiss(): void {
    this.activeModal.dismiss();
  }
}
