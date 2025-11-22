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
        public BookService(IBookRepository bookRepository, Context context)
        {
            _bookRepository = bookRepository;
        }
        public Task<IEnumerable<Book>> Get() => _bookRepository.Get();
        public Task<Book?> GetById(int id) => _bookRepository.GetById(id);
        public Task<Book> Add(Book book) => _bookRepository.Add(book);
        public Task<Book> Update(Book book) => _bookRepository.Update(book);
        public Task <bool>Delete(int id) => _bookRepository.Delete(id);
        public Task<IEnumerable<Book>> GetByAuthor(string authorName) => _bookRepository.GetByAuthor(authorName);
        public Task<ReadingStats> GetReadingStats() => _bookRepository.GetReadingStats();
       
    }
}
