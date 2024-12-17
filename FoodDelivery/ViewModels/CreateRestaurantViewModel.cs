using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.ViewModels;

public class CreateRestaurantViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Название заведения не может быть пустым!")]
    [Remote(action: "CheckRestaurantName", controller: "Validation", ErrorMessage = "Заведение с таким названием уже существует")]
    public string RestaurantName { get; set; }
    [Required(ErrorMessage = "Фото заведения не может быть пустым!")]
    public IFormFile Photo { get; set; }
    [Required(ErrorMessage = "Описание заведения не может быть пустым!")]
    public string Description { get; set; }
}