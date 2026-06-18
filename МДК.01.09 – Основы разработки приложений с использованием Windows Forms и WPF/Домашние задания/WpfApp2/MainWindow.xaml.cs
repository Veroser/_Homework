using System;
using System.Windows;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            double[] expenses = new double[7];
            expenses[0] = ParseDouble(Monday.Text);
            expenses[1] = ParseDouble(Tuesday.Text);
            expenses[2] = ParseDouble(Wednesday.Text);
            expenses[3] = ParseDouble(Thursday.Text);
            expenses[4] = ParseDouble(Friday.Text);
            expenses[5] = ParseDouble(Saturday.Text);
            expenses[6] = ParseDouble(Sunday.Text);

            double sum = 0;
            double max = 0;

            for (int i = 0; i < expenses.Length; i++)
            {
                sum += expenses[i];
                if (expenses[i] > max)
                {
                    max = expenses[i];
                }
            }

            double average = sum / 7;

            TotalResult.Text = $"Общая сумма: {sum:F2}";
            AverageResult.Text = $"Средний расход: {average:F2}";
            MaxResult.Text = $"Максимальный расход: {max:F2}";
        }

        private double ParseDouble(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            if (double.TryParse(input, out double result))
            {
                return result;
            }

            return 0;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Monday.Text = "";
            Tuesday.Text = "";
            Wednesday.Text = "";
            Thursday.Text = "";
            Friday.Text = "";
            Saturday.Text = "";
            Sunday.Text = "";

            TotalResult.Text = "Общая сумма: 0";
            AverageResult.Text = "Средний расход: 0";
            MaxResult.Text = "Максимальный расход: 0";
        }
    }
}