using Change.Models;
using Change.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Change.DataAccess.Repository.IRepository;

namespace ChangeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                // Get recent requests for authenticated users
                var recentRequests = _unitOfWork.Request
                    .GetAll()
                    .OrderByDescending(r => r.Date)
                    .Take(5)
                    .ToList();
                
                return View(recentRequests);
            }
            
            return View(new List<Request>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}