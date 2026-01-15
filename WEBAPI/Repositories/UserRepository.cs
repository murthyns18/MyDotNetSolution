using Dapper;
using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LMS_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection dbConnection;

        public UserRepository(string? connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
        }

        public IEnumerable<User> GetList(int userId = 0)
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserID", userId);

            return dbConnection.Query<User>(
                "User_GetList",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public string SaveUser(User user)
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserID", user.UserID);
            parameters.Add("UserName", user.UserName);
            parameters.Add("Email", user.Email);
            parameters.Add("MobileNumber", user.MobileNumber);
            parameters.Add("Address", user.Address);
            parameters.Add("RoleID", user.RoleID);
            parameters.Add("Status", user.Status);
            parameters.Add("Result", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

            dbConnection.Execute(
                "User_InsertUpdate",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<string>("Result");
        }

        public void DeleteUser(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserID", userId);

            dbConnection.Execute(
                "User_Delete",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }


    }
}
