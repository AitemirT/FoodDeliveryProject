using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models;

public class Dish
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Название блюда не может быть пустым")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Цена блюда не может быть пустым")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Описание блюда не может быть пустым")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Принадлежность к ресторану не может быть пустым")]
    public int RestaurantId { get; set; }

    public Restaurant? Restaurant { get; set; }
}