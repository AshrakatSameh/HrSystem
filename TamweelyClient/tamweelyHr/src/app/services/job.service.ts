import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JobDto, CreateJobDto, UpdateJobDto, JobQueryParameters } from '../models/job.model';
import { PagedResult } from '../models/common.model';

@Injectable({
  providedIn: 'root'
})
export class JobService {
  private apiUrl = 'https://localhost:7097/api/jobs';

  constructor(private http: HttpClient) {}

  getJobs(params?: JobQueryParameters): Observable<PagedResult<JobDto>> {
    let httpParams = new HttpParams();
    
    if (params) {
      if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
      if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
      if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
      if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
      if (params.sortDescending !== undefined) httpParams = httpParams.set('sortDescending', params.sortDescending.toString());
    }

    return this.http.get<PagedResult<JobDto>>(this.apiUrl, { params: httpParams });
  }

  getJobById(id: number): Observable<JobDto> {
    return this.http.get<JobDto>(`${this.apiUrl}/${id}`);
  }

  createJob(job: CreateJobDto): Observable<JobDto> {
    return this.http.post<JobDto>(this.apiUrl, job);
  }

  updateJob(job: UpdateJobDto): Observable<JobDto> {
    return this.http.put<JobDto>(`${this.apiUrl}/${job.id}`, job);
  }

  deleteJob(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getAllJobs(): Observable<JobDto[]> {
    return this.http.get<JobDto[]>(`${this.apiUrl}?pageSize=1000`);
  }
}
