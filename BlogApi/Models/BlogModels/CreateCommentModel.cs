
namespace BlogApi.Models.BlogModels;

public class CreateCommentModel
{
    public Guid PostId { get; set; }
    public string Message { get; set; }
}