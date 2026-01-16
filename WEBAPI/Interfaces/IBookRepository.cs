using LMS_API.Models;

namespace LMS_API.Interfaces
{
    public interface IBookRepository
    {
        /// <summary>
        /// To get list of books
        /// </summary>
        /// <param name="bookId">0, > 0, -1</param>
        /// <returns>list of all books, active books, single book</returns>

        public IEnumerable<Book> GetList(int bookId=0);

        /// <summary>
        /// To delete a book
        /// </summary>
        /// <param name="bookID"></param>
        /// <returns>Return msg book deleted successfully</returns>
        string DeleteBook(int bookID);

        /// <summary>
        /// To save the book
        /// </summary>
        /// <param name="book"></param>
        /// <returns>Return msg book added successfully</returns>
        public string SaveBook(Book book);
    }
}
