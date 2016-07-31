using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

using LPush.Web.Framework.Middleware;

using Newtonsoft.Json;
using log4net;
using Generic;
namespace LPush.Web.Framework.Filter
{
    public class ResponseFilter: ActionFilterAttribute
    {
        private AuthorizeOptions options;
        public ResponseFilter(AuthorizeOptions options):base()
        {
            this.options = options;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {

            if (context.Exception == null)
            {
                ProcessResponse(context);
            }

            base.OnActionExecuted(context);
        }

        private bool ProcessResponse(HttpActionExecutedContext context)
        {

            //读取签名
            string sign = context.Request.Headers.GetValues("Sign").FirstOrDefault();
            if (string.IsNullOrEmpty(sign))
            {
                return false;
            }
            sign = sign.FromBase64();
            string[] signArray = sign.Split("|".ToCharArray());
            if (signArray == null || signArray.Length != 2)
            {
                return false;
            }

            //读取密钥
            string aesKey = INIFileUtils.ReadString(options.ConfigPath, signArray[0], "AESKey");
            string aesIV = INIFileUtils.ReadString(options.ConfigPath, signArray[0], "AESIV");
            if (string.IsNullOrEmpty(aesKey) || string.IsNullOrEmpty(aesIV))
            {
                return false;
            }

            var response = context.Response;
            string msg = response.Content.ReadAsStringAsync().Result;
            DateTime now = DateTime.Now;
            //签名，token
            response.Headers.Add("Sign", (signArray[0] + "|" + now.ToString()).ToBase64());
            response.Headers.Add("Token", EncryptUtils.GetMD5String(now.ToString() + msg));
            //加密
            response.Content = new StringContent(EncryptUtils.AESEncrypt(msg, aesKey, aesIV));

            return true;
        }

    }
}
