using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using FSD_P2_T02_Group2.Models;
using FSD_P2_T02_Group2.DAL;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace FSD_P2_T02_Group2.Controllers
{
    public class HomeController : Controller
    {

        public UserDAL userDAL = new UserDAL();
        public AdminDAL adminDAL = new AdminDAL();

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
            //return Redirect("http://54.147.90.7");
            return RedirectToAction("UserMain", "User");
            //return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(IFormCollection formData)
        {

            string username = formData["txtLoginID"].ToString();
            string password = formData["txtPassword"].ToString();

            User user = userDAL.CheckLogin(username, password);
            //DateTime logintime = DateTime.Now;

            if (user.Username != null)
            {
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Alias", user.Alias);
                
                string role = "User";
                HttpContext.Session.SetString("Role", role);
                Set("Username", user.Alias, 60);
                
                //var resp = new HttpResponseMessage();

                //var cookie = new System.Net.Http.Headers.CookieHeaderValue("Username", user.Alias);

                //cookie.Expires = DateTimeOffset.Now.AddDays(1);
                //cookie.Path = "/";
                //resp.Headers.AddCookies(new System.Net.Http.Headers.CookieHeaderValue[] { cookie })

                return RedirectToAction("UserMain", "User");
            }
            else if(username == "Admin" && password == "admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                TempData["Message"] = "Invaild Login Credentials!";
                return RedirectToAction("Login");
            }
        }

        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions { IsEssential = true };

            if (expireTime.HasValue)
            {
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            }
            else
            {
                option.Expires = DateTime.Now.AddMilliseconds(10);
            }

            Response.Cookies.Append(key, value, option);
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

        public IActionResult Register()
        {
            return View();
        }

        //static async Task SignUpUser(User user)
        //{
        //    AmazonCognitoIdentityProviderClient provider =
        //        new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Region);

        //    SignUpRequest signUpRequest = new SignUpRequest()
        //    {
        //        ClientId = appClientID,
        //        Username = user.Username,
        //        Password = user.Password
        //    };
        //    List<AttributeType> attributes = new List<AttributeType>()
        //    {
        //        new AttributeType(){Name = "email", Value = user.Email},
        //        new AttributeType(){Name = "phone_number", Value = user.PhoneNo}
        //    };

        //    signUpRequest.UserAttributes = attributes;

        //    SignUpResponse result = await provider.SignUpAsync(signUpRequest);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                userDAL.RegisterUser(user);
                return RedirectToAction("UserMain");
            }
            else
            {
                return View(user);
            }
        }

        public IActionResult Counsellor()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult FAQ()
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
