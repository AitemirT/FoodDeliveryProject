using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.ViewModels;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Имя пользователя не может быть пустым")]
    [Remote(action: "CheckUserName", controller: "Validation", ErrorMessage = "Пользователь с таким именем пользователя уже существует")]

    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Почта пользователя не может быть пустой")]
    [EmailAddress(ErrorMessage = "Почта в некорректном формате")]
    [Remote(action: "CheckUserEmail", controller: "Validation", ErrorMessage = "Пользователь с такой почтой уже существует", AdditionalFields = "Id")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Номер телефона пользователя не может быть пустой")]
    public string PhoneNumber { get; set; }
    
    [Required(ErrorMessage = "Пароль не может быть пустым")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Роль пользователя не может быть пустой")]
    public string Role { get; set; }  
}