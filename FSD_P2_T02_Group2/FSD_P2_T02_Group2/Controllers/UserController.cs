using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSD_P2_T02_Group2.Controllers
{
    public class UserController : Controller
    {
        public IActionResult UserMain()
        {
            return View();
        }

        public IActionResult ChatRoom()
        {
            return View();
        }

    }
}
