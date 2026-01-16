using LMS_API.Models;
using System.Collections.Generic;

namespace LMS_API.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetList(int userId = 0);
        string SaveUser(User user);
        void DeleteUser(int userId);
    }
}
