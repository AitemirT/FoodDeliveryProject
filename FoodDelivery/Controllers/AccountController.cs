using FoodDelivery.Models;
using FoodDelivery.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<MyUser> _userManager;
    private readonly SignInManager<MyUser> _signInManager;
    private readonly IWebHostEnvironment _environment;

    public AccountController(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager, IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _environment = environment;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = "")
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            MyUser user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(model.UserNameOrEmail);
            }

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Пользователь не найден");
                return View(model);
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("", "");
            }

            ModelState.AddModelError(string.Empty, "Неправильные логин или пароль");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        MyUser user = new MyUser()
        {
            UserName = model.UserName.ToLower(),
            Email = model.Email,
            PhoneNumber = model.PhoneNumber
        };

        IdentityResult result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            await _userManager.AddToRoleAsync(user, "user");
            return RedirectToAction("", "");
        }

        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("", "");
    }
}