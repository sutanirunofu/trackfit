using System.Security.Claims;
using Fitness.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Auth;

[ApiController]
[Route("api/v1/[controller]")]
[EnableCors("AllowAngular")]
public class AuthController(AppDbContext context, JwtUtil jwtUtil) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await context.Users.AnyAsync(u => u.Username == registerModel.Username))
            return BadRequest(new { Message = "Пользователь с таким именем уже существует" });

        var userGoalType = await context.GoalTypes.FirstOrDefaultAsync(gt => gt.Name == registerModel.GoalType);

        if (userGoalType == null)
            return BadRequest(new { Message = "Некорректный тип цели" });

        // Create and save everything in one transaction
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            // Create user goal
            var userGoal = new Goal.Goal
            {
                Type = userGoalType,
                Weight = registerModel.GoalWeight
            };
            context.Goals.Add(userGoal);

            // Create user
            var user = new User.User
            {
                Username = registerModel.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(registerModel.Password),
                FirstName = registerModel.FirstName,
                Sex = registerModel.Sex,
                Birthday = registerModel.Birthday.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(registerModel.Birthday, DateTimeKind.Utc)
                    : registerModel.Birthday.ToUniversalTime(),
                Height = registerModel.Height,
                Weight = registerModel.Weight,
                Goal = userGoal,
            };
            context.Users.Add(user);
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            var token = jwtUtil.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка при регистрации");
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Некорректные данные" });
        }

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Username.Equals(loginModel.Username));

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password))
            return BadRequest(new { Message = "Некорректные данные" });

        var token = jwtUtil.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    [Authorize]
    [HttpGet("Me")]
    public async Task<IActionResult> GetUserByJwt()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();

        var user = await context.Users
            .Include(u => u.Goal)
            .ThenInclude(g => g.Type)
            .Include(u => u.Diets)
            .ThenInclude(d => d.Product)
            .Include(u => u.WaterDiets)
            .FirstOrDefaultAsync(u => u.Id.Equals(Guid.Parse(userId)));

        if (user == null)
            return Unauthorized();

        return Ok(user);
    }

    [HttpGet("Check")]
    public IActionResult CheckAuth()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value
                   ?? User.FindFirst("role")?.Value;

        return Ok(new
        {
            User.Identity?.IsAuthenticated,
            Username = User.Identity?.Name,
            Role = role,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}