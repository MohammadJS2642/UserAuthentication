using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers;

[Authorize(Roles = "admin")]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IJWTManagerRepository _repository;
    public UsersController(IJWTManagerRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public List<string> Get()
    {
        var users = new List<string>
        {
            "Satinder Singh",
            "Amit Sarna",
            "Davin Jon"
        };
        return users;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("authenticate")]
    public IActionResult Authenticate(Users userData)
    {
        var token = _repository.Authenticate(userData);
        if (token == null)
        {
            return Unauthorized();
        }
        return Ok(token);
    }

}
