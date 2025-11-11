
using DrinksApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DrinksApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/DrinkForm
        [HttpGet]
        public ActionResult DrinkForm()
        {
            ViewBag.Drinks = new SelectList(new[] { "Tea", "Coffee", "Juice", "Alcohol" });
            return View();
        }

        // POST: /Home/DrinkForm
        [HttpPost]
        public ActionResult DrinkForm(Drink model)
        {
            ViewBag.Drinks = new SelectList(new[] { "Tea", "Coffee", "Juice", "Alcohol" });

            if (ModelState.IsValid)
            {
                // Определяем цену напитка
                switch (model.DrinkType)
                {
                    case "Tea":
                        model.Price = 50;
                        break;
                    case "Coffee":
                        model.Price = 80;
                        break;
                    case "Juice":
                        model.Price = 60;
                        break;
                    case "Alcohol":
                        model.Price = 150;
                        break;
                    default:
                        model.Price = 0;
                        break;
                }

                ViewBag.Message = $"Вы выбрали {model.DrinkType} с {model.Milk} мл молока и {model.Sugar} ложками сахара.";
                return View("Result", model);
            }

            return View(model);
        }
    }
}
