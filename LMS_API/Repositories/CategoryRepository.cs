using Dapper;
using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LMS_API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnection dbConnection;

        public CategoryRepository(string? connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
        }

        public IEnumerable<Category> GetList(int categoryId = 0)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("CategoryID", categoryId);

            return dbConnection.Query<Category>(
                "Category_GetList",
                dynamicParameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

        public string SaveCategory(Category category)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("CategoryID", category.CategoryID);
            dynamicParameters.Add("CategoryName", category.CategoryName);
            dynamicParameters.Add("IsActive", category.IsActive);
            dynamicParameters.Add("CreatedBy", category.CreatedBy);
            dynamicParameters.Add("ModifiedBy", category.ModifiedBy);
            dynamicParameters.Add("Result", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

            dbConnection.Query<string>(
                "Category_InsertUpdate",
                dynamicParameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );

            return dynamicParameters.Get<string>("Result");
        }

        public string DeleteCategory(int categoryID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryID", categoryID);

            return dbConnection.QuerySingle<string>(
                "Category_Delete",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }
    }
}
