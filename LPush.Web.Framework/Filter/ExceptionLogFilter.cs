using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Net.Http;

using Newtonsoft.Json;
using log4net;
using Generic;

using LPush.Core.Data;
using LPush.Web.Framework.Middleware;
namespace LPush.Web.Framework.Filter
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class ExceptionFilter: ExceptionFilterAttribute
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(ExceptionFilter));

        private AuthorizeOptions options;
        public ExceptionFilter(AuthorizeOptions options)
        {
            this.options = options;
        }

        public override void OnException(HttpActionExecutedContext filterContext)
        {
            Exception ex = null;
            if (filterContext.Exception != null)
            {
                logger.Error(filterContext.Exception);
                ex = filterContext.Exception;
            }

            if (filterContext.Exception.InnerException != null)
            {
                logger.Error(filterContext.Exception.InnerException);
                ex = filterContext.Exception.InnerException;
            }

            if (options.IsAuthorize)
            {
                SendError(filterContext, ex.Message);
                return;
            }

            base.OnException(filterContext);
        }

        private bool SendError(HttpActionExecutedContext context, string error)
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
            if (string.IsNullOrEmpty(aesKey)||string.IsNullOrEmpty(aesIV))
            {
                return false;
            }

            //返回消息
            var response = new HttpResponseMessage();
            DateTime now = DateTime.Now;
            DataResult result = new DataResult() { IsSuccess = false, Message = error };
            string responseData = JsonConvert.SerializeObject(result);
            //签名，token
            response.Headers.Add("Sign", (signArray[0]+"|" + now.ToString()).ToBase64());
            response.Headers.Add("Token", EncryptUtils.GetMD5String(now.ToString()+responseData));
            //加密
            responseData = EncryptUtils.AESEncrypt(responseData,aesKey,aesIV);
            response.Content = new StringContent(responseData);
            context.Response = response;

            return true;
        }
    }
}
