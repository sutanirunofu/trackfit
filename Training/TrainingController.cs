using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Training;

[ApiController]
[Route("api/v1/[controller]")]
[EnableCors("AllowAngular")]
public class TrainingController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTrainings()
    {
        var trainings = await context.Trainings
            .Include(t => t.Author)
            .ToListAsync();
        return Ok(trainings);
    }

    [HttpGet("{trainingId:guid}")]
    public async Task<IActionResult> GetTrainingById(Guid trainingId)
    {
        var training = await context.Trainings
            .Include(t => t.Author)
            .FirstOrDefaultAsync(t => t.Id.Equals(trainingId));

        if (training == null)
        {
            return NotFound();
        }

        return Ok(training);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateTraining([FromBody] CreateTrainingModel createTrainingModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id.Equals(Guid.Parse(userId)));

        if (user == null)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var training = new Training
        {
            Author = user,
            Title = createTrainingModel.Title,
            Body = createTrainingModel.Body,
            PreviewUrl = createTrainingModel.PreviewUrl,
            VideoUrl = createTrainingModel.VideoUrl
        };

        context.Trainings.Add(training);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTrainingById),
            new { id = training.Id },
            training
        );
    }

    [HttpPatch("{trainingId:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateTrainingById(Guid trainingId,
        [FromBody] UpdateTrainingModel updateTrainingModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var existingTraining =
            await context.Trainings.Include(t => t.Author).FirstOrDefaultAsync(t => t.Id.Equals(trainingId));

        if (existingTraining == null)
        {
            return NotFound();
        }

        if (!existingTraining.Author.Id.Equals(Guid.Parse(userId)))
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        existingTraining.Title = updateTrainingModel.Title ?? existingTraining.Title;
        existingTraining.Body = updateTrainingModel.Body ?? existingTraining.Body;
        existingTraining.PreviewUrl = updateTrainingModel.PreviewUrl ?? existingTraining.PreviewUrl;
        existingTraining.VideoUrl = updateTrainingModel.VideoUrl ?? existingTraining.VideoUrl;
        existingTraining.ModificationDate = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{trainingId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteTrainingById(Guid trainingId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var existingTraining = await context.Trainings
            .Include(t => t.Author)
            .FirstOrDefaultAsync(t => t.Id.Equals(trainingId));

        if (existingTraining == null)
        {
            return NotFound();
        }

        if (!existingTraining.Author.Id.Equals(Guid.Parse(userId)))
        {
            return Forbid();
        }

        context.Trainings.Remove(existingTraining);
        await context.SaveChangesAsync();

        return NoContent();
    }
}