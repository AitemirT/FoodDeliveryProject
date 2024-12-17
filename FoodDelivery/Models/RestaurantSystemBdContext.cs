using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Models;

public class RestaurantSystemBdContext : IdentityDbContext<MyUser, IdentityRole<int>, int>
{
    public DbSet<Restaurant> Restaurants { get; set; }
    
    public DbSet<Dish> Dishes { get; set; }

    public DbSet<CartDish> CartDishes { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public RestaurantSystemBdContext(DbContextOptions<RestaurantSystemBdContext> options) : base(options) {}
}