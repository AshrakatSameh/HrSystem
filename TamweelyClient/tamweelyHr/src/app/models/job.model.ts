export interface JobDto {
  id: number;
  title: string;
  description?: string;
  isActive: boolean;
}

export interface CreateJobDto {
  title: string;
  description?: string;
}

export interface UpdateJobDto extends CreateJobDto {
  id: number;
}

export interface JobQueryParameters {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  sortBy?: string;
  sortDescending?: boolean;
}
