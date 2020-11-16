using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using FSD_P2_T02_Group2.Models;
using FSD_P2_T02_Group2.DAL;
using FSD_P2_T2_Group2.Models;

namespace FSD_P2_T2_Group2.Controllers
{
    public class HomeController : Controller
    {
        public UserDAL userDAL = new UserDAL();

        public readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChatRoom()
        {
            return Redirect("http://52.86.100.250");
        }

        [HttpPost]
        public ActionResult UserLogin(IFormCollection formData)
        {

            string username = formData["txtLoginID"].ToString();
            string password = formData["txtPassword"].ToString();

            User user = userDAL.CheckLogin(username, password);

            //DateTime logintime = DateTime.Now;

            if (user != null)
            {
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Alias", user.Alias);

                string role = "User";
                HttpContext.Session.SetString("Role", role);

                //HttpContext.Session.SetString("Time", logintime.ToString());

                return RedirectToAction("ChatRoom", "Home");
            }
            else
            {
                TempData["Message"] = "Invaild Login Credentials!";
                return RedirectToAction("Index");
            }
        }

        public ActionResult UserMain()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Counsellor()
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
