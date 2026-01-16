using Dapper;
using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LMS_API.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IDbConnection dbConnection;

        public BookRepository(string? connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
        }

        
        public IEnumerable<Book> GetList(int bookId = 0)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BookId", bookId);

            return dbConnection.Query<Book>(
                "Book_GetList",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

        public string SaveBook(Book book)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@BookId", book.BookID);
            parameters.Add("@Title", book.Title);
            parameters.Add("@ISBN", book.ISBN);
            parameters.Add("@Price", book.Price);
            parameters.Add("@Quantity", book.Quantity);
            parameters.Add("@PublisherId", book.PublisherId);
            parameters.Add("@CategoryId", book.CategoryId);
            parameters.Add("@IsActive", book.IsActive);

            parameters.Add(
                "@Result",
                dbType: DbType.String,
                direction: ParameterDirection.Output,
                size: 500
            );

            dbConnection.QuerySingle(
                "Book_InsertUpdate",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );

            return parameters.Get<string>("@Result");
        }


        public string DeleteBook(int bookID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BookId", bookID);

            return dbConnection.QuerySingle<string>(
                "Book_Delete",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

    }
}
