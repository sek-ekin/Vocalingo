using System.ComponentModel.DataAnnotations;

namespace vocalingo_api.DTOs
{
    public class TranslateRequest
    {
        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
