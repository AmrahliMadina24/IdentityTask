using System.Diagnostics;
using LoginTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginTask.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

   
    }
}
