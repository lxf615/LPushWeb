using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using log4net;
using Microsoft.Owin;
using Generic;

namespace LPush.Web.Framework.Middleware
{
    public class AuthorizeMiddleware : OwinMiddleware
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(AuthorizeMiddleware));

        private AuthorizeOptions options;
        public AuthorizeMiddleware(OwinMiddleware next, AuthorizeOptions options) : base(next)
        {
            this.options = options;
        }

        public override async Task Invoke(IOwinContext context)
        {
            AuthorizeResult result = await ProcessRequest(context);
            if (result.IsValid)
            {
                if (!string.IsNullOrEmpty(result.Msg) && options.IsAuthorize)
                {
                    using (Stream stream = new MemoryStream())
                    {
                        using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
                        {
                            context.Request.Body.Dispose();
                            //Reset request body
                            await streamWriter.WriteAsync(result.Msg);
                            streamWriter.Flush();
                            stream.Position = 0;
                            context.Request.Body = stream;

                            //process reponse.
                            await Next.Invoke(context);
                        }
                    }
                }
                else
                {
                    context.Request.Body.Position = 0;
                    await Next.Invoke(context);
                }

            }
        }


        /// <summary>
        /// 授权处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<AuthorizeResult> ProcessRequest(IOwinContext context)
        {

            AuthorizeResult result;

            IFormCollection formCollection = await context.Request.ReadFormAsync();
            string path = context.Request.Path.Value.ToLower();
            if (path.Equals("/"))
            {
                result = new AuthorizeResult()
                {
                    IsValid = true,
                };
                return result;
            }

            if (options.IsAuthorize)
            {
                result = await ValidateSign(context);
                if (!result.IsValid)
                {
                    throw new UnauthorizedAccessException("授权失败!");
                }
            }
            else
            {
                //不做token校验
                string msg = await ReadMsg(context);
                result = new AuthorizeResult()
                {
                    IsValid = true,
                    Msg = msg
                };

                //记录日志
                LogMsg(context, msg);
            }

            return result;
        }

        /// <summary>
        /// 授权验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<AuthorizeResult> ValidateSign(IOwinContext context)
        {
            //读取签名
            string sign = context.Request.Headers.GetValues("Sign").FirstOrDefault();
            if (string.IsNullOrEmpty(sign))
            {
                return new AuthorizeResult()
                {
                    IsValid = false
                };
            }
            sign = sign.FromBase64();
            string[] signArray = sign.Split("|".ToCharArray());
            if (signArray == null || signArray.Length != 2)
            {
                return new AuthorizeResult()
                {
                    IsValid = false
                };
            }

            //读取密钥
            string aesKey = INIFileUtils.ReadString(options.ConfigPath, signArray[0], "AESKey");
            string aesIV = INIFileUtils.ReadString(options.ConfigPath, signArray[0], "AESIV");
            if (string.IsNullOrEmpty(aesKey))
            {
                return new AuthorizeResult()
                {
                    IsValid = false
                };
            }

            //读取token
            string token = context.Request.Headers.GetValues("Token").FirstOrDefault();
            //token = EncryptUtils.AESDecrypt(token, aesKey, aesIV);
            if (string.IsNullOrEmpty(token))
            {
                return new AuthorizeResult()
                {
                    IsValid = false
                };
            }


            //读取密文
            string scretMsg = await ReadMsg(context);

            //校验token
            return await ValidateToken(context, token, signArray[1], scretMsg, aesKey, aesIV);
        }

        /// <summary>
        /// 读取消息
        /// </summary>
        /// <returns></returns>
        public async Task<string> ReadMsg(IOwinContext context)
        {
            string scretMsg = string.Empty;
            context.Request.Body.Position = 0;
            //using (StreamReader stremReader = new StreamReader(context.Request.Body, Encoding.UTF8))
            //{
            //    scretMsg = await stremReader.ReadToEndAsync();
            //    scretMsg = scretMsg.Trim(@"""".ToCharArray());
            //}

            StreamReader stremReader = new StreamReader(context.Request.Body, Encoding.UTF8);
            scretMsg = await stremReader.ReadToEndAsync();
            scretMsg = scretMsg.Trim(@"""".ToCharArray());

            return scretMsg;
        }

        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timestamp"></param>
        /// <param name="scretMsg"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        public async Task<AuthorizeResult> ValidateToken(IOwinContext context, string token,
            string timestamp, string scretMsg, string aesKey, string aesIV)
        {
            //解密
            string msg = EncryptUtils.AESDecrypt(scretMsg, aesKey, aesIV);
            if (string.IsNullOrEmpty(msg))
            {
                return new AuthorizeResult()
                {
                    IsValid = false
                };
            }

            //校验
            if (!(string.Format("{0}|{1}", timestamp, msg)).ToMD5().Equals(token))
            {
                return new AuthorizeResult()
                {
                    IsValid = false
                };
            }

            //记录日志
            LogMsg(context, msg);

            return new AuthorizeResult()
            {
                IsValid = true,
                Msg = msg,
                AesKey = aesKey,
                AesIV = aesIV
            };
        }

        /// <summary>
        ///记录消息日志 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private void LogMsg(IOwinContext context, string msg)
        {
            string contentType = string.Empty;
            IList<string> headers = context.Request.Headers.GetValues("Content-Type");
            if (headers != null)
            {
                contentType = headers.FirstOrDefault();
            }

            //记录日志
            logger.Info((string.Format("{0}--{1}--{2}", context.Request.Uri.ToString(),
                contentType, msg)));
        }

        private async Task Response(IOwinResponse response, string msg)
        {
            await response.WriteAsync(msg);
        }
    }

    public class AuthorizeResult
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// 消息明文
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 加密密钥
        /// </summary>
        public string AesKey { get; set; }

        /// <summary>
        /// 位移量
        /// </summary>
        public string AesIV { get; set; }
    }
}
