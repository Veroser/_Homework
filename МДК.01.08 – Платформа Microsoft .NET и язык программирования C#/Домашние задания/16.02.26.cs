using System;
using System.Collections.Generic;

class Program102
{
    static void Main()
    {
        Dictionary<string, int> shopItems = new Dictionary<string, int>
        {
            {"Health Potion", 50},
            {"Mana Potion", 75},
            {"Sword", 200},
            {"Shield", 150}
        };

        while (true)
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Вывести все товары");
            Console.WriteLine("2. Заменить товар");
            Console.WriteLine("3. Добавить товар");
            Console.WriteLine("4. Удалить товар");
            Console.WriteLine("5. Подсчитать стоимость");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт: ");
            int choice = int.Parse(Console.ReadLine());

            if (choice == 0)
            {
                break;
            }
            else if (choice == 1)
            {
                Console.WriteLine("Товары:");
                foreach (KeyValuePair<string, int> item in shopItems)
                {
                    Console.WriteLine(item.Key + " - " + item.Value);
                }
            }
            else if (choice == 2)
            {
                Console.Write("Введите название товара для замены: ");
                string oldName = Console.ReadLine();
                if (shopItems.ContainsKey(oldName))
                {
                    Console.Write("Введите новое название: ");
                    string newName = Console.ReadLine();
                    Console.Write("Введите новую цену: ");
                    int newPrice = int.Parse(Console.ReadLine());
                    shopItems.Remove(oldName);
                    shopItems.Add(newName, newPrice);
                    Console.WriteLine("Товар заменен.");
                }
                else
                {
                    Console.WriteLine("Товар не найден.");
                }
            }
            else if (choice == 3)
            {
                Console.Write("Введите название товара: ");
                string name = Console.ReadLine();
                Console.Write("Введите цену: ");
                int price = int.Parse(Console.ReadLine());
                shopItems.Add(name, price);
                Console.WriteLine("Товар добавлен.");
            }
            else if (choice == 4)
            {
                Console.Write("Введите название товара для удаления: ");
                string name = Console.ReadLine();
                if (shopItems.Remove(name))
                {
                    Console.WriteLine("Товар удален.");
                }
                else
                {
                    Console.WriteLine("Товар не найден.");
                }
            }
            else if (choice == 5)
            {
                Console.Write("Введите название товара: ");
                string name = Console.ReadLine();
                if (shopItems.ContainsKey(name))
                {
                    Console.Write("Введите количество: ");
                    int count = int.Parse(Console.ReadLine());
                    int total = shopItems[name] * count;
                    Console.WriteLine("Стоимость: " + total);
                }
                else
                {
                    Console.WriteLine("Товар не найден.");
                }
            }
        }
    }
}
