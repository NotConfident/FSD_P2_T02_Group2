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

        public async Task CreateCounsellingSession(IFormCollection form)
        {
            string userID = form["userID"].ToString();
            string counsellorID = HttpContext.Session.GetInt32("CounsellorID").ToString();
            string session = counsellorID + "-" + userID;

            var projectName = "fir-chat-ukiyo";
            var authFilePath = "/Users/tee/Downloads/fir-chat-ukiyo-firebase-adminsdk.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", authFilePath);
            FirestoreDb firestoreDb = FirestoreDb.Create(projectName);
            FirestoreDb db = FirestoreDb.Create(projectName);

            DocumentReference docRef = db.Collection("CounsellingChat").Document(session);

            Dictionary<string, object> start = new Dictionary<string, object>
            {
                { "Date", DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt")},
            };

            await docRef.SetAsync(start);

            //return View();
        }
    }
}