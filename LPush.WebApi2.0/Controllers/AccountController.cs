using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using LPush.Core.Data;
using LPush.Model.Basic;
using LPush.WebApi.Models;
namespace LPush.WebApi.Controllers
{
    public class AccountController : ApiController
    {
        [HttpPost]
        public DataResult<Login> GetUser(UserReqeust request)
        {
            DataResult<Login> result = new DataResult<Login>
            {
                IsSuccess = true,
                Content = new Login()
                {
                    Id = request.Id,
                },
            };

            return result;
        }
    }
}
