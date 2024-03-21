using Microsoft.AspNetCore.Mvc;

namespace RentAutoPoc.Api.Controllers;

[Route("/")]
public class HelloWorldController : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> Get()
    {
        return Task.FromResult<IActionResult>(Ok("Hello world!"));
    }
}