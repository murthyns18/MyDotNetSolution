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

        string DeleteBook(int bookID);
        public string SaveBook(Book book);
    }
}
