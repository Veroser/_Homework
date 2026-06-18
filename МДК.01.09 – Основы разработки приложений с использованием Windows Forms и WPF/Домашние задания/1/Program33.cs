namespace ConsoleApp1;

public class Order
{
    private static int nextId = 1;
    public int OrderId { get; private set; }

    public Order()
    {
        OrderId = nextId;
        nextId++;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Order #{OrderId}");
    }
}

public class DatabaseConnector
{
    private static string connectionString;

    static DatabaseConnector()
    {
        connectionString = "Server=localhost;DB=Test";
        Console.WriteLine("Static constructor called");
    }

    public DatabaseConnector()
    {
        Console.WriteLine("Instance created");
    }

    public void Connect()
    {
        Console.WriteLine($"Connecting with: {connectionString}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Task 1:");
        Order[] orders = new Order[4];
        for (int i = 0; i < 4; i++)
        {
            orders[i] = new Order();
            orders[i].DisplayInfo();
        }

        Console.WriteLine("\nTask 2:");
        DatabaseConnector db1 = new DatabaseConnector();
        db1.Connect();
        
        DatabaseConnector db2 = new DatabaseConnector();
        db2.Connect();
    }
}
