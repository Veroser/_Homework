using System;

abstract class Figure
{
    public abstract double getArea();
    public abstract double getPerimeter();
    
    public void printInfo()
    {
        Console.WriteLine($"Тип фигуры: {this.GetType().Name}, Площадь: {getArea()}, Периметр: {getPerimeter()}");
    }
}

class Circle : Figure
{
    private double radius;
    
    public Circle(double radius)
    {
        this.radius = radius;
    }
    
    public override double getArea()
    {
        return Math.PI * radius * radius;
    }
    
    public override double getPerimeter()
    {
        return 2 * Math.PI * radius;
    }
}

class Rectangle : Figure
{
    private double width;
    private double height;
    
    public Rectangle(double width, double height)
    {
        this.width = width;
        this.height = height;
    }
    
    public override double getArea()
    {
        return width * height;
    }
    
    public override double getPerimeter()
    {
        return 2 * (width + height);
    }
}

abstract class Payment
{
    protected double amount;
    protected string transactionId;
    
    public Payment(double amount, string transactionId)
    {
        this.amount = amount;
        this.transactionId = transactionId;
    }
    
    public abstract void processPayment();
}

class CreditCardPayment : Payment
{
    public CreditCardPayment(double amount, string transactionId) : base(amount, transactionId) {}
    
    public override void processPayment()
    {
        if (amount > 100000)
        {
            Console.WriteLine("Отказ: сумма превышает лимит");
        }
        else
        {
            Console.WriteLine("Успех: оплата прошла успешно");
        }
    }
}

class PayPalPayment : Payment
{
    public PayPalPayment(double amount, string transactionId) : base(amount, transactionId) {}
    
    public override void processPayment()
    {
        Console.WriteLine($"Перенаправление на PayPal... Оплата {amount} успешна");
    }
}

class Program
{
    static void Main()
    {
        Figure[] figures = {
            new Circle(5),
            new Rectangle(4, 6)
        };
        
        foreach (Figure figure in figures)
        {
            figure.printInfo();
        }
        
        Console.WriteLine();
        
        Payment[] payments = {
            new CreditCardPayment(50000, "CC001"),
            new CreditCardPayment(150000, "CC002"),
            new PayPalPayment(30000, "PP001")
        };
        
        foreach (Payment payment in payments)
        {
            payment.processPayment();
        }
    }
}