using System;

using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using Nancy.Authentication.Token;

using LPush.WebApi.Infrastructure;
using LPush.Web.Framework;
using LPush.Core.Data;
using LPush.Model;
using LPush.Service.Basic;
using LPush.Web.Framework.Models;

using Generic;
namespace LPush.WebApi
{
    public class AuthModule : ApiModule
    {
        private IUserService service;
        private ITokenizer tokenizer;
        public AuthModule(ITokenizer tokenizer, IUserService service) : base("auth/token")
        {
            this.service = service;
            this.tokenizer = tokenizer;
            Post["/"] = x =>
            {
                var user = this.Bind<Login>();
                return GetToken(user.LoginName,user.Password);
            };
        }

        private DataResult<Host> GetToken(string username,string password)
        {
            DataResult<Host> result = new DataResult<Host>();

            var validateUser = service.ValidateUser(username, password);
            if (validateUser == null)
            {
                result.Message = "连接服务器失败!";
            }


            IUserIdentity userIdentity = new UserIdentity(username);
            try
            {
                result.Content = new Host()
                {
                    Token = tokenizer.Tokenize(userIdentity, Context),
                    IP = "192.168.47.1",
                    Port = 5222,
                };
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

    }
}