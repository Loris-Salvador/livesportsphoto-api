using Application.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class AdminController : Controller
{
    public AdminController(IUserRepository userRepository)
    {
        UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    private IUserRepository UserRepository { get; }

    [Authorize]
    public Task<IActionResult> Content()
    {
        return Task.FromResult<IActionResult>(View());
    }

    public Task<IActionResult> Index()
    {
        return Task.FromResult<IActionResult>(View());
    }
}