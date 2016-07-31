using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using Nancy;
using Nancy.Security;
using Nancy.Authentication.Forms;

using LPush.Web.Admin.Models;
using LPush.Web.Framework;


namespace LPush.Web.Admin.Models
{
    [Serializable]
    public class UserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserMapper : IUserMapper
    {
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            UserInfo userRecord = context.Request.Session[identifier.ToString()] as UserInfo;

            return userRecord == null ? null
                       : new UserIdentity(userRecord.UserName);
        }
    }
}