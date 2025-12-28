import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { JobService } from '../../services/job.service';
import { NotificationService } from '../../shared/services/notification.service';
import { JobDto } from '../../models/job.model';

@Component({
  selector: 'app-job-form',
  templateUrl: './job-form.component.html',
  styleUrls: ['./job-form.component.css']
})
export class JobFormComponent implements OnInit {
  @Input() job?: JobDto;

  form: FormGroup;
  loading = false;
  isEdit = false;

  constructor(
    private fb: FormBuilder,
    public activeModal: NgbActiveModal,
    private jobService: JobService,
    private notificationService: NotificationService
  ) {
    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(2)]],
      description: ['']
    });
  }

  ngOnInit(): void {
    if (this.job) {
      this.isEdit = true;
      this.form.patchValue({
        title: this.job.title,
        description: this.job.description
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

    if (this.isEdit && this.job) {
      this.jobService.updateJob({ ...data, id: this.job.id }).subscribe({
        next: () => {
          this.notificationService.success('Job title updated successfully');
          this.activeModal.close();
        },
        error: () => {
          this.loading = false;
          this.notificationService.error('Failed to update job title');
        }
      });
    } else {
      this.jobService.createJob(data).subscribe({
        next: () => {
          this.notificationService.success('Job title created successfully');
          this.activeModal.close();
        },
        error: () => {
          this.loading = false;
          this.notificationService.error('Failed to create job title');
        }
      });
    }
  }

  dismiss(): void {
    this.activeModal.dismiss();
  }
}
