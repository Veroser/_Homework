using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

string connectionString = configuration.GetConnectionString("CoffeeShop")
    ?? throw new InvalidOperationException("Строка подключения не найдена.");

string appTitle = configuration["Application:Title"] ?? "Приложение";
bool showPrices = bool.TryParse(configuration["Application:ShowPrices"], out bool sp) && sp;

async Task CheckConnectionAsync(string cs)
{
    await using var connection = new SqlConnection(cs);
    await connection.OpenAsync();
    using var command = new SqlCommand("SELECT DB_NAME()", connection);
    string databaseName = (string)(await command.ExecuteScalarAsync())!;
    Console.WriteLine($"Подключено к базе: {databaseName}");
}

async Task ShowStatisticsAsync(string cs)
{
    await using var connection = new SqlConnection(cs);
    await connection.OpenAsync();

    using var cmd1 = new SqlCommand("SELECT COUNT(*) FROM products", connection);
    int products = (int)(await cmd1.ExecuteScalarAsync())!;

    using var cmd2 = new SqlCommand("SELECT COUNT(*) FROM customers", connection);
    int customers = (int)(await cmd2.ExecuteScalarAsync())!;

    using var cmd3 = new SqlCommand("SELECT COUNT(*) FROM orders", connection);
    int orders = (int)(await cmd3.ExecuteScalarAsync())!;

    Console.WriteLine("Статистика кофейни");
    Console.WriteLine($"Товаров: {products}");
    Console.WriteLine($"Клиентов: {customers}");
    Console.WriteLine($"Заказов: {orders}");
}

async Task ShowAvailableProductsAsync(string cs)
{
    await using var connection = new SqlConnection(cs);
    await connection.OpenAsync();
    using var command = new SqlCommand("SELECT id, name, price FROM products WHERE is_available = 1 ORDER BY name;", connection);
    await using var reader = await command.ExecuteReaderAsync();
    if (!await reader.ReadAsync())
    {
        Console.WriteLine("Товары не найдены.");
        return;
    }
    do
    {
        if (showPrices)
            Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)} – {reader.GetDecimal(2)} руб.");
        else
            Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)}");
    } while (await reader.ReadAsync());
}

async Task FindProductsAsync(string cs, string search)
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
        if (showPrices)
            Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)} – {reader.GetDecimal(2)} руб.");
        else
            Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)}");
    } while (await reader.ReadAsync());
}

void ShowAppSettings()
{
    Console.WriteLine($"Название приложения: {appTitle}");
    Console.WriteLine($"Показывать цены: {(showPrices ? "Да" : "Нет")}");
}

try
{
    await CheckConnectionAsync(connectionString);
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка подключения: {ex.Message}");
}

while (true)
{
    Console.WriteLine($"=== {appTitle} ===");
    Console.WriteLine("1. Проверить подключение к базе");
    Console.WriteLine("2. Показать статистику");
    Console.WriteLine("3. Вывести список доступных товаров");
    Console.WriteLine("4. Найти товары по части названия");
    Console.WriteLine("5. Показать настройки приложения");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите операцию: ");
    string choice = Console.ReadLine()!;

    try
    {
        switch (choice)
        {
            case "1":
            {
                await CheckConnectionAsync(connectionString);
                break;
            }
            case "2":
            {
                await ShowStatisticsAsync(connectionString);
                break;
            }
            case "3":
            {
                await ShowAvailableProductsAsync(connectionString);
                break;
            }
            case "4":
            {
                Console.Write("Введите часть названия: ");
                string search = Console.ReadLine()!;
                await FindProductsAsync(connectionString, search);
                break;
            }
            case "5":
            {
                ShowAppSettings();
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