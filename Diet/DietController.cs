using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Diet;

[ApiController]
[Route("api/v1/[controller]")]
[EnableCors("AllowAngular")]
public class DietController(AppDbContext context) : ControllerBase
{
    // Получить все записи диеты
    [HttpGet]
    public async Task<IActionResult> GetAllDiets()
    {
        var diets = await context.Diets
            .Include(d => d.User)
            .Include(d => d.Product)
            .ToListAsync();
        return Ok(diets);
    }

    // Получить запись диеты по ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDietById(Guid id)
    {
        var diet = await context.Diets
            .Include(d => d.User)
            .Include(d => d.Product)
            .FirstOrDefaultAsync(d => d.Id == id);
        
        if (diet == null)
        {
            return NotFound();
        }
        
        return Ok(diet);
    }

    // Создать новую запись диеты
    [HttpPost]
    public async Task<IActionResult> CreateDiet([FromBody] Diet diet)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        diet.Id = Guid.NewGuid();
        diet.CreationDate = DateTime.UtcNow;
        
        context.Diets.Add(diet);
        await context.SaveChangesAsync();
        
        return CreatedAtAction(
            nameof(GetDietById),
            new { id = diet.Id },
            diet);
    }

    // Обновить запись диеты
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDiet(Guid id, [FromBody] Diet diet)
    {
        if (id != diet.Id)
        {
            return BadRequest("ID в URL не соответствует ID записи диеты");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        context.Entry(diet).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DietExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // Удалить запись диеты
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiet(Guid id)
    {
        var diet = await context.Diets.FindAsync(id);
        if (diet == null)
        {
            return NotFound();
        }

        context.Diets.Remove(diet);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool DietExists(Guid id)
    {
        return context.Diets.Any(e => e.Id == id);
    }
}