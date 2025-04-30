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
            .Include(u => u.Diets)
            .Include(u => u.WaterDiets)
            .Include(u => u.Products)
            .FirstOrDefaultAsync(u => u.Id.Equals(Guid.Parse(userId)));

        if (user == null)
            return NotFound();
        
        GoalType? userGoalType = null;

        if (updateUserModel.GoalTypeName != null)
        {
            userGoalType = await context.GoalTypes.FirstOrDefaultAsync(gt => gt.Name.Equals(updateUserModel.GoalTypeName));

            if (userGoalType == null)
                return BadRequest(new { Message = "Типа цели `" + updateUserModel.GoalTypeName + "` не существует" });
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        user.Username = updateUserModel.Username ?? user.Username;
        user.FirstName = updateUserModel.FirstName ?? user.FirstName;
        user.Sex = updateUserModel.Sex ?? user.Sex;
        user.Birthday = updateUserModel.Birthday?.ToUniversalTime().AddDays(1).AddHours(-21) ?? user.Birthday;
        user.Height = updateUserModel.Height ?? user.Height;
        user.Weight = updateUserModel.Weight ?? user.Weight;
        user.Avatar = updateUserModel.Avatar ?? user.Avatar;
        user.ModificationDate = DateTime.UtcNow;

        user.Goal.Type = userGoalType ?? user.Goal.Type;
        user.Goal.Weight = updateUserModel.GoalWeight ?? user.Goal.Weight;
        
        await context.SaveChangesAsync();

        return Ok(user);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllUserProducts()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await context.Users
            .Include(u => u.Products)
            .FirstOrDefaultAsync(u => u.Id.Equals(Guid.Parse(userId)));

        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(user.Products);
    }

    [Authorize]
    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetUserProductById(Guid productId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await context.Users
            .Include(u => u.Products)
            .Where(u => u.Id.Equals(Guid.Parse(userId)))
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return Unauthorized();
        }

        var product = user.Products.Find(p => p.Id.Equals(productId));

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    
    [Authorize]
    [HttpPost("Product")]
    public async Task<IActionResult> CreateUserProduct([FromBody] CreateUserProductModel createUserProductModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await context.Users
            .Include(u => u.Goal)
            .ThenInclude(g => g.Type)
            .Include(u => u.Diets)
            .ThenInclude(d => d.Product)
            .Include(u => u.WaterDiets)
            .Include(u => u.Products)
            .FirstOrDefaultAsync(u => u.Id.Equals(Guid.Parse(userId)));

        if (user == null)
        {
            return Unauthorized();
        }

        var product = new Product.Product
        {
            Name = createUserProductModel.Name,
            Calories = createUserProductModel.Calories ?? 0,
            Proteins = createUserProductModel.Proteins ?? 0,
            Fats = createUserProductModel.Fats ?? 0,
            Carbohydrates = createUserProductModel.Carbohydrates ?? 0
        };
        
        user.Products.Add(product);

        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetUserProductById),
            new { id = user.Id },
            user);
    }
}