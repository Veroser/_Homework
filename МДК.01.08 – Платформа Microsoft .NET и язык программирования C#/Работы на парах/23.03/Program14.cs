/*Задача 3: Конвертер валют (Статические методы)
Создайте статический класс CurrencyConverter.
Статическое поле usdToRubRate (курс доллара к рублю).
Статический метод SetRate(double rate) для изменения курса.
Статический метод ConvertUsdToRub(double usd), возвращающий сумму в рублях.
Статический метод ConvertRubToUsd(double rub), возвращающий сумму в долларах.
Проверка: Вызовите методы без создания экземпляра класса.

Задача 4: Математическая константа
Создайте нестатический класс Circle.
Статическое поле (константу) PI = 3.14159 (используйте const или static readonly).
Обычное поле Radius.
Метод GetArea(), который использует PI для расчета площади.
Проверка: Создайте два круга с разными радиусами, покажите их площади.*/

namespace ConsoleApp1 {
   internal class Program14
    {
        static void Main(string[] args)
        {
            Console.WriteLine(CurrencyConverter.ConvertUsdToRub(100));

            Console.WriteLine(CurrencyConverter.ConvertRubToUsd(100));

            Circle c1 = new Circle(10);
            Circle c2 = new Circle(5);
            Console.WriteLine(c1.GetArea());
            Console.WriteLine(c2.GetArea());

        }
    }

    static class CurrencyConverter
    {
        public static double usdToRubRate = 84.41;
        
        public static void setRate(double rate)
        {
            usdToRubRate = rate;
        }

        public static double ConvertUsdToRub(double usd)
        {
            return usd * usdToRubRate;
        }

        public static double ConvertRubToUsd(double rub)
        {
            return rub / usdToRubRate;
        }
    }
    class Circle
    {
        public static readonly double PI = 3.14159;
        public double Radius;

        public Circle(double radius)
        {
            Radius = radius;
        }

        public double GetArea()
        {
            return PI * Radius * Radius;
        }
    }


}