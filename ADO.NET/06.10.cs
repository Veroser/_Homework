using Microsoft.Data.SqlClient;

string connectionString = "Server=localhost;Database=coffee_shop;Trusted_Connection=True;TrustServerCertificate=True;";

using var connection = new SqlConnection(connectionString);
connection.Open();

Console.WriteLine("Подключение к базе данных установлено.");

string databaseName;
using (var command = new SqlCommand("SELECT DB_NAME();", connection))
{
    databaseName = Convert.ToString(command.ExecuteScalar());
}
Console.WriteLine($"База данных: {databaseName}");

int productCount;
using (var command = new SqlCommand("SELECT COUNT(*) FROM products;", connection))
{
    productCount = Convert.ToInt32(command.ExecuteScalar());
}
Console.WriteLine($"Количество товаров: {productCount}");

Console.WriteLine("Список товаров:");
using (var command = new SqlCommand("SELECT id, name, price FROM products;", connection))
using (var reader = command.ExecuteReader())
{
    while (reader.Read())
    {
        Console.WriteLine($"{reader["id"]}. {reader["name"]} - {reader["price"]} руб.");
    }
}

int rowsAffected;
using (var command = new SqlCommand("INSERT INTO products (name, category_id, price) VALUES (N'Mokka', 1, 240.00);", connection))
{
    rowsAffected = command.ExecuteNonQuery();
}
Console.WriteLine($"Добавлено строк: {rowsAffected}");

using (var command = new SqlCommand("UPDATE products SET price = 250.00 WHERE name = N'Mokka';", connection))
{
    rowsAffected = command.ExecuteNonQuery();
}
Console.WriteLine($"Изменено строк: {rowsAffected}");

decimal maxPrice;
using (var command = new SqlCommand("SELECT MAX(price) FROM products;", connection))
{
    maxPrice = Convert.ToDecimal(command.ExecuteScalar());
}
Console.WriteLine($"Максимальная цена: {maxPrice} руб.");

int orderCount;
using (var command = new SqlCommand("SELECT COUNT(*) FROM orders;", connection))
{
    orderCount = Convert.ToInt32(command.ExecuteScalar());
}
Console.WriteLine($"Количество заказов: {orderCount}");

using (var command = new SqlCommand("DELETE FROM products WHERE name = N'Mokka';", connection))
{
    rowsAffected = command.ExecuteNonQuery();
}
Console.WriteLine($"Удалено строк: {rowsAffected}");

int customerCount;
using (var command = new SqlCommand("SELECT COUNT(*) FROM customers;", connection))
{
    customerCount = Convert.ToInt32(command.ExecuteScalar());
}
Console.WriteLine($"Количество клиентов: {customerCount}");

int availableProducts;
using (var command = new SqlCommand("SELECT COUNT(*) FROM products WHERE is_available = 1;", connection))
{
    availableProducts = Convert.ToInt32(command.ExecuteScalar());
}
Console.WriteLine($"Количество доступных товаров: {availableProducts}");

decimal minPrice;
using (var command = new SqlCommand("SELECT MIN(price) FROM products;", connection))
{
    minPrice = Convert.ToDecimal(command.ExecuteScalar());
}
Console.WriteLine($"Минимальная цена: {minPrice} руб.");

int issuedOrders;
using (var command = new SqlCommand("SELECT COUNT(*) FROM orders WHERE status = N'Выдан';", connection))
{
    issuedOrders = Convert.ToInt32(command.ExecuteScalar());
}
Console.WriteLine($"Количество выданных заказов: {issuedOrders}");

using (var command = new SqlCommand("INSERT INTO products (name, category_id, price) VALUES (N'Латте', 1, 210.00);", connection))
{
    rowsAffected = command.ExecuteNonQuery();
}
Console.WriteLine($"Добавлено строк: {rowsAffected}");