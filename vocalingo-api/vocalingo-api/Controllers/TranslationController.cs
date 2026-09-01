using Microsoft.AspNetCore.Mvc;
using vocalingo_api.DTOs;
using vocalingo_api.Services;

namespace vocalingo_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TranslationController : ControllerBase
{
    private readonly TranslationService _translationService;

    public TranslationController(
        TranslationService translationService)
    {
        _translationService = translationService;
    }

    [HttpPost("english-to-turkish")]
    public async Task<ActionResult> EnglishToTurkish(
        TranslateRequest request)
    {
        var translation =
            await _translationService.EnglishToTurkishAsync(request.Text);

        return Ok(new
        {
            originalText = request.Text,
            translatedText = translation
        });
    }

    [HttpPost("turkish-to-english")]
    public async Task<ActionResult> TurkishToEnglish(
        TranslateRequest request)
    {
        var translation =
            await _translationService.TurkishToEnglishAsync(request.Text);

        return Ok(new
        {
            originalText = request.Text,
            translatedText = translation
        });
    }
}