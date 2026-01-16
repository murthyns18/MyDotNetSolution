using LMS_API.Models;

namespace LMS_API.Interfaces
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetRoles(short roleId = 0);
        string SaveRole(Role role);

        string DeleteRole(int roleId);
    }
}
