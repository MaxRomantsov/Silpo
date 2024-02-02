using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Silpo.Core.DTO_s.Category;
using Silpo.Core.DTO_s.Product;
using Silpo.Core.Interfaces;
using Silpo.Core.Services;
using Silpo.Core.Validation.Product;
using System.Data;
using X.PagedList;

namespace Silpo.Web.Controllers
{
   
        public class ProductController : Controller
        {
            private readonly ICategoryService _categoryService;
            private readonly IProductService _productService;
            public ProductController(ICategoryService categoryService, IProductService productService)
            {
                _categoryService = categoryService;
                _productService = productService;
            }

            [AllowAnonymous]
            public async Task<IActionResult> Index(int? page)
            {
                List<ProductDto> products = await _productService.GetAll();
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(products.ToPagedList(pageNumber, pageSize));
            }


            [Authorize(Roles = "administrator")]
            public async Task<IActionResult> Create()
            {
                await LoadCategories();
                return View();
            }
            [Authorize(Roles = "administrator")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(ProductDto model)
            {
                var validator = new CreateValidation();
                var validatinResult = await validator.ValidateAsync(model);
                if (validatinResult.IsValid)
                {
                    var files = HttpContext.Request.Form.Files;
                    model.File = files;
                    await _productService.Create(model);
                    return RedirectToAction("Index", "Product");
                }
                ViewBag.AuthError = validatinResult.Errors[0];
                return View();
            }
            private async Task LoadCategories()
            {
                ViewBag.CategoryList = new SelectList(
                    await _categoryService.GettAll(),
                    nameof(CategoryDto.Id),
                    nameof(CategoryDto.Name)
                    );
                ;
            }
            public async Task<IActionResult> GoodsByCategory(int id)
            {
                List<ProductDto> products = await _productService.GetByCategory(id);
                int pageSize = 20;
                int pageNumber = 1;
                return View("Index", products.ToPagedList(pageNumber, pageSize));
            }
            [Authorize(Roles = "administrator")]
            public async Task<IActionResult> Edit(int id)
            {
                var products = await _productService.Get(id);

                if (products == null) return NotFound();

                await LoadCategories();
                return View(products);
            }

            [Authorize(Roles = "administrator")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(ProductDto model)
            {
                var validator = new CreateValidation();
                var validationResult = await validator.ValidateAsync(model);
                if (validationResult.IsValid)
                {
                    var files = HttpContext.Request.Form.Files;
                    model.File = files;
                    await _productService.Update(model);
                    return RedirectToAction("Index", "Product");
                }
                ViewBag.CreatePostError = validationResult.Errors[0];
                return View(model);
            }
            [Authorize(Roles = "administrator")]
            public async Task<IActionResult> Delete(int id)
            {
                var productsDto = await _productService.Get(id);

                if (productsDto == null)
                {
                    ViewBag.AuthError = "product not found.";
                    return View();
                }
                return View(productsDto);
            }

            [Authorize(Roles = "administrator")]
            public async Task<IActionResult> DeleteById(int id)
            {
                await _productService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
        }
    }

