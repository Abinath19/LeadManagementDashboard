using LeadManagementDashboard.ViewModels;

namespace LeadManagementDashboard.Services
{
    public enum MoveDirection { Forward, Backward }
    public interface ILeadService
    {
        Task<DashboardViewModel> GetDashboardAsync(CancellationToken ct = default);
        Task<ServiceResult> MoveLeadAsync(int leadId, MoveDirection direction, CancellationToken ct = default);
        Task<LeadDetailViewModel?> GetLeadDetailAsync(int leadId, CancellationToken ct = default);
    }
}
