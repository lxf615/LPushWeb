using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using Nancy;
using Nancy.Security;
using Nancy.Authentication.Forms;

using LPush.Web.Models;
using LPush.Web.Framework;

namespace LPush.Web.Models
{
    /// <summary>
    /// 用于Form验证
    /// </summary>
    public class UserMapper : IUserMapper
    {
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            UserInfo userRecord = context.Request.Session[identifier.ToString()] as UserInfo;

            return userRecord == null? null
                       : new UserIdentity(userRecord.UserName);
        }

        public static Guid? ValidateUser(NancyContext context, string username, string password)
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