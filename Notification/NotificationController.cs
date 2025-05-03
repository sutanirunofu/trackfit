using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Notification;

[ApiController]
[Route("api/v1/[controller]")]
[EnableCors("AllowAngular")]
public class NotificationController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetMyNotifications()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var notifications = await context.Notifications
            .Include(n => n.User)
            .Where(n => n.User.Id.Equals(Guid.Parse(userId)))
            .OrderByDescending(n => n.CreationDate)
            .ToListAsync();

        return Ok(notifications);
    }

    [HttpGet("{notificationId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetMyNotificationById(Guid notificationId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var notification = await context.Notifications
            .Include(n => n.User)
            .Where(n => n.User.Id.Equals(userId))
            .FirstOrDefaultAsync(n => n.Id.Equals(notificationId));

        if (notification == null)
        {
            return NotFound();
        }

        return Ok(notification);
    }

    [HttpPatch("Read/{notificationId:guid}")]
    [Authorize]
    public async Task<IActionResult> ReadUserNotificationById(Guid notificationId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var notification = await context.Notifications
            .Include(n => n.User)
            .Where(n => n.User.Id.Equals(Guid.Parse(userId)))
            .FirstOrDefaultAsync(n => n.Id.Equals(notificationId));

        Console.WriteLine($"Notification: {notification?.Id}");
        
        if (notification == null)
        {
            return NotFound();
        }

        notification.IsRead = true;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{notificationId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteUserNotificationById(Guid notificationId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var notification = await context.Notifications
            .Include(n => n.User)
            .Where(n => n.User.Id.Equals(Guid.Parse(userId)))
            .FirstOrDefaultAsync(n => n.Id.Equals(notificationId));

        if (notification == null)
        {
            return NotFound();
        }
        
        context.Notifications.Remove(notification);
        await context.SaveChangesAsync();

        return NoContent();
    }
}