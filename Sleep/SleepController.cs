using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Sleep;

[ApiController]
[Route("api/v1/[controller]")]
[EnableCors("AllowAngular")]
public class SleepController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserSleeps()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized(new { Message = "Не авторизован" });
        }

        var sleeps = await context.Sleeps
            .Include(s => s.User)
            .Where(s => s.User.Id.Equals(Guid.Parse(userId)))
            .OrderByDescending(s => s.CreationDate)
            .ToListAsync();

        return Ok(sleeps);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserSleepById(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized(new { Message = "Не авторизован" });
        }

        var sleep = await context.Sleeps
            .Include(s => s.User)
            .Where(s => s.User.Id.Equals(Guid.Parse(userId)))
            .FirstOrDefaultAsync(s => s.Id.Equals(id));

        if (sleep == null)
        {
            return NotFound();
        }

        return Ok(sleep);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddUserSleep([FromBody] AddSleepModel addSleepModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized(new { Message = "Не авторизован" });
        }

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id.Equals(Guid.Parse(userId)));
        
        if (user == null)
        {
            return Unauthorized(new { Message = "Не авторизован" });
        }
        
        var existingSleep = await context.Sleeps
            .Include(s => s.User)
            .Where(s => s.User.Id.Equals(Guid.Parse(userId)))
            .FirstOrDefaultAsync(s =>
                s.CreationDate.Day == DateTime.Today.Day && s.CreationDate.Month == DateTime.Today.Month &&
                s.CreationDate.Year == DateTime.Today.Year);

        if (existingSleep == null)
        {
            var sleep = new Sleep
            {
                User = user,
                Hours = addSleepModel.Hours
            };

            context.Sleeps.Add(sleep);
            await context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetUserSleepById),
                new { id = sleep.Id },
                sleep
            );
        }

        existingSleep.Hours += addSleepModel.Hours;
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetUserSleepById),
            new { id = existingSleep.Id },
            existingSleep
        );
    }

    [HttpPatch("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateUserSleepById(Guid id, UpdateSleepModel updateSleepModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized(new { Message = "Не авторизован" });
        }

        var existingSleep = await context.Sleeps
            .Include(s => s.User)
            .Where(s => s.User.Id.Equals(Guid.Parse(userId)))
            .FirstOrDefaultAsync(s => s.Id.Equals(id));

        if (existingSleep == null)
        {
            return NotFound();
        }

        existingSleep.Hours = updateSleepModel.Hours ?? existingSleep.Hours;

        await context.SaveChangesAsync();

        return Ok(existingSleep);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteUserSleepById(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized(new { Message = "Не авторизован" });
        }

        var existingSleep = await context.Sleeps
            .Include(s => s.User)
            .Where(s => s.User.Id.Equals(Guid.Parse(userId)))
            .FirstOrDefaultAsync(s => s.Id.Equals(id));

        if (existingSleep == null)
        {
            return NotFound();
        }

        context.Sleeps.Remove(existingSleep);
        await context.SaveChangesAsync();

        return NoContent();
    }
}