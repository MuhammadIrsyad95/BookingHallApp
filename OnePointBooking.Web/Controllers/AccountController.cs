using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnePointBooking.Application.Common.Interfaces;
using OnePointBooking.Application.Common.Utility;
using OnePointBooking.Application.Services.Interface;
using OnePointBooking.Domain.Entities;
using OnePointBooking.Web.ViewModels;

namespace OnePointBooking.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICompanyService _companyService; // Ganti IRoomService dengan ICompanyService


        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ICompanyService companyService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _companyService = companyService;
        }
        public IActionResult Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            LoginVM loginVM = new()
            {
                RedirectUrl = returnUrl,
            };

            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Register(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var companies = _companyService.GetAllCompanies(); // Mengambil data perusahaan dari databas
            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }),
                CompanyList = companies.Select(c => new SelectListItem // Tambahkan CompanyList
                {
                    Text = c.CompanyName,
                    Value = c.Id.ToString()
                }),
                RedirectUrl = returnUrl
            };

            return View(registerVM);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    Name = registerVM.Name,
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    EmailConfirmed = true,
                    UserName = registerVM.Email,
                    CreatedAt = DateTime.Now,
                    CompanyId = registerVM.SelectedCompanyId // Set the selected CompanyId
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(registerVM.Role))
                    {
                        await _userManager.AddToRoleAsync(user, registerVM.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    }

                    // Tampilkan pemberitahuan jika admin
                    if (registerVM.Role == SD.Role_Admin)
                    {
                        TempData["Success"] = "Registration successful! You can now log in as an admin.";
                        return RedirectToAction("Register");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(registerVM.RedirectUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // Re-populate the RoleList and CompanyList if the model is invalid
            registerVM.RoleList = _roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            });
            registerVM.CompanyList = _companyService.GetAllCompanies().Select(c => new SelectListItem
            {
                Text = c.CompanyName,
                Value = c.Id.ToString()
            });

            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(loginVM.Email,
                    loginVM.Password,
                    loginVM.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(loginVM.Email);
                    if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                    {
                        return RedirectToAction("Index", "Booking");//ganti Room dengan Dashboard kalau sudah ada
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                        {
                            return RedirectToAction("Index", "Booking");//ganti Room dengan Dashboard kalau sudah ada
                        }
                        else
                        {
                            return LocalRedirect(loginVM.RedirectUrl);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }

            }
            return View(loginVM);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(forgotPasswordVM.Username);
                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return View();
                }

                // Arahkan langsung ke halaman ResetPassword tanpa token
                return RedirectToAction("ResetPassword", new { username = user.UserName });
            }
            return View(forgotPasswordVM);
        }

        public IActionResult ResetPassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            return View(new ResetPasswordVM { Username = username });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(resetPasswordVM.Username);
                if (user == null)
                {
                    TempData["Error"] = "Invalid reset attempt.";
                    return RedirectToAction("ResetPassword");
                }

                // Reset password langsung tanpa token
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPasswordVM.Password);

                if (result.Succeeded)
                {
                    TempData["Success"] = "Your password has been reset successfully.";
                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(resetPasswordVM);
        }

    }
}
