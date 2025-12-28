using AutoMapper;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamweelyHr.Domain.Entities;
using TamweelyHR.Application.DTOs.Common;
using TamweelyHR.Application.DTOs.Departments;
using TamweelyHR.Application.Exceptions;
using TamweelyHR.Application.Interfaces;
using TamweelyHR.Infrastructure.Data;

namespace TamweelyHR.Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<DepartmentDto>> GetAllAsync(QueryParameters parameters)
        {
            var query = _context.Departments
                .Where(d => d.IsActive)
                .AsQueryable();

            // Simple search by name
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower();
                query = query.Where(d => d.Name.ToLower().Contains(searchLower));
            }

            // Sort by name
            query = parameters.SortDescending
                ? query.OrderByDescending(d => d.Name)
                : query.OrderBy(d => d.Name);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<DepartmentDto>
            {
                Data = _mapper.Map<List<DepartmentDto>>(items),
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);

            return department == null ? null : _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
        {
            // Department name must be unique
            if (await _context.Departments.AnyAsync(d => d.Name == dto.Name && d.IsActive))
            {
                throw new DuplicateException($"Department '{dto.Name}' already exists");
            }

            // Map CreateDepartmentDto to Department entity
             var department = _mapper.Map<Department>(dto);
            //var department = new Department
            //{
            //    Name = dto.Name,
            //    Description = dto.Description,
            //    IsActive = true,
            //    CreatedAt = DateTime.UtcNow
            //};
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> UpdateAsync(UpdateDepartmentDto dto)
        {
            var department = await _context.Departments.FindAsync(dto.Id);

            if (department == null || !department.IsActive)
            {
                throw new NotFoundException($"Department with ID {dto.Id} not found");
            }

            // Check for duplicate name (excluding current record)
            var duplicateName = await _context.Departments
                .AnyAsync(d => d.Name == dto.Name && d.Id != dto.Id && d.IsActive);

            if (duplicateName)
            {
                throw new DuplicateException($"Department '{dto.Name}' already exists");
            }

            _mapper.Map(dto, department);
            await _context.SaveChangesAsync();

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null || !department.IsActive)
            {
                return false;
            }

            // Check if any active employees belong to this department
            var hasEmployees = await _context.Employees
                .AnyAsync(e => e.DepartmentId == id && e.IsActive);

            if (hasEmployees)
            {
                throw new InvalidOperationException(
                    "Cannot delete department with active employees. " +
                    "Please reassign employees first.");
            }

            department.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
