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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FSD_P2_T02_Group2.Controllers
{
    public class AdminController : Controller
    {
        public AdminDAL adminDAL = new AdminDAL();
        public UserDAL userDAL = new UserDAL();

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewUsers()
        {

            List<User> userList = new List<User>();
            userList = userDAL.GetUsers();

            ViewBag.userList = userList;
            if (userList.Count() == 0)
            {
                ViewBag.userList = null;
            }
            return View();
        }

        public IActionResult ViewCounsellors()
        {

            List<Counsellor> counsellorList = new List<Counsellor>();
            counsellorList = adminDAL.GetCounsellors();

            ViewBag.counsellorList = counsellorList;
            if (counsellorList.Count() == 0)
            {
                ViewBag.counsellorList = null;
            }
            return View();
        }


        public IActionResult PendingCounsellor()
        {
            List<PendingCounsellor> pCounsellorList = new List<PendingCounsellor>();
            pCounsellorList = adminDAL.retrievePendingCounsellor();

            ViewBag.pCounsellorList = pCounsellorList;
            if (pCounsellorList.Count() == 0)
            {
                ViewBag.pCounsellorList = null;
            }
            return View();
        }


        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            bool success = adminDAL.DeleteUser(id);
            return RedirectToAction("ViewUsers");
        }

        [HttpGet]
        public IActionResult DeleteCounsellor(int id)
        {
            bool success = adminDAL.DeleteCounsellor(id);
            return RedirectToAction("ViewCounsellors");
        }

        [HttpGet]
        public IActionResult ApproveCounsellor(int id)
        {
            bool success = adminDAL.ApproveCounsellor(id);
            return RedirectToAction("PendingCounsellor");
        }

        [HttpGet]
        public IActionResult RejectCounsellor(int id)
        {
            bool success = adminDAL.RejectCounsellor(id);
            return RedirectToAction("PendingCounsellor");
        }

    }
}
