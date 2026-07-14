using LeadManagementDashboard.Services;
using LeadManagementDashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LeadManagementDashboard.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILeadService _leadService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILeadService leadService, ILogger<IndexModel> logger)
        {
            _leadService = leadService;
            _logger = logger;
        }

        public DashboardViewModel Dashboard { get; private set; } = new();

        [TempData]
        public string? ToastMessage { get; set; }

        [TempData]
        public bool ToastIsError { get; set; }

        // GET
        public async Task OnGetAsync(CancellationToken ct)
        {
            Dashboard = await _leadService.GetDashboardAsync(ct);
        }

        // POST
        public async Task<IActionResult> OnPostMoveAsync(int leadId, string direction, CancellationToken ct)
        {
            if (!Enum.TryParse<MoveDirection>(direction, ignoreCase: true, out var moveDirection))
            {
                ToastMessage = "Invalid move direction.";
                ToastIsError = true;
                return RedirectToPage();
            }

            try
            {
                var result = await _leadService.MoveLeadAsync(leadId, moveDirection, ct);
                ToastMessage = result.Message;
                ToastIsError = !result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to move lead {LeadId}", leadId);
                ToastMessage = "An unexpected error occurred while moving the lead.";
                ToastIsError = true;
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetLeadDetailAsync(int id, CancellationToken ct)
        {
            var detail = await _leadService.GetLeadDetailAsync(id, ct);
            if (detail is null)
            {
                return NotFound();
            }

            return new JsonResult(detail);
        }
    }
}
