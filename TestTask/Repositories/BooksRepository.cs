using TestTask.Data;
using TestTask.Models;
using Microsoft.EntityFrameworkCore;
using System;
using TestTask.Interfaces;

namespace TestTask.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly Context _context;
        public BookRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> Get()
        {
            // SQL: SELECT * FROM Books JOIN Authors ON Books.AuthorId = Authors.Id
            return await _context.Books
                .Include(b => b.Authors)
                .ToListAsync();
        }

        public async Task<Book?> GetById(int id)
        {
            // SQL: SELECT * FROM Books JOIN Authors ON Books.AuthorId = Authors.Id WHERE Books.Id = @id
            return await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> Add(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> Update(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            var books = await _context.Books.ToListAsync();
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
                return true;
        }
            

        public async Task<IEnumerable<Book>> GetByAuthor(string authorName)
        {
            return await _context.Books
                .Include(b => b.Authors)
                .Where(b => b.Authors.Name.Contains(authorName))
                .ToListAsync();
        }

        public async Task<ReadingStats> GetReadingStats()
        {

            /* SQL: SELECT 
                     COUNT(*) as TotalBooks,
                     SUM(CASE WHEN IsRead = 1 THEN 1 ELSE 0 END) as ReadBooks,
                     SUM(CASE WHEN IsRead = 0 THEN 1 ELSE 0 END) as UnreadBooks
                 FROM Books
            */

            var stats = new
            {
                TotalBooks = await _context.Books.CountAsync(),
                ReadBooks = await _context.Books.CountAsync(b => b.IsRead),
                UnreadBooks = await _context.Books.CountAsync(b => !b.IsRead)
            };

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