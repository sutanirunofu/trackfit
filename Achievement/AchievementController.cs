using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Achievement;

[ApiController]
[Route("api/v1/[controller]")]
[EnableCors("AllowAngular")]
public class AchievementController(AppDbContext context): ControllerBase
{
    [HttpGet]
    public IActionResult GetAchievements()
    {
        return Ok(context.Achievements.ToList());
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetAchievementById(Guid id)
    {
        var a = context.Achievements.Find(id);

        if (a == null)
        {
            return NotFound();
        }

        return Ok(a);
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateAchievement([FromBody] CreateAchievementModel createAchievementModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var achievementWithSameSymbol =
            context.Achievements.FirstOrDefault(a => a.Symbol.Equals(createAchievementModel.Symbol));

        if (achievementWithSameSymbol != null)
        {
            return BadRequest(new { Message = $"Достижение с ключевым словом '{createAchievementModel.Symbol}' уже существует" });
        }

        var a = new Achievement
        {
            Symbol = createAchievementModel.Symbol,
            Name = createAchievementModel.Name,
            Description = createAchievementModel.Description,
            ImageUrl = createAchievementModel.ImageUrl
        };

        context.Achievements.Add(a);
        context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    [Authorize]
    public IActionResult UpdateAchievementById(Guid id, UpdateAchievementModel updateAchievementModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingAchievement = context.Achievements.Find(id);

        if (existingAchievement == null)
        {
            return NotFound(new {Message = $"Достижение с id '{id}' не найдено"});
        }

        if (updateAchievementModel.Symbol != null)
        {
            var achievementWithSameSymbol =
                context.Achievements.FirstOrDefault(a => a.Symbol.Equals(updateAchievementModel.Symbol));

            if (achievementWithSameSymbol != null)
            {
                return BadRequest(new { Message = $"Достижение с ключевым словом '{updateAchievementModel.Symbol}' уже существует" });
            }

            existingAchievement.Symbol = updateAchievementModel.Symbol;
        }

        existingAchievement.Name = updateAchievementModel.Name ?? existingAchievement.Name;
        existingAchievement.Description = updateAchievementModel.Description ?? existingAchievement.Description;
        existingAchievement.ImageUrl = updateAchievementModel.ImageUrl ?? existingAchievement.ImageUrl;

        context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public IActionResult DeleteAchievementById(Guid id)
    {
        var existingAchievement = context.Achievements.Find(id);

        if (existingAchievement == null)
        {
            return NotFound(new {Message = $"Достижение с id '{id}' не найдено"});
        }

        context.Achievements.Remove(existingAchievement);
        context.SaveChanges();
        
        return NoContent();
    }
}