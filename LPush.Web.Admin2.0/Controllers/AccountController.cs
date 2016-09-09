using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;

using LPush.Service.Basic;
using LPush.Web.Admin.Models;
using LPush.Core.Data;
using LPush.Model;
namespace LPush.Web.Admin.Controllers
{
    public class AccountController : Controller
    {
        ILoginService loginService;
        public AccountController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            DataResult<Login> result = await loginService.ValidateLogin(model.LoginName, model.Password);
            if (result.IsSuccess)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}