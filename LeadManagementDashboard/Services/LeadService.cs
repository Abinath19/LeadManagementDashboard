using LeadManagementDashboard.Data;
using LeadManagementDashboard.Models;
using LeadManagementDashboard.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementDashboard.Services
{
    public class LeadService : ILeadService
    {
        private readonly AppDbContext _db;

        public LeadService(AppDbContext db) => _db = db;   

        public async Task<DashboardViewModel> GetDashboardAsync(CancellationToken ct = default)
        {
            var statuses = await _db.Statuses
                .AsNoTracking()
                .Where(s => s.IsActive)                
                .OrderBy(s => s.DisplayOrder)           
                .ToListAsync(ct);

            var leads = await _db.Leads
                .AsNoTracking()
                .OrderBy(l => l.CreatedAt)
                .ToListAsync(ct);

            var columns = statuses.Select((status, index) => new StatusColumnViewModel
            {
                StatusId = status.Id,
                Name = status.Name,
                ColorCode = status.ColorCode,
                IsFirst = index == 0,                    
                IsLast = index == statuses.Count - 1,    
                Leads = leads
                    .Where(l => l.StatusId == status.Id)
                    .Select(l => new LeadCardViewModel
                    {
                        Id = l.Id,
                        FullName = $"{l.FirstName} {l.LastName}",
                        Email = l.Email,
                        Company = l.Company
                    })
                    .ToList()
            }).ToList();

            return new DashboardViewModel { Columns = columns };
        }

        public async Task<ServiceResult> MoveLeadAsync(int leadId, MoveDirection direction, CancellationToken ct = default)
        {
            var lead = await _db.Leads
                .Include(l => l.Status)
                .FirstOrDefaultAsync(l => l.Id == leadId, ct);

            if (lead is null)
                return ServiceResult.Fail("Lead not found.");

            var statuses = await _db.Statuses
                .Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync(ct);

            var currentIndex = statuses.FindIndex(s => s.Id == lead.StatusId);
            if (currentIndex < 0)
                return ServiceResult.Fail("The lead's current status is inactive or unknown.");

            var targetIndex = direction == MoveDirection.Forward
                ? currentIndex + 1
                : currentIndex - 1;

            if (targetIndex >= statuses.Count)
                return ServiceResult.Fail($"Cannot move forward: '{lead.Status.Name}' is the last status.");
            if (targetIndex < 0)
                return ServiceResult.Fail($"Cannot move backward: '{lead.Status.Name}' is the first status.");

            var fromStatus = statuses[currentIndex];
            var toStatus = statuses[targetIndex];

            lead.StatusId = toStatus.Id;
            _db.LeadActivities.Add(new LeadActivity
            {
                LeadId = lead.Id,
                FromStatusId = fromStatus.Id,
                ToStatusId = toStatus.Id,
                ChangedAt = DateTime.UtcNow,
                Note = "Status changed by user"
            });

            await _db.SaveChangesAsync(ct);

            return ServiceResult.Ok(
                $"{lead.FirstName} {lead.LastName} moved from '{fromStatus.Name}' to '{toStatus.Name}'.");
        }

        public async Task<LeadDetailViewModel?> GetLeadDetailAsync(int leadId, CancellationToken ct = default)
        {
            var lead = await _db.Leads
                .AsNoTracking()
                .Include(l => l.Status)
                .Include(l => l.Activities.OrderByDescending(a => a.ChangedAt))
                    .ThenInclude(a => a.FromStatus)
                .Include(l => l.Activities)
                    .ThenInclude(a => a.ToStatus)
                .FirstOrDefaultAsync(l => l.Id == leadId, ct);

            if (lead is null) return null;

            return new LeadDetailViewModel
            {
                Id = lead.Id,
                FullName = $"{lead.FirstName} {lead.LastName}",
                Email = lead.Email,
                Phone = lead.Phone,
                Company = lead.Company,
                StatusName = lead.Status.Name,
                CreatedAt = lead.CreatedAt,
                Activities = lead.Activities.Select(a => new ActivityViewModel
                {
                    FromStatus = a.FromStatus?.Name,
                    ToStatus = a.ToStatus.Name,
                    ChangedAt = a.ChangedAt,
                    Note = a.Note
                }).ToList()
            };
        }
    }
}
