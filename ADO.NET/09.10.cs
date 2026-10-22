using Microsoft.Data.SqlClient;

string connectionString = "Server=localhost;Database=coffee_shop;Trusted_Connection=True;TrustServerCertificate=True;";

using var connection = new SqlConnection(connectionString);
connection.Open();

while (true)
{
    Console.WriteLine("МЕНЮ");
    Console.WriteLine("1. Список товаров");
    Console.WriteLine("2. Поиск товара по ID");
    Console.WriteLine("3. Поиск товаров по названию");
    Console.WriteLine("4. Список клиентов");
    Console.WriteLine("5. Список заказов");
    Console.WriteLine("6. Доступные товары по цене");
    Console.WriteLine("7. Товары дороже указанной цены");
    Console.WriteLine("8. Поиск клиента по Ф.И.О.");
    Console.WriteLine("9. Заказы по статусу");
    Console.WriteLine("10. Заказы с именами клиентов");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите операцию: ");
    string choice = Console.ReadLine()!;

    switch (choice)
    {
        case "1":
        {
            using var command = new SqlCommand("SELECT id, name, price, is_available FROM products;", connection);
            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Товары не найдены.");
                break;
            }
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal price = reader.GetDecimal(2);
                bool isAvailable = reader.GetBoolean(3);
                Console.WriteLine($"{id}. {name}");
                Console.WriteLine($"Цена: {price} руб.");
                Console.WriteLine($"Доступен: {(isAvailable ? "Да" : "Нет")}");
                Console.WriteLine();
            }
            break;
        }
        case "2":
        {
            Console.Write("Введите ID товара: ");
            int productId = Convert.ToInt32(Console.ReadLine());
            using var command = new SqlCommand("SELECT id, name, price, is_available FROM products WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@id", productId);
            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Товар не найден.");
                break;
            }
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal price = reader.GetDecimal(2);
                bool isAvailable = reader.GetBoolean(3);
                Console.WriteLine($"{id}. {name}");
                Console.WriteLine($"Цена: {price} руб.");
                Console.WriteLine($"Доступен: {(isAvailable ? "Да" : "Нет")}");
            }
            break;
        }
        case "3":
        {
            Console.Write("Введите часть названия: ");
            string search = Console.ReadLine()!;
            using var command = new SqlCommand("SELECT id, name, price FROM products WHERE name LIKE @search;", connection);
            command.Parameters.AddWithValue("@search", $"%{search}%");
            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Товары не найдены.");
                break;
            }
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal price = reader.GetDecimal(2);
                Console.WriteLine($"{id}. {name} - {price} руб.");
            }
            break;
        }
        case "4":
        {
            using var command = new SqlCommand("SELECT id, full_name, phone, email FROM customers;", connection);
            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Клиенты не найдены.");
                break;
            }
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string fullName = reader.GetString(1);
                string phone = reader.IsDBNull(2) ? "Не указан" : reader.GetString(2);
                string email = reader.IsDBNull(3) ? "Не указан" : reader.GetString(3);
                Console.WriteLine($"{id}. {fullName}, тел: {phone}, email: {email}");
            }
            break;
        }
        case "5":
        {
            using var command = new SqlCommand("SELECT id, customer_id, order_date, status FROM orders;", connection);
            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Заказы не найдены.");
                break;
            }
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int customerId = reader.GetInt32(1);
                DateTime orderDate = reader.GetDateTime(2);
                string status = reader.GetString(3);
                Console.WriteLine($"Заказ {id}, клиент {customerId}, {orderDate:dd.MM.yyyy HH:mm}, статус: {status}");
            }
            break;
        }
        case "6":
        {
            using var command = new SqlCommand("SELECT id, name, price FROM products WHERE is_available = 1 ORDER BY price ASC;", connection);
            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Товары не найдены.");
                break;
            }
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal price = reader.GetDecimal(2);
                Console.WriteLine($"{id}. {name} - {price} руб.");
            }
            break;
        }
        case "7":
        {
            Console.Write("Введите минимальную цену: ");
            decimal minPrice = Convert.ToDecimal(Console.ReadLine());
            using var command = new SqlCommand("SELECT id, name, price FROM products WHERE price > @min_price;", connection);
            command.Parameters.AddWithValue("@min_price", minPrice);
            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Товары не найдены.");
                break;
            }
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal price = reader.GetDecimal(2);
                Console.WriteLine($"{id}. {name} - {price} руб.");
            }
            break;
        }
        case "8":
        {
            Console.Write("Введите часть Ф.И.О.: ");
            string search = Console.ReadLine()!;
            using var command = new SqlCommand("SELECT id, full_name, phone, email FROM customers WHERE full_name LIKE @search;", connection);
            command.Parameters.AddWithValue("@search", $"%{search}%");
            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Клиенты не найдены.");
                break;
            }
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string fullName = reader.GetString(1);
                string phone = reader.IsDBNull(2) ? "Не указан" : reader.GetString(2);
                string email = reader.IsDBNull(3) ? "Не указан" : reader.GetString(3);
                Console.WriteLine($"{id}. {fullName}, тел: {phone}, email: {email}");
            }
            break;
        }
        case "9":
        {
            Console.Write("Введите статус заказа: ");
            string status = Console.ReadLine()!;
            using var command = new SqlCommand("SELECT id, customer_id, order_date, status FROM orders WHERE status = @status;", connection);
            command.Parameters.AddWithValue("@status", status);
            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Заказы не найдены.");
                break;
            }
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int customerId = reader.GetInt32(1);
                DateTime orderDate = reader.GetDateTime(2);
                string orderStatus = reader.GetString(3);
                Console.WriteLine($"Заказ {id}, клиент {customerId}, {orderDate:dd.MM.yyyy HH:mm}, статус: {orderStatus}");
            }
            break;
        }
        case "10":
        {
            string sql = @"SELECT o.id, c.full_name, o.order_date, o.status
                           FROM orders o
                           JOIN customers c ON o.customer_id = c.id;";
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Заказы не найдены.");
                break;
            }
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string fullName = reader.GetString(1);
                DateTime orderDate = reader.GetDateTime(2);
                string status = reader.GetString(3);
                Console.WriteLine($"Заказ {id}, клиент: {fullName}, {orderDate:dd.MM.yyyy HH:mm}, статус: {status}");
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