using Microsoft.AspNetCore.Mvc;
using RentAutoPoc.Api.Application.Services;

namespace RentAutoPoc.Api.Controllers;

[Route("api/v1/images")]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
    }

    [HttpGet("{image-name}")]
    public async Task<IActionResult> GetImage([FromRoute(Name = "image-name")] string name)
    {
        var imageType = name.Split('.').Last();
        
        var image = await _imageService.GetImage(name, CancellationToken.None);
        return File(image, $"image/{imageType}");
    }

    [HttpPut("{image-name}")]
    public Task<IActionResult> AddImage([FromRoute(Name = "image-name")] string name)
    {
        return Task.FromResult<IActionResult>(Ok());
    }
}