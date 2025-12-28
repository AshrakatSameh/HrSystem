using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamweelyHR.Application.DTOs.Common;
using TamweelyHR.Application.DTOs.Departments;

namespace TamweelyHR.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<PagedResult<DepartmentDto>> GetAllAsync(QueryParameters parameters);
        Task<DepartmentDto?> GetByIdAsync(int id);
        Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
        Task<DepartmentDto> UpdateAsync(UpdateDepartmentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
