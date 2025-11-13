using DrinksApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace DrinksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: список напитков
        public async Task<IActionResult> Index()
        {
            var drinks = await _context.Drinks.ToListAsync();
            return View(drinks);
        }

        // Добавление напитка
        [HttpPost]
        public async Task<IActionResult> AddDrink([FromBody] Drink drink)
        {
            if (ModelState.IsValid)
            {
                _context.Drinks.Add(drink);
                await _context.SaveChangesAsync();
                var drinksList = await _context.Drinks.ToListAsync();
                return Json(drinksList);
            }
            return BadRequest();
        }

        // Удаление напитка
        [HttpPost]
        public async Task<IActionResult> DeleteDrink(int id)
        {
            var drink = await _context.Drinks.FindAsync(id);
            if (drink != null)
            {
                _context.Drinks.Remove(drink);
                await _context.SaveChangesAsync();
            }
            var drinksList = await _context.Drinks.ToListAsync();
            return Json(drinksList);
        }

        // Редактирование напитка
        [HttpPost]
        public async Task<IActionResult> EditDrink(int id, [FromBody] Drink drink)
        {
            if (id != drink.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(drink);
                await _context.SaveChangesAsync();
            }
            var drinksList = await _context.Drinks.ToListAsync();
            return Json(drinksList);
        }

        // Поиск напитков
        [HttpGet]
        public JsonResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                var all = _context.Drinks
                    .Select(d => new { d.Id, d.DrinkType, d.Milk, d.Sugar, d.Price })
                    .ToList();
                return Json(all);
            }

            var results = _context.Drinks
                .Where(d => d.DrinkType.Contains(query)
                         || d.Sugar.ToString().Contains(query)
                         || d.Milk.ToString().Contains(query)
                         || d.Price.ToString().Contains(query))
                .Select(d => new { d.Id, d.DrinkType, d.Milk, d.Sugar, d.Price })
                .ToList();

            return Json(results);
        }
    }
}
