using Microsoft.Data.SqlClient;

string connectionString = "Server=localhost;Database=coffee_shop;Trusted_Connection=True;TrustServerCertificate=True;";

async Task<int> GetProductCountAsync(string cs)
{
    await using var connection = new SqlConnection(cs);
    await connection.OpenAsync();
    using var command = new SqlCommand("SELECT COUNT(*) FROM products", connection);
    return (int)(await command.ExecuteScalarAsync())!;
}

async Task ShowProductsAsync(string cs, bool onlyAvailable = false)
{
    await using var connection = new SqlConnection(cs);
    await connection.OpenAsync();
    string sql = onlyAvailable
        ? "SELECT id, name, price, is_available FROM products WHERE is_available = 1 ORDER BY name;"
        : "SELECT id, name, price, is_available FROM products ORDER BY name;";
    using var command = new SqlCommand(sql, connection);
    await using var reader = await command.ExecuteReaderAsync();
    if (!await reader.ReadAsync())
    {
        Console.WriteLine("Товары не найдены.");
        return;
    }
    do
    {
        int id = reader.GetInt32(0);
        string name = reader.GetString(1);
        decimal price = reader.GetDecimal(2);
        bool available = reader.GetBoolean(3);
        Console.WriteLine($"{id}. {name} – {price} руб. | {(available ? "Есть" : "Нет")}");
    } while (await reader.ReadAsync());
}

async Task ShowProductsFromPriceAsync(string cs, decimal minPrice)
{
    await using var connection = new SqlConnection(cs);
    await connection.OpenAsync();
    using var command = new SqlCommand("SELECT id, name, price FROM products WHERE price >= @min_price ORDER BY price;", connection);
    command.Parameters.AddWithValue("@min_price", minPrice);
    await using var reader = await command.ExecuteReaderAsync();
    if (!await reader.ReadAsync())
    {
        Console.WriteLine("Товары не найдены.");
        return;
    }
    do
    {
        Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)} – {reader.GetDecimal(2)} руб.");
    } while (await reader.ReadAsync());
}

async Task ShowProductsBySearchAsync(string cs, string search)
{
    await using var connection = new SqlConnection(cs);
    await connection.OpenAsync();
    using var command = new SqlCommand("SELECT id, name, price FROM products WHERE name LIKE @search ORDER BY name;", connection);
    command.Parameters.AddWithValue("@search", $"%{search}%");
    await using var reader = await command.ExecuteReaderAsync();
    if (!await reader.ReadAsync())
    {
        Console.WriteLine("Товары не найдены.");
        return;
    }
    do
    {
        Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)} – {reader.GetDecimal(2)} руб.");
    } while (await reader.ReadAsync());
}

async Task<int> GetCustomerCountAsync(string cs)
{
    await using var connection = new SqlConnection(cs);
    await connection.OpenAsync();
    using var command = new SqlCommand("SELECT COUNT(*) FROM customers", connection);
    return (int)(await command.ExecuteScalarAsync())!;
}

async Task UpdateProductAvailabilityAsync(string cs, int productId, bool isAvailable)
{
    await using var connection = new SqlConnection(cs);
    await connection.OpenAsync();
    using var command = new SqlCommand("UPDATE products SET is_available = @is_available WHERE id = @id;", connection);
    command.Parameters.AddWithValue("@is_available", isAvailable);
    command.Parameters.AddWithValue("@id", productId);
    int rows = await command.ExecuteNonQueryAsync();
    if (rows == 0)
    {
        Console.WriteLine("Товар не найден.");
    }
    else
    {
        Console.WriteLine($"Изменено строк: {rows}");
    }
}

async Task ShowCustomerOrdersAsync(string cs, int customerId, CancellationToken token = default)
{
    await using var connection = new SqlConnection(cs);
    await connection.OpenAsync(token);
    using var command = new SqlCommand(
        @"SELECT o.id, o.order_date, o.status
          FROM orders o
          JOIN customers c ON o.customer_id = c.id
          WHERE c.id = @customer_id
          ORDER BY o.order_date DESC;", connection);
    command.Parameters.AddWithValue("@customer_id", customerId);
    await using var reader = await command.ExecuteReaderAsync(token);
    if (!await reader.ReadAsync(token))
    {
        Console.WriteLine("Заказы не найдены.");
        return;
    }
    do
    {
        Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetDateTime(1):dd.MM.yyyy HH:mm} – {reader.GetString(2)}");
    } while (await reader.ReadAsync(token));
}

while (true)
{
    Console.WriteLine("=== АСИНХРОННЫЙ КАТАЛОГ КОФЕЙНИ ===");
    Console.WriteLine("1. Показать все товары");
    Console.WriteLine("2. Показать только доступные товары");
    Console.WriteLine("3. Найти товары по части названия");
    Console.WriteLine("4. Показать число клиентов");
    Console.WriteLine("5. Изменить доступность товара");
    Console.WriteLine("6. Найти товары дороже указанной цены");
    Console.WriteLine("7. Показать заказы клиента");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите операцию: ");
    string choice = Console.ReadLine()!;

    try
    {
        switch (choice)
        {
            case "1":
            {
                await ShowProductsAsync(connectionString);
                break;
            }
            case "2":
            {
                await ShowProductsAsync(connectionString, true);
                break;
            }
            case "3":
            {
                Console.Write("Введите часть названия: ");
                string search = Console.ReadLine()!;
                await ShowProductsBySearchAsync(connectionString, search);
                break;
            }
            case "4":
            {
                int count = await GetCustomerCountAsync(connectionString);
                Console.WriteLine($"Количество клиентов: {count}");
                break;
            }
            case "5":
            {
                Console.Write("ID товара: ");
                if (!int.TryParse(Console.ReadLine(), out int productId))
                {
                    Console.WriteLine("Некорректный ID.");
                    break;
                }
                Console.Write("Доступен (1/0): ");
                string availInput = Console.ReadLine()!;
                if (availInput != "0" && availInput != "1")
                {
                    Console.WriteLine("Некорректное значение.");
                    break;
                }
                bool isAvailable = availInput == "1";
                await UpdateProductAvailabilityAsync(connectionString, productId, isAvailable);
                break;
            }
            case "6":
            {
                Console.Write("Введите минимальную цену: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal minPrice))
                {
                    Console.WriteLine("Некорректная цена.");
                    break;
                }
                await ShowProductsFromPriceAsync(connectionString, minPrice);
                break;
            }
            case "7":
            {
                Console.Write("ID клиента: ");
                if (!int.TryParse(Console.ReadLine(), out int customerId))
                {
                    Console.WriteLine("Некорректный ID.");
                    break;
                }
                using var cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(5));
                try
                {
                    await ShowCustomerOrdersAsync(connectionString, customerId, cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Операция отменена.");
                }
                break;
            }
            case "0":
            {
                return;
            }
            default:
            {
                Console.WriteLine("Неверный выбор.");
                break;
            }
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
}