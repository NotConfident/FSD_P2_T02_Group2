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
using Firebase.Storage;
using FSD_P2_T02_Group2.DAL;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public ActionResult General()
        {
            HttpContext.Session.SetString("room", "General");
            return RedirectToAction("ChatRoom");
        }
        public ActionResult Sports()
        {
            HttpContext.Session.SetString("room", "Sports");
            return RedirectToAction("ChatRoom");
        }
        public ActionResult Football()
        {
            HttpContext.Session.SetString("room", "Football");
            return RedirectToAction("ChatRoom");
        }
        public ActionResult Badminton()
        {
            HttpContext.Session.SetString("room", "Badminton");
            return RedirectToAction("ChatRoom");
        }
        public ActionResult Basketball()
        {
            HttpContext.Session.SetString("room", "Basketball");
            return RedirectToAction("ChatRoom");
        }
        public ActionResult ChatRoom()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChatRoom(ChatMessage messageVar)
        {
            User user = new User();
            user.Alias = HttpContext.Session.GetString("Alias");
            string room = HttpContext.Session.GetString("room");
            userDAL.sendMessage(user, messageVar, room);
            ModelState.Clear(); // Clears textbox
            return View();
        }
        public ActionResult Counselling()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
           (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            CounselReq counselS = new CounselReq();
            counselS.Queue = userDAL.getSessions();
            return View(counselS);
        }

        [HttpPost]
        public ActionResult Counselling(CounselReq counsel)
        {
            int id = (int)HttpContext.Session.GetInt32("UserID");
            bool inQueue = userDAL.checkReq(id);
            if (inQueue == false)
            {
                userDAL.reqHelp(id, counsel);
                
                TempData["CounselMsg"] = "Thank you for reaching out";
            }
            else
            {
                TempData["CounselMsg"] = "You are already in queue, please wait";
            }
            ModelState.Clear(); // Clears textbox
            CounselReq counselS = new CounselReq();
            counselS.Queue = userDAL.getSessions();
            return View(counselS) ;
        }
        public async Task<ActionResult> CounselChatAsync(String id)
        {
            if (id != "")
            {
                HttpContext.Session.SetString("roomID", id);
                return View();
            }
            else
                return RedirectToAction("Counselling");
        }
        [HttpPost]
        public ActionResult CounselChat(ChatMessage messageVar)
        {
            string Alias = HttpContext.Session.GetString("Alias");
            string room = HttpContext.Session.GetString("roomID");
            Console.WriteLine(room);
            userDAL.sendCMessage(Alias, messageVar, room);
            ModelState.Clear(); // Clears textbox
            return View();
        }
        public ActionResult Account()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                    (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            User user = userDAL.GetUser((int)HttpContext.Session.GetInt32("UserID"));
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        public ActionResult ViewAccDetails()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                    (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            User user = userDAL.GetUser((int)HttpContext.Session.GetInt32("UserID"));
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        public ActionResult EditAccount()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                    (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            User user = userDAL.GetUser((int)HttpContext.Session.GetInt32("UserID"));
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount(User user)
        {
            if (ModelState.IsValid)
            {
                user.Image = Request.Form["Base64Image"];
                int id = userDAL.UpdateUser(user);
                if (id != 0)
                {
                    return RedirectToAction("ViewAccDetails");
                }
                else
                {
                    TempData["Error"] = "Information not changed!";
                    return View(user);
                }
            }
            else
            {
                TempData["Error"] = "Information not changed!";
                return View(user);
            }
        }
        private List<SelectListItem> GetPostCategories()
        {
            List<SelectListItem> categoryList = new List<SelectListItem>();
            categoryList.Add(new SelectListItem
            {
                Value = "All",
                Text = "All"
            });
            categoryList.Add(new SelectListItem
            {
                Value = "Information Technology",
                Text = "Information Technology"
            });
            categoryList.Add(new SelectListItem
            {
                Value = "Art",
                Text = "Art"
            });
            categoryList.Add(new SelectListItem
            {
                Value = "Engineering",
                Text = "Engineering"
            });
            return categoryList;
        }

        public async Task<ActionResult> TalentsAsync()
        {
            //Check if role is user
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            //Check user's details
            User user = userDAL.GetUser((int)HttpContext.Session.GetInt32("UserID"));
            if (user == null)
            {
                return RedirectToAction("Index", "Home");   //if there is no current user, redirect back home
            }

            ViewData["PostCategories"] = GetPostCategories();
            ViewData["Users"] = userDAL.GetUsers();

            List<PostViewModel> postVMList = await userDAL.RetrievePostsAsync("All");
            TempData.Put("Posts", postVMList);

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> TalentsAsync(PostViewModel newPost)
        {
            ViewData["PostCategories"] = GetPostCategories();
            ViewData["Users"] = userDAL.GetUsers();

            string Category = "All";
            if (newPost.Category != null && newPost.Category != "")
            {
                Category = newPost.Category;
            }
            List<PostViewModel> postVMList = await userDAL.RetrievePostsAsync(Category);
            TempData.Put("Posts", postVMList);
            string media = Request.Form["uploadImg"];
            if (media != "")
            {
                newPost.post.hasMedia = true;
                newPost.Image = media;
            }
            else
            {
                newPost.post.hasMedia = false;
            }

            if (newPost.post.Description != null || newPost.Image != null)
            {
                newPost.post.UserID = (int)HttpContext.Session.GetInt32("UserID");
                await userDAL.CreatePostAsync(newPost.post, newPost.Image);
                //PostViewModel postVM = new PostViewModel();
                //postVM.postList = await userDAL.RetrievePostsAsync("All");
                return View(newPost);
            }
            else
            {
                //newPost.postList = await userDAL.RetrievePostsAsync("All");
                return View(newPost);
            } 
        }
 

    }
}