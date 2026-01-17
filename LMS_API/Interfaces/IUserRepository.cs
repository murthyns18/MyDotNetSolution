using LMS_API.Models;
using System.Collections.Generic;

namespace LMS_API.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// To get user list or user by id
        /// </summary>
        /// <param name="userId">0, -1, >0</param>
        /// <returns>Returns user list</returns>
        IEnumerable<User> GetList(int userId = 0);

        /// <summary>
        /// To add or update user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns success message</returns>
        string SaveUser(User user);

        /// <summary>
        /// To delete user
        /// </summary>
        /// <param name="userId"></param>
        void DeleteUser(int userId);
    }
}
