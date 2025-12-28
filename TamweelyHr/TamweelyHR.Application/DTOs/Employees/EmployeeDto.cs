using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamweelyHR.Application.DTOs.Employees
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int Age => DateTime.Today.Year - DateOfBirth.Year;
        public DateTime HireDate { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;

        public int JobId { get; set; }
        public string JobName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    //DTO creare new emp
    public class CreateEmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public int DepartmentId { get; set; }
        public int JobId { get; set; }
    }
    //DTO update emp
    public class UpdateEmployeeDto : CreateEmployeeDto
    {
      public int Id { get; set; }
    }
}
