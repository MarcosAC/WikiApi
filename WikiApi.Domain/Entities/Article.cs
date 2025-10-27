namespace WikiApi.Domain.Entities;

public class Article
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public string Tags { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; private set; }

    public Article(string title, string content, string tags, string category)
    {
        Update(title, content, tags, category);
        CreatedAt = DateTime.UtcNow;
    }

    public Article() { }

    public void Update(string title, string content, string tags, string category)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Titulo é obrigatório");
        if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Conteúdo é obrigatório");

        Title = title;
        Content = content;
        Tags = tags ?? string.Empty;
        Category = category ?? string.Empty;
        UpdateAt = DateTime.UtcNow;
    }
}