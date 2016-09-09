using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LPush.Web.Admin.Models;
namespace LPush.Web.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (GlobalVariables.CurrentUser == null)
            {
                Response.Redirect("/Account/Login");
            }

            return View();
        } 
    }
}