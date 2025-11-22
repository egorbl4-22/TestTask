using TestTask.Models;
using TestTask.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TestTask.Interfaces;
using TestTask.Data;
using Microsoft.EntityFrameworkCore;
using TestTask;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;


        public BooksController(IBookService bookService)
        {
            _bookService = bookService;

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
        public async Task<ActionResult<UpdateBookDto>> Add(UpdateBookDto bookDto)
        {

            var book = new Book
            {
                Title = bookDto.Title,
                Year = bookDto.Year,
                Genre = bookDto.Genre,
                IsRead = bookDto.IsRead,
                Authors = new Author
                {
                    Name = bookDto.Author.Name,
                    Country = bookDto.Author.Country
                }
            };

            var createdBook = await _bookService.Add(book);

            // Возвращаем DTO вместо Entity
            var result = new UpdateBookDto
                {
                    Title = createdBook.Title,
                    Genre = createdBook.Genre,
                    Year = createdBook.Year,
                    IsRead = createdBook.IsRead,
                    Author = new AuthorDto
                    {
                        Name = createdBook.Authors.Name,
                        Country = createdBook.Authors.Country
                    }
                };

                return Ok(result);
            }
                 

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateBookDto>> UpdateBookAsync(int id, UpdateBookDto bookDto)
        {

                var existingBook = await _bookService.GetById(id);
                if (existingBook == null)
                    return NotFound("Книга не найдена");

                existingBook.Title = bookDto.Title;
                existingBook.Year = bookDto.Year;
                existingBook.Genre = bookDto.Genre;
                existingBook.IsRead = bookDto.IsRead;

                var updatedBook = await _bookService.Update(existingBook);

                // Возвращаем DTO
                var result = new UpdateBookDto
                {
                    Title = updatedBook.Title,
                    Genre = updatedBook.Genre,
                    Year = updatedBook.Year,
                    IsRead = updatedBook.IsRead,
                    Author = new AuthorDto
                    {
                        Name = updatedBook.Authors.Name,
                        Country = updatedBook.Authors.Country
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