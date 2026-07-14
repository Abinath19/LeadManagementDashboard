namespace LeadManagementDashboard.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public string ColorCode { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}
