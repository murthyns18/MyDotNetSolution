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
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
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
            parameters.Add("Gender", user.Gender);
            parameters.Add("LanguagesKnown", user.LanguagesKnown);
            parameters.Add("CountryId", user.CountryId);
            parameters.Add("StateId", user.StateId);
            parameters.Add("CityId", user.CityId);
            parameters.Add("DOB", user.DOB);
            parameters.Add("InterestedCategories", user.InterestedCategories);
            parameters.Add("TermsAccepted", user.TermsAccepted);
            parameters.Add("Status", user.Status);
            parameters.Add("Password", user.Password);

            parameters.Add(
                "Result",
                dbType: DbType.String,
                direction: ParameterDirection.Output,
                size: 500
            );

            dbConnection.Execute(
                "User_InsertUpdate",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );

            return parameters.Get<string>("Result");
        }

        public string DeleteUser(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserID", userId);

            return dbConnection.QuerySingle<string>(
                "User_Delete",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

        public Tuple<User, IEnumerable<Menu>> AuthenticateUser(AuthenticateUser authenticateUser)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Email", authenticateUser.Email);
            parameters.Add("Password", authenticateUser.Password);

            var result = dbConnection.QueryMultiple(
                "User_AuthenticateUser",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );

            var user = result.ReadSingleOrDefault<User>();
            var menus = result.Read<Menu>();

            if (user == null)
                return null;

            return new Tuple<User, IEnumerable<Menu>>(user, menus);
        }

        public IEnumerable<User> GetCountries()
        {
            return dbConnection.Query<User>(
                "GetCountries",
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

        public IEnumerable<User> GetStatesByCountry(int countryId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("CountryId", countryId);

            return dbConnection.Query<User>(
                "GetStatesByCountry",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

        public IEnumerable<User> GetCitiesByState(int stateId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("StateId", stateId);

            return dbConnection.Query<User>(
                "GetCitiesByState",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }
    }
}
