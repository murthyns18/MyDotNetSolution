using Dapper;
using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LMS_API.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly IDbConnection dbConnection;

        public PublisherRepository(string? connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null");

            dbConnection = new SqlConnection(connectionString);
        }


        public string DeletePublisher(int publisherID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PublisherId", publisherID);

            return dbConnection.QuerySingle<string>(
                "Publisher_Delete",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

        public IEnumerable<Publisher> GetList(int publisherId = 0)
        {
            var parameters = new DynamicParameters();
            parameters.Add("PublisherID", publisherId);

            return dbConnection.Query<Publisher>(
                "Publisher_GetList",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

        public string SavePublisher(Publisher publisher)
        {
            if (publisher == null)
                throw new ArgumentNullException(nameof(publisher));

            var parameters = new DynamicParameters();
            parameters.Add("PublisherID", publisher.PublisherID);
            parameters.Add("PublisherName", publisher.PublisherName);
            parameters.Add("IsActive", publisher.IsActive);
            parameters.Add("CreatedBy", publisher.CreatedBy);
            parameters.Add("ModifiedBy", publisher.ModifiedBy);
            parameters.Add("Result", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

            dbConnection.Execute(
                "Publisher_InsertUpdate",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );

            return parameters.Get<string>("Result");
        }
    }
}
