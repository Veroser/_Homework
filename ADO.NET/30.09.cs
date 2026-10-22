using Microsoft.Data.SqlClient;

string connectionString = "Server=localhost;Database=CoffeeShop;Trusted_Connection=True;TrustServerCertificate=True;";

using var connection = new SqlConnection(connectionString);
connection.Open();

Console.WriteLine("Информация о базе данных");

using var commandDbName = new SqlCommand("SELECT DB_NAME();", connection);
var databaseName = commandDbName.ExecuteScalar();
Console.WriteLine($"База данных: {databaseName}");

using var commandServerName = new SqlCommand("SELECT @@SERVERNAME;", connection);
var serverName = commandServerName.ExecuteScalar();
Console.WriteLine($"SQL Server: {serverName}");

using var commandCount = new SqlCommand("SELECT COUNT(*) FROM Products;", connection);
var productCount = commandCount.ExecuteScalar();
Console.WriteLine($"Количество товаров: {productCount}");

using var commandMinPrice = new SqlCommand("SELECT MIN(price) FROM Products;", connection);
var minPrice = commandMinPrice.ExecuteScalar();
Console.WriteLine($"Минимальная цена: {minPrice}");

using var commandMaxPrice = new SqlCommand("SELECT MAX(price) FROM Products;", connection);
var maxPrice = commandMaxPrice.ExecuteScalar();
Console.WriteLine($"Максимальная цена: {maxPrice}");