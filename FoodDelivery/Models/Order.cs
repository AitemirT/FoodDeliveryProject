using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;

namespace FoodDelivery.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public MyUser? User { get; set; }
    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerName { get; set; }
    public double TotalPrice { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }

    public List<Dish> Dishes { get; set; } = new List<Dish>();
}