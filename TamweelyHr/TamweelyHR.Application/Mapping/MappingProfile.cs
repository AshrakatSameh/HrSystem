using AutoMapper;
using TamweelyHr.Domain.Entities;
using TamweelyHR.Application.DTOs.Departments;
using TamweelyHR.Application.DTOs.Employees;
using TamweelyHR.Application.DTOs.Jobs;

namespace TamweelyHR.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ========== DEPARTMENT ==========
            CreateMap<Department, DepartmentDto>();

            CreateMap<CreateDepartmentDto, Department>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Employees, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.Employees, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            // ========== JOB TITLE ==========
            CreateMap<Job, JobDto>();

            CreateMap<CreateJobDto, Job>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Employees, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateJobDto, Job>()
                .ForMember(dest => dest.Employees, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            // ========== EMPLOYEE ==========
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                .ForMember(dest => dest.JobName,
                    opt => opt.MapFrom(src => src.Job != null ? src.Job.Title : null));

            CreateMap<CreateEmployeeDto, Employee>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Job, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateEmployeeDto, Employee>()
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Job, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        }
    }
}