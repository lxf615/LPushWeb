using System;
using System.Collections.Generic;

using Nancy;
using System.Threading.Tasks;

using Nancy.Security;
using LPush.WebApi.Infrastructure;
using Nancy.ModelBinding;
using LPush.Service.Basic;
using LPush.Model.Basic;

using LPush.Core.Data;
using Generic;


namespace LPush.WebApi.Controller
{
    public class UserController:ApiModule
    {
        private IUserService service;
        public UserController(IUserService service):base("User")
        {
#if !DEBUG
            this.RequiresAuthentication();
#endif

            this.service = service;
            Post["/login",true] = async(_,ct) =>
            {
                var user = this.Bind<Login>();
                return await Task.Run(() =>
                {
                    return service.ValidateUser(user.LoginName, user.Password);
                });
            };

            Get["/{enterpriseId}",true] = async(_,ct)  => 
            {
				DataResult<IList<Login>> users = await Task.Run(()=>{
					return service.GetUsers(_.enterpriseId);
				});
                return this.Response(users);
            };

            Post["/",true] = async(_,ct) => 
            {
                var user = this.Bind<Login>();
                user.UserType = "U";
                user.CreateDate = DateTime.Now;
                return await Task.Run(()=> 
                {
                    service.InsertUser(user);
                    return user;
                });
                
            };
        }
    }
}