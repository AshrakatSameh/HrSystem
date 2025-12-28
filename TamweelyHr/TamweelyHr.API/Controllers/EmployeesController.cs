using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TamweelyHR.Application.DTOs.Common;
using TamweelyHR.Application.DTOs.Employees;
using TamweelyHR.Application.Interfaces;

namespace TamweelyHr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet]
        public async Task<ActionResult<PagedResult<EmployeeDto>>> GetAll(
        [FromQuery] EmployeeQueryParameters parameters)
        {
            var result = await _employeeService.GetAllAsync(parameters);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return employee == null ? NotFound() : Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateEmployeeDto dto)
        {
            var employee = await _employeeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateEmployeeDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            var employee = await _employeeService.UpdateAsync(dto);
            return Ok(employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeService.DeleteAsync(id);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export([FromQuery] EmployeeQueryParameters parameters)
        {
            var bytes = await _employeeService.ExportToExcelAsync(parameters);
            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Employees_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx");
        }

    }
}
