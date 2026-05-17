using Microsoft.AspNetCore.Mvc;
using TL.BLL.DTOs;
using TL.BLL.Interfaces;

namespace TL.PL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IRoomCategoryService _categoryService;

    public CategoriesController(IRoomCategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<ActionResult<RoomCategoryResponse>> Create(CreateRoomCategoryRequest request, CancellationToken cancellationToken)
    {
        var id = await _categoryService.CreateAsync(request, cancellationToken);

        var categoryResponse = await _categoryService.GetByIdAsync(id, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, categoryResponse);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _categoryService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPatch]
    public async Task<IActionResult> Update(UpdateRoomCategoryRequest request, CancellationToken cancellationToken)
    {
        await _categoryService.UpdateAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoomCategoryResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categoryService.GetByIdAsync(id, cancellationToken);

        return Ok(category);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomCategoryResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAllAsync(cancellationToken);
        return Ok(categories);
    }
}