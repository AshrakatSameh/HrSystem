import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EmployeeDto, CreateEmployeeDto, UpdateEmployeeDto, EmployeeQueryParameters } from '../models/employee.model';
import { PagedResult } from '../models/common.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'https://localhost:7097/api/employees';

  constructor(private http: HttpClient) {}

  getEmployees(params?: EmployeeQueryParameters): Observable<PagedResult<EmployeeDto>> {
    let httpParams = new HttpParams();
    
    if (params) {
      if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
      if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
      if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
      if (params.departmentId) httpParams = httpParams.set('departmentId', params.departmentId.toString());
      if (params.jobId) httpParams = httpParams.set('jobId', params.jobId.toString());
      if (params.minDateOfBirth) httpParams = httpParams.set('minDateOfBirth', params.minDateOfBirth);
      if (params.maxDateOfBirth) httpParams = httpParams.set('maxDateOfBirth', params.maxDateOfBirth);
      if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
      if (params.sortDescending !== undefined) httpParams = httpParams.set('sortDescending', params.sortDescending.toString());
    }

    return this.http.get<PagedResult<EmployeeDto>>(this.apiUrl, { params: httpParams });
  }

  getEmployeeById(id: number): Observable<EmployeeDto> {
    return this.http.get<EmployeeDto>(`${this.apiUrl}/${id}`);
  }

  createEmployee(employee: CreateEmployeeDto): Observable<EmployeeDto> {
    return this.http.post<EmployeeDto>(this.apiUrl, employee);
  }

  updateEmployee(employee: UpdateEmployeeDto): Observable<EmployeeDto> {
    return this.http.put<EmployeeDto>(`${this.apiUrl}/${employee.id}`, employee);
  }

  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  exportToExcel(params?: EmployeeQueryParameters): Observable<Blob> {
    let httpParams = new HttpParams();
    
    if (params) {
      if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
      if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
      if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
      if (params.departmentId) httpParams = httpParams.set('departmentId', params.departmentId.toString());
      if (params.jobId) httpParams = httpParams.set('jobId', params.jobId.toString());
      if (params.minDateOfBirth) httpParams = httpParams.set('minDateOfBirth', params.minDateOfBirth);
      if (params.maxDateOfBirth) httpParams = httpParams.set('maxDateOfBirth', params.maxDateOfBirth);
    }

    return this.http.get<Blob>(`${this.apiUrl}/export`, { 
      params: httpParams,
      responseType: 'blob' as 'json'
    });
  }
}
