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
    }
}