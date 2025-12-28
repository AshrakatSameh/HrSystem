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
using TamweelyHR.Application.DTOs.Jobs;
using TamweelyHR.Application.Exceptions;
using TamweelyHR.Application.Interfaces;
using TamweelyHR.Infrastructure.Data;

namespace TamweelyHR.Infrastructure.Services
{
    public class JobService : IJobService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public JobService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<JobDto>> GetAllAsync(QueryParameters parameters)
        {
            var query = _context.JobTitles
                .Where(j => j.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower();
                query = query.Where(j => j.Title.ToLower().Contains(searchLower));
            }

            query = parameters.SortDescending
                ? query.OrderByDescending(j => j.Title)
                : query.OrderBy(j => j.Title);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<JobDto>
            {
                Data = _mapper.Map<List<JobDto>>(items),
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

        }

        public async Task<JobDto> GetByIdAsync(int id)
        {
            var job = await _context.JobTitles
                .FirstOrDefaultAsync(j => j.Id == id && j.IsActive);

            return job == null ? null : _mapper.Map<JobDto>(job);
        }

        public async Task<JobDto> CreateAsync(CreateJobDto dto)
        {
            if (await _context.JobTitles.AnyAsync(j => j.Title == dto.Title && j.IsActive))
            {
                throw new DuplicateException($"Job '{dto.Title}' already exists");
            }

            // Map CreateJobDto to Job entity instead of JobDto
            var job = _mapper.Map<Job>(dto);
            _context.JobTitles.Add(job);
            await _context.SaveChangesAsync();

            // Map the saved Job entity back to JobDto
            return _mapper.Map<JobDto>(job);
        }

        public async Task<JobDto> UpdateAsync(UpdateJobDto dto)
        {
            var job = await _context.JobTitles.FindAsync(dto.Id);

            if (job == null || !job.IsActive)
            {
                throw new NotFoundException($"Job with ID {dto.Id} not found");
            }

            var duplicateName = await _context.JobTitles
                .AnyAsync(j => j.Title == dto.Title && j.Id != dto.Id && j.IsActive);

            if (duplicateName)
            {
                throw new DuplicateException($"Job '{dto.Title}' already exists");
            }

            _mapper.Map(dto, job);
            await _context.SaveChangesAsync();

            return _mapper.Map<JobDto>(job);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var job = await _context.JobTitles.FindAsync(id);

            if (job == null || !job.IsActive)
            {
                return false;
            }

            var hasEmployees = await _context.Employees
                .AnyAsync(e => e.JobId == id && e.IsActive);

            if (hasEmployees)
            {
                throw new InvalidOperationException(
                    "Cannot delete job with active employees. " +
                    "Please reassign employees first.");
            }

            job.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
