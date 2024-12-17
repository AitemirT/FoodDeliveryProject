namespace FoodDelivery.Models;

public class Restaurant
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public string PathToPhoto { get; set; }

    public string Description { get; set; }

    public List<Dish> Dishes { get; set; }

    public List<Cart> Carts { get; set; }

    public Restaurant()
    {
        Dishes = new List<Dish>();
        Carts = new List<Cart>();
    }
}