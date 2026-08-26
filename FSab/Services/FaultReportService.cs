using FSab.Data;
using FSab.DTOs;
using FSab.Models;
using FSab.Models.Enums;
using FSab.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FSab.Services
{
    public class FaultReportService : IFaultReportService
    {
        private readonly AppDbContext _context;
        private readonly ISMSNotifyer _smsnotifyer;
        public FaultReportService(AppDbContext context, ISMSNotifyer smsNotifyer)
        {
            _context = context;
            _smsnotifyer = smsNotifyer;
        }

        public async Task<IEnumerable<FaultReportDto>> GetFaultReportsAsync(ClaimsPrincipal user)
        {
            if (user.IsInRole("PropertyManager"))
            {
                return await _context.FaultReports
                    .Select(x => new FaultReportDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        Status = x.Status,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    })
                    .ToListAsync();
            }

            var oid = user.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            if (oid is null)
                return [];

            return await _context.FaultReports
                .Where(x => x.CreatedBy == oid)
                .Select(x => new FaultReportDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();
        }
        public async Task<FaultReportDto?> GetFaultReportByIdAsync(int id, ClaimsPrincipal user)
        {
            var faultReport = await _context.FaultReports
                .FirstOrDefaultAsync(x => x.Id == id);

            if (faultReport is null)
                return null;

            if (user.IsInRole("PropertyManager"))
                return MapToDto(faultReport);

            var oid = user.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            if (faultReport.CreatedBy != oid)
                return null;

            return MapToDto(faultReport);
        }
        public async Task<FaultReportDto> CreateFaultReportAsync(CreateFaultReportDto dto, ClaimsPrincipal user)
        {
            var oid = user.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            if (oid is null)
                throw new UnauthorizedAccessException();

            var faultReport = new FaultReport
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = FaultReportStatus.InProgress,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = oid
            };

            _context.FaultReports.Add(faultReport);
            await _context.SaveChangesAsync();

            await _smsnotifyer.SendSMSAsync($"Ny felanmälan skapad: {faultReport.Title}");

            return MapToDto(faultReport);
        }
        public async Task<bool> UpdateFaultReportAsync(int id, UpdateFaultReportDto dto)
        {
            var faultReport = await _context.FaultReports
                .FirstOrDefaultAsync(x => x.Id == id);

            if (faultReport is null)
                return false;

            faultReport.Title = dto.Title;
            faultReport.Description = dto.Description;
            faultReport.Status = dto.Status;
            faultReport.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteFaultReportAsync(int id)
        {
            var faultReport = await _context.FaultReports
                .FirstOrDefaultAsync(x => x.Id == id);

            if (faultReport is null)
                return false;

            _context.FaultReports.Remove(faultReport);
            await _context.SaveChangesAsync();

            return true;
        }

        private static FaultReportDto MapToDto(FaultReport faultReport)
        {
            return new FaultReportDto
            {
                Id = faultReport.Id,
                Title = faultReport.Title,
                Description = faultReport.Description,
                Status = faultReport.Status,
                CreatedAt = faultReport.CreatedAt,
                UpdatedAt = faultReport.UpdatedAt
            };
        }
    }
}
