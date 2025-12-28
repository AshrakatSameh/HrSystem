using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamweelyHR.Application.DTOs.Departments;

namespace TamweelyHR.Application.Validators
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentValidator()
        {
            Include(new CreateDepartmentValidator());
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
