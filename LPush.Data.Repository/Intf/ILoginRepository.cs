using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LPush.Core.Data;
using LPush.Model;
namespace LPush.Data.Repository
{
    public interface ILoginRepository:IRepository<Login>
    {

        /// <summary>
        /// 根据登录名帐号
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        Task<Login> GetByLoginName(string loginName);
    }
}
