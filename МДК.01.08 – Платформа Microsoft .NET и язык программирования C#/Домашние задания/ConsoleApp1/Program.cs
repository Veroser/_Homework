using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ConsoleApp1
{
    internal class task1
    {
        static double IMT(double weight, double height)// create a function
        {
            double imt = weight / (height * height);
            return imt;
        } 
            static void Main(string[] args)
        {
            Console.WriteLine("--- Калькулятор индекса массы тела ---");
            Console.WriteLine("Введите ваш вес(кг): ");
            double weight = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Введите ваш рост(м): ");
            double height = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("--- Ваш Результат ---");
            Console.WriteLine($"При весе {weight}кг и росте {height}м, ваш ИМТ состовляет: {IMT(weight, height)}");
        } 
               
    } 
               
}
