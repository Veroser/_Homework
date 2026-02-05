using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ConsoleApp1
{
    internal class task1
    {

        static int merge_sorted_lists(int[] list1, int[] list2)
        {
            
        }
        static int find_max(int[] numbers)
        {
            int max_numbers = numbers[0];
            foreach (int i in numbers)
            {
                if (i > max_numbers)
                {
                    max_numbers = i;
                }
            }

            return max_numbers;
        }
        static int sum_range(int start, int end)
        {
            int sum = 0;
            for (int i = start; i <= end; i++)
            {
                sum += i;
            }
            return sum;
        }

       static void Main(string[] args)
        {
            int[] numbers = new int[4] {1, 3, 6, 5};
           Console.WriteLine(find_max(numbers));
           Console.WriteLine(sum_range(1, 5));
        } 
               
    } 
               
}
