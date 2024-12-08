using Microsoft.AspNetCore.Mvc;
using ProjektPlaner.Models;
using System.Diagnostics;

namespace ProjektPlaner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Pobierz aktualn� dat�
            var today = DateTime.Today;

            // Liczba dni w bie��cym miesi�cu
            int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            // Tworzenie modelu zawieraj�cego wszystkie dni miesi�ca
            var calendarDays = Enumerable.Range(1, daysInMonth)
                                         .Select(day => new DateTime(today.Year, today.Month, day))
                                         .ToList();

            return View(calendarDays);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
