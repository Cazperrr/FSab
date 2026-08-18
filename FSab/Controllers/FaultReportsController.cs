using FSab.DTOs;
using FSab.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FSab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FaultReportsController(IFaultReportService service) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FaultReportDto>>> GetFaultReports()
        {
            var result = await service.GetFaultReportsAsync(User);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FaultReportDto>> GetFaultReport(int id)
        {
            var result = await service.GetFaultReportByIdAsync(id, User);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Resident,PropertyManager")]
        public async Task<ActionResult<FaultReportDto>> CreateFaultReport(CreateFaultReportDto dto)
        {
            var result = await service.CreateFaultReportAsync(dto, User);

            return CreatedAtAction(nameof(GetFaultReport), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> UpdateFaultReport(int id, UpdateFaultReportDto dto)
        {
            var updated = await service.UpdateFaultReportAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "PropertyManager")]
        public async Task<IActionResult> DeleteFaultReport(int id)
        {
            var deleted = await service.DeleteFaultReportAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
