using Microsoft.Data.SqlClient;

string connectionString = "Server=localhost;Database=coffee_shop;Trusted_Connection=True;TrustServerCertificate=True;";

async Task<int> GetAvailableProductCountAsync(string cs)
{
    await using var connection = new SqlConnection(cs);
    await connection.OpenAsync();
    using var command = new SqlCommand("SELECT COUNT(*) FROM products WHERE is_available = 1", connection);
    return (int)(await command.ExecuteScalarAsync())!;
}

async Task SearchProductByIdAsync(string cs, int productId)
{
    await using var connection = new SqlConnection(cs);
    await connection.OpenAsync();
    using var command = new SqlCommand("SELECT id, name, price, is_available FROM products WHERE id = @id", connection);
    command.Parameters.AddWithValue("@id", productId);
    await using var reader = await command.ExecuteReaderAsync();
    if (await reader.ReadAsync())
    {
        int id = reader.GetInt32(0);
        string name = reader.GetString(1);
        decimal price = reader.GetDecimal(2);
        bool isAvailable = reader.GetBoolean(3);
        Console.WriteLine($"{id}. {name} - {price} руб. | Доступен: {(isAvailable ? "Да" : "Нет")}");
    }
    else
    {
        Console.WriteLine("Товар не найден.");
    }
}

try
{
    int availableCount = await GetAvailableProductCountAsync(connectionString);
    Console.WriteLine($"Количество доступных товаров: {availableCount}");

    Console.Write("Введите ID товара для поиска: ");
    if (int.TryParse(Console.ReadLine(), out int productId))
    {
        await SearchProductByIdAsync(connectionString, productId);
    }
    else
    {
        Console.WriteLine("Некорректный ID.");
    }
}
catch (SqlException ex)
{
    Console.WriteLine($"Ошибка SQL Server: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}