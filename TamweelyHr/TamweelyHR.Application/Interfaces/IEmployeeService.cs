using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamweelyHR.Application.DTOs.Common;
using TamweelyHR.Application.DTOs.Employees;

namespace TamweelyHR.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<PagedResult<EmployeeDto>> GetAllAsync(EmployeeQueryParameters parameters);
        Task<EmployeeDto> GetByIdAsync(int id);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
        Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto dto);
        Task<bool> DeleteAsync(int id);
        Task<byte[]> ExportToExcelAsync(EmployeeQueryParameters parameters);
    }
}
