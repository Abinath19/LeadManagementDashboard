namespace LeadManagementDashboard.ViewModels
{
    public class LeadDetailViewModel
    {
        public int Id { get; init; }
        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? Phone { get; init; }
        public string? Company { get; init; }
        public string StatusName { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public IReadOnlyList<ActivityViewModel> Activities { get; init; } = [];
    }
    public class ActivityViewModel
    {
        public string? FromStatus { get; init; } 
        public string ToStatus { get; init; } = string.Empty;
        public DateTime ChangedAt { get; init; }
        public string? Note { get; init; }
    }
}
