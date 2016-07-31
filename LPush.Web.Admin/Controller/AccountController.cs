using System;
using System.Dynamic;

using Nancy;
using Nancy.Extensions;
using Nancy.Security;
using Nancy.ModelBinding;
using Nancy.Authentication.Forms;

using LPush.Core.Caching;
using LPush.Web.Admin.Models;

namespace LPush.Web.Admin.Controller
{
    public class AccountController : AdminModule
    {
        private ICacheManager cacheManager = null;

        public AccountController(ICacheManager cacheManager) : base("Account")
        {
            Get["/Login"] = paramters =>
            {
                this.CreateNewCsrfToken();

                ViewBag.Title = "登录";
                ViewBag.ErrorMsg = this.Request.Query.error;

                return View["Login"];
            };

            Post["/Login"] = _ =>
            {
                this.ValidateCsrfToken();

                string userName = (string)this.Request.Form.UserName;
                string password = (string)this.Request.Form.Password;

                return Login(userName,password);
            };

            Get["/Logout"] = x => 
            {
                this.CreateNewCsrfToken();

                Session.DeleteAll();
                return this.LogoutAndRedirect("~/");
            };
        }

        private dynamic Login(string userName,string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return this.Context.GetRedirect(string.Format("~/account/login?error={0}", "用户名或密码不能为空!"));
            }


            var userGuid = ValidateUser(this.Context, userName, password);

            if (userGuid == null)
            {
                return this.Context.GetRedirect(string.Format("~/account/login?error={0}", "用户不存在或密码错误！"));
            }

            DateTime? expiry = null;
            if (this.Request.Form.RememberMe.HasValue)
            {
                expiry = DateTime.Now.AddDays(7);
            }

            return this.LoginAndRedirect(userGuid.Value, expiry);
        }

        private Guid? ValidateUser(NancyContext context, string username, string password)
        {
            Guid? guid = null;
            if (username == "admin" && password == DateTime.Today.ToString("yyyyMMdd"))
            {
                guid = Guid.NewGuid();
                context.Request.Session[guid.ToString()] = new UserInfo
                {
                    UserName = username,
                    Password = password
                };

                return guid;
            }

            return null;
        }
    }
}