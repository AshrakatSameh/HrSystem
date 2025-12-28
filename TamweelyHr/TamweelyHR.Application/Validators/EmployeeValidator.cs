using FluentValidation;
using TamweelyHR.Application.DTOs.Employees;

namespace TamweelyHR.Application.Validators
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Mobile is required")
                .Matches(@"^01[0-2,5]{1}[0-9]{8}$")
                .WithMessage("Invalid Egyptian mobile format. Must be 11 digits starting with 01");

            RuleFor(x => x.HireDate)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Hire date cannot be in the future");



            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department is required");

            RuleFor(x => x.JobId)
                .GreaterThan(0).WithMessage("Job title is required");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past")
                .GreaterThan(DateTime.Today.AddYears(-65)).WithMessage("Employee must be younger than 65 years old");
        }

       
    }
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeValidator()
        {
            Include(new CreateEmployeeValidator());
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid employee ID");
        }
    }
}
