using Microsoft.AspNetCore.Mvc;
using Silpo.Core.DTO_s.Product;
using Silpo.Core.Interfaces;
using Silpo.Web.Models;
using System.Diagnostics;
using X.PagedList;

namespace Silpo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            List<ProductDto> products = await _productService.GetAll();
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}