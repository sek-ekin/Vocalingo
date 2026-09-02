using System.ComponentModel.DataAnnotations;

namespace vocalingo_api.DTOs;

public class UpdateWordRequest
{
    [Required]
    [MaxLength(500)]
    public string EnglishText { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string TurkishText { get; set; } = string.Empty;
}