using BlogApi.Entities;

namespace BlogApi.Models.IdentityModels;

public class UserModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string UserName { get; set; }

    public UserModel(User user)
    {
        Id = user.Id;
        Name = user.Name;
        UserName = user.Username;
    }
}