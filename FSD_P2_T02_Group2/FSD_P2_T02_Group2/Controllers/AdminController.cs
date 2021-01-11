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

        // GET: /<controller>/
        public IActionResult Index()
        {
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
