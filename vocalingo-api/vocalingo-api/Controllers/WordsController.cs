using Microsoft.AspNetCore.Mvc;
using vocalingo_api.DTOs;
using vocalingo_api.Services;

namespace vocalingo_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WordsController : ControllerBase
{
    private readonly WordService _wordService;

    public WordsController(WordService wordService)
    {
        _wordService = wordService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var words = await _wordService.GetAllAsync();
        return Ok(words);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var word = await _wordService.GetByIdAsync(id);
        if (word is null)
            return NotFound(new { message = "Kelime bulunamadı." });

        return Ok(word);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateWordRequest request)
    {
        var word = await _wordService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = word.Id }, word);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateWordRequest request)
    {
        var word = await _wordService.UpdateAsync(id, request);
        if (word is null)
            return NotFound(new { message = "Kelime bulunamadı." });

        return Ok(word);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _wordService.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Kelime bulunamadı." });

        return NoContent();
    }
}