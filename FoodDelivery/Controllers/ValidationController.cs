using FoodDelivery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Controllers;

public class ValidationController : Controller
{
    private readonly UserManager<MyUser> _userManager;
    private readonly RestaurantSystemBdContext _context;

    public ValidationController(UserManager<MyUser> userManager, RestaurantSystemBdContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> CheckUserEmail(string email, int id)
    {
        MyUser myUser = await _userManager.FindByEmailAsync(email);

        if (myUser != null && myUser.Id != id)
        {
            return Json(false);
        }
        return Json(true);
    }
    
    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> CheckUserName(string username, int id)
    {
        MyUser myUser = await _userManager.FindByNameAsync(username);

        if (myUser != null && myUser.Id != id)
        {
            return Json(false);
        }

        return Json(true);
    }
    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> CheckRestaurantName(string restaurantName, int id)
    {
        Restaurant restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Name == restaurantName);
        if (restaurant !=  null && restaurant.Id != id)
        {
            return Json(false);
        }
        return Json(true);
    }
}