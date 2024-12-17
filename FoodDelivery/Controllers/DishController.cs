using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Authorization;

namespace FoodDelivery.Controllers
{
    [Authorize]
    public class DishController : Controller
    {
        private readonly RestaurantSystemBdContext _context;

        public DishController(RestaurantSystemBdContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }
        
        public IActionResult Create()
        {
            ViewBag.RestaurantId = new SelectList(_context.Restaurants, "Id", "Name");
            
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dish dish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Restaurant", new{id = dish.RestaurantId});
            }
            
            ViewBag.RestaurantId = new SelectList(_context.Restaurants, "Id", "Name");
            return View(dish);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
            }

            await _context.SaveChangesAsync();
            
            string refererUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl);
            }
            
            return RedirectToAction("Index", "Restaurant");
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
    }
}
