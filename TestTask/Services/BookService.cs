using Microsoft.EntityFrameworkCore;
using System;
using TestTask.Data;
using TestTask.Interfaces;
using TestTask.Models;
namespace TestTask.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly Context _context;
        public BookService(IBookRepository bookRepository, Context context)
        {
            _bookRepository = bookRepository;
            _context = context;
        }
        public Task<IEnumerable<Book>> Get() => _bookRepository.Get();
        public Task<Book?> GetById(int id) => _bookRepository.GetById(id);
        public Task<Book> Add(Book book) => _bookRepository.Add(book);
        public Task<Book> Update(Book book) => _bookRepository.Update(book);
        public Task <bool>Delete(int id) => _bookRepository.Delete(id);
        public Task<IEnumerable<Book>> GetByAuthor(string authorName) => _bookRepository.GetByAuthor(authorName);
        public async Task<ReadingStats> GetReadingStats()
        {

            /* SQL: SELECT 
                     COUNT(*) as TotalBooks,
                     SUM(CASE WHEN IsRead = 1 THEN 1 ELSE 0 END) as ReadBooks,
                     SUM(CASE WHEN IsRead = 0 THEN 1 ELSE 0 END) as UnreadBooks
                 FROM Books
            */

            var stats = await _context.Books
                .GroupBy(b => 1)
                .Select(g => new
                {
                    TotalBooks = g.Count(),
                    ReadBooks = g.Count(b => b.IsRead),
                    UnreadBooks = g.Count(b => !b.IsRead)
                })
                .FirstOrDefaultAsync();

            if (stats == null)
            {
                return new ReadingStats();
            }

            return new ReadingStats
            {
                TotalBooks = stats.TotalBooks,
                ReadBooks = stats.ReadBooks,
                UnreadBooks = stats.UnreadBooks,
                ReadPercentage = stats.TotalBooks > 0 ?
                    Math.Round((double)stats.ReadBooks / stats.TotalBooks * 100, 2) : 0
            };
        }
    }
}
