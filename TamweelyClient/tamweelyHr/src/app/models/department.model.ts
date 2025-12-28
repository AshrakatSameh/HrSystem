export interface DepartmentDto {
  id: number;
  name: string;
  description?: string;
  isActive: boolean;
}

export interface CreateDepartmentDto {
  name: string;
  description?: string;
}

export interface UpdateDepartmentDto extends CreateDepartmentDto {
  id: number;
}

export interface DepartmentQueryParameters {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  sortBy?: string;
  sortDescending?: boolean;
}
