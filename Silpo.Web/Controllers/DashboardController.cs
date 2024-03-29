﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Silpo.Core.DTO_s.User;
using Silpo.Core.Services;
using Silpo.Core.Validation;

namespace Silpo.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserService _userService;

        public DashboardController(UserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {

            return View();
        }

        [AllowAnonymous] // Method GET
        public async Task<IActionResult> SignIn()
        {
            return View();
        }


        [AllowAnonymous] // Method POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginUserDto model)
        {
            var validator = new LoginUserValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                ServiceResponse result = await _userService.LoginUserAsync(model);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.AuthError = result.Message;
                return View(model);
            }
            ViewBag.AuthError = validationResult.Errors[0];
            return View(model);
        }
    }
}
