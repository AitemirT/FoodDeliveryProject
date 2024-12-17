using System.Security.AccessControl;
using FoodDelivery.Models;
using FoodDelivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly RestaurantSystemBdContext _context;
    private readonly UserManager<MyUser> _userManager;
    private readonly EmailService _emailService;

    public OrderController(RestaurantSystemBdContext context, UserManager<MyUser> userManager, EmailService emailService)
    {
        _context = context;
        _userManager = userManager;
        _emailService = emailService;
    }
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Orders.Include(o => o.User).Include(o => o.Restaurant).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(int cartId, string customerName, string address, string phoneNumber, string email)
    {
        MyUser user = await _userManager.GetUserAsync(User);

        Cart? cart = await _context.Carts
            .Include(c => c.CartDishes)
            .ThenInclude(c => c.Dish)
            .FirstOrDefaultAsync(c => c.Id == cartId);
        
        if (cart == null || !cart.CartDishes.Any())
        {
            return NotFound();
        }

        Order order = new Order
        {
            UserId = user.Id,
            RestaurantId = cart.RestaurantId,
            TotalPrice = (double)cart.CartDishes.Sum(c => c.Dish.Price * c.Quantity),
            CustomerName = customerName,
            PhoneNumber = phoneNumber,
            Address = address,
            Email = email,
            OrderDate = DateTime.UtcNow,
            Dishes = cart.CartDishes.Select(ct => ct.Dish).ToList()
        };

        await _context.Orders.AddAsync(order);
        _context.CartDishes.RemoveRange(cart.CartDishes);
        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();

        string subject = $"Заказ #{order.Id}";
        string emailBody = $"Детали:\n" +
                           $"Получатель: {customerName}\n" +
                           $"Адрес доставки: {address}\n" +
                           $"Почта получателя: {email}\n" +
                           $"Номер телефона получателя: {phoneNumber}\n" +
                           $"Блюда: \n" +
                           $"{string.Join(", ", order.Dishes.Select(d => d.Name).ToList())}";

        await _emailService.SendEmailAsync(email, subject, emailBody);
        
        return RedirectToAction("Details", "Restaurant", new {id = cart.RestaurantId});
    }
}