using Dapper;
using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;

namespace LMS_API.Repositories
{
    public class BookRepository : IBookRepository
    {
        IDbConnection dbConnection;

        public BookRepository(string? connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
        }

        public IEnumerable<Book> GetList(int bookId = 0)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("BookId", bookId);
            return dbConnection.Query<Book>("Book_GetList", dynamicParameters, commandType: CommandType.StoredProcedure, commandTimeout: 600);

        }

        public string SaveBook(Book book)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("BookId", book.BookID);
            dynamicParameters.Add("Title", book.Title);
            dynamicParameters.Add("ISBN", book.ISBN);
            dynamicParameters.Add("Price", book.Price);
            dynamicParameters.Add("Quantity", book.Quantity);
            dynamicParameters.Add("PublisherId", book.PublisherId);
            dynamicParameters.Add("CategoryId", book.CategoryId);
            dynamicParameters.Add("IsActive", book.IsActive);
            dynamicParameters.Add("Result", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
            dbConnection.Query<string>("Book_InsertUpdate", dynamicParameters, commandType: CommandType.StoredProcedure, commandTimeout: 600);
            return dynamicParameters.Get<string>("Result");
        }
    }
}
