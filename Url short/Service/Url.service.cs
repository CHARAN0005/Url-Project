using System.Reflection.Metadata;
using Npgsql;
using Url;

public class UrlService : IUrl
{
    private readonly string _connection;

    public UrlService(IConfiguration configuration)
    {
        _connection = configuration.GetConnectionString("AuthDb")
            ?? throw new InvalidOperationException(
                "Connection string 'Db' was not found.");
    }
    public async Task<UrlRes> createUrl(string url)
    {
        var shortCode = GenerateShortCode(url);

        await using var connection = new NpgsqlConnection(_connection);
        await connection.OpenAsync();

        var sql = """
        INSERT INTO URL (URL, SHORTURL)
        VALUES (@URL, @SHORTURL)
        RETURNING *;
        """;

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("URL", url);
        command.Parameters.AddWithValue("SHORTURL", shortCode);

        await using var read = await command.ExecuteReaderAsync();

        if (await read.ReadAsync())
        {
            return new UrlRes
            {
                id = read.GetInt32(read.GetOrdinal("id")),
                Url = read.GetString(read.GetOrdinal("url")),
                shortCode = read.GetString(read.GetOrdinal("shorturl")),
                createdAt = read.GetDateTime(read.GetOrdinal("createdAt")),
                updatedAt = read.GetDateTime(read.GetOrdinal("updatedAt"))
            };
        }

        throw new Exception("URL creation failed");
    }
    public async Task<UrlRes> getUrl(string shorturl)
    {
        await using var connection = new NpgsqlConnection(_connection);
        await connection.OpenAsync();

        var sql = """
        UPDATE URL SET VISIT_COUNT = VISIT_COUNT + 1 WHERE SHORTURL=@SHORTCODE
        RETURNING *;

        """;

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("SHORTCODE", shorturl);

        await using var read = await command.ExecuteReaderAsync();

        if (await read.ReadAsync())
        {
            return new UrlRes
            {
                id = read.GetInt32(read.GetOrdinal("id")),
                Url = read.GetString(read.GetOrdinal("url")),
                shortCode = read.GetString(read.GetOrdinal("shorturl")),
                createdAt = read.GetDateTime(read.GetOrdinal("createdAt")),
                updatedAt = read.GetDateTime(read.GetOrdinal("updatedAt"))
            };
        }

        throw new Exception("URL creation failed");
    }


    public async Task<UrlStatsRes> getStats(string shorturl)
    {
        await using var connection = new NpgsqlConnection(_connection);
        await connection.OpenAsync();

        var sql = """
        SELECT * FROM URL WHERE SHORTURL=@SHORTCODE;
        """;

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("SHORTCODE", shorturl);

        await using var read = await command.ExecuteReaderAsync();

        if (await read.ReadAsync())
        {
            return new UrlStatsRes
            {
                id = read.GetInt32(read.GetOrdinal("id")),
                Url = read.GetString(read.GetOrdinal("url")),
                shortCode = read.GetString(read.GetOrdinal("shorturl")),
                createdAt = read.GetDateTime(read.GetOrdinal("createdAt")),
                updatedAt = read.GetDateTime(read.GetOrdinal("updatedAt")),
                accessCount = read.GetInt32(read.GetOrdinal("visit_count"))
            };
        }

        throw new Exception("URL creation failed");
    }


    public async Task<UrlRes> updateUrl(string shorturl, string url)
    {
        await using var connection = new NpgsqlConnection(_connection);
        await connection.OpenAsync();

        var sql = """
        UPDATE URL SET VISIT_COUNT = VISIT_COUNT + 1 , URL = @URLCODE , UPDATEDAT=@CURRENTDATE WHERE SHORTURL=@SHORTCODE
        RETURNING *;

        """;

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("SHORTCODE", shorturl);
        command.Parameters.AddWithValue("URLCODE", url);
        command.Parameters.AddWithValue("CURRENTDATE", DateTime.UtcNow);

        await using var read = await command.ExecuteReaderAsync();

        if (await read.ReadAsync())
        {
            return new UrlRes
            {
                id = read.GetInt32(read.GetOrdinal("id")),
                Url = read.GetString(read.GetOrdinal("url")),
                shortCode = read.GetString(read.GetOrdinal("shorturl")),
                createdAt = read.GetDateTime(read.GetOrdinal("createdAt")),
                updatedAt = read.GetDateTime(read.GetOrdinal("updatedAt"))
            };
        }

        throw new Exception("URL creation failed");
    }

    public async Task<bool> deleteUrl(string shorturl)
    {
        await using var connection = new NpgsqlConnection(_connection);
        await connection.OpenAsync();

        var sql = """
        DELETE FROM URL WHERE SHORTURL=@SHORTCODE;

        """;

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("SHORTCODE", shorturl);

        int rows = command.ExecuteNonQuery();

        return rows > 0;

    }


    public string GenerateShortCode(string url)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(
            Enumerable.Range(0, 7)
            .Select(_ => chars[Random.Shared.Next(chars.Length)])
            .ToArray()
        );
    }
}