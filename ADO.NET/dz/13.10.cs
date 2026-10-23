using Microsoft.Data.SqlClient;

string connectionString = "Server=localhost;Database=coffee_shop;Trusted_Connection=True;TrustServerCertificate=True;";

using var connection = new SqlConnection(connectionString);
connection.Open();

int customerId = 1;

var items = new (int productId, int quantity)[]
{
    (1, 2),
    (2, 1)
};

using var transaction = connection.BeginTransaction();

try
{
    string orderSql = @"
        INSERT INTO orders (customer_id, order_date, status)
        VALUES (@customer_id, SYSDATETIME(), N'Новый');
        SELECT CAST(SCOPE_IDENTITY() AS int);";

    using var orderCommand = new SqlCommand(orderSql, connection, transaction);
    orderCommand.Parameters.AddWithValue("@customer_id", customerId);
    int orderId = (int)orderCommand.ExecuteScalar()!;

    decimal total = 0;

    foreach (var (productId, quantity) in items)
    {
        using var priceCommand = new SqlCommand(
            "SELECT price FROM products WHERE id = @id;", connection, transaction);
        priceCommand.Parameters.AddWithValue("@id", productId);
        object? priceObj = priceCommand.ExecuteScalar();
        if (priceObj == null)
        {
            throw new InvalidOperationException($"Товар {productId} не найден.");
        }
        decimal unitPrice = Convert.ToDecimal(priceObj);

        string itemSql = @"
            INSERT INTO order_items (order_id, product_id, quantity, unit_price)
            VALUES (@order_id, @product_id, @quantity, @unit_price);";

        using var itemCommand = new SqlCommand(itemSql, connection, transaction);
        itemCommand.Parameters.AddWithValue("@order_id", orderId);
        itemCommand.Parameters.AddWithValue("@product_id", productId);
        itemCommand.Parameters.AddWithValue("@quantity", quantity);
        itemCommand.Parameters.AddWithValue("@unit_price", unitPrice);
        itemCommand.ExecuteNonQuery();

        total += unitPrice * quantity;
    }

    transaction.Commit();
    Console.WriteLine($"Заказ №{orderId} создан на сумму {total} руб.");
}
catch (Exception ex)
{
    try
    {
        transaction.Rollback();
    }
    catch (Exception rollbackEx)
    {
        Console.WriteLine($"Не удалось выполнить откат: {rollbackEx.Message}");
    }
    Console.WriteLine($"Ошибка при создании заказа: {ex.Message}");
}