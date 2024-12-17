using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Models;

public class MyUser : IdentityUser<int>
{
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    public List<Cart> Carts { get; set; }

    public MyUser()
    {
        Carts = new List<Cart>();
    }
}