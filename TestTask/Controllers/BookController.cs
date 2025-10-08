using TestTask.Models;
using TestTask.Services;
using Microsoft.AspNetCore.Mvc;
using TestTask.Interfaces;
using TestTask.Data;
using Microsoft.EntityFrameworkCore;
using TestTask;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly Context _context;

        public BooksController(IBookService bookService, Context context)
        {
            _bookService = bookService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _bookService.Get();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetById(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost]
        public async Task<Book> Add(Book book)
        {
            var existingAuthor = await _context.Authors
                .FirstOrDefaultAsync(a => a.Name.ToLower() == book.Authors.Name.ToLower());

            if (existingAuthor != null)
            {
                book.AuthorId = existingAuthor.Id;
                book.Authors = existingAuthor;
            }
            else
            {
                _context.Authors.Add(book.Authors);
                await _context.SaveChangesAsync();
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateBookDto>> UpdateBookAsync(int id, UpdateBookDto bookDto)
        {
            var existingBook = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existingBook == null)
                return NotFound("Книга не найдена");

            var author = await _context.Authors
                .FirstOrDefaultAsync(a => a.Name == bookDto.Author.Name);

            if (author == null)
                return NotFound("Автор не найден");

            // Обновляем книгу
            existingBook.Title = bookDto.Title;
            existingBook.Year = bookDto.Year;
            existingBook.Genre = bookDto.Genre;
            existingBook.IsRead = bookDto.IsRead;
            existingBook.AuthorId = author.Id;

            // Обновляем автора
            author.Name = bookDto.Author.Name;
            author.Country = bookDto.Author.Country;

            await _context.SaveChangesAsync();

            var result = new UpdateBookDto
            {
                Title = existingBook.Title,
                Genre = existingBook.Genre,
                Year = bookDto.Year,
                IsRead = existingBook.IsRead,
                Author = new AuthorDto
                {
                    Name = author.Name,
                    Country = author.Country
                }
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var result = await _bookService.Delete(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("stats")]
        public async Task<ActionResult<ReadingStats>> GetStats()
        {
            var stats = await _bookService.GetReadingStats();
            return Ok(stats);
        }
    }
}