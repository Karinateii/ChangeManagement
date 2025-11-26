using Change.DataAccess.Repository;
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
    public class NotApprovedController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotApprovedController> _logger;

        public NotApprovedController(IUnitOfWork unitOfWork, ILogger<NotApprovedController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var notApprovedRequests = _unitOfWork.Request.GetAll()
                    .Where(r => r.Status == "Not Approved")
                    .OrderByDescending(r => r.AdminApprovalDate)
                    .ToList();
                return View(notApprovedRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving not approved requests");
                TempData["error"] = "An error occurred while loading not approved requests.";
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
                    _logger.LogWarning("Not approved request not found with id: {Id}", id);
                    return NotFound();
                }

                return View(requestFromDb);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving not approved request details with id: {Id}", id);
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
                var notApprovedRequests = _unitOfWork.Request.GetAll()
                    .Where(r => r.Status == "Not Approved")
                    .OrderByDescending(r => r.AdminApprovalDate)
                    .ToList();

                var jsonOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                var jsonData = JsonSerializer.Serialize(new { data = notApprovedRequests }, jsonOptions);
                return Content(jsonData, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAll API call for not approved requests");
                return StatusCode(500, new { error = "An error occurred while retrieving not approved requests." });
            }
        }
        #endregion
    }
}
