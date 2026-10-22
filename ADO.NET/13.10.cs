using Microsoft.Data.SqlClient;

string connectionString = "Server=localhost;Database=coffee_shop;Trusted_Connection=True;TrustServerCertificate=True;";

using var connection = new SqlConnection(connectionString);
connection.Open();

void ShowCustomers()
{
    using var command = new SqlCommand("SELECT id, full_name FROM customers;", connection);
    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"{reader.GetInt32(0)}: {reader.GetString(1)}");
    }
}

void ShowProducts()
{
    using var command = new SqlCommand("SELECT id, name, price FROM products;", connection);
    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"{reader.GetInt32(0)}: {reader.GetString(1)} - {reader.GetDecimal(2)} руб.");
    }
}

void SimpleTransaction()
{
    using var transaction = connection.BeginTransaction();
    try
    {
        using var command1 = new SqlCommand("UPDATE products SET price = @price WHERE id = @id;", connection, transaction);
        command1.Parameters.AddWithValue("@price", 999.00m);
        command1.Parameters.AddWithValue("@id", 1);
        int rows1 = command1.ExecuteNonQuery();

        using var command2 = new SqlCommand("UPDATE products SET price = @price WHERE id = @id;", connection, transaction);
        command2.Parameters.AddWithValue("@price", 999.00m);
        command2.Parameters.AddWithValue("@id", 2);
        int rows2 = command2.ExecuteNonQuery();

        if (rows1 == 1 && rows2 == 1)
        {
            transaction.Commit();
            Console.WriteLine("Транзакция подтверждена.");
        }
        else
        {
            transaction.Rollback();
            Console.WriteLine("Транзакция отменена: не все товары найдены.");
        }
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}

void RollbackTest()
{
    using var transaction = connection.BeginTransaction();
    try
    {
        using var command1 = new SqlCommand("UPDATE products SET price = @price WHERE id = @id;", connection, transaction);
        command1.Parameters.AddWithValue("@price", 888.00m);
        command1.Parameters.AddWithValue("@id", 1);
        command1.ExecuteNonQuery();

        throw new InvalidOperationException("Тестовая ошибка");
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        Console.WriteLine($"Откат выполнен: {ex.Message}");
    }
}

void CreateOrder(int customerId, (int productId, int quantity)[] items)
{
    using var transaction = connection.BeginTransaction();
    try
    {
        using var checkCustomer = new SqlCommand("SELECT COUNT(*) FROM customers WHERE id = @id;", connection, transaction);
        checkCustomer.Parameters.AddWithValue("@id", customerId);
        int customerExists = Convert.ToInt32(checkCustomer.ExecuteScalar());
        if (customerExists == 0)
        {
            throw new InvalidOperationException("Клиент не найден.");
        }

        using var insertOrder = new SqlCommand(
            "INSERT INTO orders (customer_id, order_date, status) VALUES (@customer_id, SYSDATETIME(), N'Новый'); SELECT CAST(SCOPE_IDENTITY() AS int);",
            connection, transaction);
        insertOrder.Parameters.AddWithValue("@customer_id", customerId);
        int orderId = Convert.ToInt32(insertOrder.ExecuteScalar());

        decimal total = 0;

        foreach (var item in items)
        {
            using var getPrice = new SqlCommand("SELECT price FROM products WHERE id = @id;", connection, transaction);
            getPrice.Parameters.AddWithValue("@id", item.productId);
            object? priceObj = getPrice.ExecuteScalar();
            if (priceObj == null)
            {
                throw new InvalidOperationException($"Товар {item.productId} не найден.");
            }
            decimal unitPrice = Convert.ToDecimal(priceObj);

            using var insertItem = new SqlCommand(
                "INSERT INTO order_items (order_id, product_id, quantity, unit_price) VALUES (@order_id, @product_id, @quantity, @unit_price);",
                connection, transaction);
            insertItem.Parameters.AddWithValue("@order_id", orderId);
            insertItem.Parameters.AddWithValue("@product_id", item.productId);
            insertItem.Parameters.AddWithValue("@quantity", item.quantity);
            insertItem.Parameters.AddWithValue("@unit_price", unitPrice);
            insertItem.ExecuteNonQuery();

            total += unitPrice * item.quantity;
        }

        transaction.Commit();
        Console.WriteLine($"Заказ №{orderId} создан. Итоговая сумма: {total} руб.");
    }
    catch (Exception ex)
    {
        try
        {
            transaction.Rollback();
            Console.WriteLine($"Транзакция отменена: {ex.Message}");
        }
        catch (Exception rollbackEx)
        {
            Console.WriteLine($"Ошибка отката: {rollbackEx.Message}");
        }
    }
}

void CancelOrder(int orderId)
{
    using var transaction = connection.BeginTransaction();
    try
    {
        using var deleteItems = new SqlCommand("DELETE FROM order_items WHERE order_id = @order_id;", connection, transaction);
        deleteItems.Parameters.AddWithValue("@order_id", orderId);
        deleteItems.ExecuteNonQuery();

        using var deleteOrder = new SqlCommand("DELETE FROM orders WHERE id = @id;", connection, transaction);
        deleteOrder.Parameters.AddWithValue("@id", orderId);
        int rows = deleteOrder.ExecuteNonQuery();

        if (rows == 1)
        {
            transaction.Commit();
            Console.WriteLine("Заказ отменён.");
        }
        else
        {
            transaction.Rollback();
            Console.WriteLine("Заказ не найден.");
        }
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        Console.WriteLine($"Ошибка отмены: {ex.Message}");
    }
}

while (true)
{
    Console.WriteLine("МЕНЮ");
    Console.WriteLine("1. Простая транзакция (изменение двух цен)");
    Console.WriteLine("2. Проверка отката");
    Console.WriteLine("3. Список клиентов");
    Console.WriteLine("4. Список товаров");
    Console.WriteLine("5. Оформить заказ (одна позиция)");
    Console.WriteLine("6. Оформить заказ (две позиции)");
    Console.WriteLine("7. Отменить заказ");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите операцию: ");
    string choice = Console.ReadLine()!;

    switch (choice)
    {
        case "1":
        {
            SimpleTransaction();
            break;
        }
        case "2":
        {
            RollbackTest();
            break;
        }
        case "3":
        {
            ShowCustomers();
            break;
        }
        case "4":
        {
            ShowProducts();
            break;
        }
        case "5":
        {
            ShowCustomers();
            Console.Write("ID клиента: ");
            int customerId = Convert.ToInt32(Console.ReadLine());
            ShowProducts();
            Console.Write("ID товара: ");
            int productId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Количество: ");
            int quantity = Convert.ToInt32(Console.ReadLine());
            CreateOrder(customerId, new[] { (productId, quantity) });
            break;
        }
        case "6":
        {
            ShowCustomers();
            Console.Write("ID клиента: ");
            int customerId = Convert.ToInt32(Console.ReadLine());
            ShowProducts();
            Console.Write("ID первого товара: ");
            int productId1 = Convert.ToInt32(Console.ReadLine());
            Console.Write("Количество первого товара: ");
            int quantity1 = Convert.ToInt32(Console.ReadLine());
            Console.Write("ID второго товара: ");
            int productId2 = Convert.ToInt32(Console.ReadLine());
            Console.Write("Количество второго товара: ");
            int quantity2 = Convert.ToInt32(Console.ReadLine());
            CreateOrder(customerId, new[] { (productId1, quantity1), (productId2, quantity2) });
            break;
        }
        case "7":
        {
            Console.Write("ID заказа: ");
            int orderId = Convert.ToInt32(Console.ReadLine());
            CancelOrder(orderId);
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