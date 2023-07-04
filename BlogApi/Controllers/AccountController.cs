using BlogApi.Managers.Identity;
using BlogApi.Models;
using BlogApi.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager _userManager;
    private ILogger<AccountController> _logger;
    private readonly UserProvider _userProvider;

    public AccountController(UserManager userManager, 
        ILogger<AccountController> logger,
        UserProvider userProvider)
    {
        _userManager = userManager;
        _logger = logger;
        _userProvider = userProvider;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.Register(model);
        return Ok(new UserModel(user));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await _userManager.Login(model);

        return Ok(new { Token = token });
    }


    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = _userProvider.UserId;

        var user = await _userManager.GetUser(userId);
        if (user == null)
        {
            return Unauthorized();
        }
        return Ok(new UserModel(user));

    }
    [HttpPost("GetUser")]
    public async Task<IActionResult> GetUser(string userName)
    {
        var user = await _userManager.GetUser(userName);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new UserModel(user));
    }
}