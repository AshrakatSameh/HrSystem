using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamweelyHR.Application.DTOs.Jobs
{
    public class JobDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
    //Create Job
    public class CreateJobDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
    //Update Job
    public class UpdateJobDto : CreateJobDto
    {
        public int Id { get; set; }
    }
}
