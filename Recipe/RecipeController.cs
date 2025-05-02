using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Recipe;

[ApiController]
[Route("api/v1/[controller]")]
[EnableCors("AllowAngular")]
public class RecipeController(AppDbContext context) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetRecipes()
    {
        var recipes = await context.Recipes.OrderByDescending(r => r.CreationDate).ToListAsync();
        return Ok(recipes);
    }

    [HttpGet("{recipeId:guid}")]
    public async Task<IActionResult> GetRecipeById(Guid recipeId)
    {
        var recipe = await context.Recipes.FirstOrDefaultAsync(r => r.Id.Equals(recipeId));

        if (recipe == null)
        {
            return NotFound();
        }

        return Ok(recipe);
    }

    [HttpPost]
    public async Task<IActionResult> AddRecipe([FromBody] AddRecipeModel addRecipeModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var recipe = new Recipe
        {
            Title = addRecipeModel.Title,
            Description = addRecipeModel.Description,
            ImageUrl = addRecipeModel.ImageUrl
        };

        context.Recipes.Add(recipe);
        await context.SaveChangesAsync();
        
        return CreatedAtAction(
            nameof(GetRecipeById),
            new { id = recipe.Id },
            recipe
        );
    }

    [HttpPatch("{recipeId:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateRecipeById(Guid recipeId, [FromBody] UpdateRecipeModel updateRecipeModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingRecipe = await context.Recipes.FirstOrDefaultAsync(r => r.Id.Equals(recipeId));

        if (existingRecipe == null)
        {
            return NotFound();
        }

        existingRecipe.Title = updateRecipeModel.Title ?? existingRecipe.Title;
        existingRecipe.Description = updateRecipeModel.Description ?? existingRecipe.Description;
        existingRecipe.ImageUrl = updateRecipeModel.ImageUrl ?? existingRecipe.ImageUrl;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{recipeId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteRecipeById(Guid recipeId)
    {
        var existingRecipe = await context.Recipes.FirstOrDefaultAsync(r => r.Id.Equals(recipeId));

        if (existingRecipe == null)
        {
            return NotFound();
        }

        context.Recipes.Remove(existingRecipe);
        await context.SaveChangesAsync();

        return NoContent();
    }
}