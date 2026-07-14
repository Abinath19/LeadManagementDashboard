namespace LeadManagementDashboard.ViewModels
{
    public class DashboardViewModel
    {
        public IReadOnlyList<StatusColumnViewModel> Columns { get; init; } = [];
    }

    public class StatusColumnViewModel
    {
        public int StatusId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string ColorCode { get; init; } = string.Empty;
        public bool IsFirst { get; init; }   
        public bool IsLast { get; init; }    
        public IReadOnlyList<LeadCardViewModel> Leads { get; init; } = [];
        public int LeadCount => Leads.Count; 
    }

    public class LeadCardViewModel
    {
        public int Id { get; init; }
        public string FullName { get; init; } = string.Empty;  
        public string Email { get; init; } = string.Empty;
        public string? Company { get; init; }
    }
}
