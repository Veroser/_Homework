using System;
using System.Collections.Generic;
using System.Linq;
namespace ConsoleApp2;
class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public double AverageScore { get; set; }
}

class Program
{
    static void Main()
    {
        List<Student> students = new List<Student>()
        {
            new Student { FirstName = "Иван", LastName = "Петров", Age = 20, AverageScore = 85.5 },
            new Student { FirstName = "Анна", LastName = "Сидорова", Age = 23, AverageScore = 78.1 },
            new Student { FirstName = "Олег", LastName = "Кузнецов", Age = 19, AverageScore = 74.9 },
            new Student { FirstName = "Мария", LastName = "Васильева", Age = 26, AverageScore = 91.3 },
            new Student { FirstName = "Алексей", LastName = "Смирнов", Age = 22, AverageScore = 95.2 }
        };

        Console.WriteLine("--- Список студентов-хорошистов (балл от 75 до 90) ---");
        var goodStudents = students.Where(s => s.AverageScore >= 75 && s.AverageScore <= 90);
        foreach (var s in goodStudents)
        {
            Console.WriteLine(s.LastName + " " + s.FirstName + " - " + s.AverageScore);
        }
        Console.WriteLine();

        Console.WriteLine("--- Все студенты ---");
        var names = students.Select(s => s.LastName + " " + s.FirstName);
        foreach (var n in names)
        {
            Console.WriteLine(n);
        }
        Console.WriteLine();

        Console.WriteLine("--- Сортировка по возрасту ---");
        var sortedByAge = students.OrderBy(s => s.Age);
        foreach (var s in sortedByAge)
        {
            Console.WriteLine(s.LastName + " " + s.FirstName + " - " + s.Age + " лет");
        }
        Console.WriteLine();

        Console.WriteLine("--- Рейтинг студентов младше 25 лет (по убыванию балла) ---");
        var rating = students
            .Where(s => s.Age < 25)
            .OrderByDescending(s => s.AverageScore)
            .Select(s => s.LastName + " " + s.FirstName + " - " + s.AverageScore);
        foreach (var r in rating)
        {
            Console.WriteLine(r);
        }
    }
}