using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using log4net;
using Microsoft.Owin;
using Generic;
namespace LPush.Web.Framework.Middleware
{
    public class AuthorizeResult
    {
       /// <summary>
       /// 是否有效
       /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// 消息
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
                if (!string.IsNullOrEmpty(result.Msg))
                {

                    using (Stream stream = new MemoryStream() )
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
                            //await ProcessResponse(context,result.AesKey,result.AesIV);
                        }
                    }
                }
                else
                {
                    await Next.Invoke(context);
                }
                
            }
        }



        /// <summary>
        /// 授权返回
        /// </summary>
        /// <returns></returns>
        public async Task ProcessResponse(IOwinContext context,string aesKey,string aesIV)
        {
            using (var buffer = new MemoryStream())
            {
                // Buffer the response
                var stream = context.Response.Body;
                context.Response.Body = buffer;

                await Next.Invoke(context);

                try
                {


                    buffer.Seek(0, SeekOrigin.Begin);
                    using (StreamReader stremReader = new StreamReader(buffer, Encoding.UTF8))
                    {
                        string msg = await stremReader.ReadToEndAsync();
                        int len = msg.Length;

                        msg = "{\"Content\":{\"UserName\":null,\"Password\":null,\"FirstName\":null,\"LastName\":null,\"Email\":null,\"EnterpriseID\":-1,\"UserType\":\"U\",\"CreateDate\":\"2016 - 07 - 15T16: 18:14.5624385 + 08:00\",\"CreateBy\":null,\"ModifyDate\":null,\"ModifyBy\":null,\"IsDeleted\":false,\"Id\":124}}";
                        //msg = EncryptUtils.AESEncrypt(msg, aesKey, aesIV);
                        msg = msg.PadRight(len);

                        byte[] bytes = msg.ToBytes();
                        buffer.Seek(0, SeekOrigin.Begin);
                        using (var memStream = new MemoryStream(bytes))
                        {
                            await memStream.CopyToAsync(buffer);
                            buffer.Seek(0, SeekOrigin.Begin);
                            await buffer.CopyToAsync(stream);
                        }

                        //msg = "{\"Content\":{\"UserName\":null,\"Password\":null,\"FirstName\":null,\"LastName\":null,\"Email\":null,\"EnterpriseID\":-1,\"UserType\":\"U\",\"CreateDate\":\"2016 - 07 - 15T16: 18:14.5624385 + 08:00\",\"CreateBy\":null,\"ModifyDate\":null,\"ModifyBy\":null,\"IsDeleted\":false,\"Id\":124}}";
                        //byte[] bytes = msg.ToBytes();
                        //var memStream = new MemoryStream(bytes);
                        //memStream.Position = 0;
                        //memStream.CopyTo(stream);
                    }
                }
                catch (Exception ex)
                {

                    throw;
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

            result = await ValidateSign(context);
            if (!result.IsValid)
            {
                throw new UnauthorizedAccessException("授权失败!");
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
            string scretMsg = string.Empty;
            context.Request.Body.Position = 0;
            using (StreamReader stremReader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                scretMsg = await stremReader.ReadToEndAsync();
                scretMsg= scretMsg.Trim(@"""".ToCharArray());
            }

            //校验token
            return await ValidateToken(context, token, signArray[1], scretMsg, aesKey, aesIV);
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
            if (!(string.Format("{0}|{1}", timestamp ,msg)).ToMD5().Equals(token))
            {
                return new AuthorizeResult()
                {
                    IsValid = false
                };
            }

            //记录日志
            logger.Info(msg);

            return new AuthorizeResult()
            {
                IsValid = true,
                Msg = msg,
                AesKey = aesKey,
                AesIV = aesIV
            };
        }

        private async Task Response(IOwinResponse response, string msg)
        {
            await response.WriteAsync(msg);
        }
    }
}
