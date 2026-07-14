namespace LeadManagementDashboard.Models
{
    public class Lead
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Company { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Status Status { get; set; } = null!;
        public ICollection<LeadActivity> Activities { get; set; } = new List<LeadActivity>();
    }
}
