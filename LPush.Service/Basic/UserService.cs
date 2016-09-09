using System;
using System.Linq;
using System.Collections.Generic;

using Generic;
using LPush.Core.Data;
using LPush.Core.Caching;
using LPush.Model;

namespace LPush.Service.Basic
{
    public class UserService: IUserService
    {
		/// <summary>
		/// Key for caching
		/// </summary>
		/// <remarks>
		/// {0} : User ID
		/// </remarks>
		private const string Users_By_UserName_Key = "LPush.User.UserName-{0}";
        private const string Users_By_EnterpriseId_Key= "LPush.User.EnterpriseId-{0}";

        private const string Users_Pattern_Key = "LPush.User.";

        private readonly IRepository<Login> userRepository =null;

        private readonly ICacheManager _cacheManager;
		//private readonly IEventPublisher _eventPublisher;
		public UserService(IRepository<Login> userRepository, ICacheManager cacheManager)
        {
            this.userRepository = userRepository;
            this._cacheManager = cacheManager;
        }


        /// <summary>
        /// 验证用户与密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public DataResult<Login> ValidateUser(string userName, string password)
        {
            DataResult<Login> result = new DataResult<Login>();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                result.Message = "用户名或密码不能为空!";
                return result;
            }

            if (userName.ToLower().Equals("admin") &&
                !password.Equals(DateTime.Today.ToString("yyyyMMdd").ToMD5()))
            {
                result.Message = "用户名或密码不能为空!";
                return result;
            }

            var user = GetUser(userName);
            if (user == null || user.Password != password.ToMD5())
            {
                result.Message = "用户名或密码错误!";
                return result;
            }

            result.Content = user;
            result.IsSuccess = true;

            return result;
        }


        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public IList<Login> GetUsers(int enterpriseId)
        {
            string key = string.Format(Users_By_EnterpriseId_Key,enterpriseId);
            return _cacheManager.Get<List<Login>>(key,()=> 
            {
                return null;
            });
        }


        /// <summary>
        ///  Gets the User by loginName.
        /// </summary>
        /// <returns>The User</returns>
        /// <param name="userName">Login Name</param>
        /// <returns></returns>
        public virtual Login GetUser(string userName)
        {
            //string key = string.Format(Users_By_UserName_Key, userName);

            //var user = _cacheManager.Get(key, () => (from item in userRepository.SlaveTable
            //                                               where item.LoginName == userName
            //                                               select item).SingleOrDefault());
            //return user;

            return null;
        }

        /// <summary>
        /// Inserts the User.
        /// </summary>
        /// <returns><c>true</c>, if User was inserted, <c>false</c> otherwise.</returns>
        /// <param name="User">User.</param>
        public virtual bool InsertUser(Login user)
		{
			if (user == null)
				throw new ArgumentNullException("User");

			userRepository.Insert(user);
            string key = string.Format(Users_By_EnterpriseId_Key, user.EnterpriseId);
            _cacheManager.Remove(key);

            return true;
        }

        /// <summary>
        /// Update a User
        /// </summary>
        /// <param name="User"></param>
        public virtual void UpdateUser(Login user)
        {
            if (user == null)
            {
                return;
            }
            //validate category hierarchy
            this.userRepository.Update(user);

            //cache
            string key = string.Format(Users_By_UserName_Key, user.LoginName);
            _cacheManager.Remove(key);

            key = string.Format(Users_By_EnterpriseId_Key, user.EnterpriseId);
            _cacheManager.Remove(key);
        }

        /// <summary>
        /// Delete a User
        /// </summary>
        /// <param name="User"></param>
        public virtual void DeleteUser(string userName)
        {
            Login user = GetUser(userName);
            if (user == null)
            {
                return;
            }

            user.IsDeleted = true;
            UpdateUser(user);
        }
    }

}
