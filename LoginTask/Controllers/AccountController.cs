using LoginTask.Models;
using LoginTask.ViewModels.UserVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LoginTask.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var existUser = await _userManager.FindByNameAsync(vm.UserName);
            if (existUser is { })
            {
                ModelState.AddModelError("UserName", " Username is already taken.");
                return View(vm);
            }
            var existUserEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (existUser is { })
            {
                ModelState.AddModelError("Email", "Email is already taken.");
                return View(vm);
            }

            AppUser appUser = new()
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                UserName = vm.UserName
            };

            var result = await _userManager.CreateAsync(appUser, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }

            return Ok("User Created");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var existUserEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (existUserEmail is not { })
            {
                ModelState.AddModelError("Email", "Email or Password is wrong. ");
                return View(vm);
            }
            var existUserPassword = await _userManager.CheckPasswordAsync(existUserEmail, vm.Password);
            if (existUserPassword is false)
            {
                ModelState.AddModelError("Password", "Email or Password is wrong. ");
                return View(vm);
            }

            await _signInManager.SignInAsync(existUserEmail, vm.RememberMe);


            return Ok("Logged In ");
         
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home");
        }
    }
}
