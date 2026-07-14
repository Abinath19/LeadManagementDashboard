namespace LeadManagementDashboard.Models
{
    public class LeadActivity
    {
        public int Id { get; set; }
        public int LeadId { get; set; }
        public int? FromStatusId { get; set; }
        public int ToStatusId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? Note { get; set; }
        public Lead Lead { get; set; } = null!;
        public Status? FromStatus { get; set; }
        public Status ToStatus { get; set; } = null!;
    }
}
