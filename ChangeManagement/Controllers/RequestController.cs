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
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

    public class RequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly UserManager<ApplicationUser> _userManager;

        public RequestController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            //_userManager = userManager;
        }

        // GET: Suppliers
        public IActionResult Index()
        {
            List<Request> requestList = _unitOfWork.Request.GetAll().ToList();
            return View(requestList);
        }

        [Authorize(Roles = SD.Role_Employee)]
        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = SD.Role_Employee)]
        // POST: Suppliers/Create
        [HttpPost]
        public IActionResult Create(Request obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Request.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Request Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Suppliers/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Request? requestFromDb = _unitOfWork.Request.Get(u => u.Id == id);
            // Supplier? supplierFromDb1 = _db.Suppliers.FirstOrDefault(u=>u.SupplierID==id);
            // Supplier? supplierFromDb2 = _db.Suppliers.Where(u=>u.SupplierID==id).FirstOrDefault();

            if (requestFromDb == null)
            {
                return NotFound();
            }
            return View(requestFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Request obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Request.update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Request Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Suppliers/Details/5
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

        //Get Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Request? requestFromDb = _unitOfWork.Request.Get(u => u.Id == id);

            if (requestFromDb == null)
            {
                return NotFound();
            }
            return View(requestFromDb);
        }

        //POST Delete
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Request? obj = _unitOfWork.Request.Get(u => u.Id == id);
            {
                if (obj == null)
                {
                    return NotFound();
                }

                _unitOfWork.Request.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "Request Deleted Successfully";
                return RedirectToAction("Index");
            }

        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Request> requestList = _unitOfWork.Request.GetAll().ToList();

            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                // Add any other serialization options if needed
            };

            var jsonData = JsonSerializer.Serialize(new { data = requestList }, jsonOptions);

            return Content(jsonData, "application/json");
        }
        #endregion
    }
}
