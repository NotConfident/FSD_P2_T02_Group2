using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FSD_P2_T2_Group2.Models;
using Microsoft.AspNetCore.Http;
using FSD_P2_T02_Group2.Models;

namespace FSD_P2_T2_Group2.Controllers
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
            return View();
        }

        public IActionResult ChatRoom()
        {
            return Redirect("https://www.google.com");
        }

        [HttpPost]
        public ActionResult UserLogin(IFormCollection formData)
        {
            // Read inputs from textboxes
            // Email address converted to lowercase 
            string username = formData["txtLoginID"].ToString().ToLower();
            string password = formData["txtPassword"].ToString();

            // challenge create variable
            DateTime logintime = DateTime.Now;

            if (username == "username" && password == "password")
            {
                // store username in session with the key "LoginID"
                HttpContext.Session.SetString("LoginID", username);
                // Store user role "User" as a string in session with the key "Role"
                string role = "User";
                HttpContext.Session.SetString("Role", role);

                //store timing as a string with the key "Time"
                HttpContext.Session.SetString("Time", logintime.ToString());

                // Redirect user to the "UserMain" view through an action 
                return RedirectToAction("ChatRoom", "Home");
            }
            else
            {
                // Store an error message in TempData for display at the index view
                TempData["Message"] = "Invaild Login Credentials!";
                // Redirect user back to the index view through an action 
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

        public IActionResult LogIn()
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
