using System;
using System.Collections.Generic;
using Nancy.Security;


namespace LPush.Web.Framework
{
    public class UserIdentity: IUserIdentity
    {
        public UserIdentity(string userName):
            this(userName,new List<string>())
        {
        }
        public UserIdentity(string userName, IEnumerable<string> claims) {
            this.UserName = userName;
            this.Claims = claims;
        }

        public IEnumerable<string> Claims
        {
            get;private set;
        }

        public string UserName
        {
            get; private set;
        }
    }
}