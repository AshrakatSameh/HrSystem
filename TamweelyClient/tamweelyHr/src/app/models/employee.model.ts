export interface EmployeeDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  dateOfBirth: string | Date;
  age: number;
  hireDate: string | Date;
  departmentId: number;
  departmentName: string;
  jobId: number;
  jobName: string;
  isActive: boolean;
}

export interface CreateEmployeeDto {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  dateOfBirth: string | Date;
  hireDate: string | Date;
  departmentId: number;
  jobId: number;
}

export interface UpdateEmployeeDto extends CreateEmployeeDto {
  id: number;
}

export interface EmployeeQueryParameters {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  departmentId?: number;
  jobId?: number;
  minDateOfBirth?: string;
  maxDateOfBirth?: string;
  sortBy?: string;
  sortDescending?: boolean;
}
