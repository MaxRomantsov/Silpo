using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Silpo.Core.DTO_s.Category;
using Silpo.Core.DTO_s.Product;
using Silpo.Core.Interfaces;
using Silpo.Core.Validation.Category;
using System.Data;
using X.PagedList;

namespace Silpo.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;


        public CategoryController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GettAll());
        }
        [Authorize(Roles = "administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDto model)
        {
            var validator = new CreateValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _categoryService.GetByName(model);
                if (!result.Success)
                {
                    ViewBag.AuthError = "Category exists.";
                    return View(model);
                }
                await _categoryService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AuthError = validationResult.Errors[0];
            return View(model);
        }

        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var categoryDto = await _categoryService.Get(id);

            if (categoryDto == null)
            {
                ViewBag.AuthError = "Category not found.";
                return View();
            }
            return View(categoryDto);
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDto model) // 1-FromForm, 2-FromRoute, 
        {
            var validator = new CreateValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                await _categoryService.Update(model);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AuthError = validationResult.Errors[0];
            return View(model);
        }

        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryDto = await _categoryService.Get(id);

            if (categoryDto == null)
            {
                ViewBag.AuthError = "Category not found.";
                return View();
            }

            List<ProductDto> posts = await _productService.GetByCategory(id);
            ViewBag.CategoryName = categoryDto.Name;
            ViewBag.CategoryId = categoryDto.Id;

            int pageSize = 20;
            int pageNumber = 1;
            return View("Delete", posts.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var categoryDto = await _categoryService.Get(id);
            if (categoryDto == null)
            {
                ViewBag.AuthError = "Category not found.";
                return View();
            }
            await _categoryService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
