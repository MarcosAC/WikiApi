namespace WikiApi.Domain.Entities;

public class Article
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public string Tags { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; }

    public Article(string title, string content, string tags, string category, string author)
    {
        Update(title, content, tags, category);
        Author = author;
        CreatedAt = DateTime.UtcNow;
    }

    public Article() { }

    public void Update(string title, string content, string tags, string category)
    {
        Title = title;
        Content = content;
        Tags = tags ?? string.Empty;
        Category = category ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }
}
