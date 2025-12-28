using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamweelyHr.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
