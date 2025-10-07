using TestTask.Models;

public class UpdateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } =string.Empty;
    public int Year { get; set; }
    public bool IsRead { get; set; }
    public AuthorDto Author { get; set; } = null!;
}

public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
