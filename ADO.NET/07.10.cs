using Microsoft.Data.SqlClient;

string connectionString = "Server=localhost;Database=coffee_shop;Trusted_Connection=True;TrustServerCertificate=True;";

using var connection = new SqlConnection(connectionString);
connection.Open();

Console.WriteLine("Подключение установлено.");

while (true)
{
    Console.WriteLine("УПРАВЛЕНИЕ ТОВАРАМИ");
    Console.WriteLine("1. Найти товар по ID");
    Console.WriteLine("2. Найти товар по названию");
    Console.WriteLine("3. Добавить товар");
    Console.WriteLine("4. Изменить цену");
    Console.WriteLine("5. Удалить товар");
    Console.WriteLine("6. Найти товары дороже указанной цены");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите операцию: ");
    string choice = Console.ReadLine()!;

    switch (choice)
    {
        case "1":
        {
            Console.Write("Введите ID товара: ");
            int productId = Convert.ToInt32(Console.ReadLine());
            using var command = new SqlCommand("SELECT id, name, price FROM products WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@id", productId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader["id"]}: {reader["name"]} - {reader["price"]} руб.");
            }
            break;
        }
        case "2":
        {
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine()!;
            using var command = new SqlCommand("SELECT id, name, price FROM products WHERE name = @name;", connection);
            command.Parameters.AddWithValue("@name", name);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader["id"]}: {reader["name"]} - {reader["price"]} руб.");
            }
            break;
        }
        case "3":
        {
            Console.Write("Название товара: ");
            string name = Console.ReadLine()!;
            Console.Write("ID категории: ");
            int categoryId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Цена: ");
            decimal price = Convert.ToDecimal(Console.ReadLine());
            using var command = new SqlCommand("INSERT INTO products (name, category_id, price) VALUES (@name, @category_id, @price);", connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@category_id", categoryId);
            command.Parameters.AddWithValue("@price", price);
            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine($"Добавлено строк: {rowsAffected}");
            break;
        }
        case "4":
        {
            Console.Write("Введите ID товара: ");
            int productId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Новая цена: ");
            decimal price = Convert.ToDecimal(Console.ReadLine());
            using var command = new SqlCommand("UPDATE products SET price = @price WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@id", productId);
            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine($"Изменено строк: {rowsAffected}");
            break;
        }
        case "5":
        {
            Console.Write("Введите ID товара: ");
            int productId = Convert.ToInt32(Console.ReadLine());
            using var command = new SqlCommand("DELETE FROM products WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@id", productId);
            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine($"Удалено строк: {rowsAffected}");
            break;
        }
        case "6":
        {
            Console.Write("Введите минимальную цену: ");
            decimal minPrice = Convert.ToDecimal(Console.ReadLine());
            using var command = new SqlCommand("SELECT id, name, price FROM products WHERE price >= @min_price;", connection);
            command.Parameters.AddWithValue("@min_price", minPrice);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader["id"]}: {reader["name"]} - {reader["price"]} руб.");
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