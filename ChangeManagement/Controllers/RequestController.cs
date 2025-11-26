using Change.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;
using System.Text.Json;
using Change.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Change.Utility;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ChangeManagement.Controllers
{
    [Authorize(Policy = "AdminOrEmployee")]
    public class RequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RequestController> _logger;

        public RequestController(IUnitOfWork unitOfWork, ILogger<RequestController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                List<Request> requestList = _unitOfWork.Request.GetAll().ToList();
                return View(requestList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving request list");
                TempData["error"] = "An error occurred while loading requests.";
                return View(new List<Request>());
            }
        }

        [Authorize(Policy = "EmployeeOnly")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "EmployeeOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Request obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Set default values
                    obj.Date = DateTime.Now;
                    obj.Status = "Pending";
                    
                    _unitOfWork.Request.Add(obj);
                    _unitOfWork.Save();
                    
                    _logger.LogInformation("Request created successfully by {User}: {Title}", obj.SubmittedBy, obj.Title);
                    TempData["success"] = "Request Created Successfully";
                    return RedirectToAction("Index");
                }
                
                _logger.LogWarning("Invalid model state in Create action");
                return View(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating request");
                TempData["error"] = "An error occurred while creating the request.";
                return View(obj);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                _logger.LogWarning("Edit called with null or invalid id");
                return NotFound();
            }

            try
            {
                Request? requestFromDb = _unitOfWork.Request.Get(u => u.Id == id);
                
                if (requestFromDb == null)
                {
                    _logger.LogWarning("Request not found with id: {Id}", id);
                    return NotFound();
                }
                
                return View(requestFromDb);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving request for edit with id: {Id}", id);
                TempData["error"] = "An error occurred while loading the request.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Request obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Request.update(obj);
                    _unitOfWork.Save();
                    
                    _logger.LogInformation("Request updated successfully: {Id}", obj.Id);
                    TempData["success"] = "Request Updated Successfully";
                    return RedirectToAction("Index");
                }
                
                _logger.LogWarning("Invalid model state in Edit action for request: {Id}", obj.Id);
                return View(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating request: {Id}", obj.Id);
                TempData["error"] = "An error occurred while updating the request.";
                return View(obj);
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
                    _logger.LogWarning("Request not found with id: {Id}", id);
                    return NotFound();
                }

                return View(requestFromDb);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving request details with id: {Id}", id);
                TempData["error"] = "An error occurred while loading request details.";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                _logger.LogWarning("Delete called with null or invalid id");
                return NotFound();
            }

            try
            {
                Request? requestFromDb = _unitOfWork.Request.Get(u => u.Id == id);

                if (requestFromDb == null)
                {
                    _logger.LogWarning("Request not found for deletion with id: {Id}", id);
                    return NotFound();
                }
                
                return View(requestFromDb);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving request for deletion with id: {Id}", id);
                TempData["error"] = "An error occurred while loading the request.";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            try
            {
                Request? obj = _unitOfWork.Request.Get(u => u.Id == id);
                
                if (obj == null)
                {
                    _logger.LogWarning("Request not found for deletion with id: {Id}", id);
                    return NotFound();
                }

                _unitOfWork.Request.Remove(obj);
                _unitOfWork.Save();
                
                _logger.LogInformation("Request deleted successfully: {Id}", id);
                TempData["success"] = "Request Deleted Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting request with id: {Id}", id);
                TempData["error"] = "An error occurred while deleting the request.";
                return RedirectToAction("Index");
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                List<Request> requestList = _unitOfWork.Request.GetAll().OrderByDescending(r => r.Date).ToList();

                var jsonOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNamingPolicy = null // Keep PascalCase
                };

                return Json(new { data = requestList }, jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAll API call");
                return StatusCode(500, new { error = "An error occurred while retrieving requests." });
            }
        }
        #endregion
    }
}
