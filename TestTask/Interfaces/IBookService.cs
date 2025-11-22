using TestTask.Models;

namespace TestTask.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> Get();
        Task<Book?> GetById(int id);
        Task<Book> Update(Book book);
        Task<Book> Add(Book book);
        Task <bool>Delete(int id);
        Task<IEnumerable<Book>> GetByAuthor(string authorName);
        Task<ReadingStats> GetReadingStats();
    }

}
