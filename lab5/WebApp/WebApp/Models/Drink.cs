using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinksApp.Models
{
    public class Drink
    {
        [Key]
        public int Id { get; set; }

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

        [Required(ErrorMessage = "Введите цену")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, 10000, ErrorMessage = "Цена должна быть от 1 до 10000 руб.")]
        [Display(Name = "Цена (руб.)")]
        public decimal Price { get; set; }
    }
}
