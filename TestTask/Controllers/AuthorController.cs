using Microsoft.AspNetCore.Mvc;
using TestTask.Data;
using TestTask.Interfaces;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController: ControllerBase
    {
        private readonly IBookService _bookService;

        public AuthorController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("author/{authorName}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor(string authorName)
        {
            var books = await _bookService.GetByAuthor(authorName);
            return Ok(books);
        }
    }
}
