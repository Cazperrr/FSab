using FSab.Models.Enums;

namespace FSab.DTOs
{
    public class UpdateFaultReportDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public FaultReportStatus Status { get; set; }
    }
}
