namespace BlogApi.Models;

public class LoginUserModel
{
    public required string Password { get; set; }
    public required string Username { get; set; }
}