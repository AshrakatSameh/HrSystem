import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DepartmentService } from '../../services/department.service';
import { NotificationService } from '../../shared/services/notification.service';
import { DepartmentDto } from '../../models/department.model';

@Component({
  selector: 'app-department-form',
  templateUrl: './department-form.component.html',
  styleUrls: ['./department-form.component.css']
})
export class DepartmentFormComponent implements OnInit {
  @Input() department?: DepartmentDto;

  form: FormGroup;
  loading = false;
  isEdit = false;

  constructor(
    private fb: FormBuilder,
    public activeModal: NgbActiveModal,
    private departmentService: DepartmentService,
    private notificationService: NotificationService
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      description: ['']
    });
  }

  ngOnInit(): void {
    if (this.department) {
      this.isEdit = true;
      this.form.patchValue({
        name: this.department.name,
        description: this.department.description
      });
    }
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

    if (this.isEdit && this.department) {
      this.departmentService.updateDepartment({ ...data, id: this.department.id }).subscribe({
        next: () => {
          this.notificationService.success('Department updated successfully');
          this.activeModal.close();
        },
        error: () => {
          this.loading = false;
          this.notificationService.error('Failed to update department');
        }
      });
    } else {
      this.departmentService.createDepartment(data).subscribe({
        next: () => {
          this.notificationService.success('Department created successfully');
          this.activeModal.close();
        },
        error: () => {
          this.loading = false;
          this.notificationService.error('Failed to create department');
        }
      });
    }
  }

  dismiss(): void {
    this.activeModal.dismiss();
  }
}
