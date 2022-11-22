using Microsoft.AspNetCore.Mvc;
using Sistema_De_Gestion.Models;
using System.Diagnostics;
//agregamos el using para las cookies
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace Sistema_De_Gestion.Controllers
{
    //de esta forma limitamos el acceso solo si tiene la authentication con las cookies
    [Authorize]
    public class HomeController : Controller
    {
        private readonly EstudioJuridicoContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(EstudioJuridicoContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize(Roles = "Master, Admin, Lectura")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Master, Admin, Lectura")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Master")]
        public IActionResult Usuarios()
        {
            return RedirectToAction("Index", "Usuarios");
        }

        [Authorize(Roles = "Master")]
        public async Task<IActionResult> Logs()
        {
            var logsContext = _context.TableLogs;
            return View(await logsContext.ToListAsync());
            //return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}