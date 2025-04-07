using System.Security.Claims;
using Fitness.Goal.GoalType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.User;

[ApiController]
[Route("api/v1/[controller]")]
[EnableCors("AllowAngular")]
public class UserController(AppDbContext context) : ControllerBase
{

    [Authorize]
    [HttpPatch]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserModel updateUserModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();

        var user = await context.Users
            .Include(u => u.Goal)
            .Include(u => u.Goal.Type)
            .FirstOrDefaultAsync(u => u.Id.Equals(Guid.Parse(userId)));

        if (user == null)
            return NotFound();
        
        var userGoal = await context.Goals
            .Include(g => g.Type)
            .FirstOrDefaultAsync(g => g.Id.Equals(user.GoalId));

        if (userGoal == null)
            return StatusCode(StatusCodes.Status500InternalServerError, "Что-то пошло не так");

        GoalType? userGoalType = null;

        if (updateUserModel.GoalType != null)
        {
            userGoalType = await context.GoalTypes.FirstOrDefaultAsync(gt => gt.Name.Equals(updateUserModel.GoalType));

            if (userGoalType == null)
                return BadRequest("Типа цели `" + updateUserModel.GoalType + "` не существует");
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        user.Username = updateUserModel.Username ?? user.Username;
        user.FirstName = updateUserModel.FirstName ?? user.FirstName;
        user.Birthday = updateUserModel.Birthday?.ToUniversalTime().AddDays(1).AddHours(-21) ?? user.Birthday;
        user.Height = updateUserModel.Height ?? user.Height;
        user.Weight = updateUserModel.Weight ?? user.Weight;
        user.ModificationDate = DateTime.UtcNow;

        userGoal.Type = userGoalType ?? userGoal.Type;
        userGoal.Weight = updateUserModel.GoalWeight ?? userGoal.Weight;
        
        await context.SaveChangesAsync();

        return Ok(user);
    }
}