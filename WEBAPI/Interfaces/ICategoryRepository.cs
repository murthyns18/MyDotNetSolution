using LMS_API.Models;
using System.Collections.Generic;

namespace LMS_API.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetList(int categoryId = 0);
        string SaveCategory(Category category);
    }
}
