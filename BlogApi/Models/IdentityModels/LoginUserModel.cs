namespace BlogApi.Models.IdentityModels;

public class LoginUserModel
{
    public required string Password { get; set; }
    public required string Username { get; set; }
}