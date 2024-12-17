using FoodDelivery.Models;
using FoodDelivery.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Controllers;

[Authorize(Roles = "admin")]
public class UserController : Controller
{
    private readonly RestaurantSystemBdContext _context;
    private readonly UserManager<MyUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public UserController(RestaurantSystemBdContext context, UserManager<MyUser> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync(); 
        return View(users); 
    }

    [HttpGet]
    public async Task<IActionResult> Profile(int? id)
    {
        MyUser? user = await _userManager.GetUserAsync(User);

        if (id != null && await _userManager.Users.AnyAsync(u => u.Id == id))
        {
            user = await _userManager.FindByIdAsync(id.ToString());
        }

        return View(user);
    }
    
    [Authorize(Roles = "admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            MyUser user = new MyUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }

                return RedirectToAction("Index", "User");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeRights(int id)
    {
        MyUser? user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return NotFound();
        }

        if (await _userManager.IsInRoleAsync(user, "user"))
        {
            await _userManager.RemoveFromRoleAsync(user, "user");
            await _userManager.AddToRoleAsync(user, "admin");
        }
        else
        {
            await _userManager.RemoveFromRoleAsync(user, "admin");
            await _userManager.AddToRoleAsync(user, "user");
        }

        await _userManager.UpdateAsync(user);
        return RedirectToAction("Index", "User");
    }
}