using System;
using System.Collections.Generic;

using LPush.Model;
using LPush.Core.Data;

namespace LPush.Service.Basic
{
    public interface IUserService
    {
        /// <summary>
        /// 验证用户与密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        DataResult<Login> ValidateUser(string userName, string password);


        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        IList<Login> GetUsers(int enterpriseId);        


        /// <summary>
        ///  Gets the User by userName.
        /// </summary>
        /// <returns>The User</returns>
        /// <param name="userName">User Name</param>
        /// <returns></returns>
        Login GetUser(string userName);

        /// <summary>
        ///  Inserts the User.
        /// </summary>
        /// <returns><c>true</c>, if User was inserted, <c>false</c> otherwise.</returns>
        /// <param name="user">user</param>
		bool InsertUser(Login user);

        /// <summary>
        /// Update a User
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(Login user);

        /// <summary>
        /// Delete a User
        /// </summary>
        /// <param name="userName"></param>
        void DeleteUser(string userName);
    }
}
