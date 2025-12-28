using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamweelyHR.Application.DTOs.Common;
using TamweelyHR.Application.DTOs.Jobs;

namespace TamweelyHR.Application.Interfaces
{
    public interface IJobService
    {
        Task<PagedResult<JobDto>> GetAllAsync(QueryParameters parameters);
        Task<JobDto> GetByIdAsync(int id);
        Task<JobDto> CreateAsync(CreateJobDto dto);
        Task<JobDto> UpdateAsync(UpdateJobDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
