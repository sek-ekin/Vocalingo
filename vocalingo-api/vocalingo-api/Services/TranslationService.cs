using System.Text.Json.Serialization;

namespace vocalingo_api.Services
{
    public class TranslationService
    {
        private readonly HttpClient _httpClient;

        public TranslationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<string> EnglishToTurkishAsync(string text)
        {
            return TranslateAsync(text, "en", "tr");
        }

        public Task<string> TurkishToEnglishAsync(string text)
        {
            return TranslateAsync(text, "tr", "en");
        }

        private async Task<string> TranslateAsync(
            string text,
            string sourceLanguage,
            string targetLanguage)
        {
            var encodedText = Uri.EscapeDataString(text.Trim());

            var url =
                $"get?q={encodedText}&langpair={sourceLanguage}%7C{targetLanguage}";

            var result =
                await _httpClient.GetFromJsonAsync<MyMemoryResponse>(url);

            return result?.ResponseData.TranslatedText
                ?? throw new InvalidOperationException("Çeviri alınamadı.");
        }

        private class MyMemoryResponse
        {
            [JsonPropertyName("responseData")]
            public ResponseData ResponseData { get; set; } = new();
        }

        private class ResponseData
        {
            [JsonPropertyName("translatedText")]
            public string TranslatedText { get; set; } = string.Empty;
        }
    }
}
