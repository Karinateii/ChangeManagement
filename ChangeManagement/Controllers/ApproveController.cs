using Change.DataAccess.Repository.IRepository;
using Change.Models.Models;
using Change.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ChangeManagement.Controllers
{
    [Authorize(Policy = "AdminOrEmployee")]
    public class ApproveController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ApproveController> _logger;

        public ApproveController(IUnitOfWork unitOfWork, ILogger<ApproveController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var approvedRequests = _unitOfWork.Request.GetAll()
                    .Where(r => r.Status == "Approved")
                    .OrderByDescending(r => r.AdminApprovalDate)
                    .ToList();
                return View(approvedRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving approved requests");
                TempData["error"] = "An error occurred while loading approved requests.";
                return View(new List<Request>());
            }
        }

        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                _logger.LogWarning("Details called with null or invalid id");
                return NotFound();
            }

            try
            {
                Request? requestFromDb = _unitOfWork.Request.Get(u => u.Id == id);

                if (requestFromDb == null)
                {
                    _logger.LogWarning("Approved request not found with id: {Id}", id);
                    return NotFound();
                }

                return View(requestFromDb);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving approved request details with id: {Id}", id);
                TempData["error"] = "An error occurred while loading request details.";
                return RedirectToAction("Index");
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var approvedRequests = _unitOfWork.Request.GetAll()
                    .Where(r => r.Status == "Approved")
                    .OrderByDescending(r => r.AdminApprovalDate)
                    .ToList();

                var jsonOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                var jsonData = JsonSerializer.Serialize(new { data = approvedRequests }, jsonOptions);
                return Content(jsonData, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAll API call for approved requests");
                return StatusCode(500, new { error = "An error occurred while retrieving approved requests." });
            }
        }
        #endregion
    }
}
