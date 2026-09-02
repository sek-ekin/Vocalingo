using Microsoft.Data.SqlClient;
using vocalingo_api.DTOs;
using vocalingo_api.Models;

namespace vocalingo_api.Services;

public class WordService
{
    private readonly string _connectionString;

    public WordService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "DefaultConnection bulunamadı.");
    }

    // --- SELECT: hepsini getir ---
    public async Task<List<Word>> GetAllAsync()
    {
        const string sql = """
            SELECT Id, EnglishText, TurkishText, CreatedAt, UpdatedAt
            FROM dbo.Words
            ORDER BY CreatedAt DESC;
            """;

        var words = new List<Word>();

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            words.Add(MapWord(reader));
        }

        return words;
    }

    // --- SELECT: tek kayıt ---
    public async Task<Word?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT Id, EnglishText, TurkishText, CreatedAt, UpdatedAt
            FROM dbo.Words
            WHERE Id = @Id;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return MapWord(reader);
    }

    // --- INSERT ---
    public async Task<Word> CreateAsync(CreateWordRequest request)
    {
        const string sql = """
            INSERT INTO dbo.Words (EnglishText, TurkishText, CreatedAt)
            OUTPUT INSERTED.Id, INSERTED.EnglishText, INSERTED.TurkishText,
                   INSERTED.CreatedAt, INSERTED.UpdatedAt
            VALUES (@EnglishText, @TurkishText, @CreatedAt);
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@EnglishText", request.EnglishText.Trim());
        command.Parameters.AddWithValue("@TurkishText", request.TurkishText.Trim());
        command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

        await using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();

        return MapWord(reader);
    }

    // --- UPDATE ---
    public async Task<Word?> UpdateAsync(int id, UpdateWordRequest request)
    {
        const string sql = """
            UPDATE dbo.Words
            SET EnglishText = @EnglishText,
                TurkishText = @TurkishText,
                UpdatedAt = @UpdatedAt
            OUTPUT INSERTED.Id, INSERTED.EnglishText, INSERTED.TurkishText,
                   INSERTED.CreatedAt, INSERTED.UpdatedAt
            WHERE Id = @Id;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@EnglishText", request.EnglishText.Trim());
        command.Parameters.AddWithValue("@TurkishText", request.TurkishText.Trim());
        command.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return MapWord(reader);
    }

    // --- DELETE ---
    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """
            DELETE FROM dbo.Words
            WHERE Id = @Id;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);

        var affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }

    // SQL sonucunu Word nesnesine çevir
    private static Word MapWord(SqlDataReader reader)
    {
        return new Word
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            EnglishText = reader.GetString(reader.GetOrdinal("EnglishText")),
            TurkishText = reader.GetString(reader.GetOrdinal("TurkishText")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                ? null
                : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
        };
    }
}