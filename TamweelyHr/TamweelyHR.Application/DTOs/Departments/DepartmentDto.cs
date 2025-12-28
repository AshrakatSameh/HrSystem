using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamweelyHR.Application.DTOs.Departments
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
    //Create dept
    public class CreateDepartmentDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
    //Update dept
    public class UpdateDepartmentDto : CreateDepartmentDto
    {
        public int Id { get; set; }
    }
}
