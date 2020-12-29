using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using FSD_P2_T02_Group2.Models;
using Microsoft.AspNetCore.Http;
using Google.Cloud.Firestore;
using FSD_P2_T02_Group2.DAL;

namespace FSD_P2_T02_Group2.Controllers
{
    public class UserController : Controller
    {
        private UserDAL userDAL = new UserDAL();

        public IActionResult UserMain()
        {
            return View();
        }

        public IActionResult ChatRooms()
        {
            return View();
        }

        public ActionResult BadmintonChatRoom()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BadmintonChatRoom(ChatMessage messageVar)
        {
            User user = new User();
            user.Alias = HttpContext.Session.GetString("Alias");
            userDAL.sendMessage(user, messageVar);
            ModelState.Clear(); // Clears textbox
            return View();
        }

    }
}
