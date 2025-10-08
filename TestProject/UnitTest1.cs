using TestTask.Data;
using Microsoft.EntityFrameworkCore;
using System;
using TestTask.Models;
using TestTask.Repositories;
using TestTask.Services;
using Xunit;

namespace TestTask.Tests
{
    public class BookServiceTests
    {
        private readonly Context _context;
        private readonly BookService _bookService;

        [Fact]
        public async Task CreateBookAsync_ShouldCreateNewBook()
        {
            var author = new Author { Id = 10, Name = "Test Author", Country = "Test Country" };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            var newBook = new Book
            {
                Title = "Test Book",
                AuthorId = 10,
                Year = 2024,
                Genre = "Test Genre",
                IsRead = false
            };

            // Создаем
            var createdBook = await _bookService.Add(newBook);

            // Проверяем
            Assert.NotNull(createdBook);
            Assert.Equal("Test Book", createdBook.Title);
            Assert.Equal(10, createdBook.AuthorId);
            Assert.False(createdBook.IsRead);

            // Проверяем в бд
            var bookInDb = await _context.Books.FindAsync(createdBook.Id);
            Assert.NotNull(bookInDb);
            Assert.Equal("Test Book", bookInDb.Title);
        }
    }
}