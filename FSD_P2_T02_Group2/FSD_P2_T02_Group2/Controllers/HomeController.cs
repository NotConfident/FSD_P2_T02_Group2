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
using FSD_P2_T2_Group2.Models;
//using Amazon.CognitoIdentityProvider;
//using Amazon.CognitoIdentityProvider.Model;

namespace FSD_P2_T2_Group2.Controllers
{
    public class HomeController : Controller
    {
        //string poolID = "us-east-1_Vtm1usZsi";
        //string appClientID = "6np915u728h5botst3fd7jsd21";
        //static Amazon.RegionEndpoint Region = Amazon.RegionEndpoint.USEast1;

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
            return Redirect("http://54.147.90.7");
            //return Redirect("https://localhost:5001/");
            //return RedirectToAction("Index", "Home");
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
                
                //localStorage.SetItem("Alias", user.Alias);
                string role = "User";
                HttpContext.Session.SetString("Role", role);

                Set("Username", user.Alias, 60);

                return RedirectToAction("ChatRoom", "Home");
            }
            else
            {
                TempData["Message"] = "Invaild Login Credentials!";
                return RedirectToAction("Index");
            }
        }

        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
