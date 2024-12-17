using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.Models;
using FoodDelivery.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Controllers
{
    [Authorize]
    public class RestaurantController : Controller
    {
        private readonly RestaurantSystemBdContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<MyUser> _userManager;

        public RestaurantController(RestaurantSystemBdContext context, IWebHostEnvironment environment, UserManager<MyUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Restaurants.ToListAsync());
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .Include(d => d.Dishes)
                .Include(r => r.Carts)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (restaurant == null)
            {
                return NotFound();
            }

            MyUser user = await _userManager.GetUserAsync(User);
            
            Cart? cart = await _context.Carts.Include(c => c.CartDishes).ThenInclude(ct => ct.Dish).FirstOrDefaultAsync(c => c.UserId == user.Id && c.RestaurantId == restaurant.Id);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = user.Id,
                    RestaurantId = restaurant.Id
                };

                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }
            
            return View(new RestaurantDetailsViewModel
            {
                Restaurant = restaurant,
                Cart = cart
            });
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRestaurantViewModel model)
        {
            if (ModelState.IsValid)
            {
                string fileName = $"photOf_{model.RestaurantName}{Path.GetExtension(model.Photo.FileName)}";
            
                if (model.Photo != null && model.Photo.Length > 0 && model.Photo.ContentType.StartsWith("image/"))
                {
                    string filePath = Path.Combine(_environment.WebRootPath, "pictures", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(stream);
                    }
                }
                else
                {
                    ModelState.AddModelError("Photo", "Phone может быть только картинкой");
                    return View(model);
                }

                Restaurant restaurant = new Restaurant()
                {
                    Name = model.RestaurantName,
                    Description = model.Description,
                    PathToPhoto = $"/pictures/{fileName}"
                };

                await _context.AddAsync(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Restaurant");
            }
            return View(model);
            
            
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
            }
            else
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
