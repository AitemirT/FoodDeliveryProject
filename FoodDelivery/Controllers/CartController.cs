using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace FoodDelivery.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly UserManager<MyUser> _userManager;
    private readonly RestaurantSystemBdContext _context;

    public CartController(UserManager<MyUser> userManager, RestaurantSystemBdContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int dishId, int restaurantId, int count)
    {
        MyUser user = await _userManager.GetUserAsync(User);

        Dish? dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

        if (dish == null)
        {
            return NotFound();
        }

        Cart? cart = await _context.Carts.Include(c => c.CartDishes).ThenInclude(ct => ct.Dish).FirstOrDefaultAsync(c => c.UserId == user.Id && c.RestaurantId == restaurantId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = user.Id,
                RestaurantId = restaurantId
            };

            await _context.Carts.AddAsync(cart);
        }

        CartDish? cartDish = await _context.CartDishes.FirstOrDefaultAsync(ct => ct.CartId == cart.Id && ct.DishId == dishId);

        if (cartDish != null)
        {
            cartDish.Quantity += count;
            _context.CartDishes.Update(cartDish);
        }
        else
        {
            cartDish = new CartDish
            {
                CartId = cart.Id,
                DishId = dish.Id,
                Quantity = count
            };

            await _context.CartDishes.AddAsync(cartDish);
        }
        
        
        await _context.SaveChangesAsync();

        return PartialView("_CartPartialView", cart);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteFromCart(int cartDishId)
    {
        CartDish? cartDish = await _context.CartDishes.Include(ct => ct.Cart).FirstOrDefaultAsync(ct => ct.Id == cartDishId);

        if (cartDish == null)
        {
            return NotFound();
        }
        
        _context.CartDishes.Remove(cartDish);
        await _context.SaveChangesAsync();

        Cart? cart = await _context.Carts.Include(c => c.CartDishes).ThenInclude(ct => ct.Dish).FirstOrDefaultAsync(c => c.Id == cartDish.CartId);

        if (cart == null)
        {
            return NotFound();
        }
        
        return PartialView("_CartPartialView", cart);
    }
}