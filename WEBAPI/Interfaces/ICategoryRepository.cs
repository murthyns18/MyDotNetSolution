using LMS_API.Models;
using System.Collections.Generic;

namespace LMS_API.Interfaces
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// To list the Categories
        /// </summary>
        /// <param name="categoryId">-1, 0, any category id</param>
        /// <returns>list of categories</returns>
        IEnumerable<Category> GetList(int categoryId = 0);

        /// <summary>
        /// To save the category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Return msg Category added successfully</returns>
        string SaveCategory(Category category);

        string DeleteCategory(int categoryId);

    }
}
