using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamweelyHR.Application.DTOs.Common;

namespace TamweelyHR.Application.DTOs.Employees
{
    public class EmployeeQueryParameters : QueryParameters
    {
        //Filter by DepartmentId
        public int? DepartmentId { get; set; }
        //Filter by JobId
        public int? JobId { get; set; }
        // Filter employees born on or after this date
        public DateTime? DateOfBirthFrom { get; set; }

        // Filter employees born on or before this date
        public DateTime? DateOfBirthTo { get; set; }
        //filter by HireDateFrom
        public DateTime? HireDateFrom { get; set; }
        //filter by HireDateTo
        public DateTime? HireDateTo { get; set; }
    }
}
