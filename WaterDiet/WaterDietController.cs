using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.WaterDiet;

[ApiController]
[Route("/api/v1/[controller]")]
[EnableCors("AllowAngular")]
public class WaterDietController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllWaterDietRecords()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var waterDietsRecords = await context.WaterDiet
            .Where(w => w.User.Id.Equals(Guid.Parse(userId)))
            .ToListAsync();
        
        return Ok(waterDietsRecords);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetWaterDietRecordById(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }
        
        var waterDietsRecord = await context.WaterDiet
            .Where(w => w.User.Id.Equals(Guid.Parse(userId)))
            .FirstOrDefaultAsync(w => w.Id.Equals(id));

        if (waterDietsRecord == null)
        {
            return NotFound();
        }

        return Ok(waterDietsRecord);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWaterDietRecord([FromBody] CreateWaterDietModel createWaterDietModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var existingUser = await context.Users.FindAsync(Guid.Parse(userId));

        if (existingUser == null)
        {
            return Unauthorized();
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Неверные данные" });
        }

        var waterDiet = new WaterDiet
        {
            User = existingUser,
            Count = createWaterDietModel.Count,
        };

        context.WaterDiet.Add(waterDiet);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetWaterDietRecordById),
            new { id = waterDiet.Id },
            waterDiet
        );
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateWaterDietRecordById(Guid id,
        [FromBody] UpdateWaterDietModel updateWaterDietModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(new {Message = "Неверные данные"});
        }

        var existingWaterDietRecord = await context.WaterDiet
            .Where(w => w.User.Id.Equals(Guid.Parse(userId)))
            .FirstOrDefaultAsync(w => w.Id.Equals(id));

        if (existingWaterDietRecord == null)
        {
            return NotFound();
        }

        existingWaterDietRecord.Count = updateWaterDietModel.Count;

        await context.SaveChangesAsync();

        return Ok(existingWaterDietRecord);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteWaterDietRecordById(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }
        
        var existingWaterDietRecord = await context.WaterDiet
            .Where(w => w.User.Id.Equals(Guid.Parse(userId)))
            .FirstOrDefaultAsync(w => w.Id.Equals(id));

        if (existingWaterDietRecord == null)
        {
            return NotFound();
        }

        context.WaterDiet.Remove(existingWaterDietRecord);
        await context.SaveChangesAsync();

        return NoContent();
    }
}