using System.Data;
using Microsoft.Data.SqlClient;

string connectionString = "Server=localhost;Database=coffee_shop;Trusted_Connection=True;TrustServerCertificate=True;";

DataSet? dataSet = null;

void LoadData()
{
    dataSet = new DataSet("coffee_shop");
    using var connection = new SqlConnection(connectionString);
    using var productsAdapter = new SqlDataAdapter(
        "SELECT id, name, category_id, price, is_available FROM products", connection);
    using var categoriesAdapter = new SqlDataAdapter(
        "SELECT id, name FROM categories", connection);

    productsAdapter.Fill(dataSet, "products");
    categoriesAdapter.Fill(dataSet, "categories");

    var categories = dataSet.Tables["categories"]!;
    var products = dataSet.Tables["products"]!;

    if (!dataSet.Relations.Contains("category_products"))
    {
        dataSet.Relations.Add("category_products",
            categories.Columns["id"]!,
            products.Columns["category_id"]!);
    }

    Console.WriteLine($"Загружено таблиц: {dataSet.Tables.Count}");
    Console.WriteLine($"Товаров: {products.Rows.Count}, категорий: {categories.Rows.Count}");
}

void ShowAllProducts()
{
    if (dataSet == null) { Console.WriteLine("Данные не загружены."); return; }
    var products = dataSet.Tables["products"]!;
    if (products.Rows.Count == 0) { Console.WriteLine("Товары не найдены."); return; }
    foreach (DataRow row in products.Rows)
    {
        int id = row.Field<int>("id");
        string name = row.Field<string>("name")!;
        decimal price = row.Field<decimal>("price");
        bool available = row.Field<bool>("is_available");
        Console.WriteLine($"{id}. {name} – {price} руб. | {(available ? "Доступен" : "Недоступен")}");
    }
}

void ShowAvailableProducts()
{
    if (dataSet == null) { Console.WriteLine("Данные не загружены."); return; }
    var products = dataSet.Tables["products"]!;
    var rows = products.Select("is_available = true");
    if (rows.Length == 0) { Console.WriteLine("Доступные товары не найдены."); return; }
    foreach (var row in rows)
    {
        Console.WriteLine($"{row["name"]} – {row["price"]} руб.");
    }
}

void ShowSortedByPrice()
{
    if (dataSet == null) { Console.WriteLine("Данные не загружены."); return; }
    var products = dataSet.Tables["products"]!;
    var rows = products.Select("", "price ASC");
    if (rows.Length == 0) { Console.WriteLine("Товары не найдены."); return; }
    foreach (var row in rows)
    {
        Console.WriteLine($"{row["name"]} – {row["price"]} руб.");
    }
}

void ShowProductsByCategory()
{
    if (dataSet == null) { Console.WriteLine("Данные не загружены."); return; }
    var categories = dataSet.Tables["categories"]!;
    if (categories.Rows.Count == 0) { Console.WriteLine("Категории не найдены."); return; }
    foreach (DataRow cat in categories.Rows)
    {
        Console.WriteLine($"{cat["id"]}. {cat["name"]}");
    }
    Console.Write("Введите ID категории: ");
    if (!int.TryParse(Console.ReadLine(), out int catId))
    {
        Console.WriteLine("Некорректный ID.");
        return;
    }
    var catRow = categories.Rows.Find(catId);
    if (catRow == null) { Console.WriteLine("Категория не найдена."); return; }
    Console.WriteLine($"Категория: {catRow["name"]}");
    var children = catRow.GetChildRows("category_products");
    if (children.Length == 0) { Console.WriteLine("Товары не найдены."); return; }
    foreach (var p in children)
    {
        Console.WriteLine($" - {p["name"]}");
    }
}

void EditPriceInMemory()
{
    if (dataSet == null) { Console.WriteLine("Данные не загружены."); return; }
    var products = dataSet.Tables["products"]!;
    if (products.Rows.Count == 0) { Console.WriteLine("Товары не найдены."); return; }
    Console.Write("Введите ID товара: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Некорректный ID.");
        return;
    }
    var row = products.Rows.Find(id);
    if (row == null) { Console.WriteLine("Товар не найден."); return; }
    Console.Write("Новая цена: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal newPrice))
    {
        Console.WriteLine("Некорректная цена.");
        return;
    }
    Console.WriteLine($"До: {row["price"]}");
    row["price"] = newPrice;
    Console.WriteLine($"После: {row["price"]}");
    Console.WriteLine($"Состояние: {row.RowState}");
}

void ReloadAndCompare()
{
    if (dataSet == null) { Console.WriteLine("Данные не загружены."); return; }
    var oldProducts = dataSet.Tables["products"]!;
    if (oldProducts.Rows.Count == 0) { Console.WriteLine("Товары не найдены."); return; }
    Console.Write("Введите ID товара: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Некорректный ID.");
        return;
    }
    var oldRow = oldProducts.Rows.Find(id);
    if (oldRow == null) { Console.WriteLine("Товар не найден."); return; }
    decimal localPrice = oldRow.Field<decimal>("price");

    var newTable = new DataTable();
    using var connection = new SqlConnection(connectionString);
    using var adapter = new SqlDataAdapter("SELECT id, name, price FROM products WHERE id = @id", connection);
    adapter.SelectCommand!.Parameters.AddWithValue("@id", id);
    adapter.Fill(newTable);
    if (newTable.Rows.Count == 0) { Console.WriteLine("Товар не найден в базе."); return; }
    decimal dbPrice = newTable.Rows[0].Field<decimal>("price");

    Console.WriteLine($"Цена в памяти: {localPrice}");
    Console.WriteLine($"Цена в базе:  {dbPrice}");
    Console.WriteLine(localPrice == dbPrice ? "Значения совпадают." : "Значения различаются (локальные изменения не сохранены в базе).");
}

void ShowCategoryCounts()
{
    if (dataSet == null) { Console.WriteLine("Данные не загружены."); return; }
    var categories = dataSet.Tables["categories"]!;
    var list = new List<(string Name, int Count)>();
    foreach (DataRow cat in categories.Rows)
    {
        int count = cat.GetChildRows("category_products").Length;
        list.Add((cat.Field<string>("name")!, count));
    }
    foreach (var item in list.OrderByDescending(x => x.Count))
    {
        Console.WriteLine($"{item.Name}: {item.Count}");
    }
}

while (true)
{
    Console.WriteLine("=== КАТАЛОГ КОФЕЙНИ В ПАМЯТИ ===");
    Console.WriteLine("1. Загрузить данные");
    Console.WriteLine("2. Показать все товары");
    Console.WriteLine("3. Показать только доступные товары");
    Console.WriteLine("4. Отсортировать товары по цене");
    Console.WriteLine("5. Показать товары выбранной категории");
    Console.WriteLine("6. Изменить цену товара в памяти");
    Console.WriteLine("7. Повторно загрузить и сравнить цену");
    Console.WriteLine("8. Количество товаров по категориям");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите операцию: ");
    string choice = Console.ReadLine()!;

    try
    {
        switch (choice)
        {
            case "1": LoadData(); break;
            case "2": ShowAllProducts(); break;
            case "3": ShowAvailableProducts(); break;
            case "4": ShowSortedByPrice(); break;
            case "5": ShowProductsByCategory(); break;
            case "6": EditPriceInMemory(); break;
            case "7": ReloadAndCompare(); break;
            case "8": ShowCategoryCounts(); break;
            case "0": return;
            default: Console.WriteLine("Неверный выбор."); break;
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