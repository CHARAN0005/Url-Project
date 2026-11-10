using Npgsql;
namespace UrlTable;


public class UrlTableService
{
    private readonly string _connection;
    public UrlTableService(IConfiguration configuration)
    {
        _connection = configuration.GetConnectionString("AuthDb")
            ?? throw new InvalidOperationException(
                "Connection string 'Db' was not found.");
    }


     public Task CreateUrlTableAsync() => ExecuteScriptAsync("url.sql");

     private async Task ExecuteScriptAsync(string fileName)
    {
        var sqlFilePath = Path.Combine(
            AppContext.BaseDirectory,
            "Storage",
            fileName);

        var sql = await File.ReadAllTextAsync(sqlFilePath);

        await using var connection = new NpgsqlConnection(_connection);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }

}