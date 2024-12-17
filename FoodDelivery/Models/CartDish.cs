namespace FoodDelivery.Models;

public class CartDish
{
    public int Id { get; set; }

    public int CartId { get; set; }

    public Cart Cart { get; set; }

    public int DishId { get; set; }
    
    public Dish Dish { get; set; }
    
    public int Quantity { get; set; }
}