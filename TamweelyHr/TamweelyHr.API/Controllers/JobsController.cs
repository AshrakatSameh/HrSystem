using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TamweelyHR.Application.DTOs.Common;
using TamweelyHR.Application.DTOs.Jobs;
using TamweelyHR.Application.Interfaces;

namespace TamweelyHr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<PagedResult<JobDto>>> GetAll(
            [FromQuery] QueryParameters parameters)
        {
            var result = await _jobService.GetAllAsync(parameters);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> GetById(int id)
        {
            var job = await _jobService.GetByIdAsync(id);
            return job == null ? NotFound() : Ok(job);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateJobDto dto)
        {
            var job = await _jobService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateJobDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            var job = await _jobService.UpdateAsync(dto);
            return Ok(job);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _jobService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
