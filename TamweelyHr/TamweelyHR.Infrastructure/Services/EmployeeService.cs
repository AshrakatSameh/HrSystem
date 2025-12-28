using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TamweelyHr.Domain.Entities;
using TamweelyHR.Application.DTOs.Common;
using TamweelyHR.Application.DTOs.Employees;
using TamweelyHR.Application.Exceptions;
using TamweelyHR.Application.Interfaces;
using TamweelyHR.Infrastructure.Data;

namespace TamweelyHR.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public EmployeeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
        {
            if(await _context.Employees.AnyAsync(e => e.Email == dto.Email))
            {
                throw new DuplicateException("An employee with the same email already exists.");
            }
            if(!await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId))
            {
                throw new NotFoundException("Department not found.");
            }
            if (!await _context.JobTitles.AnyAsync(j => j.Id == dto.JobId))
            {
                throw new NotFoundException("Job title not found.");
            }
            var employee = _mapper.Map<Employee>(dto);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var result = await GetByIdAsync(employee.Id)
                         ?? throw new Exception("Failed to retrieve created employee");

            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null || !employee.IsActive)
            {
                return false;
            }

            // Soft delete 
            employee.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<byte[]> ExportToExcelAsync(EmployeeQueryParameters parameters)
        {
            var exportParams = new EmployeeQueryParameters
            {
                SearchTerm = parameters.SearchTerm,
                DepartmentId = parameters.DepartmentId,
                JobId = parameters.JobId,
                DateOfBirthFrom = parameters.DateOfBirthFrom,
                DateOfBirthTo = parameters.DateOfBirthTo,
                SortBy = parameters.SortBy,
                SortDescending = parameters.SortDescending,
                PageSize = int.MaxValue, // Get all records
                PageNumber = 1
            };

            var result = await GetAllAsync(exportParams);

            // Delegate to Excel utility
            return ExcelExporter.ExportEmployees(result.Data);
            // Delegate to Excel utility
            //var excelData = ExcelExporter.ExportEmployees(result.Data);

            // Since the method is async and returns Task, we cannot return a value.
            // You can save the excelData to a file, send it to a stream, or handle it as needed.
            // For now, we assume the method's purpose is to perform the export operation.
        }

        public async Task<PagedResult<EmployeeDto>> GetAllAsync(EmployeeQueryParameters parameters)
        {
            var query = _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Job)
            .Where(e => e.IsActive)
            .AsQueryable();
            // Filtering
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var search = parameters.SearchTerm.ToLower();
                query = query.Where(e =>
                    e.FirstName.ToLower().Contains(search) ||
                    e.LastName.ToLower().Contains(search) ||
                    e.Email.ToLower().Contains(search) ||
                    e.PhoneNumber.Contains(search));
            }
            if (parameters.DepartmentId.HasValue)
                query = query.Where(e => e.DepartmentId == parameters.DepartmentId.Value);

            if (parameters.JobId.HasValue)
                query = query.Where(e => e.JobId == parameters.JobId.Value);

            if (parameters.DateOfBirthFrom.HasValue)
                query = query.Where(e => e.DateOfBirth >= parameters.DateOfBirthFrom.Value);

            if (parameters.DateOfBirthTo.HasValue)
                query = query.Where(e => e.DateOfBirth <= parameters.DateOfBirthTo.Value);
            if (parameters.HireDateFrom.HasValue)
                query = query.Where(e => e.HireDate >= parameters.HireDateFrom.Value);
            if (parameters.HireDateTo.HasValue)
                query = query.Where(e => e.HireDate <= parameters.HireDateTo.Value);


            // Sorting
            query = parameters.SortBy?.ToLower() switch
            {
                "firstname" => parameters.SortDescending
                    ? query.OrderByDescending(e => e.FirstName)
                    : query.OrderBy(e => e.FirstName),
                "lastname" => parameters.SortDescending
                    ? query.OrderByDescending(e => e.LastName)
                    : query.OrderBy(e => e.LastName),
                "email" => parameters.SortDescending
                    ? query.OrderByDescending(e => e.Email)
                    : query.OrderBy(e => e.Email),
                "dateofbirth" => parameters.SortDescending
                    ? query.OrderByDescending(e => e.DateOfBirth)
                    : query.OrderBy(e => e.DateOfBirth),
                "hiredate" => parameters.SortDescending
                    ? query.OrderByDescending(e => e.HireDate)
                    : query.OrderBy(e => e.HireDate),
                "department" => parameters.SortDescending
                ? query.OrderByDescending(e => e.Department.Name)
                : query.OrderBy(e => e.Department.Name),

                "job" => parameters.SortDescending
                    ? query.OrderByDescending(e => e.Job.Title)
                    : query.OrderBy(e => e.Job.Title),

                // Default sort by last name
                _ => query.OrderBy(e => e.LastName).ThenBy(e => e.FirstName)
            };
            var totalCount = await query.CountAsync();

            // PAGINATION
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            // Map to DTOs
            var dtos = _mapper.Map<List<EmployeeDto>>(items);

            return new PagedResult<EmployeeDto>
            {
                Data = dtos,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        public async Task<EmployeeDto> GetByIdAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Job)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                throw new NotFoundException("Employee not found.");
            }
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(dto.Id);

            if (employee == null || !employee.IsActive)
            {
                throw new NotFoundException($"Employee with ID {dto.Id} not found");
            }

           
            var duplicateEmail = await _context.Employees
                .AnyAsync(e => e.Email == dto.Email && e.Id != dto.Id && e.IsActive);

            if (duplicateEmail)
            {
                throw new DuplicateException($"Email '{dto.Email}' is already in use");
            }

            // Validate department
            var departmentExists = await _context.Departments
                .AnyAsync(d => d.Id == dto.DepartmentId && d.IsActive);

            if (!departmentExists)
            {
                throw new NotFoundException($"Department with ID {dto.DepartmentId} not found");
            }

            // Validate job
            var jobExists = await _context.JobTitles
                .AnyAsync(j => j.Id == dto.JobId && j.IsActive);

            if (!jobExists)
            {
                throw new NotFoundException($"Job with ID {dto.JobId} not found");
            }

            _mapper.Map(dto, employee);

            await _context.SaveChangesAsync();

            return await GetByIdAsync(employee.Id)
                ?? throw new Exception("Failed to retrieve updated employee");
        }
    }
}
