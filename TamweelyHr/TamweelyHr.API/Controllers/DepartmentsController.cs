using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TamweelyHR.Application.DTOs.Common;
using TamweelyHR.Application.DTOs.Departments;
using TamweelyHR.Application.Interfaces;

namespace TamweelyHr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<PagedResult<DepartmentDto>>> GetAll(
            [FromQuery] QueryParameters parameters)
        {
            var result = await _departmentService.GetAllAsync(parameters);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            return department == null ? NotFound() : Ok(department);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateDepartmentDto dto)
        {
            var department = await _departmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateDepartmentDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            var department = await _departmentService.UpdateAsync(dto);
            return Ok(department);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _departmentService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
