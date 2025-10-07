using System.Text.Json.Serialization;

namespace TestTask.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Book> Books { get; set; } = [];

    }
}
