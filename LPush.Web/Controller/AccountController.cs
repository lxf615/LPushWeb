using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Security.Claims;

using Nancy;
using Nancy.Extensions;
using Nancy.Security;
using Nancy.ModelBinding;
using Nancy.Authentication.Forms;

using LPush.Core.Caching;
using LPush.Web.Models;
using LPush.Web.Framework;
using LPush.Service.Basic;

namespace LPush.Web.Controller
{
    public class AccountController : BaseModule
    {
        private ICacheManager cacheManager = null;

        public AccountController(ICacheManager cacheManager, IUserService client) : base("Account")
        {
            Get["/Login"] = paramters =>
            {
                this.CreateNewCsrfToken();

                ViewBag.Title = "登录";

                dynamic model = new ExpandoObject();
                model.Errored = this.Request.Query.error.HasValue;

                return View["Login", model];
            };

            Post["/Login"] = _ =>
            {

                this.ValidateCsrfToken();
                string userName = (string)this.Request.Form.Username;
                string password = (string)this.Request.Form.Password;


                var userGuid = UserMapper.ValidateUser(this.Context, userName, password);

                if (userGuid == null)
                {
                    return this.Context.GetRedirect("~/account/login?error=true");
                }

                DateTime? expiry = null;
                if (this.Request.Form.RememberMe.HasValue)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };

            Get["/Logout"] = x =>
            {
                Session.DeleteAll();
                return this.LogoutAndRedirect("~/");
            };
        }
    }
}