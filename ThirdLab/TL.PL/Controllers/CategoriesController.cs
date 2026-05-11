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
    public async Task<ActionResult<RoomCategoryResponse>> Create([FromBody] CreateRoomCategoryRequest request)
    {
        var id = await _categoryService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteAsync(id);
        return Ok(new { message = "Category deleted successfully." });
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateRoomCategoryRequest request)
    {
        await _categoryService.UpdateAsync(request);
        return Ok(new { message = "Category updated successfully." });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoomCategoryResponse>> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        return Ok(category);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomCategoryResponse>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }
}