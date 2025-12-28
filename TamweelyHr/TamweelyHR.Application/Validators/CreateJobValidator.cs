using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamweelyHR.Application.DTOs.Jobs;

namespace TamweelyHR.Application.Validators
{
    public class CreateJobValidator : AbstractValidator<CreateJobDto>
    {
        public CreateJobValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Job title is required")
                .MaximumLength(100).WithMessage("Job title cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
    public class UpdateJobValidator : AbstractValidator<UpdateJobDto>
    {
        public UpdateJobValidator()
        {
            Include(new CreateJobValidator());
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
