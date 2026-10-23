using Microsoft.Data.SqlClient;

string connectionString = "Server=localhost;Database=coffee_shop;Trusted_Connection=True;TrustServerCertificate=True;";

Console.WriteLine("=== ДОСТУПНЫЕ ТОВАРЫ ===");

using (var connection = new SqlConnection(connectionString))
{
    connection.Open();

    string sql = "SELECT id, name, price, is_available FROM products WHERE is_available = 1 ORDER BY name;";
    using var command = new SqlCommand(sql, connection);
    using var reader = command.ExecuteReader();

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            decimal price = reader.GetDecimal(2);
            bool isAvailable = reader.GetBoolean(3);

            Console.WriteLine($"{id}. {name} - {price} руб. | Доступен: {(isAvailable ? "Да" : "Нет")}");
        }
    }
    else
    {
        Console.WriteLine("Доступные товары не найдены.");
    }
}

Console.WriteLine();
Console.Write("Введите минимальную цену для фильтрации: ");
if (!decimal.TryParse(Console.ReadLine(), out decimal minPrice))
{
    Console.WriteLine("Некорректная цена.");
    return;
}

Console.WriteLine($"=== ТОВАРЫ ДОРОЖЕ {minPrice} РУБ. ===");

using (var connection = new SqlConnection(connectionString))
{
    connection.Open();

    string sql = "SELECT id, name, price FROM products WHERE price > @min_price ORDER BY price;";
    using var command = new SqlCommand(sql, connection);
    command.Parameters.AddWithValue("@min_price", minPrice);
    using var reader = command.ExecuteReader();

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            decimal price = reader.GetDecimal(2);

            Console.WriteLine($"{id}. {name} - {price} руб.");
        }
    }
    else
    {
        Console.WriteLine($"Товары дороже {minPrice} руб. не найдены.");
    }
}