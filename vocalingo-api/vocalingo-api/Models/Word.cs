namespace vocalingo_api.Models;

public class Word
{
    public int Id { get; set; }
    public string EnglishText { get; set; } = string.Empty;
    public string TurkishText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} //Bu sadece veri taşıyıcı. Veritabanına bağlanmaz; Service okur/yazar.


