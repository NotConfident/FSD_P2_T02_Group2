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
    public class CounsellorController : Controller
    {
        public CounsellorDAL counsellorDAL = new CounsellorDAL();
        public UserDAL userDAL = new UserDAL();

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PendingCounsellorSessions()
        {
            List<PendingCounsellorSession> pcSessionList = new List<PendingCounsellorSession>();
            pcSessionList = counsellorDAL.retrieveUserForms();

            ViewBag.pcSessionList = pcSessionList;

            if (pcSessionList.Count() == 0)
            {
                ViewBag.pcSessionList = null;
            }
            return View();
        }

        //[HttpPost]
        //public IActionResult PendingCounsellorSessions(int sessionID)
        //{
        //    List<PendingCounsellorSession> session = new List<PendingCounsellorSession>();
        //    List<PendingCounsellorSession> pcSessionList = new List<PendingCounsellorSession>();
        //    pcSessionList = counsellorDAL.retrieveUserForms();

        //    if(sessionID != null)
        //    {
        //        foreach (PendingCounsellorSession item in pcSessionList)
        //        {
        //            if (item.SessionID == sessionID)
        //            {
        //                session.Add(item);
        //                break;
        //            }
        //        }
        //    }
        //    return View(session);
        //}

        public ActionResult ViewFormDetails(int sessionID)
        {
            List<PendingCounsellorSession> session = new List<PendingCounsellorSession>();
            //PendingCounsellorSession session = null;
            List<PendingCounsellorSession> pcSessionList = new List<PendingCounsellorSession>();
            pcSessionList = counsellorDAL.retrieveUserForms();

            ViewBag.pcSessionList1 = pcSessionList;
            if (pcSessionList.Count() == 0)
            {
                ViewBag.pcSessionList = null;
            }

            foreach (PendingCounsellorSession item in pcSessionList)
            {
                if (item.SessionID == sessionID)
                {
                    session.Add(item);
                    //session = item;
                    break;
                }
            }
            return View(session);
            //return RedirectToAction("PendingCounsellorSessions");
        }
        public ActionResult CounsellorChat(int sessionID)
        {
            Console.WriteLine(sessionID);
            if (HttpContext.Session.GetInt32("CounsellorID") != null)
            {
                int cID = HttpContext.Session.GetInt32("CounsellorID").Value;

                int uID = counsellorDAL.getUserSession(sessionID);

                string roomNo = Convert.ToString(cID) + "-" + Convert.ToString(uID);
                HttpContext.Session.SetString("roomID", roomNo);
                Console.WriteLine(roomNo);
                counsellorDAL.startChat(roomNo);
                return View();
            }
            else
                return RedirectToAction("PendingCounsellorSession");
        }

        [HttpPost]
        public ActionResult CounsellorChat(ChatMessage messageVar)
        {
            string Alias = HttpContext.Session.GetString("Alias");
            string room = HttpContext.Session.GetString("roomID");
            Console.WriteLine(room);
            userDAL.sendCMessage(Alias, messageVar, room);
            ModelState.Clear(); // Clears textbox
            return View();
        }
    }
}