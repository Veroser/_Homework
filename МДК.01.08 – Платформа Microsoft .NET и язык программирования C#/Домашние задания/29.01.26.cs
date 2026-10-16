using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Задание 1");
        string[] fruits = { "яблоко", "банан", "апельсин", "груша", "виноград" };
        Console.WriteLine("Исходный массив:");
        for (int i = 0; i < fruits.Length; i++)
        {
            Console.WriteLine(fruits[i]);
        }
        string[] newFruits = new string[fruits.Length - 1];
        int index = 0;
        for (int i = 0; i < fruits.Length; i++)
        {
            if (fruits[i] != "апельсин")
            {
                newFruits[index] = fruits[i];
                index++;
            }
        }
        Console.WriteLine("Обновленный массив:");
        for (int i = 0; i < newFruits.Length; i++)
        {
            Console.WriteLine(newFruits[i]);
        }

        Console.WriteLine("Задание 2");
        string[] names = { "Иван", "Мария", "Алекс", "Петр", "Анна" };
        bool found = false;
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i] == "Алекс")
            {
                found = true;
            }
        }
        if (found)
        {
            Console.WriteLine("Имя Алекс найдено");
        }
        else
        {
            Console.WriteLine("Имя Алекс не найдено");
        }

        Console.WriteLine("Задание 3");
        int[] numbers = { 3, 7, 2, 8, 5 };
        int sum = 0;
        for (int i = 0; i < numbers.Length; i++)
        {
            sum = sum + numbers[i];
        }
        Console.WriteLine("Сумма элементов: " + sum);

        Console.WriteLine("Задание 4");
        int[] nums = new int[10];
        int[] squares = new int[10];
        int[] cubes = new int[10];
        for (int i = 0; i < 10; i++)
        {
            nums[i] = i + 1;
            squares[i] = (i + 1) * (i + 1);
            cubes[i] = (i + 1) * (i + 1) * (i + 1);
        }
        Console.WriteLine("Числа:");
        for (int i = 0; i < 10; i++)
        {
            Console.Write(nums[i] + " ");
        }
        Console.WriteLine();
        Console.WriteLine("Квадраты:");
        for (int i = 0; i < 10; i++)
        {
            Console.Write(squares[i] + " ");
        }
        Console.WriteLine();
        Console.WriteLine("Кубы:");
        for (int i = 0; i < 10; i++)
        {
            Console.Write(cubes[i] + " ");
        }
        Console.WriteLine();
    }
}