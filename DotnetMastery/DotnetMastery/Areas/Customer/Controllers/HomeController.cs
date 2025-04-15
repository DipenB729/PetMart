using System.Diagnostics;
using Dotnet.Models;
using DotnetMastery.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace DotnetMastery.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string searchTerm)
        {
            IEnumerable<Product> productList;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                productList = _unitOfWork.Product.GetAll(includeProperties: "Category")
                              .Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            }

            return View(productList);
        }

        public IActionResult Details(int productId)
        {
           Product product= _unitOfWork.Product.Get(u=>u.Id==productId,includeProperties: "Category");
            return View(product);
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
