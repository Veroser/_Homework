using System;
namespace ConsoleApp3;
struct Book
{
    public string Title;
    public string Author;
}

class Student
{
    private static int count = 0;

    public string Name;
    public Book FavoriteBook;

    public static int Count
    {
        get { return count; }
    }

    public Student(string name, Book favoriteBook)
    {
        Name = name;
        FavoriteBook = favoriteBook;
        count++;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("--- Система учета студентов ---");
        Console.WriteLine("Начальное количество студентов в системе: " + Student.Count);

        Book book1 = new Book();
        book1.Title = "Хоббит";
        book1.Author = "Дж. Р. Р. Толкин";

        Student student1 = new Student("Иван", book1);
        Console.WriteLine("Создан студент Иван.");
        Console.WriteLine("Текущее количество студентов в системе: " + Student.Count);

        Book book2 = new Book();
        book2.Title = "Гарри Поттер";
        book2.Author = "Дж. К. Роулинг";

        Student student2 = new Student("Анна", book2);
        Console.WriteLine("Создан студент Анна.");
        Console.WriteLine("Текущее количество студентов в системе: " + Student.Count);

        Console.WriteLine("--- Эксперимент с копированием ---");
        Console.WriteLine("Оригинальный студент: " + student1.Name + ", его любимая книга: \"" + student1.FavoriteBook.Title + "\" автора " + student1.FavoriteBook.Author + ".");

        Student studentCopy = student1;
        Book bookCopy = student1.FavoriteBook;

        Console.WriteLine("...Копируем данные и вносим изменения...");

        studentCopy.Name = "Петр";
        Console.WriteLine("Изменяем имя у копии студента на 'Петр'.");

        bookCopy.Title = "Властелин Колец";
        Console.WriteLine("Изменяем название у копии книги на 'Властелин Колец'.");

        Console.WriteLine("Результат после изменений:");
        Console.WriteLine("Имя оригинального студента (student1): " + student1.Name);
        Console.WriteLine("Название любимой книги оригинального студента (student1.FavoriteBook): " + student1.FavoriteBook.Title);

        Console.WriteLine("Вывод: Имя студента изменилось, так как классы копируются по ссылке.");
        Console.WriteLine("Вывод: Книга не изменилась, так как структуры копируются по значению.");
    }
}