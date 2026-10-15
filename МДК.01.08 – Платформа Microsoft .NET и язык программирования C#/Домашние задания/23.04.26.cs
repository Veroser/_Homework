using System;
namespace ConsoleApp1;
abstract class Delivery
{
public string Address { get; set; }
public string Recipient { get; set; }
public abstract int BaseCost { get; }
public abstract DateTime SendDateTime { get; set; }

public abstract double CalculateCost();
public abstract double GetDeliveryTime();
}

class HomeDelivery : Delivery
{
public double Distance { get; set; }
public double Weight { get; set; }

public override int BaseCost
{
get { return 10; }
}

public override DateTime SendDateTime { get; set; }

public override double CalculateCost()
{
return Distance * BaseCost + Weight * BaseCost;
}

public override double GetDeliveryTime()
{
return Distance / 50.0;
}
}

class PickPointDelivery : Delivery
{
public double Weight { get; set; }

public override int BaseCost
{
get { return 15; }
}

public override DateTime SendDateTime { get; set; }

public override double CalculateCost()
{
return BaseCost + Weight * BaseCost;
}

public override double GetDeliveryTime()
{
return 2;
}
}

class ExpressDelivery : Delivery
{
public double Distance { get; set; }
public double Weight { get; set; }

public override int BaseCost
{
get { return 20; }
}

public override DateTime SendDateTime { get; set; }

public override double CalculateCost()
{
return (Distance * BaseCost + Weight * BaseCost) * 2;
}

public override double GetDeliveryTime()
{
return (Distance / 50.0) / 3.0;
}
}

interface IUserInterface
{
void ShowInfo(Delivery delivery);
}

class ConsoleUserInterface : IUserInterface
{
public void ShowInfo(Delivery delivery)
{
Console.WriteLine("Адрес: " + delivery.Address);
Console.WriteLine("Получатель: " + delivery.Recipient);
Console.WriteLine("Стоимость: " + delivery.CalculateCost());
Console.WriteLine("Время доставки: " + delivery.GetDeliveryTime());
DateTime received = delivery.SendDateTime.AddHours(delivery.GetDeliveryTime());
Console.WriteLine("Дата получения: " + received.ToString("dd.MM.yyyy"));
Console.WriteLine();
}
}

class Program1
{
static void Main()
{
HomeDelivery hd = new HomeDelivery();
hd.Address = "ул. Ленина, 1";
hd.Recipient = "Иванов";
hd.Distance = 100;
hd.Weight = 5;
hd.SendDateTime = new DateTime(2025, 1, 1);

PickPointDelivery pp = new PickPointDelivery();
pp.Address = "Пункт выдачи №1";
pp.Recipient = "Петров";
pp.Weight = 3;
pp.SendDateTime = new DateTime(2025, 1, 1);

ExpressDelivery ex = new ExpressDelivery();
ex.Address = "ул. Пушкина, 10";
ex.Recipient = "Сидоров";
ex.Distance = 200;
ex.Weight = 2;
ex.SendDateTime = new DateTime(2025, 1, 1);

IUserInterface ui = new ConsoleUserInterface();
ui.ShowInfo(hd);
ui.ShowInfo(pp);
ui.ShowInfo(ex);
}
}