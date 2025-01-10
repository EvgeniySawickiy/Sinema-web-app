using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Core.Entities;

namespace NotificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    public NotificationController()
    {
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Send")]
    public IActionResult Send([FromBody] Notification notification)
    {
        return Ok();
    }
}