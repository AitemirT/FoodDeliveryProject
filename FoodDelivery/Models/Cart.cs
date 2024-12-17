namespace FoodDelivery.Models;

public class Cart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public MyUser? User { get; set; }

    public int RestaurantId { get; set; }

    public Restaurant? Restaurant { get; set; }

    public List<CartDish> CartDishes { get; set; }

    public Cart()
    {
        CartDishes = new List<CartDish>();
    }
}