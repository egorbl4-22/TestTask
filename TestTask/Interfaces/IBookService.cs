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
    public class ReadingStats
    {
        public int TotalBooks { get; set; }
        public int ReadBooks { get; set; }
        public int UnreadBooks { get; set; }
        public double ReadPercentage { get; set; }
    }
}
