using FSab.DTOs;
using FSab.Models;
using System.Security.Claims;

namespace FSab.Services.Interfaces
{
    public interface IFaultReportService
    {
        Task<IEnumerable<FaultReportDto>> GetFaultReportsAsync(ClaimsPrincipal user);
        Task<FaultReportDto?> GetFaultReportByIdAsync(int id, ClaimsPrincipal user);
        Task<FaultReportDto> CreateFaultReportAsync(CreateFaultReportDto dto, ClaimsPrincipal user);
        Task<bool> UpdateFaultReportAsync(int id, UpdateFaultReportDto dto);
        Task<bool> DeleteFaultReportAsync(int id);
    }
}
