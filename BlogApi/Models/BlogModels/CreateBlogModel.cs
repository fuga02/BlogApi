namespace BlogApi.Models.BlogModels;

public class CreateBlogModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}