using Microsoft.AspNetCore.Mvc;
using ProjektPlaner.Data;
using ProjektPlaner.Models;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjektPlaner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Pobieramy bie¿¹cy miesi¹c
            var today = DateTime.Today;
            int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Jeœli u¿ytkownik nie jest zalogowany
            if (string.IsNullOrEmpty(currentUserId))
            {
                // Pobieramy ID grupy "wszyscy"
                var everyoneGroupId = await _context.CalendarGroup
                    .Where(g => g.Name == "wszyscy")
                    .Select(g => g.Id)
                    .FirstOrDefaultAsync();

                // Pobieramy tylko wydarzenia z grupy "wszyscy"
                var calendarElements = await _context.CalendarElement
                    .Where(e => e.GroupId == everyoneGroupId)
                    .ToListAsync();

                // Tworzymy kalendarz dla bie¿¹cego miesi¹ca
                var calendarDays = Enumerable.Range(1, daysInMonth)
                    .Select(day =>
                    {
                        var dayDate = new DateTime(today.Year, today.Month, day);
                        var eventsForDay = calendarElements
                            .Where(e => e.StartDate.Date == dayDate)
                            .Select(e => e.Name)
                            .ToList();

                        return new CalendarDayViewModel
                        {
                            Date = dayDate,
                            EventNames = eventsForDay
                        };
                    })
                    .ToList();


                return View(calendarDays);
            }

            // Obs³uga zalogowanego u¿ytkownika
            var userGroupIds = await _context.CalendarGroup
                .Where(g => g.FounderId == currentUserId
                         || g.Users.Any(u => u.Id == currentUserId)
                         || g.Administrators.Any(a => a.Id == currentUserId))
                .Select(g => g.Id)
                .ToListAsync();

            // Pobieranie wydarzeñ dla u¿ytkownika i jego grup
            var calendarElementsForUser = await _context.CalendarElement
                .Include(e => e.Group)
                .Include(e => e.User)
                .Where(e => e.UserId == currentUserId
                         || (e.GroupId.HasValue && userGroupIds.Contains(e.GroupId.Value)))
                .ToListAsync();

            // Tworzenie kalendarza z dniami miesi¹ca
            // Pobraæ wszystkie wydarzenia na dany dzieñ
            var calendarDaysWithEvents = Enumerable.Range(1, daysInMonth)
                .Select(day =>
                {
                    var dayDate = new DateTime(today.Year, today.Month, day);
                    var eventsForDay = calendarElementsForUser
                        .Where(e => e.StartDate.Date == dayDate)
                        .Select(e => e.Name)
                        .ToList();

                    return new CalendarDayViewModel
                    {
                        Date = dayDate,
                        EventNames = eventsForDay
                    };
                })
                .ToList();



            return View(calendarDaysWithEvents);
        }


        public async Task<IActionResult> Calendar()
        {
            // Pobieranie ID aktualnego u¿ytkownika
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentUserId))
            {
                // Jeœli u¿ytkownik nie jest zalogowany, zwróæ pust¹ listê
                return View(new List<CalendarElement>());
            }

            // Pobierz ID grup, do których nale¿y u¿ytkownik
            var userGroupIds = await _context.CalendarGroup
                .Where(g => g.FounderId == currentUserId
                         || g.Users.Any(u => u.Id == currentUserId)
                         || g.Administrators.Any(a => a.Id == currentUserId))
                .Select(g => g.Id)
                .ToListAsync();

            // Pobierz elementy kalendarza powi¹zane z u¿ytkownikiem lub grupami
            var calendarElements = await _context.CalendarElement
                .Include(e => e.Group)
                .Include(e => e.User)
                .Where(e => e.UserId == currentUserId
                         || (e.GroupId.HasValue && userGroupIds.Contains(e.GroupId.Value)))
                .ToListAsync();

            // Pobierz bie¿¹cy miesi¹c
            var today = DateTime.Today;
            int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            // Przygotuj listê dni miesi¹ca
            var calendarDays = Enumerable.Range(1, daysInMonth)
                                         .Select(day => new DateTime(today.Year, today.Month, day))
                                         .ToList();

            // Przekazanie danych do ViewData
            ViewData["CalendarDays"] = calendarDays;
            ViewData["CalendarElements"] = calendarElements;

            return View();
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
