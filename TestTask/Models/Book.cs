namespace TestTask.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Year { get; set; }
        public bool IsRead {  get; set; }
        //Ссылка один ко многим у автора много книг оставляем ключ ид автора
        public int AuthorId { get; set; }
        public Author? Authors { get; set; } = null!;
    }
}
