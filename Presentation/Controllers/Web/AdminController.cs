using Application.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Web;

public class AdminController : Controller
{
    public AdminController(IUserRepository userRepository)
    {
        UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    private IUserRepository UserRepository { get; }

    public async Task<IActionResult> Index(string name, string password, CancellationToken cancellation)
    {
        var user = await UserRepository.GetUser(name, cancellation);

        if (user == null || user.Password != password)
        {
            return BadRequest();
        }

        return View();
    }
}