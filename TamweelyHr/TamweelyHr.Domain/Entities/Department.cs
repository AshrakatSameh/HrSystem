using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamweelyHr.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
