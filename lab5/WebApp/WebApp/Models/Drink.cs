using System.ComponentModel.DataAnnotations;

namespace DrinksApp.Models
{
    public class Drink
    {
        [Required(ErrorMessage = "Выберите тип напитка")]
        [Display(Name = "Drink Type")]
        public string DrinkType { get; set; }

        [Required(ErrorMessage = "Введите количество молока")]
        [Range(0, 10, ErrorMessage = "Количество молока должно быть от 0 до 10")]
        [Display(Name = "Milk")]
        public int Milk { get; set; }

        [Required(ErrorMessage = "Введите количество сахара")]
        [Range(0, 10, ErrorMessage = "Количество сахара должно быть от 0 до 10")]
        [Display(Name = "Sugar")]
        public int Sugar { get; set; }


        [Display(Name = "Цена (руб.)")]
        public decimal Price { get; set; }  // Добавили цену
    }
}
