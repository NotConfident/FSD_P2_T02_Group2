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
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
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

        public ActionResult ViewFormDetails(int sessionID)
        {
            List<PendingCounsellorSession> session = new List<PendingCounsellorSession>();
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
                    break;
                }
            }
            return View(session);
        }

        public async Task<ActionResult> CreateCounsellingSession(IFormCollection form)
        {
            string userID = form["userID"].ToString();
            int sessionID = Convert.ToInt32(form["SessionID"]);
            int cID = HttpContext.Session.GetInt32("CounsellorID").Value;
            string counsellorID = HttpContext.Session.GetInt32("CounsellorID").ToString();
            string session = counsellorID + "-" + userID;
            HttpContext.Session.SetString("roomID", session);
            var projectName = "fir-chat-ukiyo";
            var authFilePath = "/Users/jaxch/Downloads/fir-chat-ukiyo-firebase-adminsdk.json";
            //var authFilePath = "/Users/tee/Downloads/fir-chat-ukiyo-firebase-adminsdk.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", authFilePath);
            FirestoreDb db = FirestoreDb.Create(projectName);

            DocumentReference docRef = db.Collection("CounsellingChat").Document(session);

            Dictionary<string, object> start = new Dictionary<string, object>
            {
                { "Date", Timestamp.GetCurrentTimestamp()},
                {"Status","Online" },
                {"UserID", userID }
            };
            counsellorDAL.confirmSes(sessionID,cID);
            await docRef.SetAsync(start);
            return RedirectToAction("CounsellorChat");
            //return View();
        }
        public ActionResult CounsellorChat()
        {
            if (HttpContext.Session.GetString("roomID") != null)
            {

                string roomNo = HttpContext.Session.GetString("roomID");
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

        public ActionResult EndChat()
        {
            string room = HttpContext.Session.GetString("roomID");
            counsellorDAL.endChat(room);
            return RedirectToAction("PendingCounsellorSessions");
        }

        public ActionResult CAccount()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                    (HttpContext.Session.GetString("Role") != "Counsellor"))
            {
                return RedirectToAction("Index", "Home");
            }
            Counsellor counsellor = counsellorDAL.GetCounsellor((int)HttpContext.Session.GetInt32("CounsellorID"));
            if (counsellor == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(counsellor);
        }
    }
}