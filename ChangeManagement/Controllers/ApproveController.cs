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
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class ApproveController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApproveController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // Retrieve only requests with "Approved" status
            var approvedRequests = _unitOfWork.Request.GetAll().Where(r => r.Status == "Approved").ToList();
            return View(approvedRequests);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Request requestFromDb = _unitOfWork.Request.Get(u => u.Id == id);

            if (requestFromDb == null)
            {
                return NotFound();
            }

            return View(requestFromDb);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var approvedRequests = _unitOfWork.Request.GetAll().Where(r => r.Status == "Approved").ToList();

            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                // Add any other serialization options if needed
            };

            var jsonData = JsonSerializer.Serialize(new { data = approvedRequests }, jsonOptions);

            return Content(jsonData, "application/json");
        }
        #endregion
    }
}
